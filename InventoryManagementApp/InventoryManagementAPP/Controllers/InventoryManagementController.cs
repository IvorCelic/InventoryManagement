using InventoryManagementAPP.Data;
using InventoryManagementAPP.Mappers;
using InventoryManagementAPP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementAPP.Controllers
{
    /// <summary>
    /// Abstract base controller for inventory management, supporting basic CRUD operations with generics.
    /// Includes handling of authorization, database operations, and error responses.
    /// </summary>
    [Authorize]
    public abstract class InventoryManagementController<T, TDR, TDI> : ControllerBase where T : Entity
    {
        /// <summary>
        /// The DbSet associated with the entity type for CRUD operations.
        /// </summary>
        protected DbSet<T> DbSet;

        /// <summary>
        /// Mapper for converting between entities and DTOs.
        /// </summary>
        protected Mapping<T, TDR, TDI> _mapper;

        /// <summary>
        /// Method to control deletion of an entity.
        /// This method is designed to perform checks and ensure that an entity can be deleted.
        /// If the conditions for deletion are not met, it throws an exception with a suitable message.
        /// </summary>
        /// <param name="entity">The entity to be checked for deletion.</param>
        protected abstract void ControlDelete(T entity);

        /// <summary>
        /// The database context used for data operations.
        /// </summary>
        protected readonly InventoryManagementContext _context;

        /// <summary>
        /// Initializes the controller with a given database context.
        /// </summary>
        /// <param name="context">The database context for inventory management operations.</param>
        public InventoryManagementController(InventoryManagementContext context)
        {
            _context = context;
            _mapper = new Mapping<T, TDR, TDI>();
        }

        /// <summary>
        /// Gets all entities from the database, returning them as a JSON result.
        /// </summary>
        /// <returns>Returns a JSON result with the list of entities or a bad request if there's an error.</returns>
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

        /// <summary>
        /// Gets an entity by its ID, returning it as a JSON result.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>Returns the entity as a JSON result or a bad request if there's an error.</returns>
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
                var entity = LoadEntity(id);

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

        /// <summary>
        /// Updates an existing entity by its ID using data from the provided DTO.
        /// </summary>
        /// <param name="id">The ID of the entity to update.</param>
        /// <param name="entityDTO">The data transfer object with updated entity details.</param>
        /// <returns>Returns the updated entity or a bad request in case of an error.</returns>
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
                var entityFromDB = LoadEntity(id);
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

        /// <summary>
        /// Deletes an existing entity by its ID, if no control conditions block it.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>Returns a success message if deletion is successful or a bad request in case of an error.</returns>
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
                var entityFromDB = LoadEntity(id);
                ControlDelete(entityFromDB);

                _context.Remove(entityFromDB);
                _context.SaveChanges();
                return Ok("Item successfully deleted.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Loads an entity from the database by its ID. If the entity does not exist, throws an exception.
        /// </summary>
        /// <param name="id">The ID of the entity to load.</param>
        /// <returns>Returns the loaded entity or throws an exception if not found.</returns>
        protected virtual T LoadEntity(int id)
        {
            var entityFromDB = DbSet.Find(id);
            if (entityFromDB == null)
            {
                throw new Exception("There is no entity with ID: " + id + " in database.");
            }

            return entityFromDB;
        }

        /// <summary>
        /// Loads all entities from the database.
        /// Throws an exception if no entities are found.
        /// </summary>
        /// <returns>Returns a list of all entities or throws an exception if none are found.</returns>
        protected virtual List<TDR> LoadEntites()
        {
            var list = DbSet.ToList();
            if (list == null || list.Count == 0)
            {
                throw new Exception("No data in database.");
            }

            return _mapper.MapReadList(list);
        }

        /// <summary>
        /// Creates a new entity from the provided DTO.
        /// </summary>
        /// <param name="entityDTO">The DTO containing the new entity's data.</param>
        /// <returns>Returns the created entity.</returns>
        protected virtual T CreateEntity(TDI entityDTO)
        {
            return _mapper.MapInsertUpdatedFromDTO(entityDTO);
        }

        /// <summary>
        /// Edits an existing entity using data from the provided DTO.
        /// </summary>
        /// <param name="entityDTO">The DTO with updated entity details.</param>
        /// <param name="existingEntity">The existing entity to update.</param>
        /// <returns>Returns the edited entity.</returns>
        protected virtual T EditEntity(TDI entityDTO, T services)
        {
            return _mapper.MapInsertUpdatedFromDTO(entityDTO);
        }
    }
}
