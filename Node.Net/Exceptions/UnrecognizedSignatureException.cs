using System;

namespace Node.Net
{
    /// <summary>
    /// Exception used when an unrecognized file signature is encounted while reading a stream
    /// </summary>
    [Serializable]
    public sealed class UnrecognizedSignatureException : Exception
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message"></param>
		public UnrecognizedSignatureException(string message) : base(message) { }

        public UnrecognizedSignatureException()
        {
        }

        public UnrecognizedSignatureException(string message, Exception innerException) : base(message, innerException)
        {
        }

        private UnrecognizedSignatureException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
            throw new NotImplementedException();
        }
    }
}