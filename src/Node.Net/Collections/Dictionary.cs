using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Collections
{
	public class Dictionary : Dictionary<string,object>
	{
		public string Json { get { return this.ToJson(); } set { } }
	}
}
