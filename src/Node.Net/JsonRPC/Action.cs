namespace Node.Net.JsonRPC
{
	public sealed class Action : IResponder
	{
		public Action(System.Action action)
		{
			_action = action;
		}

		public Response Respond(Request request)
		{
			_action();
			return new Response(request.Id,null);
		}

		private readonly System.Action _action;
	}

	public sealed class Action<T> : IResponder
	{
		public Action(System.Action<T> action)
		{
			_action = action;
		}

		public Response Respond(Request request)
		{
			_action.Invoke(request.GetParameterArray());
			return new Response(request.Id,null);
		}

		private readonly System.Action<T> _action;
	}

	public sealed class Action<T1, T2> : IResponder
	{
		public Action(System.Action<T1, T2> action)
		{
			_action = action;
		}

		public Response Respond(Request request)
		{
			_action.Invoke(request.GetParameterArray());
			return new Response(request.Id,null);
		}

		private readonly System.Action<T1, T2> _action;
	}

	public sealed class Action<T1, T2, T3> : IResponder
	{
		public Action(System.Action<T1, T2, T3> action)
		{
			_action = action;
		}

		public Response Respond(Request request)
		{
			var parameters = request.GetParameterArray();
			_action((T1)parameters[0], (T2)parameters[1], (T3)parameters[2]);
			return new Response(request.Id,null);
		}

		private readonly System.Action<T1, T2, T3> _action;
	}

	public sealed class Action<T1, T2, T3, T4> : IResponder
	{
		public Action(System.Action<T1, T2, T3, T4> action)
		{
			_action = action;
		}

		public Response Respond(Request request)
		{
			var parameters = request.GetParameterArray();
			_action((T1)parameters[0], (T2)parameters[1], (T3)parameters[2], (T4)parameters[3]);
			return new Response(request.Id,null);
		}

		private readonly System.Action<T1, T2, T3, T4> _action;
	}

	public sealed class Action<T1, T2, T3, T4, T5> : IResponder
	{
		public Action(System.Action<T1, T2, T3, T4, T5> action)
		{
			_action = action;
		}

		public Response Respond(Request request)
		{
			var parameters = request.GetParameterArray();
			_action((T1)parameters[0], (T2)parameters[1], (T3)parameters[2], (T4)parameters[3], (T5)parameters[4]);
			return new Response(request.Id,null);
		}

		private readonly System.Action<T1, T2, T3, T4, T5> _action;
	}

	public sealed class Action<T1, T2, T3, T4, T5, T6> : IResponder
	{
		public Action(System.Action<T1, T2, T3, T4, T5, T6> action)
		{
			_action = action;
		}

		public Response Respond(Request request)
		{
			var parameters = request.GetParameterArray();
			_action((T1)parameters[0], (T2)parameters[1], (T3)parameters[2], (T4)parameters[3], (T5)parameters[4], (T6)parameters[4]);
			return new Response(request.Id,null);
		}

		private readonly System.Action<T1, T2, T3, T4, T5, T6> _action;
	}
}