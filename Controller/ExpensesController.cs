using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Models;
using SQLitePCL;
using Swashbuckle.AspNetCore.Annotations;


namespace ExpenseTracker.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly ExpensesDB _context;
        private int nextId;
        public ExpensesController(ExpensesDB context)
        {
            _context = context;
        }

        // GET: api/Expenses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expenses>>> GetExpenses()
        {
            return await _context.Expenses.ToListAsync();
        }

        // GET: api/Expenses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Expenses>> GetExpenses(int id)
        {
            var expenses = await _context.Expenses.FindAsync(id);

            if (expenses == null)
            {
                return NotFound();
            }

            return expenses;
        }

        [HttpGet("category")]
        public async Task<ActionResult<IEnumerable<Expenses>>> GetExpensesbyCategory(ExpensesCategory category)
        {

            var filteredExpenses = _context.Expenses.AsQueryable();

            filteredExpenses = filteredExpenses.Where(item => item.ItemCategory == category);
            

            return Ok(await filteredExpenses.ToListAsync()) ;
        }

        [HttpGet("date")]
        public async Task<ActionResult<IEnumerable<Expenses>>> GetExpensesbyCategory(
            [FromQuery, SwaggerParameter("Start Date Filter format : yyyy-mm-dd")] DateTime? startDate, 
            [FromQuery, SwaggerParameter("End Date Date Filter format : yyyy-mm-dd")] DateTime? endDate)
        {

            var filteredExpenses = _context.Expenses.AsQueryable();

            filteredExpenses = filteredExpenses.Where(item => item.BuyDate >= startDate.Value && item.BuyDate <= endDate.Value);

            if(filteredExpenses.ToListAsync() == null)
            {
                return NotFound();
            }
            

            return Ok(await filteredExpenses.ToListAsync()) ;
        }

        // PUT: api/Expenses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpenses(int? id, Expenses expenses)
        {
            if (id != expenses.Id)
            {
                return BadRequest();
            }

            _context.Entry(expenses).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpensesExists(id))
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

        // POST: api/Expenses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Expenses>> PostExpenses(Expenses expenses)
        {
            nextId = _context.Expenses.ToList().Count + 1; 

            expenses.Id = nextId;

            _context.Expenses.Add(expenses);



            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExpenses", new { id = expenses.Id }, expenses);
        }

        // DELETE: api/Expenses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpenses(int? id)
        {
            var expenses = await _context.Expenses.FindAsync(id);
            if (expenses == null)
            {
                return NotFound();
            }

            _context.Expenses.Remove(expenses);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExpensesExists(int? id)
        {
            return _context.Expenses.Any(e => e.Id == id);
        }
    }
}


