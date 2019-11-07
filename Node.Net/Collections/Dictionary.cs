using System.Collections.Generic;
/* Unmerged change from project 'Node.Net (net48)'
Before:
using System.Runtime.CompilerServices;
After:
using System.Linq;
using System.Runtime.CompilerServices;
*/

/* Unmerged change from project 'Node.Net (net48)'
Before:
using System.Security.Permissions;

using System.Linq;
After:
using System.Security.Permissions;
*/

namespace Node.Net.Collections
{
	public class Dictionary : Dictionary<string, object>
	{
		public string Json { get { return this.ToJson(); } set { } }
	}
}
