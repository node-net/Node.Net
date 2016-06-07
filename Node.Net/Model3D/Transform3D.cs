﻿namespace Node.Net.Model3D
{
    public class Transform3D
    {
        public Transform3D() { }
        /*
        public Transform3D(System.Collections.IDictionary dictionary)
        {
            Rendering.MatrixTransform3DRenderer r = new Rendering.MatrixTransform3DRenderer(null);
            Transform3D tmp = r.GetTransform3D_NoScale(dictionary);
            Translation = tmp.Translation;
            RotationOTS = tmp.RotationOTS;
        }*/
        public enum TransformType { LocalToParent, ParentToLocal, LocalToWorld, WorldToLocal, ParentToWorld, WorldToParent };
        private Transform3D parent = null;
        public Transform3D Parent
        {
            get { return parent; }
            set 
            {
                if (!object.ReferenceEquals(parent, value))
                {
                    parent = value;
                    Flush();
                }
            }
        }

        public void Flush()
        {
            localToParentCacheDirty = true;
            localToWorldCacheDirty = true;
            worldToLocalCacheDirty = true;
        }

        private System.Windows.Media.Media3D.Point3D translation = new System.Windows.Media.Media3D.Point3D();
        public System.Windows.Media.Media3D.Point3D Translation
        {
            get { return translation; }
            set
            {
                translation = value;
                Flush();
            }
        }

        private System.Windows.Media.Media3D.Point3D scale = new System.Windows.Media.Media3D.Point3D(1,1,1);
        public System.Windows.Media.Media3D.Point3D Scale
        {
            get { return scale; }
            set { scale = value; Flush(); }
        }
        private System.Windows.Media.Media3D.Point3D rotationOTS = new System.Windows.Media.Media3D.Point3D();
        /// <summary>
        /// RotationsOTS are defined with the Right Hand Rule
        /// 
        /// Orientation is rotation about positive Z axis.
        /// Orientation of zero is aligned with the positive X axis.
        /// Orientation of 90 is aligned with the positive Y axis.
        /// 
        /// Tilt is rotation about the positive Y axis.
        /// Tilt of zero is aligned with the positive Z axis
        /// Tilt of 90 is aligned with the positive X axis.
        /// 
        /// Spin is rotation about eh positive X axis.
        /// Spin of zero is aligned with the positive Y axis.
        /// Spin of 90 is aligned with the positive Z axis.
        /// </summary>
        public System.Windows.Media.Media3D.Point3D RotationOTS
        {
            get { return rotationOTS; }
            set
            {
                rotationOTS = value;
                Flush();
            }
        }

        public System.Windows.Media.Media3D.Point3D WorldRotationOTS => new System.Windows.Media.Media3D.Point3D(GetWorldOrientation(),
    GetWorldTilt(), GetWorldSpin());
        private System.Windows.Media.Media3D.Vector3D[] GetWorldDirectionVectors()
        {
            System.Collections.Generic.List<System.Windows.Media.Media3D.Vector3D>
                directionVectors = new System.Collections.Generic.List<System.Windows.Media.Media3D.Vector3D>();
            directionVectors.Add(ApplyTransform(new System.Windows.Media.Media3D.Vector3D(1, 0, 0), LocalToWorld));
            directionVectors.Add(ApplyTransform(new System.Windows.Media.Media3D.Vector3D(0, 1, 0), LocalToWorld));
            directionVectors.Add(ApplyTransform(new System.Windows.Media.Media3D.Vector3D(0, 0, 1), LocalToWorld));
            return directionVectors.ToArray();
        }
        private double GetWorldOrientation()
        {
            // Orientation is rotation about the +Z axis (in the XY plane)
            System.Windows.Media.Media3D.Vector3D[] worldDirectionVectors = GetWorldDirectionVectors();

            double orientation = 0;
            if (System.Math.Abs(System.Math.Round(worldDirectionVectors[0].Z, 4)) == 1)
            {
                // Edge case where X Axis is normal to XY plane, use Y Axis to compute orientation,
                // any spin will be combined into the orientation
                System.Windows.Media.Media3D.Point3D localYAxisProjectedIntoWorldXY
                = ProjectPointToPlane(
                   new System.Windows.Media.Media3D.Point3D(
                       worldDirectionVectors[1].X,
                       worldDirectionVectors[1].Y,
                       worldDirectionVectors[1].Z),
                   new System.Windows.Media.Media3D.Point3D(0, 0, 0),
                   new System.Windows.Media.Media3D.Vector3D(0, 0, 1));
                orientation = System.Windows.Media.Media3D.Vector3D.AngleBetween(
                    new System.Windows.Media.Media3D.Vector3D(0, 1, 0),
                    new System.Windows.Media.Media3D.Vector3D(
                        localYAxisProjectedIntoWorldXY.X,
                        localYAxisProjectedIntoWorldXY.Y,
                        localYAxisProjectedIntoWorldXY.Z)
                    );
                if (localYAxisProjectedIntoWorldXY.X > 0) orientation *= -1;
            }
            else
            {
                System.Windows.Media.Media3D.Point3D localXAxisProjectedIntoWorldXY
                    = ProjectPointToPlane(
                       new System.Windows.Media.Media3D.Point3D(
                           worldDirectionVectors[0].X,
                           worldDirectionVectors[0].Y,
                           worldDirectionVectors[0].Z),
                       new System.Windows.Media.Media3D.Point3D(0, 0, 0),
                       new System.Windows.Media.Media3D.Vector3D(0, 0, 1));
                if (worldDirectionVectors[2].Z < 0)
                {
                    orientation = -System.Windows.Media.Media3D.Vector3D.AngleBetween(
                        new System.Windows.Media.Media3D.Vector3D(-1, 0, 0),
                        new System.Windows.Media.Media3D.Vector3D(
                            localXAxisProjectedIntoWorldXY.X,
                            localXAxisProjectedIntoWorldXY.Y,
                            localXAxisProjectedIntoWorldXY.Z)
                        );
                    if (localXAxisProjectedIntoWorldXY.Y < 0) orientation *= -1;
                }
                else
                {
                    orientation = System.Windows.Media.Media3D.Vector3D.AngleBetween(
                        new System.Windows.Media.Media3D.Vector3D(1, 0, 0),
                        new System.Windows.Media.Media3D.Vector3D(
                            localXAxisProjectedIntoWorldXY.X,
                            localXAxisProjectedIntoWorldXY.Y,
                            localXAxisProjectedIntoWorldXY.Z)
                        );
                    if (worldDirectionVectors[0].Y < 0) orientation *= -1;
                }
                
            }


            return orientation;
        }
        private double GetWorldTilt()
        {
            // Tilt is rotation about the +Y axis (in the ZX plane)
            System.Windows.Media.Media3D.Vector3D[] worldDirectionVectors = GetWorldDirectionVectors();

            System.Windows.Media.Media3D.Point3D localZAxisProjectedIntoWorldZX
                = ProjectPointToPlane(
                   new System.Windows.Media.Media3D.Point3D(
                       worldDirectionVectors[2].X,
                       worldDirectionVectors[2].Y,
                       worldDirectionVectors[2].Z),
                   new System.Windows.Media.Media3D.Point3D(0, 0, 0),
                   worldDirectionVectors[1]);
            double tilt = System.Windows.Media.Media3D.Vector3D.AngleBetween(
                new System.Windows.Media.Media3D.Vector3D(0, 0, 1),
                new System.Windows.Media.Media3D.Vector3D(
                    localZAxisProjectedIntoWorldZX.X,
                    localZAxisProjectedIntoWorldZX.Y,
                    localZAxisProjectedIntoWorldZX.Z)
                );

            if (worldDirectionVectors[0].Z > 0) tilt *= -1;
            return tilt;
        }
        private double GetWorldSpin()
        {
            double worldOrientation = GetWorldOrientation();
            double worldTilt = GetWorldTilt();

            // Spin is rotation about the +X axis (in the YZ plane)
            System.Windows.Media.Media3D.Vector3D[] worldDirectionVectors = GetWorldDirectionVectors();

            // Back out the tilt component
            Transform3D adjust = new Transform3D();
            adjust.RotationOTS = new System.Windows.Media.Media3D.Point3D(-worldOrientation, 0, 0);
            worldDirectionVectors = adjust.Transform(worldDirectionVectors, TransformType.LocalToParent);
            adjust.RotationOTS = new System.Windows.Media.Media3D.Point3D(0,-worldTilt, 0);
            worldDirectionVectors = adjust.Transform(worldDirectionVectors, TransformType.LocalToParent);
            
            System.Windows.Media.Media3D.Point3D localYAxisProjectedIntoWorldZY
                = ProjectPointToPlane(
                   new System.Windows.Media.Media3D.Point3D(
                       worldDirectionVectors[1].X,
                       worldDirectionVectors[1].Y,
                       worldDirectionVectors[1].Z),
                   new System.Windows.Media.Media3D.Point3D(0,0,0),
                   new System.Windows.Media.Media3D.Vector3D(1,0,0));
            double spin = System.Windows.Media.Media3D.Vector3D.AngleBetween(
                new System.Windows.Media.Media3D.Vector3D(0, 1, 0),
                new System.Windows.Media.Media3D.Vector3D(
                    localYAxisProjectedIntoWorldZY.X,
                    localYAxisProjectedIntoWorldZY.Y,
                    localYAxisProjectedIntoWorldZY.Z)
                );

            System.Windows.Media.Media3D.Vector3D yzCross
                = System.Windows.Media.Media3D.Vector3D.CrossProduct(
                worldDirectionVectors[1], worldDirectionVectors[2]);

            if (worldDirectionVectors[1].Z < 0) spin *= -1;
            return spin;
        }
        #region Transformations TO Ancestors
        private bool localToParentCacheDirty = true;
        private System.Windows.Media.Media3D.Matrix3D localToParentCached = new System.Windows.Media.Media3D.Matrix3D();
        public System.Windows.Media.Media3D.Matrix3D LocalToParent
        {
            get
            {
                if(localToParentCacheDirty)
                {
                    System.Windows.Media.Media3D.Matrix3D matrix = new System.Windows.Media.Media3D.Matrix3D();
                    matrix.Scale(new System.Windows.Media.Media3D.Vector3D(scale.X, scale.Y, scale.Z));
                    System.Windows.Media.Media3D.Quaternion rotation = ComputeQuaternionFromOTS_ParentZUp(rotationOTS.X, rotationOTS.Y, rotationOTS.Z);
                    matrix.Rotate(rotation);
                
                    matrix.Translate(new System.Windows.Media.Media3D.Vector3D(translation.X,
                                         translation.Y,translation.Z));
                    localToParentCached = matrix;
                    localToParentCacheDirty = false;
                }
                return localToParentCached;
            }
        }

        public System.Windows.Media.Media3D.Matrix3D ParentToWorld
        {
            get
            {
                System.Windows.Media.Media3D.Matrix3D parentToWorld = new System.Windows.Media.Media3D.Matrix3D();
                Transform3D currentParent = parent;
                while (!object.ReferenceEquals(null, currentParent))
                {
                    parentToWorld.Append(currentParent.LocalToParent);
                    currentParent = currentParent.Parent;
                }
                return parentToWorld;
            }
        }

        private bool localToWorldCacheDirty = true;
        private System.Windows.Media.Media3D.Matrix3D localToWorldCached = new System.Windows.Media.Media3D.Matrix3D();
        public System.Windows.Media.Media3D.Matrix3D LocalToWorld
        {
            get
            {
                if (localToWorldCacheDirty)
                {
                    System.Windows.Media.Media3D.Matrix3D localToWorld = System.Windows.Media.Media3D.Matrix3D.Multiply(LocalToParent, new System.Windows.Media.Media3D.Matrix3D());
                    localToWorld.Append(ParentToWorld);
                    localToWorldCached = localToWorld;
                    localToWorldCacheDirty = false;
                }
                return localToWorldCached;
            }
        }
        #endregion

        #region Transformations FROM Ancestors
        public System.Windows.Media.Media3D.Matrix3D ParentToLocal
        {
            get
            {
                System.Windows.Media.Media3D.Matrix3D identity = new System.Windows.Media.Media3D.Matrix3D();
                System.Windows.Media.Media3D.Matrix3D p2l = System.Windows.Media.Media3D.Matrix3D.Multiply(LocalToParent, identity);
                p2l.Invert();
                return p2l;
            }
        }
        public System.Windows.Media.Media3D.Matrix3D WorldToParent
        {
            get
            {
                System.Windows.Media.Media3D.Matrix3D identity = new System.Windows.Media.Media3D.Matrix3D();
                System.Windows.Media.Media3D.Matrix3D p2l = System.Windows.Media.Media3D.Matrix3D.Multiply(ParentToWorld, identity);
                p2l.Invert();
                return p2l;
            }
        }

        private bool worldToLocalCacheDirty = true;
        private System.Windows.Media.Media3D.Matrix3D worldToLocalCached = new System.Windows.Media.Media3D.Matrix3D();
        public System.Windows.Media.Media3D.Matrix3D WorldToLocal
        {
            get
            {
                if (worldToLocalCacheDirty)
                {
                    System.Windows.Media.Media3D.Matrix3D identity = new System.Windows.Media.Media3D.Matrix3D();
                    System.Windows.Media.Media3D.Matrix3D p2l = System.Windows.Media.Media3D.Matrix3D.Multiply(LocalToWorld, identity);
                    p2l.Invert();
                    worldToLocalCached = p2l;
                    worldToLocalCacheDirty = false;
                }
                return worldToLocalCached;
            }
        }
        #endregion

        public System.Windows.Media.Media3D.Point3D Transform(System.Windows.Media.Media3D.Point3D source,TransformType transformType)
        {
            switch(transformType)
            {
                case TransformType.LocalToParent: return ApplyTransform(source, LocalToParent);
                case TransformType.LocalToWorld: return ApplyTransform(source, LocalToWorld);
                case TransformType.WorldToLocal: return ApplyTransform(source, WorldToLocal);
                case TransformType.ParentToLocal: return ApplyTransform(source, ParentToLocal);
                case TransformType.ParentToWorld: return ApplyTransform(source, ParentToWorld);
                case TransformType.WorldToParent: return ApplyTransform(source, WorldToParent);
            }
            return source;
        }

        public System.Windows.Media.Media3D.Vector3D Transform(System.Windows.Media.Media3D.Vector3D source, TransformType transformType)
        {
            switch (transformType)
            {
                case TransformType.LocalToParent: return ApplyTransform(source, LocalToParent);
                case TransformType.LocalToWorld: return ApplyTransform(source, LocalToWorld);
                case TransformType.WorldToLocal: return ApplyTransform(source, WorldToLocal);
                case TransformType.ParentToLocal: return ApplyTransform(source, ParentToLocal);
                case TransformType.ParentToWorld: return ApplyTransform(source, ParentToWorld);
                case TransformType.WorldToParent: return ApplyTransform(source, WorldToParent);
            }
            return source;
        }

        public System.Windows.Media.Media3D.Vector3D[] Transform(System.Windows.Media.Media3D.Vector3D[] source,TransformType transformType)
        {
            System.Collections.Generic.List<System.Windows.Media.Media3D.Vector3D> results = new System.Collections.Generic.List<System.Windows.Media.Media3D.Vector3D>();
            foreach(System.Windows.Media.Media3D.Vector3D item in source)
            {
                results.Add(Transform(item, transformType));
            }
            return results.ToArray();
        }
        #region static methods
        public static System.Windows.Media.Media3D.Quaternion ComputeQuaternionFromOTS_ParentZUp(double orientation_degrees, double tilt_degrees, double spin_degrees)
        {
            System.Windows.Media.Media3D.Matrix3D rotationMatrix = new System.Windows.Media.Media3D.Matrix3D();

            // Rotate for Orientation about the Z axis
            double RHR_rotation = orientation_degrees;
            global::System.Windows.Media.Media3D.Vector3D axis = new global::System.Windows.Media.Media3D.Vector3D(0.0, 0.0, 1.0);
            System.Windows.Media.Media3D.Quaternion orientation = new System.Windows.Media.Media3D.Quaternion(axis, RHR_rotation);
            rotationMatrix.Rotate(orientation);

            // Rotate for Tilt about the Y axis
            System.Windows.Media.Media3D.Vector3D tilt_axis = new System.Windows.Media.Media3D.Vector3D(0.0, 1.0, 0.0);
            tilt_axis = System.Windows.Media.Media3D.Vector3D.Multiply(tilt_axis, rotationMatrix);

            double tRHR = tilt_degrees;
            System.Windows.Media.Media3D.Quaternion tilt = new System.Windows.Media.Media3D.Quaternion(tilt_axis, tRHR);
            rotationMatrix.Rotate(tilt);

            // Rotate for Spin about X axis
            System.Windows.Media.Media3D.Vector3D spin_axis = new System.Windows.Media.Media3D.Vector3D(1.0, 0.0, 0.0);
            spin_axis = System.Windows.Media.Media3D.Vector3D.Multiply(spin_axis, rotationMatrix);
            System.Windows.Media.Media3D.Quaternion spin = new System.Windows.Media.Media3D.Quaternion(spin_axis, spin_degrees);
            //matrix.Rotate(spin);

            // Combine the orientation and tilt Quaternions
            System.Windows.Media.Media3D.Quaternion total_rotation = System.Windows.Media.Media3D.Quaternion.Multiply(tilt, orientation);
            total_rotation = System.Windows.Media.Media3D.Quaternion.Multiply(spin, total_rotation);
            return total_rotation;
        }
        public static System.Windows.Media.Media3D.Point3D ApplyTransform(System.Windows.Media.Media3D.Point3D source, System.Windows.Media.Media3D.Matrix3D trans)
        {
            System.Windows.Media.Media3D.Point4D point4D =
                new System.Windows.Media.Media3D.Point4D(source.X, source.Y, source.Z, 1.0);
            point4D = trans.Transform(point4D);
            return new System.Windows.Media.Media3D.Point3D(point4D.X, point4D.Y, point4D.Z);
        }
        public static System.Windows.Media.Media3D.Vector3D ApplyTransform(System.Windows.Media.Media3D.Vector3D source, System.Windows.Media.Media3D.Matrix3D trans)
        {
            System.Windows.Media.Media3D.Vector3D mv = new System.Windows.Media.Media3D.Vector3D(source.X, source.Y, source.Z);
            mv = trans.Transform(mv);
            System.Windows.Media.Media3D.Vector3D result = new System.Windows.Media.Media3D.Vector3D(mv.X, mv.Y, mv.Z);
            return result;
        }
        #endregion

        public static System.Windows.Media.Media3D.Point3D ProjectPointToPlane(
System.Windows.Media.Media3D.Point3D point,
System.Windows.Media.Media3D.Point3D pointOnPlane,
System.Windows.Media.Media3D.Vector3D planeNormal) => point - System.Windows.Media.Media3D.Vector3D.DotProduct(
         point - pointOnPlane, planeNormal) * planeNormal;
    }
}
