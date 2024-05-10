using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using InventoryManagementAPP.Models;
using InventoryManagementAPP.Data;
using InventoryManagementAPP.Mappers;

using iTextSharp.text;
using iTextSharp.text.pdf;

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
        public IActionResult GenerateSingleQR(int productId)
        {
            try
            {
                var qrCodeContent = $"https://inventorymanagement.runasp.net/api/v1/QRCode/PostFromQR?transactionId=1&warehouseId=1&productId={productId}&quantity=1";
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


        [HttpGet]
        [Route("GenerateAllQRs")]
        public IActionResult GenerateAllQRs()
        {
            try
            {
                using (var qrStream = new MemoryStream())
                {
                    Document document = new Document();
                    PdfWriter pdfWriter = PdfWriter.GetInstance(document, qrStream);

                    document.Open();

                    var products = GetProducts();
                    if (products == null)
                    {
                        return BadRequest(ModelState);
                    }

                    var table = new PdfPTable(3) { WidthPercentage = 100 };

                    foreach (var product in products)
                    {
                        var qrContent = $"https://inventorymanagement.runasp.net?transactionId=1&warehouseId=1&productId={product.Id}&quantity=1";
                        var qrBytes = GenerateQRCode(qrContent);

                        var qrImage = iTextSharp.text.Image.GetInstance(qrBytes);
                        qrImage.ScaleToFit(100, 100);
                        qrImage.Alignment = Element.ALIGN_CENTER;

                        var cell = new PdfPCell
                        {
                            Padding = 10,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        };

                        cell.AddElement(new Paragraph($"{product.ProductName}"));
                        cell.AddElement(qrImage);

                        table.AddCell(cell);
                    }

                    while (table.Rows[table.Rows.Count - 1].GetCells().Length < 3)
                    {
                        table.AddCell("");
                    }

                    document.Add(table);

                    document.Close();

                    return new FileContentResult(qrStream.ToArray(), "application/pdf")
                    {
                        FileDownloadName = "QRCodes.pdf"
                    };
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error generating PDF with QR code: {ex.Message}");
            }
        }


        private List<Product> GetProducts()
        {
            var products = _context.Products.ToList();
            return products;
        }


        private byte[] GenerateQRCode(string content)
        {
            QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
            QRCodeData qRCodeData = qrCodeGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            QRCode qRCode = new QRCode(qRCodeData);

            using (var qrStream = new MemoryStream())
            {
                Bitmap qrBitmap = qRCode.GetGraphic(20);
                qrBitmap.Save(qrStream, ImageFormat.Png);

                return qrStream.ToArray();
            }
        }


        [HttpGet]
        [Route("PostFromQR")]
        public IActionResult PostFromQR([FromQuery] int transactionId, [FromQuery] int warehouseId, [FromQuery] int productId, [FromQuery] int quantity)
        {
            try
            {
                var transaction = _context.InventoryTransactions.Find(transactionId);
                if (transaction == null)
                {
                    throw new Exception("There is no Inventory Transaction with ID: " + transaction.Id + " in database.");
                }

                var warehouse = _context.Warehouses.Find(warehouseId);
                if (warehouse == null)
                {
                    throw new Exception("There is no Warehouse with ID: " + warehouse.Id + " in database.");
                }

                var product = _context.Products.Find(productId);
                if (product == null)
                {
                    throw new Exception("There is no Product with ID: " + product.Id + " in database.");
                }

                var existingItem = _context.InventoryTransactionItems.FirstOrDefault(item => item.InventoryTransaction.Id == transactionId && item.Product.Id == productId);
                if (existingItem != null)
                {
                    return BadRequest("Product already exists in this transaction!");
                }

                var entity = new InventoryTransactionItem
                {
                    InventoryTransaction = transaction,
                    Warehouse = warehouse,
                    Product = product,
                    Quantity = quantity
                };

                _context.InventoryTransactionItems.Add(entity);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status201Created, _mapper.MapReadToDTO(entity));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
