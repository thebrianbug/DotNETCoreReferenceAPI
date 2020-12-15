using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoService.Models;
using TodoService.Services;

namespace TodoService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodosController(ITodoService todoRepository)
        {
            _todoService = todoRepository;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoDTO>>> GetTodoItems()
        {
            return await _todoService.GetAllAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoDTO>> GetTodoItem(long id)
        {
            return await _todoService.GetByIdAsync(id);
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoDTO todoDTO)
        {
            if (id != todoDTO.Id)
                return BadRequest();

            var todo = await _todoService.GetByIdAsync(id);

            if (todo == null)
                return NotFound();

            try
            {
                await _todoService.UpdateAsync(todoDTO);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _todoService.ExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoDTO>> PostTodoItem(TodoDTO todoDTO)
        {
            await _todoService.AddAsync(todoDTO);

            return CreatedAtAction(nameof(GetTodoItem), new { id = todoDTO.Id }, todoDTO);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var found = await _todoService.DeleteAsync(id);

            if (!found)
                return NotFound();

            return NoContent();
        }
    }
}
