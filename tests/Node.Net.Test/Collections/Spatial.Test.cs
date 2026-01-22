using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Node.Net.Collections;
using Node.Net;

namespace Node.Net.Collections
{
	internal class SpatialTest
	{
		[Test]
		public async Task Construction()
		{
			var empty = new Spatial();
			await Task.CompletedTask;
		}

		[Test]
		public async Task Usage()
		{
			var yard = Yard;
			var mailbox = yard.Descendants.FirstOrDefault(e => e.Name == "mailbox");
			await Assert.That(ReferenceEquals(yard, mailbox.Parent)).IsTrue();
			var stake1 = yard.Descendants.FirstOrDefault(e => e.Name == "stake1");

			var json = yard.Json;
			// Normalize line endings for cross-platform compatibility
			json = json.Replace("\r\n", "\n").Replace("\r", "\n");
			await Assert.That(json.Length).IsEqualTo(61);
		}

		public static Spatial Yard
		{
			get
			{
				var yard = new Spatial();
				yard.Add("mailbox", new Spatial());
				var garden = new Spatial();
				garden.Add("stake1", new Spatial());
				yard.Add("garden", garden);
				yard.DeepUpdateParents();
				return yard;
			}
		}
	}
}
