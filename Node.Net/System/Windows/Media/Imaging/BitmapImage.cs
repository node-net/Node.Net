using System;
using System.IO;
using System.Windows.Media;

namespace System.Windows.Media.Imaging
{
#if !IS_WINDOWS
    /// <summary>
    /// Provides a BitmapSource that is optimized for loading XAML content.
    /// </summary>
    public class BitmapImage : BitmapSource
    {
        private Stream? _streamSource;
        private Uri? _uriSource;
        private bool _isInitializing;

        /// <summary>
        /// Gets or sets the stream source of the BitmapImage.
        /// </summary>
        public Stream? StreamSource
        {
            get => _streamSource;
            set
            {
                if (!_isInitializing)
                {
                    throw new InvalidOperationException("Cannot set StreamSource outside of BeginInit/EndInit block.");
                }
                _streamSource = value;
            }
        }

        /// <summary>
        /// Gets or sets the Uri source of the BitmapImage.
        /// </summary>
        public Uri? UriSource
        {
            get => _uriSource;
            set
            {
                if (!_isInitializing)
                {
                    throw new InvalidOperationException("Cannot set UriSource outside of BeginInit/EndInit block.");
                }
                _uriSource = value;
            }
        }

        /// <summary>
        /// Signals the start of the BitmapImage initialization.
        /// </summary>
        public void BeginInit()
        {
            _isInitializing = true;
        }

        /// <summary>
        /// Signals the end of the BitmapImage initialization.
        /// </summary>
        public void EndInit()
        {
            _isInitializing = false;
            // In a real implementation, this would decode the image and set PixelWidth/PixelHeight
            // For now, we'll set default values
            if (PixelWidth == 0 && PixelHeight == 0)
            {
                PixelWidth = 100;
                PixelHeight = 100;
            }
        }

        /// <summary>
        /// Initializes a new instance of the BitmapImage class.
        /// </summary>
        public BitmapImage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the BitmapImage class with the specified Uri.
        /// </summary>
        /// <param name="uriSource">The Uri of the bitmap source.</param>
        public BitmapImage(Uri uriSource)
        {
            BeginInit();
            UriSource = uriSource;
            EndInit();
        }
    }
#endif
}

