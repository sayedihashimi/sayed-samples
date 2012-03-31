using System;
using System.Activities;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace ImageResizer.Components
{
    public sealed class ResizeImage : AsyncCodeActivity
    {
        public InArgument<FileInfo> InputFile { get; set; }
        public InArgument<Options> Options { get; set; }
        public OutArgument<FileInfo> OutputFile { get; set; }

        protected override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback, object state)
        {
            // Obtain the runtime value of the Text input argument
            FileInfo image = context.GetValue(this.InputFile);
            Options options = context.GetValue(this.Options);

            Func<FileInfo, Options, AsyncCodeActivityContext, FileInfo> resizeDelegate = new Func<FileInfo, Options, AsyncCodeActivityContext, FileInfo>(ResizeImageFile);
            context.UserState = resizeDelegate;
            IAsyncResult result = resizeDelegate.BeginInvoke(image, options, context, callback, state);

            return result;
        }

        protected override void EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
        {
            Func<FileInfo, Options, AsyncCodeActivityContext, FileInfo> resizeDelegate = context.UserState as Func<FileInfo, Options, AsyncCodeActivityContext, FileInfo>;
            FileInfo resizedFile = resizeDelegate.EndInvoke(result);

            OutputFile.Set(context, resizedFile);
        }

        public FileInfo ResizeImageFile(FileInfo image, Options options, AsyncCodeActivityContext context)
        {
            // Get the image codec info
            ImageCodecInfo CodecInfo = GetEncoderInfo("image/jpeg");

            //Save the bitmap as a JPEG file with quality level 75.
            System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameter encoderParameter = new System.Drawing.Imaging.EncoderParameter(encoder, 100L);
            EncoderParameters encoderParameters = new EncoderParameters();
            encoderParameters.Param[0] = encoderParameter;

            System.Drawing.Image img = null;
            System.Drawing.Bitmap bitmap = null;
            string savePath = string.Empty;

            try
            {
                img = System.Drawing.Image.FromFile(image.FullName);

                if (options.AutoRotate == true)
                {
                    var pi = img.PropertyItems.FirstOrDefault(p => p.Id == 0x0112);
                    if (pi != null)
                    {
                        switch (pi.Value[0])
                        {
                            case 6:
                                img.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
                                break;
                            case 8:
                                img.RotateFlip(System.Drawing.RotateFlipType.Rotate270FlipNone);
                                break;
                            default:
                                break;
                        }
                    }
                }

                //set the width and height, using the original values if not specified
                int width = options.Width == 0 ? img.Width : options.Width;
                int height = options.Height == 0 ? img.Height : options.Height;

                if (img.Width < img.Height)
                {
                    int tempWidth = width;
                    width = height;
                    height = tempWidth;
                }

                bitmap = new System.Drawing.Bitmap(img, new System.Drawing.Size(width, height));

                //make sure the target directory exists. If not, create it!
                if (!Directory.Exists(options.TargetDirectory))
                    Directory.CreateDirectory(options.TargetDirectory);

                savePath = Path.Combine(options.TargetDirectory, image.Name);
                bitmap.Save(savePath, CodecInfo, encoderParameters);

                if (!string.IsNullOrWhiteSpace(savePath))
                    return new FileInfo(savePath);

                return null;
            }
            catch
            {
                throw new Exception
                    (
                        string.Format("Cannot resize '{0} as it is not a valid image file!", image.Name)
                    );
            }
            finally
            {
                if (bitmap != null)
                    bitmap.Dispose();

                if (img != null)
                    img.Dispose();
            }
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            var encoders = ImageCodecInfo.GetImageEncoders();

            var codec = Array.Find<ImageCodecInfo>(
                encoders, 
                e => e.MimeType.Equals(mimeType, StringComparison.CurrentCultureIgnoreCase)
                );

            if (codec != null)
                return codec;

            return null;
        }

    }
}
