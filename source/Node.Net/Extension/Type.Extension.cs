using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Extension;

public static class TypeExtensions
{
	public static string GetFolderPath(this Type type, Environment.SpecialFolder specialFolder)
	{
		return System.IO.Path.Combine(type.Assembly.GetFolderPath(specialFolder), type.FullName ?? type.Name);
	}
}
