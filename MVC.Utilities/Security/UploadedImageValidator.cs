using System.Drawing.Imaging;
using System.IO;
using System.Drawing;

namespace MVC.Utilities.Security
{
    /// <summary>
    /// A simple utility class for performing some integrity checks on images uploaded by end-users.
    /// 
    /// Uses GDI+ to determine if the uploaded file is a valid web-friendly format - see the article:
    /// http://www.aaronstannard.com/post/2011/06/24/How-to-Securely-Verify-and-Validate-Image-Uploads-in-ASPNET-and-ASPNET-MVC.aspx
    /// </summary>
    public static class UploadedImageValidator
    {
        public static bool FileIsWebFriendlyImage(Stream stream)
        {
            try
            {
                //Read an image from the stream...
                var i = Image.FromStream(stream);

                //Move the pointer back to the beginning of the stream
                stream.Seek(0, SeekOrigin.Begin);

                //Check to see if the raw format of the image matches either the GIF, JPEG, or PNG codecs.
                if (ImageFormat.Jpeg.Equals(i.RawFormat))
                    return true;
                return ImageFormat.Png.Equals(i.RawFormat) || ImageFormat.Gif.Equals(i.RawFormat);
            }
            catch
            {
                return false;
            }
        }

        public static bool FileIsWebFriendlyImage(Stream stream, long size)
        {
            return stream.Length <= size && FileIsWebFriendlyImage(stream);
        }
    }
}
