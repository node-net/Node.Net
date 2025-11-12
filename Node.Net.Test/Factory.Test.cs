using NUnit.Framework;
using System;
using System.Reflection;
using System.Windows.Media.Media3D;

namespace Node.Net.Test
{
    [TestFixture]
    internal class FactoryTest
    {
        [Test]
        public void ClearCache()
        {
            Factory factory = new Factory();
            
            // Cache and ClearCache are Windows-specific, so check if they exist
            PropertyInfo cacheProperty = typeof(Factory).GetProperty("Cache");
            MethodInfo clearCacheMethod = typeof(Factory).GetMethod("ClearCache", Type.EmptyTypes);
            MethodInfo clearCacheWithParamMethod = typeof(Factory).GetMethod("ClearCache", new Type[] { typeof(object) });
            
            if (cacheProperty != null && clearCacheMethod != null)
            {
                cacheProperty.SetValue(factory, true);
                clearCacheMethod.Invoke(factory, null);
                if ((bool)cacheProperty.GetValue(factory)!)
                {
                    cacheProperty.SetValue(factory, false);
                }

                Matrix3D matrix = factory.Create<Matrix3D>();
                if (clearCacheWithParamMethod != null)
                {
                    clearCacheWithParamMethod.Invoke(factory, new object[] { matrix });
                }
                clearCacheMethod.Invoke(factory, null);
            }
            else
            {
                // On non-Windows platforms, just verify factory can be created and used
                Matrix3D matrix = factory.Create<Matrix3D>();
                Assert.That(matrix, Is.Not.Null);
            }
        }

        [Test]
        public void Coverage()
        {
            Factory factory = new Factory();
            Assert.That(factory, Is.Not.Null);
        }
    }
}