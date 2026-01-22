using System.Threading.Tasks;
using Node.Net.JsonRPC;

namespace Node.Net.JsonRPC
{
    internal class RequestTest
    {
        [Test]
        public async Task StreamConstructor()
        {
            Request request = new Request("SayHello");
            request.Parameters.Add("name", "test");

            System.IO.Stream rs = request.ToStream();
            Request request1 = new Request(rs);

            object name = request1.Parameters["name"];
            await Assert.That(name).IsEqualTo("test");
        }
    }
}