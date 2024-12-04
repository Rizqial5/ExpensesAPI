using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{
    public class Expenses
    {
        public int Id {get;set;}
        public string? ItemName {get;set;}
        public ExpensesCategory ItemCategory {get;set;}
        public DateTime BuyDate {get;set;}
        public float ItemPrice {get;set;}
        

    }

    public enum ExpensesCategory
    {
        Electronics,
        Groceries,
        Leisure
    }
}


