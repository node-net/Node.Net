﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Media3D;

/// <summary>
/// Visual3D
///  |
///  +--ModelVisual3D
///
/// Model3D
///  |
///  +--GeometryModel3D
///  |
///  +--Light
///  |    |
///  |    +--AmbientLight
///  |    |
///  |    +--DirectionalLight
///  |    |
///  |    +--PointLightBase
///  |
///  +--Model3DGroup
///
/// </summary>
/// <param name="value"></param>
/// <returns></returns>
///
namespace Node.Net._Model3D
{
    public interface IRenderer : IVisual3DTransformer, IModel3DTransformer,IResources
    {
        IFilter TypeNameFilter { get; set; }
        IMetaDataManager MetaData { get; set; }
        IResources Resources { get; set; }
        Dictionary<string, IModel3DTransformer> TypeModel3DTransformers { get; }
        Dictionary<string, IModel3DGroupTransformer> TypeModel3DGroupTransformers { get; }
        List<string> Model3DKeys { get; }
        ModelVisual3D GetModelVisual3D(object value);
        GeometryModel3D GetGeometryModel3D(object value);
        System.Windows.Media.Media3D.Transform3D GetTransform3D(object value);
        Vector3D GetTranslation(object value);
        Vector3D GetScale(object value);
        Material GetMaterial(object value);
        Material GetBackMaterial(object value);
    }
}