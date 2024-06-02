using Google.Protobuf;
using Grpc.Core;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace Common
{
    public class FileStoreCommon
    {
        public static string SaveUploadedFile(HttpPostedFileBase fileBase, string createdBy, string sign, string nameCore)
        {
            try
            {
                if (fileBase.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(fileBase.FileName);
                    string _Extension = Path.GetExtension(fileBase.FileName).ToLower();

                    var renameFile = $"{nameCore}-{sign}";
                    var filePath = Path.Combine(HttpContext.Current.Server.MapPath($"/uploads/{createdBy}/images"), $"{renameFile}{_Extension}");

                    fileBase.SaveAs(filePath);

                    return $"/uploads/{createdBy}/images/{renameFile}{_Extension}";

                }
                return string.Empty;
            }
            catch(Exception e)
            {
                File.WriteAllText("error.txt", $"Error: {e.ToString()}" );
                return string.Empty;
            }
        }
        //public static string SaveUploadedFile(HttpPostedFileBase file, string directoryPath, string sign)
        //{
        //    if (file != null && file.ContentLength > 0)
        //    {
        //        var fileName = Path.GetFileName(file.FileName);

        //        // Tạo thư mục nếu không tồn tại
        //        if (!Directory.Exists(directoryPath))
        //        {
        //            Directory.CreateDirectory(directoryPath);
        //        }

        //        var filePath = Path.Combine(directoryPath, fileName);

        //        // Lưu tệp lên máy chủ
        //        file.SaveAs(filePath);


        //        return fileName;
        //    }

        //    return null;
        //}
        public static string SaveUploadedFile(HttpPostedFileBase file, string directoryPath, string sign, string nameCore, long quality)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(file.FileName).ToLower();

                // Tạo thư mục nếu không tồn tại
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var uniqueFileName = $"{nameCore}-{sign}";
                var filePath = Path.Combine(directoryPath, $"{uniqueFileName}{extension}");

                // Nén chất lượng ảnh
                using (var image = Image.FromStream(file.InputStream, true, true))
                {
                    var encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                    var codecInfo = GetEncoderInfo(extension);

                    image.Save(filePath, codecInfo, encoderParameters);
                }

                return $"{uniqueFileName}{extension}";
            }

            return null;
        }
        private static ImageCodecInfo GetEncoderInfo(string extension)
        {
            string mimeType;
            switch (extension)
            {
                case ".jpeg":
                case ".jpg":
                    mimeType = "image/jpeg";
                    break;
                case ".png":
                    mimeType = "image/png";
                    break;
                case ".gif":
                    mimeType = "image/gif";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(extension), $"Unsupported file extension {extension}");
            }

            var codecs = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < codecs.Length; i++)
            {
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            }
            return null;
        }
    }
}
