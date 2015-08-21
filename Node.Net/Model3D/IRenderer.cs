namespace Node.Net.Model3D
{
    public delegate System.Windows.Media.Media3D.Model3D Model3DRequestedEventHandler(object value, Node.Net.Measurement.LengthUnit units);

    public interface IRenderer
    {
        System.Windows.Controls.Viewport3D GetViewport3D(object value);
        System.Windows.Media.Media3D.Model3D GetModel3D(object value);
        System.Windows.ResourceDictionary Resources { get; }
        event Model3DRequestedEventHandler Model3DRequested;
    }
}
