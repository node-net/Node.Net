using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Node.Net.JsonRPC
{
	public sealed class Response : Dictionary<string, dynamic>
	{
		public Response(byte[] data)
		{
			this.Add("jsonrpc", "2.0");
			using (var memory = new MemoryStream(data))
			{
				var dictionary = new Reader().Read<IDictionary>(memory);
				if (dictionary.Contains("id"))
				{
					this.Add("id", dictionary["id"]);
				}

				if (dictionary.Contains("result"))
				{
					this.Add("result", dictionary["result"]);
				}

				if (dictionary.Contains("error"))
				{
					var error = new Error(dictionary["error"] as IDictionary);
					this.Add("error", error);
				}
			}
		}

		public Response(IDictionary data)
		{
			SetData(data);
		}

		public Response(int id, object result)
		{
			Add("jsonrpc", "2.0");
			Add("id", id);
			Add("result", result);
		}

		public Response(Error error, int id)
		{
			Add("jsonrpc", "2.0");
			Add("id", id);
			Add("error", error);
		}

		public Response(Stream stream)
		{
			var data = new Reader().Read<IDictionary>(stream);
			if (data != null)
			{
				SetData(data);
			}
		}

		private void SetData(IDictionary data)
		{
			this.Add("jsonrpc", "2.0");
			if (data.Contains("id"))
			{
				this.Add("id", data["id"]);
			}

			if (data.Contains("result"))
			{
				this.Add("result", data["result"]);
			}

			if (data.Contains("error"))
			{
				var error = new Error(data["error"] as IDictionary);
				this.Add("error", error);
			}
		}

		public object Result
		{
			get
			{
				var result = this.Get<object>("result");
				if (result != null && (result is string))
				{
					var original = result.ToString();
					return original.Replace(@"\u0022", @"""").Replace("u0022", @"""");
				}
				return result;
			}
		}

		public Error Error { get { return this.Get<Error>("error"); } }
		public int Id { get { return this.Get<int>("id"); } }

		public byte[] GetBytes()
		{
			using (var memory = new MemoryStream())
			{
				using (var writer = new StreamWriter(memory))
				{
					writer.WriteLine(this.ToJson());
					writer.Close();
					return memory.ToArray();
				}
			}
		}

		public string ToJson()
		{
			return IDictionaryExtension.ToJson(this);
		}

		public override string ToString()
		{
			return ToJson();
		}
	}
}