using System;

namespace Node.Net
{
	/// <summary>
	/// Exception used when an unrecognized file signature is encounted while reading a stream
	/// </summary>
	public sealed class UnrecognizedSignatureException : Exception
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message"></param>
		public UnrecognizedSignatureException(string message) : base(message) { }
	}
}