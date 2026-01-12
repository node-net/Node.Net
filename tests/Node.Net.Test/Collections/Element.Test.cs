#if IS_WINDOWS
extern alias NodeNet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using NUnit.Framework;
using NodeNet::Node.Net.Collections;
using NodeNet::Node.Net; // For extension methods

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
			Assert.That(states.Count, Is.EqualTo(50), "states.Count");
		}

		[Test]
		public void Parent()
		{
			var stream = typeof(FormatterTest).Assembly.FindManifestResourceStream("States.json");
			var states = new Element(stream);
			Assert.That(states.Count, Is.EqualTo(50), "states.Count");

			foreach(var state_name in states.Keys)
			{
				var state = states[state_name] as Element;
				Assert.That(states, Is.SameAs(state.Parent));
			}
		}
	}
}
#endif
