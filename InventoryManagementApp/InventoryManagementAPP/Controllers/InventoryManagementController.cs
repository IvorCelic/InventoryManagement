﻿using InventoryManagementAPP.Data;
using InventoryManagementAPP.Extensions;
using InventoryManagementAPP.Mappers;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementAPP.Controllers
{
    public abstract class InventoryManagementController<T, TDR, TDI> : ControllerBase where T : Entity
    {
        protected DbSet<T> DbSet;

        protected Mapping<T, TDR, TDI> _mapper;
        protected abstract void ControlDelete(T entity);

        protected readonly InventoryManagementContext _context;

        public InventoryManagementController(InventoryManagementContext context)
        {
            _context = context;
            _mapper = new Mapping<T, TDR, TDI>();
        }


        [HttpGet]
        public IActionResult Get()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                return new JsonResult(LoadEntites());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       
        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            if (!ModelState.IsValid || id <= 0)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var entity = FindEntity(id);

                return new JsonResult(_mapper.MapInsertUpdateToDTO(entity));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [HttpPost]
        public IActionResult Post(TDI entityDTO)
        {
            if (!ModelState.IsValid || entityDTO == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var entity = CreateEntity(entityDTO);

                _context.Add(entity);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status201Created, _mapper.MapReadToDTO(entity));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [HttpPut]
        [Route("{id:int}")]
        public IActionResult Put(int id, TDI entityDTO)
        {
            if (id <= 0 || !ModelState.IsValid || entityDTO == null)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var entityFromDB = FindEntity(id);
                _context.Entry(entityFromDB).State = EntityState.Detached;
                
                var entity = EditEntity(entityDTO, entityFromDB);
                entity.Id = id;

                _context.Update(entity);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, _mapper.MapReadToDTO(entity));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0 || !ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var entityFromDB = FindEntity(id);
                ControlDelete(entityFromDB);

                _context.Remove(entityFromDB);
                _context.SaveChanges();
                return Ok(entityFromDB + "deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        protected virtual T FindEntity(int id)
        {
            var entityFromDB = DbSet.Find(id);
            if (entityFromDB == null)
            {
                throw new Exception("There is no entity with ID: " + id + " in database.");
            }

            return entityFromDB;
        }


        protected virtual TDR LoadEntity(int id)
        {
            return _mapper.MapReadToDTO(DbSet.Find(id));
        }


        protected virtual List<TDR> LoadEntites()
        {
            var list = DbSet.ToList();
            if (list == null || list.Count == 0)
            {
                throw new Exception("No data in database.");
            }

            return _mapper.MapReadList(list);
        }


        protected virtual T CreateEntity(TDI entityDTO)
        {
            return _mapper.MapInsertUpdatedFromDTO(entityDTO);
        }


        protected virtual T EditEntity(TDI entityDTO, T services)
        {
            return _mapper.MapInsertUpdatedFromDTO(entityDTO);
        }
    }
}