using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Models.Entities;
using Server.Services;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using System.Drawing;

[ApiController]
[Route("api/[controller]")]

public class ExpenseController : ControllerBase
{
    private readonly List<string> categoryList = new List<string>
    {
        "Food", "Clothes", "Home", "Transportation", "Bills", "Entertainment"
    };

    private readonly ApplicationDbContext _context;
    private readonly ExpenseServices _expense;
    private readonly JwtToken _jwt;

    public ExpenseController(ApplicationDbContext context, ExpenseServices expense, JwtToken jwt)
    {
        _context = context;
        _expense = expense;
        _jwt = jwt;
    }

    [Authorize]   //Can be used here as well if you want to apply per-action authorization
    [HttpPost("add-expense")]
    public  async Task<IActionResult> AddExpense([FromBody] ExpenseDTO expense)
    {
 
        try
        {
            //Extracting userId from the JWT token
            var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            int userId = _jwt.FetchUserIdfromJWT(jwtToken);


            // Validate the category
            if (!categoryList.Contains(expense.Category, StringComparer.OrdinalIgnoreCase))
            {
                return BadRequest(new { message = "Incorrect category. Please choose from the available categories: Food, Clothes, Home, Transportation, Bills, Entertainment." });
            }

            // Create a new expense object
            var newExpense = new Expense
            {
                UserId = userId,
                Amount = expense.Amount,
                Category = expense.Category,
                Comment = expense.Comment,
               Timestamp = DateTime.Now
            };

            // Add the new expense to the database
            _context.Expenses.Add(newExpense);
            await _context.SaveChangesAsync();

            // Return a success response with the new expense
            return CreatedAtAction(nameof(AddExpense), new { id = newExpense.DebitId }, newExpense);
        }
        
        catch (Exception ex)
        {
            // Return a 500 error if there's an issue saving to the database
            return StatusCode(500, new { message = "An error occurred while adding the expense.", error = ex.Message });
        }
    }

    [HttpGet("fetch-expenses")]
    [Authorize]
    public async Task<IActionResult> FetchExpenses()
    {
        try
        {
            var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            int userId = _jwt.FetchUserIdfromJWT(jwtToken);
            List<Expense> expenses = await _expense.FetchExpense(userId);
            return Ok(expenses);
        }

        catch(Exception ex){
            return StatusCode(500, new { message = "An error occurred while adding the expense.", error = ex.Message });
        }     
    }

    [HttpPost("delete-expense")]
    [Authorize] 
    public async Task<IActionResult> DeleteExpense(DeleteExpenseDTO delete)
    {
        try
        {

            var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            int userId = _jwt.FetchUserIdfromJWT(jwtToken);

            bool isDeleted = await _expense.DeleteExpenseAsync(delete.DebitID, userId);

            if (isDeleted)
            {
                return Ok(new { Message = "Expense deleted successfully." });
            }
            else
            {
                return NotFound(new { Message = "Expense not found." });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while deleting the expense.", Error = ex.Message });
        }
    }

    [HttpGet("top-expense-category")]
    [Authorize]
    public async Task<TopCategoryExpenseDTO> TopExpenseCategory()
    {
         var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
         int userId = _jwt.FetchUserIdfromJWT(jwtToken);

        TopCategoryExpenseDTO expenseResult = await _expense.FetchTopCategory(userId);
        return expenseResult;
    }


    [HttpPut("update-expense")]
    [Authorize]
    public async Task<IActionResult> UpdateExpense([FromBody] EditExpenseDTO editExpenseDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid data provided.");
        }

        
        var isUpdated = await _expense.UpdateExpenseAsync(
            editExpenseDto.DebitID,
            editExpenseDto.Amount,
            editExpenseDto.Category,
            editExpenseDto.Comment
        );

        if (!isUpdated)
        {
            return NotFound("Expense not found.");
        }

        return Ok("Expense updated successfully.");
    }
}
