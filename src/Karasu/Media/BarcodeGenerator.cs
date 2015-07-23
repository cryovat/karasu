using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ZXing;
using ZXing.QrCode;

namespace Karasu.Media
{
    public static class BarcodeGenerator
    {
        public static byte[] GeneratePngBytes(string text, int width, int height)
        {
            var hints = new Dictionary<EncodeHintType, object>
            {
                [EncodeHintType.MARGIN] = 0
            };

            var qr = new QRCodeWriter();
            var matrix = qr.encode(text, BarcodeFormat.QR_CODE, width, height, hints);

            var writer = new BarcodeWriter {Options = {Margin = 0}};

            using (var bitmap = writer.Write(matrix))
            {
                bitmap.MakeTransparent(Color.White);

                var ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.Png);

                return ms.ToArray();
            }
        }

        public static string GeneratePngDataUri(string text, int width, int height)
        {
            return $"data:image/png;base64,{Convert.ToBase64String(GeneratePngBytes(text, width, height))}";
        }
    }
}
