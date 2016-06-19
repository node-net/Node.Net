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
                    var current_parent = child.GetNearestAncestor<IModel3D>();
                    while(current_parent != null)
                    {
                        parentToWorld.Append(current_parent.LocalToParent);
                        var parent_as_child = current_parent as IChild;
                        current_parent = parent_as_child == null ? null : parent_as_child.GetNearestAncestor<IModel3D>();
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
        public static Vector3D TransformWorldToLocal(IModel3D model3D, Vector3D world)
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
    
        public static Vector3D GetWorldXDirectionVector(IModel3D model3D)
        {
            return TransformLocalToWorld(model3D, new Vector3D(1, 0, 0));
        }
        public static Vector3D GetWorldYDirectionVector(IModel3D model3D)
        {
            return TransformLocalToWorld(model3D, new Vector3D(0, 1, 0));
        }
        public static Vector3D GetWorldZDirectionVector(IModel3D model3D)
        {
            return TransformLocalToWorld(model3D, new Vector3D(0, 0, 1));
        }
        private static Vector3D[] GetWorldDirectionVectors(IModel3D model3D)
        {
            var directionVectors = new List<Vector3D>();
            directionVectors.Add(TransformLocalToWorld(model3D, new Vector3D(1, 0, 0)));
            directionVectors.Add(TransformLocalToWorld(model3D, new Vector3D(0, 1, 0)));
            directionVectors.Add(TransformLocalToWorld(model3D, new Vector3D(0, 0, 1)));
            return directionVectors.ToArray();
        }
        public static double GetWorldOrientation(IModel3D model3D)
        {
            return GetWorldOrientationF(model3D);
        }
        public static double GetWorldOrientationA(IModel3D model3D)
        {
            var worldXDirectionVector = TransformLocalToWorld(model3D, new Vector3D(1, 0, 0));
            // Orientation is rotation about the +Z axis (in the XY plane)
            //var worldDirectionVectors = GetWorldDirectionVectors(model3D);

            double orientation = 0;
            if (Abs(Round(worldXDirectionVector.Z, 4)) == 1)
            {
                // Edge case where X Axis is normal to XY plane, use Y Axis to compute orientation,
                // any spin will be combined into the orientation
                var worldYDirectionVector = TransformLocalToWorld(model3D, new Vector3D(0,1, 0));
                var localYAxisProjectedIntoWorldXY
                = ProjectPointToPlane(
                   new Point3D(
                       worldYDirectionVector.X,
                       worldYDirectionVector.Y,
                       worldYDirectionVector.Z),
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
                var localXAxisProjectedIntoWorldXY =
                new Vector3D(worldXDirectionVector.X, worldXDirectionVector.Y,0);

                var angleA = Vector3D.AngleBetween(new Vector3D(1, 0, 0), worldXDirectionVector);
                var angleB = Vector3D.AngleBetween(localXAxisProjectedIntoWorldXY, worldXDirectionVector);
                orientation = Vector3D.AngleBetween(
                       new Vector3D(1, 0, 0), localXAxisProjectedIntoWorldXY);
                if (worldXDirectionVector.Y < 0) orientation *= -1;
            }

            return orientation;
        }
        public static double GetWorldOrientationB(IModel3D model3D)
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
                var localXAxisProjectedIntoWorldXY =
                    ProjectPointToPlane(
                        new Point3D(
                            worldDirectionVectors[0].X,
                            worldDirectionVectors[0].Y,
                            worldDirectionVectors[0].Z),
                        new Point3D(0,0,0),
                        new Vector3D(0,0,1)
                        );

                //new Point3D(worldDirectionVectors[0].X, worldDirectionVectors[0].Y, 0);
                orientation = Vector3D.AngleBetween(
                       new Vector3D(1, 0, 0),
                       new Vector3D(
                           localXAxisProjectedIntoWorldXY.X,
                           localXAxisProjectedIntoWorldXY.Y,
                           localXAxisProjectedIntoWorldXY.Z)
                       );
                if (worldDirectionVectors[0].Y < 0) orientation *= -1;
            }

            return orientation;
        }

        public static double GetWorldOrientationC(IModel3D model3D)
        {
            var orientation = 0.0;
            var worldXDirectionVector = TransformLocalToWorld(model3D, new Vector3D(1, 0, 0));

            // Back the tilt out
            var tilt = GetWorldTilt(model3D);

            // Back out the tilt component
            var adjust = new Model.SpatialElement
            {
                YAxisRotation = $"{-tilt} deg"
            };
            worldXDirectionVector = adjust.TransformLocalToParent(worldXDirectionVector);
            orientation = Vector3D.AngleBetween(new Vector3D(1, 0, 0), worldXDirectionVector);
            if (worldXDirectionVector.Y < 0) orientation *= -1;


            return orientation;
        }
        public static double GetWorldOrientationD(IModel3D model3D)
        {
            var orientation = 0.0;
            var worldYDirectionVector = TransformLocalToWorld(model3D, new Vector3D(0, 1, 0));

            // Back the tilt out
            var tilt = GetWorldTilt(model3D);

            // Back out the tilt component
            var adjust = new Model.SpatialElement
            {
                YAxisRotation = $"{-tilt} deg"
            };
            worldYDirectionVector = adjust.TransformLocalToParent(worldYDirectionVector);
            orientation = Vector3D.AngleBetween(new Vector3D(0, 1, 0), worldYDirectionVector);
            if (worldYDirectionVector.X > 0) orientation *= -1;
            

            return orientation;
        }
        public static double GetWorldOrientationE(IModel3D model3D)
        {
            var orientation = 0.0;
            var worldXDirectionVector = TransformLocalToWorld(model3D, new Vector3D(1, 0, 0));

            orientation = Vector3D.AngleBetween(new Vector3D(1, 0, 0), worldXDirectionVector);
            if (worldXDirectionVector.Y < 0) orientation *= -1;


            return orientation;
        }
        public static double GetWorldOrientationF(IModel3D model3D)
        {
            // By using the Y directionVector, tilt does not effect the result
            var orientation = 0.0;
            var worldYDirectionVector = TransformLocalToWorld(model3D, new Vector3D(0, 1, 0));

            orientation = Vector3D.AngleBetween(new Vector3D(0, 1, 0), worldYDirectionVector);
            if (worldYDirectionVector.X > 0) orientation *= -1;


            return orientation;
        }
        public static double GetWorldTilt(IModel3D model3D)
        {
            return GetWorldTiltC(model3D);
        }
        public static double GetWorldTiltA(IModel3D model3D)
        {
            // Tilt is rotation about the +Y axis (in the ZX plane)
            var worldDirectionVectors = GetWorldDirectionVectors(model3D);

            /*
            // Backout world orientation
            var worldOrientation = GetWorldOrientation(model3D);
            var adjust = new Model.SpatialElement
            {
                ZAxisRotation = $"{-worldOrientation} deg"
            };
            worldDirectionVectors = adjust.TransformLocalToParent(worldDirectionVectors);
            */
            

            var localXAxisProjectedIntoWorldXZ = 
                new Point3D(worldDirectionVectors[0].X, 0, worldDirectionVectors[0].Z);
            var tilt = Vector3D.AngleBetween(
             new Vector3D(1, 0, 0),
             new Vector3D(
                 localXAxisProjectedIntoWorldXZ.X,
                 localXAxisProjectedIntoWorldXZ.Y,
                 localXAxisProjectedIntoWorldXZ.Z)
             );

            if (worldDirectionVectors[0].Z > 0) tilt *= -1;
            return tilt;
        }
        public static double GetWorldTiltB(IModel3D model3D)
        {
            var tilt = 0.0;
            var worldZDirectionVector = TransformLocalToWorld(model3D, new Vector3D(0, 0, 1));
            tilt = Vector3D.AngleBetween(
                new Vector3D(0, 0, 1), 
                new Vector3D(worldZDirectionVector.X,0,worldZDirectionVector.Z));
            if (worldZDirectionVector.X < 0) tilt *= -1;
            return tilt;
        }
        public static double GetWorldTiltC(IModel3D model3D)
        {
            var tilt = 0.0;
            var worldZDirectionVector = TransformLocalToWorld(model3D, new Vector3D(0, 0, 1));

            // Backout world orientation
            var worldOrientation = GetWorldOrientation(model3D);
            var adjust = new Model.SpatialElement
            {
                ZAxisRotation = $"{-worldOrientation} deg"
            };
            worldZDirectionVector = adjust.TransformLocalToParent(worldZDirectionVector);

            tilt = Vector3D.AngleBetween(
                new Vector3D(0, 0, 1),
                worldZDirectionVector);
            if (worldOrientation > 90)
            {
                if (worldZDirectionVector.Y > 0) tilt *= -1;
            }
            else if(worldOrientation < -90)
            {
                if (worldZDirectionVector.Y < 0) tilt *= -1;
            }
            else
            {
                if (worldZDirectionVector.X < 0) tilt *= -1;
            }
            return tilt;
        }
        public static double GetWorldSpin(IModel3D model3D)
        {
            return 0.0;
            //return GetWorldSpinB(model3D);
        }
        public static double GetWorldSpinA(IModel3D model3D)
        {
            // Spin is rotation about the +X axis (in the YZ plane)
            var worldYDirectionVector = TransformLocalToWorld(model3D, new Vector3D(0, 1, 0));

            // Back out orientation
            var worldOrientation = 0.0;
            var worldXDirectionVector = TransformLocalToWorld(model3D, new Vector3D(1, 0, 0));
            worldOrientation = Vector3D.AngleBetween(new Vector3D(1, 0, 0), worldXDirectionVector);
            if (worldXDirectionVector.Y < 0) worldOrientation *= -1;


            var adjust = new Model.SpatialElement
            {
                ZAxisRotation = $"{-worldOrientation} deg"
            };
            worldYDirectionVector = adjust.TransformLocalToParent(worldYDirectionVector);

            // Back out tilt
            var worldTilt = 0.0;
            worldXDirectionVector = TransformLocalToWorld(model3D, new Vector3D(1, 0, 0));
            worldTilt = Vector3D.AngleBetween(
                new Vector3D(1, 0, 0),
                new Vector3D(worldXDirectionVector.X, 0, worldXDirectionVector.Z));
            if (worldXDirectionVector.Z > 0) worldTilt *= -1;
            adjust.ZAxisRotation = "0 deg";
            adjust.YAxisRotation = $"{-worldTilt} deg";
            worldYDirectionVector = adjust.TransformLocalToParent(worldYDirectionVector);


            var spin = Vector3D.AngleBetween(
                new Vector3D(0, 1, 0),
                worldYDirectionVector
            );
            if (worldYDirectionVector.Z < 0) spin *= -1;
            return spin;
        }
        public static double GetWorldSpinB(IModel3D model3D)
        {
            // Spin is rotation about the +X axis (in the YZ plane)
            var worldYDirectionVector = TransformLocalToWorld(model3D, new Vector3D(0, 1, 0));
            /*
            var adjust = new Model.SpatialElement
            {
                ZAxisRotation = $"{-GetWorldOrientation(model3D)} deg",
                YAxisRotation = $"{-GetWorldTilt(model3D)} deg"
            };
            worldYDirectionVector = adjust.TransformLocalToParent(worldYDirectionVector);
            */

            var spin = Vector3D.AngleBetween(
                new Vector3D(0, 1, 0),
                new Vector3D(0,worldYDirectionVector.Y,worldYDirectionVector.Z)
            );
            if (worldYDirectionVector.Z < 0) spin *= -1;
            return spin;
        }
    }
}
