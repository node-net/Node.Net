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
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            return new Response(request.Id, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }

        private readonly System.Action _action;
    }

    public sealed class Action<T> : IResponder
    {
        public Action(System.Action<T> action)
        {
            _action = action;
        }

        public Action(System.Action<T> action, string a)
        {
            _action = action;
            _parameter_names = new string[] { a };
        }

        public Response Respond(Request request)
        {
            _action.Invoke(request.GetParameterArray<T>(_parameter_names));
            return new Response(request.Id, null);
        }

        private readonly System.Action<T> _action;
        private readonly string[] _parameter_names = null;
    }

    public sealed class Action<T1, T2> : IResponder
    {
        public Action(System.Action<T1, T2> action)
        {
            _action = action;
        }

        public Action(System.Action<T1, T2> action, string a, string b)
        {
            _action = action;
            _parameter_names = new string[] { a, b };
        }

        public Response Respond(Request request)
        {
            _action.Invoke(request.GetParameterArray<T1, T2>(_parameter_names));
            return new Response(request.Id, null);
        }

        private readonly System.Action<T1, T2> _action;
        private readonly string[] _parameter_names = null;
    }

    public sealed class Action<T1, T2, T3> : IResponder
    {
        public Action(System.Action<T1, T2, T3> action)
        {
            _action = action;
        }

        public Action(System.Action<T1, T2, T3> action, string a, string b, string c)
        {
            _action = action;
            _parameter_names = new string[] { a, b, c };
        }

        public Response Respond(Request request)
        {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            object[]? parameters = request.GetParameterArray<T1, T2, T3>(_parameter_names);
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
            _action((T1)parameters[0], (T2)parameters[1], (T3)parameters[2]);
            return new Response(request.Id, null);
        }

        private readonly System.Action<T1, T2, T3> _action;
        private readonly string[] _parameter_names = null;
    }

    public sealed class Action<T1, T2, T3, T4> : IResponder
    {
        public Action(System.Action<T1, T2, T3, T4> action)
        {
            _action = action;
        }

        public Action(System.Action<T1, T2, T3, T4> action, string a, string b, string c, string d)
        {
            _action = action;
            _parameter_names = new string[] { a, b, c, d };
        }

        public Response Respond(Request request)
        {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            object[]? parameters = request.GetParameterArray<T1, T2, T3, T4>(_parameter_names);
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
            _action((T1)parameters[0], (T2)parameters[1], (T3)parameters[2], (T4)parameters[3]);
            return new Response(request.Id, null);
        }

        private readonly System.Action<T1, T2, T3, T4> _action;
        private readonly string[] _parameter_names = null;
    }

    public sealed class Action<T1, T2, T3, T4, T5> : IResponder
    {
        public Action(System.Action<T1, T2, T3, T4, T5> action)
        {
            _action = action;
        }

        public Action(System.Action<T1, T2, T3, T4, T5> action, string a, string b, string c, string d, string e)
        {
            _action = action;
            _parameter_names = new string[] { a, b, c, d, e };
        }

        public Response Respond(Request request)
        {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            object[]? parameters = request.GetParameterArray<T1, T2, T3, T4, T5>(_parameter_names);
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
            _action((T1)parameters[0], (T2)parameters[1], (T3)parameters[2], (T4)parameters[3], (T5)parameters[4]);
            return new Response(request.Id, null);
        }

        private readonly System.Action<T1, T2, T3, T4, T5> _action;
        private readonly string[] _parameter_names = null;
    }

    public sealed class Action<T1, T2, T3, T4, T5, T6> : IResponder
    {
        public Action(System.Action<T1, T2, T3, T4, T5, T6> action)
        {
            _action = action;
        }

        public Action(System.Action<T1, T2, T3, T4, T5, T6> action, string a, string b, string c, string d, string e, string f)
        {
            _action = action;
            _parameter_names = new string[] { a, b, c, d, e, f };
        }

        public Response Respond(Request request)
        {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            object[]? parameters = request.GetParameterArray<T1, T2, T3, T4, T5, T6>(_parameter_names);
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
            _action((T1)parameters[0], (T2)parameters[1], (T3)parameters[2], (T4)parameters[3], (T5)parameters[4], (T6)parameters[4]);
            return new Response(request.Id, null);
        }

        private readonly System.Action<T1, T2, T3, T4, T5, T6> _action;
        private readonly string[] _parameter_names = null;
    }
}