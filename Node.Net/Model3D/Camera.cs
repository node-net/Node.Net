namespace Node.Net.Model3D
{
    public class Camera
    {
        public static System.Windows.Media.Media3D.Camera GetPerspectiveCamera(System.Windows.Media.Media3D.ModelVisual3D value)
        {
            System.Windows.Media.Media3D.PerspectiveCamera camera = new System.Windows.Media.Media3D.PerspectiveCamera();
            System.Windows.Media.Media3D.Rect3D bounds = value.Content.Bounds;
            System.Windows.Media.Media3D.Point3D center
                = new System.Windows.Media.Media3D.Point3D(
                     bounds.X + bounds.SizeX / 2,
                     bounds.Y + bounds.SizeY / 2,
                     bounds.Z + bounds.SizeZ / 2);
            double maxDim = bounds.SizeX;
            if (bounds.SizeY > maxDim) maxDim = bounds.SizeY;
            if (bounds.SizeZ > maxDim) maxDim = bounds.SizeZ;
            maxDim = maxDim / 2;
            camera.LookDirection
                = new System.Windows.Media.Media3D.Vector3D(
                    bounds.SizeX,
                    bounds.SizeY,
                    -bounds.SizeZ);
            camera.Position = new System.Windows.Media.Media3D.Point3D(
                center.X - camera.LookDirection.X * maxDim,
                center.Y - camera.LookDirection.Y * maxDim,
                center.Z - camera.LookDirection.Z * maxDim);
            camera.UpDirection = new System.Windows.Media.Media3D.Vector3D(0, 0, 1);
            camera.NearPlaneDistance = 1;
            camera.FarPlaneDistance = 216;
            camera.FieldOfView = 45;
            return camera;
        }

        public static System.Windows.Media.Media3D.Camera GetCamera(System.Collections.IDictionary value)
        {
            if (value.Contains("Camera"))
            {
                System.Collections.IDictionary cameraDictionary = value["Camera"] as System.Collections.IDictionary;
                if (!object.ReferenceEquals(null, cameraDictionary))
                {
                    if (cameraDictionary.Contains("Type"))
                    {
                        //if(cameraDictionary["Type"].ToString() == "OrthographicCamera")
                    }
                    System.Windows.Media.Media3D.PerspectiveCamera perspectiveCamera
                        = new System.Windows.Media.Media3D.PerspectiveCamera();
                    if (cameraDictionary.Contains("Position"))
                    {

                    }
                }
            }
            return null;
        }
    }
}
