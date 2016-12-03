// Copyright (c) 2016 Lou Parslow. Subject to the MIT license, see LICENSE.txt.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace Node.Net.Extensions
{
    public static class Viewport3DExtension
    {
        /// <summary>
        /// Code adapted from HelixToolkit.Wpf.Viewport3DHelper.Point3DtoPoint2D obtained from http://github.com/helix-toolkit/helix-toolkit
        /// License: helix-toolkit.LICENSE.txt 
        /// </summary>
        /// <param name="viewport"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Point Point3DtoPoint2D(Viewport3D viewport, Point3D point)
        {
            var matrix = GetTotalTransform(viewport);
            var pointTransformed = matrix.Transform(point);
            var pt = new Point(pointTransformed.X, pointTransformed.Y);
            return pt;
        }
        /// <summary>
        /// Code adapted from HelixToolkit.Wpf.Viewport3DHelper.GetTotalTransform obtained from http://github.com/helix-toolkit/helix-toolkit
        /// License: helix-toolkit.LICENSE.txt 
        /// </summary>
        /// <param name="viewport"></param>
        /// <returns></returns>
        public static Matrix3D GetTotalTransform(Viewport3D viewport)
        {
            var transform = GetCameraTransform(viewport);
            transform.Append(GetViewportTransform(viewport));
            return transform;
        }
        /// <summary>
        /// Code adapted from HelixToolkit.Wpf.Viewport3DHelper.GetCameraTransform obtained from http://github.com/helix-toolkit/helix-toolkit
        /// License: helix-toolkit.LICENSE.txt 
        /// </summary>
        /// <param name="viewport"></param>
        /// <returns></returns>
        public static Matrix3D GetCameraTransform(Viewport3D viewport)
        {
            return viewport.Camera.GetTotalTransform(viewport.ActualWidth / viewport.ActualHeight);
        }
        /// <summary>
        /// Code adapted from HelixToolkit.Wpf.Viewport3DHelper.GetViewportTransform obtained from http://github.com/helix-toolkit/helix-toolkit
        /// License: helix-toolkit.LICENSE.txt
        /// </summary>
        /// <param name="viewport"></param>
        /// <returns></returns>
        public static Matrix3D GetViewportTransform(this Viewport3D viewport)
        {
            return new Matrix3D(
                viewport.ActualWidth / 2,
                0,
                0,
                0,
                0,
                -viewport.ActualHeight / 2,
                0,
                0,
                0,
                0,
                1,
                0,
                viewport.ActualWidth / 2,
                viewport.ActualHeight / 2,
                0,
                1);
        }
    }
}
