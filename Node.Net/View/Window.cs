namespace Node.Net.View
{
    public class Window
    {
        public static void ShowDialog(System.Windows.FrameworkElement frameworkElement,string title)
        {
            var window = new System.Windows.Window
            {
                Content = frameworkElement,
                Title = title,
                WindowState = System.Windows.WindowState.Maximized
            };
            //window.Content = frameworkElement;
            //window.Title = title;
            window.ShowDialog();
        }
    }
}