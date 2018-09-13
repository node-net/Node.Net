using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Node.Net.JsonRPC
{
	/// <summary>
	/// Request compatiple with JSON-RPC 2.0 specification for a request object
	/// </summary>
	public sealed class Request : Dictionary<string, dynamic>
	{
		public Request(string _method)
		{
			Add("jsonrpc", "2.0");
			Add("method", _method);
			Add("id", new Random().Next(10000, 20000));
			Add("params", Parameters);
		}

		public Request(string _method, int _id)
		{
			Add("jsonrpc", "2.0");
			Add("method", _method);
			Add("id", _id);
			Add("params", Parameters);
		}

		public Request(string _method, IDictionary parameters)
		{
			Add("jsonrpc", "2.0");
			Add("method", _method);
			Add("id", new Random().Next(10000, 20000));
			foreach (var key in parameters.Keys)
			{
				Parameters.Add(key.ToString(), parameters[key]);
			}
			Add("params", Parameters);
		}

		public Request(IDictionary data)
		{
			SetData(data);
		}
		public Request(Stream stream)
		{
			var data = new Reader().Read(stream) as IDictionary;
			SetData(data);
		}
		private void SetData(IDictionary data)
		{
			this.Add("jsonrpc", "2.0");
			var random = new Random();
			this.Add("id", random.Next(100000, 200000));

			if (data.Contains("id"))
			{
				this["id"] = data["id"];
			}

			if (data.Contains("method"))
			{
				this.Add("method", data["method"]);
			}

			if (data.Contains("params"))
			{
				this.Add("params", data["params"]);
				if (data["params"] is IDictionary idictionary)
				{
					foreach (var key in idictionary.Keys)
					{
						Parameters.Add(key.ToString(), idictionary[key]);
					}
				}
			}
		}
		public string Method { get { return this.Get<string>("method"); } }
		public Dictionary<string, object> Parameters { get { return parameters; } }
		private readonly Dictionary<string, object> parameters = new Dictionary<string, object>();
		public int Id { get { return this.Get<int>("id"); } }

		public byte[] GetBytes()
		{
			using (var memory = new MemoryStream())
			{
				using (var writer = new StreamWriter(memory))
				{
					writer.WriteLine(ToJson());
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
		public Stream ToStream()
		{
			return new MemoryStream(Encoding.UTF8.GetBytes(ToJson()));
		}
	}
}