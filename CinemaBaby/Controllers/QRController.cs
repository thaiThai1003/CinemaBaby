using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Drawing;
using System.IO;

namespace CinemaBaby.Controllers
{
    public class QRController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GenerateQR()
        {
            string qrContent = @"MB Bank
STK: 123456789
Tên: CINEMA BABY
Số tiền: 100000
Nội dung: DEMO_QR";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            Bitmap qrImage = qrCode.GetGraphic(20);

            using (MemoryStream ms = new MemoryStream())
            {
                qrImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return File(ms.ToArray(), "image/png");
            }
        }
    }
}