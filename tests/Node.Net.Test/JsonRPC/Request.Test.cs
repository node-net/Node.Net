extern alias NodeNet;
using NUnit.Framework;
using NodeNet::Node.Net.JsonRPC;

namespace Node.Net.JsonRPC
{
    [TestFixture, Category(nameof(Request))]
    internal static class RequestTest
    {
        [Test]
        public static void StreamConstructor()
        {
            Request request = new Request("SayHello");
            request.Parameters.Add("name", "test");

            System.IO.Stream rs = request.ToStream();
            Request request1 = new Request(rs);

            object name = request1.Parameters["name"];
            Assert.That(name, Is.EqualTo("test"));
        }
    }
}