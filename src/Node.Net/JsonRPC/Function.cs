namespace Node.Net.JsonRPC
{
	public sealed class Function<T> : IResponder
	{
		public Function(System.Func<T> function)
		{
			_function = function;
		}

		public Response Respond(Request request)
		{
			return new Response(request.Id,_function() );
		}

		private readonly System.Func<T> _function;
	}

	public sealed class Function<T1, T2> : IResponder
	{
		public Function(System.Func<T1, T2> function)
		{
			_function = function;
		}

		public Response Respond(Request request)
		{
			var parameters = request.GetParameterArray();
			return new Response(request.Id,_function((T1)parameters[0]));
		}

		private readonly System.Func<T1, T2> _function;
	}

	public sealed class Function<T1, T2, T3> : IResponder
	{
		public Function(System.Func<T1, T2, T3> function)
		{
			_function = function;
		}

		public Response Respond(Request request)
		{
			var parameters = request.GetParameterArray();
			return new Response(request.Id,_function((T1)parameters[0], (T2)parameters[1]));
		}

		private readonly System.Func<T1, T2, T3> _function;
	}

	public sealed class Function<T1, T2, T3, T4> : IResponder
	{
		public Function(System.Func<T1, T2, T3, T4> function)
		{
			_function = function;
		}

		public Response Respond(Request request)
		{
			var parameters = request.GetParameterArray();
			return new Response(request.Id,_function((T1)parameters[0], (T2)parameters[1], (T3)parameters[2]));
		}

		private readonly System.Func<T1, T2, T3, T4> _function;
	}

	public sealed class Function<T1, T2, T3, T4, T5> : IResponder
	{
		public Function(System.Func<T1, T2, T3, T4, T5> function)
		{
			_function = function;
		}

		public Response Respond(Request request)
		{
			var parameters = request.GetParameterArray();
			return new Response(request.Id,_function((T1)parameters[0], (T2)parameters[1], (T3)parameters[2], (T4)parameters[3]));
		}

		private readonly System.Func<T1, T2, T3, T4, T5> _function;
	}
}