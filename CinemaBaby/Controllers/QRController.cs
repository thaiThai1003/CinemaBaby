using Microsoft.AspNetCore.Mvc;
using QRCoder;

namespace CinemaBaby.Controllers
{
    public class QRController : Controller
    {
        public IActionResult GenerateQR(decimal amount)
        {
            string qrContent = $@"MB Bank
STK: 123456789
Tên: CINEMA BABY
Số tiền: {amount}
Nội dung: THANH_TOAN";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);

            // 🔥 KHÔNG dùng Bitmap nữa
            var qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrBytes = qrCode.GetGraphic(20);

            return File(qrBytes, "image/png");
        }
    }
}