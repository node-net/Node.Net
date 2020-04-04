using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Node.Net.JsonRPC
{
    /// <summary>
    /// Request compatiple with JSON-RPC 2.0 specification for a request object
    /// </summary>
    [System.Serializable]
    public sealed class Request : Dictionary<string, dynamic>
    {
        public Request(string _method)
        {
            Add("jsonrpc", "2.0");
            Add("method", _method);
            Add("id", 10000);
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
            Add("id", 10000);
            foreach (object? key in parameters.Keys)
            {
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Parameters.Add(key.ToString(), parameters[key]);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8604 // Possible null reference argument.
            }
            Add("params", Parameters);
        }

        public Request(IDictionary data)
        {
            SetData(data);
        }

        public Request(Stream stream)
        {
            IDictionary? data = new Reader().Read(stream) as IDictionary;
#pragma warning disable CS8604 // Possible null reference argument.
            SetData(data);
#pragma warning restore CS8604 // Possible null reference argument.
        }

        private void SetData(IDictionary data)
        {
            this.Add("jsonrpc", "2.0");
            this.Add("id", 100000);

            if (data.Contains("id"))
            {
                this["id"] = data["id"];
            }

            if (data.Contains("method"))
            {
#pragma warning disable CS8604 // Possible null reference argument.
                this.Add("method", data["method"]);
#pragma warning restore CS8604 // Possible null reference argument.
            }

            if (data.Contains("params"))
            {
#pragma warning disable CS8604 // Possible null reference argument.
                this.Add("params", data["params"]);
#pragma warning restore CS8604 // Possible null reference argument.
                if (data["params"] is IDictionary idictionary)
                {
                    foreach (object? key in idictionary.Keys)
                    {
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        Parameters.Add(key.ToString(), idictionary[key]);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8604 // Possible null reference argument.
                    }
                }
                else
                {
                    if (data["params"] is IEnumerable ienumerable)
                    {
                        int i = 0;
                        foreach (object? p in ienumerable)
                        {
#pragma warning disable CS8604 // Possible null reference argument.
                            Parameters.Add(i.ToString(), p);
#pragma warning restore CS8604 // Possible null reference argument.
                            ++i;
                        }
                    }
                }
            }
        }

        public string Method { get { return this.Get<string>("method"); } }
        public Dictionary<string, object> Parameters { get; } = new Dictionary<string, object>();
        public int Id { get { return this.Get<int>("id"); } }

        public byte[] GetBytes()
        {
            using MemoryStream? memory = new MemoryStream();
            using StreamWriter? writer = new StreamWriter(memory);
            writer.WriteLine(ToJson());
            writer.Close();
            return memory.ToArray();
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

        private Request(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
            throw new System.NotImplementedException();
        }
    }
}