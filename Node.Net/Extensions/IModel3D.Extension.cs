using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using static System.Math;

namespace Node.Net.Extensions
{
    public static class IModel3DExtension
    {
        public static Matrix3D GetParentToWorld(IModel3D model3D)
        {
            if (model3D != null)
            {
                var child = model3D as IChild;
                if (child != null)
                {
                    var parentToWorld = new Matrix3D();
                    var current_parent = child.GetFirstAncestor<IModel3D>();
                    while(current_parent != null)
                    {
                        parentToWorld.Append(current_parent.LocalToParent);
                        var parent_as_child = current_parent as IChild;
                        current_parent = parent_as_child == null ? null : parent_as_child.GetFirstAncestor<IModel3D>();
                    }
                    return parentToWorld;
                }

            }
            return new Matrix3D();
        }

        public static Matrix3D GetLocalToWorld(IModel3D model3D)
        {
            if (model3D != null)
            {
                var localToWorld = Matrix3D.Multiply(model3D.LocalToParent, new Matrix3D());
                localToWorld.Append(GetParentToWorld(model3D));
                return localToWorld;
            }
            return new Matrix3D();
        }
        public static Matrix3D GetWorldToLocal(IModel3D model3D)
        {
            var worldToLocal = System.Windows.Media.Media3D.Matrix3D.Multiply(model3D.GetLocalToWorld(), new Matrix3D());
            worldToLocal.Invert();
            return worldToLocal;
        }

        public static Point3D ApplyTransform(Point3D source, Matrix3D trans)
        {
            var point4D =
                new Point4D(source.X, source.Y, source.Z, 1.0);
            point4D = trans.Transform(point4D);
            return new Point3D(point4D.X, point4D.Y, point4D.Z);
        }
        public static Vector3D ApplyTransform(Vector3D source, Matrix3D trans)
        {
            var mv = new Vector3D(source.X, source.Y, source.Z);
            mv = trans.Transform(mv);
            var result = new Vector3D(mv.X, mv.Y, mv.Z);
            return result;
        }

        public static Point3D TransformLocalToWorld(IModel3D model3D,Point3D local)
        {
            return ApplyTransform(local, GetLocalToWorld(model3D));
        }
        public static Point3D TransformLocalToParent(IModel3D model3D,Point3D local)
        {
            return ApplyTransform(local, model3D.LocalToParent);
        }
        public static Vector3D TransformLocalToParent(IModel3D model3D, Vector3D local)
        {
            return ApplyTransform(local, model3D.LocalToParent);
        }
        public static Vector3D[] TransformLocalToParent(IModel3D model3D,Vector3D[] localVectors)
        {
            var results = new List<Vector3D>();
            foreach(var local in localVectors)
            {
                results.Add(TransformLocalToParent(model3D, local));
            }
            return results.ToArray();
        }
        public static Point3D TransformWorldToLocal(IModel3D model3D,Point3D world)
        {
            return ApplyTransform(world, GetWorldToLocal(model3D));
        }
        public static Vector3D TransformLocalToWorld(IModel3D model3D, Vector3D local)
        {
            return ApplyTransform(local, GetLocalToWorld(model3D));
        }
        public static Point3D GetWorldOrigin(IModel3D model3D)
        {
            return ApplyTransform(new Point3D(0, 0, 0), GetLocalToWorld(model3D));
        }
        public static Point3D GetWorldRotations(IModel3D model3D)
        {
            return new Point3D(GetWorldSpin(model3D),GetWorldTilt(model3D),GetWorldOrientation(model3D));
        }
        private static Point3D ProjectPointToPlane(Point3D point,Point3D pointOnPlane,Vector3D planeNormal) 
            => point - Vector3D.DotProduct(point - pointOnPlane, planeNormal) * planeNormal;
    
