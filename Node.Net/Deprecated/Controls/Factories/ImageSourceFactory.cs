using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Node.Net.Deprecated.Controls.Factories
{
    public class ImageSourceFactory : IFactory
    {
        private static ImageSourceFactory _default;
        public static ImageSourceFactory Default
        {
            get
            {
                if (_default == null) _default = new ImageSourceFactory();
                return _default;
            }
        }
        public T Create<T>(object value)
        {
            if (value != null)
            {
                object result = null;
                if (value.GetType() == typeof(string)) return (T)(object)GetImageSource(value.ToString());
                result = GetImageSource(Internal.KeyValuePair.GetValue(value) as IDictionary);
                if (result == null) result = GetImageSource(Internal.KeyValuePair.GetValue(value).GetType());
                return (T)result;
            }
            return default(T);
        }

        public List<Assembly> ResourceAssemblies = new List<Assembly>();
        public Dictionary<string, string> AliasMap = new Dictionary<string, string>();
        private Dictionary<string, ImageSource> ImageSourceMap = new Dictionary<string, ImageSource>();

        public void AddImageSource(string name,ImageSource source)
        {
            ImageSourceMap.Add(name, source);
        }

        public ImageSource GetImageSource(string name)
        {
            if (AliasMap.ContainsKey(name))
            {
                var alias = AliasMap[name];
                if (alias != name)
                {
                    var imageSource = GetImageSource(alias);
                    if (imageSource != null) return imageSource;
                }
            }
            if (ImageSourceMap.ContainsKey(name)) return ImageSourceMap[name];

            
            var icon = Internal.Extensions.IconExtensions.GetIcon(name);
            if (icon != null)
            {
                return Internal.Extensions.IconExtensions.ToImageSource(icon);
                //return icon.ToImageSource();
            }

            foreach(Assembly assembly in ResourceAssemblies)
            {
                foreach(string resource_name in assembly.GetManifestResourceNames())
                {
                    if(resource_name == name)
                    {
                        var imageSource = Factory.Default.Create<ImageSource>(assembly.GetManifestResourceStream(resource_name));
                        ImageSourceMap.Add(name, imageSource);
                        return imageSource;
                    }
                }
            }
            return null;
        }

        public ImageSource GetImageSource(IDictionary dictionary)
        {
            if (dictionary != null)
            {
                ImageSource result = null;
                if (dictionary.Contains("Type"))
                {
                    result = GetImageSource(dictionary["Type"].ToString());
                    if (result != null) return result;
                }
                result = GetImageSource(dictionary.GetType().FullName);
                if (result != null) return result;
                result = GetImageSource(dictionary.GetType().Name);
                if (result != null) return result;
                result = GetImageSource("IDictionary");
                return result;
            }

            return null;
        }

        public ImageSource GetImageSource(Type type)
        {
            if(type != null)
            {
                var result = GetImageSource(type.FullName);
                if(result == null)
                {
                    result = GetImageSource(type.Name);
                }
                if(result == null)
                {
                    result = GetImageSource(type.BaseType);
                }
                return result;
            }
            return null;
        }

        public void Import(string name)
        {
            if(File.Exists(name))
            {
                var fileInfo = new FileInfo(name);
                if (fileInfo.Extension == ".png")
                {
                    var key = fileInfo.Name.Replace(fileInfo.Extension, "");
                    if (!ImageSourceMap.ContainsKey(key))
                    {
                        using (FileStream fs = new FileStream(name, FileMode.Open))
                        {
                            ImageSourceMap.Add(key, GetImageSource(System.Drawing.Image.FromStream(fs)));
                        }
                    }
                }
            }
            else
            {
                if(Directory.Exists(name))
                {
                    foreach(var filename in Directory.GetFiles(name))
                    {
                        Import(filename);
                    }
                }
            }
        }

        public static ImageSource GetImageSource(System.Drawing.Image image)
        {
            //var image = value as Image;
            if (!object.ReferenceEquals(null, image))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();

                var memory = new System.IO.MemoryStream();
                image.Save(memory, ImageFormat.Bmp);
                memory.Seek(0, SeekOrigin.Begin);

                bitmapImage.StreamSource = memory;
                bitmapImage.EndInit();
                return bitmapImage;
            }
            return null;
        }
    }
}
