namespace Node.Net.JsonRPC
{
	public interface IResponder
	{
		//string Respond(string request);
		Response Respond(Request request);
	}
}