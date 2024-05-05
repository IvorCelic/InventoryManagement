using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Drawing.Imaging;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
using InventoryManagementAPP.Models;
using InventoryManagementAPP.Data;
using InventoryManagementAPP.Mappers;
using System.Collections.Generic;

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
        public async Task<ActionResult> GenerateQRCode()
        {
            try
            {
                var url = $"https://localhost:7183/api/v1/QRCode?transactionId=1&warehouseId=1&productId=1&quantity=1";
            

                QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);

                using (MemoryStream ms = new MemoryStream())
                {
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);
                    qrCodeImage.Save(ms, ImageFormat.Png);
                    var bytes = ms.ToArray();
                    return File(bytes, "image/png");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
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
