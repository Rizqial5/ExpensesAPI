using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Models;
using SQLitePCL;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace ExpenseTracker.Controller
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly ExpensesDB _context;
        private readonly UserManager<IdentityUser> _userManager;
        
        private int nextId;
        public ExpensesController(ExpensesDB context, UserManager<IdentityUser> userManager )
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Expenses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expenses>>> GetExpenses()
        {
            var user = await _userManager.GetUserAsync(User);

            var expenses = _context.Expenses.Where(e=>e.UserId == user.Id).ToListAsync();

            

            return await expenses;
        }

        // GET: api/Expenses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Expenses>> GetExpenses(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var expenses = await _context.Expenses.FirstOrDefaultAsync(e=>e.Id == id && e.UserId == user.Id);

            if (expenses == null)
            {
                return NotFound();
            }

            return expenses;
        }

        [HttpGet("category")]
        public async Task<ActionResult<IEnumerable<Expenses>>> GetExpensesbyCategory(ExpensesCategory category)
        {
            
            var user = await _userManager.GetUserAsync(User);


            var filteredExpenses = _context.Expenses.AsQueryable();

            filteredExpenses = filteredExpenses.Where(item => item.ItemCategory == category && item.UserId == user.Id);
            

            return Ok(await filteredExpenses.ToListAsync()) ;
        }

        [HttpGet("date")]
        public async Task<ActionResult<IEnumerable<Expenses>>> GetExpensesbyCategory(
            [FromQuery, SwaggerParameter("Start Date Filter format : yyyy-mm-dd")] DateTime? startDate, 
            [FromQuery, SwaggerParameter("End Date Date Filter format : yyyy-mm-dd")] DateTime? endDate)
        {
            var user = await _userManager.GetUserAsync(User);
            var filteredExpenses = _context.Expenses.AsQueryable();

            filteredExpenses = filteredExpenses.Where(item => item.BuyDate >= startDate!.Value && item.BuyDate <= endDate!.Value && item.UserId == user.Id);

            if(filteredExpenses.ToListAsync() == null)
            {
                return NotFound();
            }
            

            return Ok(await filteredExpenses.ToListAsync()) ;
        }

        // PUT: api/Expenses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpenses(int? id, Expenses updatedExpenses)
        {
            var user = await _userManager.GetUserAsync(User);

            var expense = await _context.Expenses.FirstOrDefaultAsync(e=>e.Id == id && e.UserId == user.Id );

            if(expense == null)
            {
                return NotFound("Expense Not FOund");
            }

            expense.ItemName = updatedExpenses.ItemName;
            expense.ItemPrice = updatedExpenses.ItemPrice;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Expenses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Expenses>> PostExpenses(Expenses expenses)
        {
            var user = await _userManager.GetUserAsync(User);

            
            

            expenses.UserId = user.Id;

            expenses.User = null;
            
            Console.WriteLine(expenses.UserId);
            

            _context.Expenses.Add(expenses);

            

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExpenses", new { id = expenses.Id, UserId = expenses.UserId.ToString() }, expenses);
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

       
    }
}


