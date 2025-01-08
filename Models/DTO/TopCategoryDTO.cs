public class TopCategoryExpenseDTO
{
    public string YearCategory { get; set; } = null!; //Cannot Be Null
    public decimal YearAmount { get; set; }

    public string MonthCategory { get; set; } = null!;
    public decimal MonthAmount { get; set; }

    public string DayCategory { get; set; } = null!;
    public decimal DayAmount { get; set; }
}