        private static Vector3D[] GetWorldDirectionVectors(IModel3D model3D)
        {
            var directionVectors = new List<Vector3D>();
            directionVectors.Add(TransformLocalToWorld(model3D, new Vector3D(1, 0, 0)));
            directionVectors.Add(TransformLocalToWorld(model3D, new Vector3D(0, 1, 0)));
            directionVectors.Add(TransformLocalToWorld(model3D, new Vector3D(0, 0, 1)));
            return directionVectors.ToArray();
        }
        private static double GetWorldOrientation(IModel3D model3D)
        {
            // Orientation is rotation about the +Z axis (in the XY plane)
            var worldDirectionVectors = GetWorldDirectionVectors(model3D);

            double orientation = 0;
            if (Abs(Round(worldDirectionVectors[0].Z, 4)) == 1)
            {
                // Edge case where X Axis is normal to XY plane, use Y Axis to compute orientation,
                // any spin will be combined into the orientation
                var localYAxisProjectedIntoWorldXY
                = ProjectPointToPlane(
                   new Point3D(
                       worldDirectionVectors[1].X,
                       worldDirectionVectors[1].Y,
                       worldDirectionVectors[1].Z),
                   new Point3D(0, 0, 0),
                   new Vector3D(0, 0, 1));
                orientation = Vector3D.AngleBetween(
                    new Vector3D(0, 1, 0),
                    new Vector3D(
                        localYAxisProjectedIntoWorldXY.X,
                        localYAxisProjectedIntoWorldXY.Y,
                        localYAxisProjectedIntoWorldXY.Z)
                    );
                if (localYAxisProjectedIntoWorldXY.X > 0) orientation *= -1;
            }
            else
            {
                var localXAxisProjectedIntoWorldXY
                    = ProjectPointToPlane(
                       new Point3D(
                           worldDirectionVectors[0].X,
                           worldDirectionVectors[0].Y,
                           worldDirectionVectors[0].Z),
                       new Point3D(0, 0, 0),
                       new Vector3D(0, 0, 1));
                if (worldDirectionVectors[2].Z < 0)
                {
                    orientation = -Vector3D.AngleBetween(
                        new Vector3D(-1, 0, 0),
                        new Vector3D(
                            localXAxisProjectedIntoWorldXY.X,
                            localXAxisProjectedIntoWorldXY.Y,
                            localXAxisProjectedIntoWorldXY.Z)
                        );
                    if (localXAxisProjectedIntoWorldXY.Y < 0) orientation *= -1;
                }
                else
                {
                    orientation = Vector3D.AngleBetween(
                        new Vector3D(1, 0, 0),
                        new Vector3D(
                            localXAxisProjectedIntoWorldXY.X,
                            localXAxisProjectedIntoWorldXY.Y,
                            localXAxisProjectedIntoWorldXY.Z)
                        );
                    if (worldDirectionVectors[0].Y < 0) orientation *= -1;
                }

            }


            return orientation;
        }
        private static double GetWorldTilt(IModel3D model3D)
        {
            // Tilt is rotation about the +Y axis (in the ZX plane)
            var worldDirectionVectors = GetWorldDirectionVectors(model3D);

            var localZAxisProjectedIntoWorldZX
                = ProjectPointToPlane(
                   new Point3D(
                       worldDirectionVectors[2].X,
                       worldDirectionVectors[2].Y,
                       worldDirectionVectors[2].Z),
                   new Point3D(0, 0, 0),
                   worldDirectionVectors[1]);
            var tilt = Vector3D.AngleBetween(
                new Vector3D(0, 0, 1),
                new Vector3D(
                    localZAxisProjectedIntoWorldZX.X,
                    localZAxisProjectedIntoWorldZX.Y,
                    localZAxisProjectedIntoWorldZX.Z)
                );

            if (worldDirectionVectors[0].Z > 0) tilt *= -1;
            return tilt;
        }
        private static double GetWorldSpin(IModel3D model3D)
        {
            var worldOrientation = GetWorldOrientation(model3D);
            var worldTilt = GetWorldTilt(model3D);

            // Spin is rotation about the +X axis (in the YZ plane)
            var worldDirectionVectors = GetWorldDirectionVectors(model3D);

            // Back out the tilt component
            //var swo = $"{-worldOrientation} deg";
            var adjust = new Model.SpatialElement
            {
                ZAxisRotation = $"{-worldOrientation} deg"
            };
            worldDirectionVectors = adjust.TransformLocalToParent(worldDirectionVectors);
            adjust.ZAxisRotation = "0 deg";
            adjust.YAxisRotation = $"{-worldTilt} deg";
            worldDirectionVectors = adjust.TransformLocalToParent(worldDirectionVectors);
            /*
            var adjust = new Transform3D
            {
                RotationOTS = new System.Windows.Media.Media3D.Point3D(-worldOrientation, 0, 0)
            };
            */
            /*
            worldDirectionVectors = adjust.Transform(worldDirectionVectors, TransformType.LocalToParent);
            adjust.RotationOTS = new System.Windows.Media.Media3D.Point3D(0, -worldTilt, 0);
            worldDirectionVectors = adjust.Transform(worldDirectionVectors, TransformType.LocalToParent);
            */

            var localYAxisProjectedIntoWorldZY
                = ProjectPointToPlane(
                   new Point3D(
                       worldDirectionVectors[1].X,
                       worldDirectionVectors[1].Y,
                       worldDirectionVectors[1].Z),
                   new Point3D(0, 0, 0),
                   new Vector3D(1, 0, 0));
            var spin = Vector3D.AngleBetween(
                new Vector3D(0, 1, 0),
                new Vector3D(
                    localYAxisProjectedIntoWorldZY.X,
                    localYAxisProjectedIntoWorldZY.Y,
                    localYAxisProjectedIntoWorldZY.Z)
                );

            var yzCross
                = Vector3D.CrossProduct(
                worldDirectionVectors[1], worldDirectionVectors[2]);

            if (worldDirectionVectors[1].Z < 0) spin *= -1;
            return spin;
        }
        /*
        private static double GetRotationDegrees(IDictionary dictionary, string key)
        {
            if (object.ReferenceEquals(null, dictionary)) return 0;
            if (!dictionary.Contains(key)) return 0;
            if (object.ReferenceEquals(null, dictionary[key])) return 0;
            return Measurement.Angle.Parse(dictionary[key].ToString())[Measurement.AngularUnit.Degrees];
        }*/
        /*
        public static Quaternion GetQuaternionRotation(IDictionary value)
        {
            if (value == null) return new Quaternion();
            var rotationZ = new Quaternion();
            var rotationY = new Quaternion();
            var rotationX = new Quaternion();
            if (value.Contains("RotationZ"))
            {
                var rotationZ_degrees = GetRotationDegrees(value, "RotationZ");
                rotationZ = new Quaternion(new Vector3D(0, 0, 1), rotationZ_degrees);
            }
            if (value.Contains("Orientation"))
            {
                var rotationZ_degrees = GetRotationDegrees(value, "Orientation");
                rotationZ = new Quaternion(new Vector3D(0, 0, 1), rotationZ_degrees);
            }
            if (value.Contains("RotationY"))
            {
                var rotationY_degrees = GetRotationDegrees(value, "RotationY");
                rotationY = new Quaternion(new Vector3D(0, 1, 0), rotationY_degrees);
            }
            if (value.Contains("Tilt"))
            {
                var rotationY_degrees = GetRotationDegrees(value, "Tilt");
                rotationY = new Quaternion(new Vector3D(0, 1, 0), rotationY_degrees);
            }
            if (value.Contains("RotationX"))
            {
                var rotationX_degrees = GetRotationDegrees(value, "RotationX");
                rotationX = new Quaternion(new Vector3D(1, 0, 0), rotationX_degrees);
            }
            if (value.Contains("Spin"))
            {
                var rotationX_degrees = GetRotationDegrees(value, "Spin");
                rotationX = new Quaternion(new Vector3D(1, 0, 0), rotationX_degrees);
            }

            var total_rotation = Quaternion.Multiply(rotationX, Quaternion.Multiply(rotationY, rotationZ));
            return total_rotation;
        }*/
    }
}
