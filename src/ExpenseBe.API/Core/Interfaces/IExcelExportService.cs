using ExpenseBe.Core.Models;

namespace ExpenseBe.Core.Interfaces
{
    public interface IExcelExportService
    {
        Task<byte[]> ExportExpensesToExcel(IEnumerable<Expense> expenses);
        byte[] ExportIncomesToExcel(IEnumerable<Income> incomes);
    }
} 