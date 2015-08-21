namespace Node.Net.View
{
    public class MethodInfoCommand : System.Windows.Input.ICommand
    {
        private object instance;
        private System.Reflection.MethodInfo methodInfo;

        private MethodInfoCommand() { }
        public MethodInfoCommand(object instanceValue, System.Reflection.MethodInfo methodInfoValue)
        {
            instance = instanceValue;
            methodInfo = methodInfoValue;
        }

        public bool CanExecute(System.Object parameter)
        {
            return true;
        }

        public void Execute(System.Object parameter)
        {
            MethodInfoCommand.Default.OnPreMethodInvoke();
            methodInfo.Invoke(instance, null);
            MethodInfoCommand.Default.OnPostMethodInvoke();
        }

        
        public event System.EventHandler CanExecuteChanged;

        private static MethodInfoCommand _default = new MethodInfoCommand();
        public static MethodInfoCommand Default { get { return _default; } }
        private bool methodExecuting = false;
        public event System.EventHandler PreMethodInvoke;
        public void OnPreMethodInvoke()
        {
            methodExecuting = true;
            if (!object.ReferenceEquals(null, PreMethodInvoke))
            {
                PreMethodInvoke(this, new System.EventArgs());
            }
        }
        public event System.EventHandler PostMethodInvoke;
        public void OnPostMethodInvoke()
        {
            if (!object.ReferenceEquals(null, PostMethodInvoke))
            {
                PostMethodInvoke(this, new System.EventArgs());
            }
            methodExecuting = false;
        }
    }
}
