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
			return new Response(_function(), request.Id);
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
			return new Response(_function((T1)parameters[0]), request.Id);
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
			return new Response(_function((T1)parameters[0], (T2)parameters[1]), request.Id);
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
			return new Response(_function((T1)parameters[0], (T2)parameters[1], (T3)parameters[2]), request.Id);
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
			return new Response(_function((T1)parameters[0], (T2)parameters[1], (T3)parameters[2], (T4)parameters[3]), request.Id);
		}

		private readonly System.Func<T1, T2, T3, T4, T5> _function;
	}
}