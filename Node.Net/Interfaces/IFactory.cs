using System;

namespace Node.Net
{
	public interface IFactory { object? Create(Type targetType, object source); }
}