using E_Commerce_BackEnd.DAL;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericController<T> : ControllerBase where T : class
    {
        public const string IdField = "Id";
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<T> _repository;
        public GenericController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.Repository<T>();
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<T>>> GetAll()
        {
            try
            {
                // Retrieve all items from the database
                var items = await _repository.GetAll();

                if (items == null)
                {
                    return NotFound(); // If no items found, return 404 Not Found
                }

                return Ok(items); // Return 200 OK with the list of items
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<T>> GetById(int id)
        {
            try
            {
                var res = await _repository.GetById(id);

                if (res != null)
                {
                    return Ok(res);
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public virtual async Task<ActionResult<T>> CreateAsync([FromBodyAttribute] T entity)
        {
            try
            {
                if (entity == null || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _repository.Create(entity);
                await _unitOfWork.CommitAsync();
                return Ok(entity);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("BulkUpdate")]
        public virtual async Task<ActionResult<T>> BulkUpdateAsync([FromBodyAttribute] List<T> entities)
        {
            try
            {
                foreach (var entity in entities)
                {
                    await UpdateAsync(entity);
                }
                return Ok(entities);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public virtual async Task<ActionResult<T>> UpdateAsync([FromBodyAttribute] T entity)
        {
            try
            {
                if (entity == null || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _repository.Update(entity);
                await _unitOfWork.CommitAsync();
                return Ok(entity);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public virtual async Task<ActionResult<T>> DeleteAsync([FromBodyAttribute] T entity)
        {
            try
            {
                if (entity == null || !ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _repository.Delete(entity);
                await _unitOfWork.CommitAsync();
                return Accepted();
                //return StatusCode((int)HttpStatusCode.Accepted);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
