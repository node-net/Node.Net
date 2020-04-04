using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Node.Net.JsonRPC
{
    [System.Serializable]
    public sealed class Response : Dictionary<string, dynamic>
    {
        public Response(byte[] data)
        {
            this.Add("jsonrpc", "2.0");
            using (MemoryStream? memory = new MemoryStream(data))
            {
                IDictionary? dictionary = new Reader().Read<IDictionary>(memory);
                if (dictionary.Contains("id"))
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    this.Add("id", dictionary["id"]);
#pragma warning restore CS8604 // Possible null reference argument.
                }

                if (dictionary.Contains("result"))
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    this.Add("result", dictionary["result"]);
#pragma warning restore CS8604 // Possible null reference argument.
                }

                if (dictionary.Contains("error"))
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    Error? error = new Error(dictionary["error"] as IDictionary);
#pragma warning restore CS8604 // Possible null reference argument.
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
            IDictionary? data = new Reader().Read<IDictionary>(stream);
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
#pragma warning disable CS8604 // Possible null reference argument.
                this.Add("id", data["id"]);
#pragma warning restore CS8604 // Possible null reference argument.
            }

            if (data.Contains("result"))
            {
#pragma warning disable CS8604 // Possible null reference argument.
                this.Add("result", data["result"]);
#pragma warning restore CS8604 // Possible null reference argument.
            }

            if (data.Contains("error"))
            {
#pragma warning disable CS8604 // Possible null reference argument.
                Error? error = new Error(data["error"] as IDictionary);
#pragma warning restore CS8604 // Possible null reference argument.
                this.Add("error", error);
            }
        }

        public object Result
        {
            get
            {
                object? result = this.Get<object>("result");
                if (result != null && (result is string))
                {
                    string? original = result.ToString();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    return original.Replace(@"\u0022", @"""").Replace("u0022", @"""");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }
#pragma warning disable CS8603 // Possible null reference return.
                return result;
#pragma warning restore CS8603 // Possible null reference return.
            }
        }

        public Error Error { get { return this.Get<Error>("error"); } }
        public int Id { get { return this.Get<int>("id"); } }

        public byte[] GetBytes()
        {
            using (MemoryStream? memory = new MemoryStream())
            {
                using (StreamWriter? writer = new StreamWriter(memory))
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

        private Response(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
            throw new System.NotImplementedException();
        }
    }
}