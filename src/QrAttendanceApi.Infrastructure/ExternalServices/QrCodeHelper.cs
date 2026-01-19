using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace QrAttendanceApi.Infrastructure.ExternalServices
{
    public class QrCodeHelper
    {
        public static byte[] Generate(string payload, int size = 350)
        {
            using var generator = new QRCodeGenerator();
            var data = generator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);

            using var QRCode = new QRCode(data);
            using var bit = QRCode.GetGraphic(20);

            using var sizing = new Bitmap(bit, new Size(size, size));
            using var ms = new MemoryStream();
            sizing.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }
    }
}
