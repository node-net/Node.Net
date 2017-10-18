using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Node.Net
{
	public class UnrecognizedSignatureException : Exception
	{
		public UnrecognizedSignatureException(string message) : base(message) { }
	}
}
