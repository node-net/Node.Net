﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Node.Net.Collections
{
	[TestFixture]
	internal class SpatialTest
	{
		[Test]
		public void Construction()
		{
			var empty = new Spatial();
		}

		[Test]
		public void Usage()
		{
			var yard = Yard;
			var mailbox = yard.Descendants.FirstOrDefault(e => e.Name == "mailbox");
			Assert.NotNull(mailbox, nameof(mailbox));
			Assert.AreSame(yard, mailbox.Parent, "mailbox.Parent");
			var stake1 = yard.Descendants.FirstOrDefault(e => e.Name == "stake1");
			Assert.NotNull(stake1, nameof(stake1));

			var json = yard.Json;
			Assert.AreEqual(66, json.Length);
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
