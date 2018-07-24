using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
				if (dictionary.Contains("id")) this.Add("id", dictionary["id"]);
				if (dictionary.Contains("result")) this.Add("result", dictionary["result"]);
				if (dictionary.Contains("error"))
				{
					var error = new Error(dictionary["error"] as IDictionary);
					this.Add("error", error);
				}
			}
		}
		public Response(IDictionary data)
		{
			this.Add("jsonrpc", "2.0");
			if (data.Contains("id")) this.Add("id", data["id"]);
			if (data.Contains("result")) this.Add("result", data["result"]);
			if (data.Contains("error"))
			{
				var error = new Error(data["error"] as IDictionary);
				this.Add("error", error);
			}
		}
		public Response(object result, int id)
		{
			Add("jsonrpc", "2.0");
			Add("result", result);
			Add("id", id);
		}
		public Response(Error error, int id)
		{
			Add("jsonrpc", "2.0");
			Add("error", error);
			Add("id", id);
		}

		public object Result
		{
			get
			{
				//var json = this.ToJson();
				//var data = Reader.Default.Read<IDictionary>(new MemoryStream(Encoding.UTF8.GetBytes(json)));
				//return data.Get<object>("result");
				var result = this.Get<object>("result");
				if (result != null && result.GetType() == typeof(string))
				{
					var original = result.ToString();
					return original.ToString().Replace(@"\u0022", @"""").Replace("u0022", @"""");
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
		public string ToJson() { return IDictionaryExtension.ToJson(this); }
		public override string ToString()
		{
			return ToJson();
		}
	}
}
