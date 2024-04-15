﻿using InventoryManagementAPP.Data;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementAPP.Controllers
{
    /// <summary>
    /// Inventory Management API controllers for Warehouses entity CRUD operations.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : InventoryManagementController<Product, ProductDTORead, ProductDTOInsertUpdate>
    {
        public ProductController(InventoryManagementContext context) : base(context)
        {
            DbSet = _context.Products;
        }


        [HttpGet]
        [Route("searchPagination/{page}")]
        public IActionResult SearchProductPagination(int page, string condition = "")
        {
            var perPage = 8;
            condition = condition.ToLower();

            try
            {
                var products = _context.Products
                    .Where(p => EF.Functions.Like(p.ProductName.ToLower(), "%" + condition + "%"))
                    .Skip((perPage * page) - perPage)
                    .Take(perPage)
                    .OrderBy(p => p.ProductName)
                    .ToList();

                return new JsonResult(_mapper.MapReadList(products));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        protected override void ControlDelete(Product entity)
        {
            var list = _context.InventoryTransactionItems
                .Include(x => x.Product)
                .Where(x => x.Product.Id == entity.Id)
                .ToList();

            if (list != null && list.Count > 0)
            {
                throw new Exception("Product can not be deleted because it is associated with transactions: ");
            }

        }

    }
}
