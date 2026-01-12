using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Diagnostic.Generic;

/// <summary>
/// A generic test harness for testing types in a diagnostic context.
/// </summary>
/// <typeparam name="T"></typeparam>
public class TestHarness<T> : TestHarness where T : class, new()
{
	/// <summary>
	/// Initializes a new instance of the <see cref="TestHarness{T}"/> class.
	/// </summary>
	/// <remarks>This constructor sets up the test harness for the specified target type. The target type must not
	/// be null.</remarks>
	/// <exception cref="ArgumentNullException">Thrown if the target type <typeparamref name="T"/> is null.</exception>
	public TestHarness() : base(typeof(T))
	{
		if (typeof(T) == null)
		{
			throw new ArgumentNullException(nameof(T), "Target type cannot be null");
		}
	}


}