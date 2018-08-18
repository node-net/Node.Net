using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Node.Net.Internal
{
	internal sealed class ImageSourceReader
	{
		//public static ImageSourceReader Default { get; } = new ImageSourceReader();
		public ImageSourceReader()
		{
			readers.Add("png", ReadPng);
			readers.Add("tif", ReadTif);
			readers.Add("jpg", ReadJpg);
			readers.Add("gif", ReadGif);
			readers.Add("bmp", ReadBmp);
			signatureReaders.Add("42 4D", "bmp");                       // .bmp
			signatureReaders.Add("47 49 46 38 37 61", "gif");           // .gif
			signatureReaders.Add("47 49 46 38 39 61", "gif");           // .gif
			signatureReaders.Add("FF D8 FF E0", "jpg");                 // .jpg
			signatureReaders.Add("FF D8 FF E1", "jpg");                 // .jpg
			signatureReaders.Add("49 49 2A 00", "tif");                 // .tif
			signatureReaders.Add("4D 4D 00 2A", "tif");                 // .tif
			signatureReaders.Add("89 50 4E 47 0D 0A 1A 0A", "png");     // .png
		}

		public string[] Signatures
		{
			get
			{
				var signatures = new List<string>(signatureReaders.Keys);
				return signatures.ToArray();
			}
		}

		private readonly Dictionary<string, string> signatureReaders = new Dictionary<string, string>();
		private readonly Dictionary<string, Func<Stream, object>> readers = new Dictionary<string, Func<Stream, object>>();

		public object Read(Stream original_stream)
		{
			using (var signatureReader = new Internal.SignatureReader(original_stream))
			{
				var stream = signatureReader.Stream;
				var signature = signatureReader.Signature;
				foreach (string signature_key in signatureReaders.Keys)
				{
					if (signature.IndexOf(signature_key) == 0)
					{
						var instance = readers[signatureReaders[signature_key]](stream);
						return instance;
					}
				}
				throw new System.Exception($"unrecognized signature '{signature}'");
			}
		}

		public object ReadPng(Stream stream)
		{
			var decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
			return decoder.Frames[0].Clone();
		}

		public object ReadTif(Stream stream)
		{
			var decoder = new TiffBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
			return decoder.Frames[0].Clone();
		}

		public object ReadJpg(Stream stream)
		{
			return GetImageSource(System.Drawing.Image.FromStream(stream));
		}

		public object ReadGif(Stream stream)
		{
			var decoder = new GifBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
			return decoder.Frames[0].Clone();
		}

		public object ReadBmp(Stream stream)
		{
			var decoder = new BmpBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
			return decoder.Frames[0].Clone();
		}

		public static ImageSource GetImageSource(System.Drawing.Image image)
		{
			if (!object.ReferenceEquals(null, image))
			{
				var bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();

				var memory = new System.IO.MemoryStream();
				image.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
				memory.Seek(0, SeekOrigin.Begin);

				bitmapImage.StreamSource = memory;
				bitmapImage.EndInit();
				return bitmapImage;
			}
			return null;
		}
	}
}