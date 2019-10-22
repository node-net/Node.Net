namespace Node.Net.JsonRPC
{
	public interface IResponder
	{
		Response Respond(Request request);
	}
}