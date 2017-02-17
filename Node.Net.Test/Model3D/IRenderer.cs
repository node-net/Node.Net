using System.Collections.Generic;
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
namespace Node.Net.Model3D
{
    public interface IRenderer
    {
        ResourceDictionary Resources { get; set; }
        object GetResource(string name);
        List<string> Model3DKeys { get; }
        Visual3D GetVisual3D(object value);
        ModelVisual3D GetModelVisual3D(object value);
        System.Windows.Media.Media3D.Model3D GetModel3D(object value);
        GeometryModel3D GetGeometryModel3D(object value);
        Transform3D GetTransform3D(object value);
        Vector3D GetTranslation(object value);
        Vector3D GetScale(object value);
        Material GetMaterial(object value);
        Material GetBackMaterial(object value);
    }
}
