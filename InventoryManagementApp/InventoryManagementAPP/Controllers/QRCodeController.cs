using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Drawing.Imaging;
using System.Drawing;

namespace InventoryManagementAPP.Controllers
{
    public class QRCodeController : ControllerBase
    {
        [HttpGet]
        [Route("GenerateQRCode")]
        public async Task<ActionResult> GenerateQRCode()
        {
            try
            {
                var url = $"https://ivorcelic-001-site1.htempurl.com/api/v1/InventoryTransactionItem?transactionId=1&warehouseId=1&productId=1&quantity=1";
            

                QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);

                using (MemoryStream ms = new MemoryStream())
                {
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);
                    qrCodeImage.Save(ms, ImageFormat.Png);
                    var bytes = ms.ToArray();
                    return File(bytes, "image/tmp");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
