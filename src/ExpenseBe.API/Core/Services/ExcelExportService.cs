using ExpenseBe.Core.Interfaces;
using ExpenseBe.Core.Models;
using OfficeOpenXml;
using System.Globalization;
using System.Linq;

namespace ExpenseBe.Core.Services
{
    public class ExcelExportService : IExcelExportService
    {
        private readonly ICategoryService _categoryService;
        private readonly IStatusService _statusService;

        public ExcelExportService(ICategoryService categoryService, IStatusService statusService)
        {
            _categoryService = categoryService;
            _statusService = statusService;
        }

        public async Task<byte[]> ExportExpensesToExcel(IEnumerable<Expense> expenses)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Expenses");
                
                // Headers
                worksheet.Cells[1, 1].Value = "Ngày";
                worksheet.Cells[1, 2].Value = "Tiêu đề";
                worksheet.Cells[1, 3].Value = "Số tiền";
                worksheet.Cells[1, 4].Value = "Danh mục";
                worksheet.Cells[1, 5].Value = "Trạng thái";
                
                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 5])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }
                
                // Get all categories and statuses for lookup
                var categories = await _categoryService.GetAllCategoriesAsync();
                var statuses = await _statusService.GetAllStatusesAsync();
                
                var categoryDict = categories.ToDictionary(c => c.Id, c => c.Name);
                var statusDict = statuses.ToDictionary(s => s.Id, s => s.Name);
                
                // Data
                int row = 2;
                foreach (var expense in expenses)
                {
                    worksheet.Cells[row, 1].Value = expense.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    worksheet.Cells[row, 2].Value = expense.Title;
                    worksheet.Cells[row, 3].Value = expense.Amount;
                    worksheet.Cells[row, 4].Value = categoryDict.ContainsKey(expense.Category) ? categoryDict[expense.Category] : expense.Category;
                    worksheet.Cells[row, 5].Value = statusDict.ContainsKey(expense.Status) ? statusDict[expense.Status] : expense.Status;
                    row++;
                }
                
                // Auto-fit columns
                worksheet.Cells.AutoFitColumns();
                
                return package.GetAsByteArray();
            }
        }
        
        public byte[] ExportIncomesToExcel(IEnumerable<Income> incomes)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Incomes");
                
                // Clear all cells first
                worksheet.Cells.Clear();
                
                // Headers
                worksheet.Cells[1, 1].Value = "Ngày";
                worksheet.Cells[1, 2].Value = "Tiêu đề";
                worksheet.Cells[1, 3].Value = "Số tiền";
                
                // Style headers
                using (var range = worksheet.Cells[1, 1, 1, 3])
                {   
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }
                
                // Data
                int row = 2;
                foreach (var income in incomes)
                {
                    worksheet.Cells[row, 1].Value = income.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    worksheet.Cells[row, 2].Value = income.Title;
                    worksheet.Cells[row, 3].Value = income.Amount;
                    row++;
                }
                
                // Set the used range to only include our data
                if (row > 2)
                {
                    worksheet.Cells[1, 1, row - 1, 3].AutoFitColumns();
                }
                
                return package.GetAsByteArray();
            }
        }
    }
} 