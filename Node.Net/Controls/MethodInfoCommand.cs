
namespace Node.Net.Controls
{
    public class MethodInfoCommand : System.Windows.Input.ICommand
    {
        private readonly object instance;
        private readonly System.Reflection.MethodInfo methodInfo;

        private MethodInfoCommand() { }
        public MethodInfoCommand(object instanceValue, System.Reflection.MethodInfo methodInfoValue)
        {
            instance = instanceValue;
            methodInfo = methodInfoValue;
        }

        public bool CanExecute(System.Object parameter) => true;

        public void Execute(System.Object parameter)
        {
            MethodInfoCommand.Default.OnPreMethodInvoke();
            methodInfo.Invoke(instance, null);
            MethodInfoCommand.Default.OnPostMethodInvoke();
        }


        public event System.EventHandler CanExecuteChanged;

        private readonly static MethodInfoCommand _default = new MethodInfoCommand();
        public static MethodInfoCommand Default => _default;
        public event System.EventHandler PreMethodInvoke;
        public void OnPreMethodInvoke()
        {
            if(PreMethodInvoke != null)
            {
                PreMethodInvoke(this, new System.EventArgs());
            }
        }
        public event System.EventHandler PostMethodInvoke;
        public void OnPostMethodInvoke()
        {
            if (PostMethodInvoke != null)
            {
                PostMethodInvoke(this, new System.EventArgs());
            }
        }
    }
}
