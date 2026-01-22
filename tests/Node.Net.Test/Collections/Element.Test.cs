#if IS_WINDOWS
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Node.Net.Collections;
using Node.Net; // For extension methods

namespace Node.Net.Collections
{
	internal class ElementTest
	{
		[Test]
		public async Task Construction()
		{
			var empty = new Element();

			var stream = typeof(FormatterTest).Assembly.FindManifestResourceStream("States.json");
			var states = new Element(stream);
			await Assert.That(states.Count).IsEqualTo(50);
		}

		[Test]
		public async Task Parent()
		{
			var stream = typeof(FormatterTest).Assembly.FindManifestResourceStream("States.json");
			var states = new Element(stream);
			await Assert.That(states.Count).IsEqualTo(50);

			foreach(var state_name in states.Keys)
			{
				var state = states[state_name] as Element;
				await Assert.That(ReferenceEquals(states, state.Parent)).IsTrue();
			}
		}
	}
}
#endif
