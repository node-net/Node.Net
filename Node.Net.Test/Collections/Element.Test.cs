using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using NUnit.Framework;

namespace Node.Net.Collections
{
	[TestFixture]
	internal class ElementTest
	{
		[Test]
		public void Construction()
		{
			var empty = new Element();

			var stream = typeof(FormatterTest).Assembly.FindManifestResourceStream("States.json");
			var states = new Element(stream);
			Assert.AreEqual(50, states.Count, "states.Count");
		}

		[Test]
		public void Parent()
		{
			var stream = typeof(FormatterTest).Assembly.FindManifestResourceStream("States.json");
			var states = new Element(stream);
			Assert.AreEqual(50, states.Count, "states.Count");

			foreach(var state_name in states.Keys)
			{
				var state = states[state_name] as Element;
				Assert.AreSame(states, state.Parent);
			}
		}
	}
}
