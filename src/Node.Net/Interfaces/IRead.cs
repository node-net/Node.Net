using System.IO;

namespace Node.Net
{
	public interface IRead
	{
		object Read(Stream stream);
	}
}