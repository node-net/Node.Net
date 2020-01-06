using System.Collections;
using System.Collections.Generic;

namespace Node.Net.JsonRPC
{
    public enum ErrorCode
    {
        ParseError = -32700,
        InternalError = -32603,
        InvalidParameters = -32602,
        MethodNotFound = -32601,
        InvalidRequest = -32600
    }

    [System.Serializable]
    public sealed class Error : Dictionary<string, dynamic>
    {
        public Error(IDictionary data)
        {
            if (data.Contains("code"))
            {
                this.Add("code", data["code"]);
            }

            if (data.Contains("message"))
            {
                this.Add("message", data["message"]);
            }

            if (data.Contains("data"))
            {
                this.Add("data", data["data"]);
            }
        }

        public Error(int code, string message)
        {
            Add("code", code);
            Add("message", message);
        }

        public Error(int code, string message, string data)
        {
            Add("code", code);
            Add("message", message);
            Add("data", data);
        }

        public int Code { get { return this.Get<int>("code"); } }
        public string Message { get { return this.Get<string>("message"); } }
        public object Data { get { return this.Get<object>("data"); } }

        public static Dictionary<ErrorCode, string> ErrorCodeNames { get; set; } = new Dictionary<ErrorCode, string>
        {
            { ErrorCode.ParseError,"Parse error"},
            { ErrorCode.InvalidRequest,"Invalid request" },
            { ErrorCode.MethodNotFound,"Method not found" },
            { ErrorCode.InvalidParameters,"Invalid params" },
            { ErrorCode.InternalError,"Internal error" }
        };

        public string ToJson()
        {
            return IDictionaryExtension.ToJson(this);
        }

        private Error(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
            throw new System.NotImplementedException();
        }
    }
}