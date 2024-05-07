using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using InventoryManagementAPP.Models;
using InventoryManagementAPP.Data;
using InventoryManagementAPP.Mappers;

using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace InventoryManagementAPP.Controllers
{

    [ApiController]
    [Route("api/v1/[controller]")]
    public class QRCodeController : ControllerBase
    {
        private readonly InventoryManagementContext _context;
        private readonly InventoryTransactionItemMapper _mapper;

        public QRCodeController(InventoryManagementContext context)
        {
            _context = context;
            _mapper = new InventoryTransactionItemMapper();
        }


        [HttpGet]
        public IActionResult GenerateQRCodePDF()
        {
            try
            {
                var qrCodeContent = "https://inventorymanagement.runasp.net/api/v1/QRCode?transactionId=1&warehouseId=1&productId=1&quantity=1";
                QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(qrCodeContent, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);

                using (var qrStream = new MemoryStream())
                {
                    Bitmap qrBitmap = qrCode.GetGraphic(20);
                    qrBitmap.Save(qrStream, ImageFormat.Png);
                    byte[] qrBytes = qrStream.ToArray();

                    using (var pdfStream = new MemoryStream())
                    {
                        Document document = new Document();
                        PdfWriter pdfWriter = PdfWriter.GetInstance(document, pdfStream);

                        document.Open();

                        iTextSharp.text.Image qrImage = iTextSharp.text.Image.GetInstance(qrBytes);

                        // APPEARANCE
                        qrImage.ScaleToFit(350, 350);
                        qrImage.Alignment = Element.ALIGN_CENTER;

                        document.Add(qrImage);

                        document.Close();

                        return new FileContentResult(pdfStream.ToArray(), "application/pdf")
                        {
                            FileDownloadName = "qrcode.pdf"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error generating PDF with QR code: {ex.Message}");
            }
        }


        [HttpPost]
        [Route("/PostFromQR")]
        public IActionResult PostFromQR(InventoryTransactionItemDTOInsertUpdate inventoryTransactionItemDTO)
        {
            try
            {
                var transaction = _context.InventoryTransactions.Find(inventoryTransactionItemDTO.transactionId);
                if (transaction == null)
                {
                    throw new Exception("There is no Inventory Transaction with ID: " + transaction.Id + " in database.");
                }

                var warehouse = _context.Warehouses.Find(inventoryTransactionItemDTO.warehouseId);
                if (warehouse == null)
                {
                    throw new Exception("There is no Warehouse with ID: " + warehouse.Id + " in database.");
                }

                var product = _context.Products.Find(inventoryTransactionItemDTO.productId);
                if (product == null)
                {
                    throw new Exception("There is no Product with ID: " + product.Id + " in database.");
                }


                var entity = _mapper.MapInsertUpdatedFromDTO(inventoryTransactionItemDTO);

                entity.InventoryTransaction = transaction;
                entity.Product = product;
                entity.Warehouse = warehouse;

                _context.InventoryTransactionItems.Add(entity);
                _context.SaveChanges();

                return CreatedAtAction(nameof(PostFromQR), new { id = entity.Id }, entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
