# API Export Excel Documentation

## Tổng quan
Các API export Excel cho phép xuất dữ liệu expenses và incomes ra file Excel (.xlsx) để phân tích và báo cáo.

## API Endpoints

### 1. Export Expenses to Excel

#### Export tất cả expenses
```
GET /api/expenses/export-excel
```

**Response:**
- Content-Type: `application/vnd.openxmlformats-officedocument.spreadsheetml.sheet`
- File name: `expenses.xlsx`
- Download file Excel chứa tất cả expenses

#### Export expenses theo forUserId
```
GET /api/expenses/export-excel/{forUserId}
```

**Parameters:**
- `forUserId` (string): ID của user để lọc expenses
- `month` (optional, int): Tháng (1-12)
- `year` (optional, int): Năm (ví dụ: 2025)

**Response:**
- Content-Type: `application/vnd.openxmlformats-officedocument.spreadsheetml.sheet`
- File name: `expenses_{forUserId}.xlsx` hoặc `expenses_{forUserId}_{year}_{month}.xlsx`
- Download file Excel chứa expenses của user cụ thể (có thể filter theo tháng/năm)

### 2. Export Incomes to Excel

#### Export tất cả incomes
```
GET /api/incomes/export-excel
```

**Response:**
- Content-Type: `application/vnd.openxmlformats-officedocument.spreadsheetml.sheet`
- File name: `incomes.xlsx`
- Download file Excel chứa tất cả incomes

#### Export incomes theo forUserId
```
GET /api/incomes/export-excel/{forUserId}
```

**Parameters:**
- `forUserId` (string): ID của user để lọc incomes
- `month` (optional, int): Tháng (1-12)
- `year` (optional, int): Năm (ví dụ: 2025)

**Response:**
- Content-Type: `application/vnd.openxmlformats-officedocument.spreadsheetml.sheet`
- File name: `incomes_{forUserId}.xlsx` hoặc `incomes_{forUserId}_{year}_{month}.xlsx`
- Download file Excel chứa incomes của user cụ thể (có thể filter theo tháng/năm)

## Cấu trúc file Excel

### Expenses Excel
| Cột | Tên | Mô tả |
|-----|-----|-------|
| 1 | STT | Số thứ tự |
| 2 | Tiêu đề | Tên expense |
| 3 | Số tiền | Số tiền chi tiêu |
| 4 | Danh mục | Category ID |
| 5 | Trạng thái | Status ID |
| 6 | Ngày | Ngày tạo (format: dd/MM/yyyy) |
| 7 | Mô tả | Mô tả chi tiết |

### Incomes Excel
| Cột | Tên | Mô tả |
|-----|-----|-------|
| 1 | STT | Số thứ tự |
| 2 | Tiêu đề | Tên income |
| 3 | Số tiền | Số tiền thu nhập |
| 4 | Ngày | Ngày tạo (format: dd/MM/yyyy) |
| 5 | Mô tả | Mô tả chi tiết |

## Ví dụ sử dụng

### Frontend JavaScript
```javascript
// Export tất cả expenses
const exportAllExpenses = async () => {
  try {
    const response = await fetch('/api/expenses/export-excel', {
      method: 'GET',
      headers: {
        'Accept': 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
      }
    });
    
    if (response.ok) {
      const blob = await response.blob();
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = 'expenses.xlsx';
      a.click();
      window.URL.revokeObjectURL(url);
    }
  } catch (error) {
    console.error('Export failed:', error);
  }
};

// Export expenses theo forUserId
const exportExpensesByUserId = async (forUserId, month = null, year = null) => {
  try {
    let url = `/api/expenses/export-excel/${forUserId}`;
    const params = new URLSearchParams();
    if (month) params.append('month', month);
    if (year) params.append('year', year);
    if (params.toString()) {
      url += `?${params.toString()}`;
    }
    
    const response = await fetch(url, {
      method: 'GET',
      headers: {
        'Accept': 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
      }
    });
    
    if (response.ok) {
      const blob = await response.blob();
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      
      let fileName = `expenses_${forUserId}`;
      if (month && year) {
        fileName += `_${year}_${month.toString().padStart(2, '0')}`;
      }
      fileName += '.xlsx';
      
      a.download = fileName;
      a.click();
      window.URL.revokeObjectURL(url);
    }
  } catch (error) {
    console.error('Export failed:', error);
  }
};

// Export incomes theo forUserId
const exportIncomesByUserId = async (forUserId, month = null, year = null) => {
  try {
    let url = `/api/incomes/export-excel/${forUserId}`;
    const params = new URLSearchParams();
    if (month) params.append('month', month);
    if (year) params.append('year', year);
    if (params.toString()) {
      url += `?${params.toString()}`;
    }
    
    const response = await fetch(url, {
      method: 'GET',
      headers: {
        'Accept': 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
      }
    });
    
    if (response.ok) {
      const blob = await response.blob();
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      
      let fileName = `incomes_${forUserId}`;
      if (month && year) {
        fileName += `_${year}_${month.toString().padStart(2, '0')}`;
      }
      fileName += '.xlsx';
      
      a.download = fileName;
      a.click();
      window.URL.revokeObjectURL(url);
    }
  } catch (error) {
    console.error('Export failed:', error);
  }
};
```

### cURL Examples
```bash
# Export tất cả expenses
curl -X GET "http://localhost:5001/api/expenses/export-excel" \
  -H "Accept: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" \
  --output expenses.xlsx

# Export expenses theo forUserId
curl -X GET "http://localhost:5001/api/expenses/export-excel/6878b124f71d34e241ebf8f3" \
  -H "Accept: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" \
  --output expenses_6878b124f71d34e241ebf8f3.xlsx

# Export tất cả incomes
curl -X GET "http://localhost:5001/api/incomes/export-excel" \
  -H "Accept: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" \
  --output incomes.xlsx

# Export incomes theo forUserId
curl -X GET "http://localhost:5001/api/incomes/export-excel/6878b124f71d34e241ebf8f3" \
  -H "Accept: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" \
  --output incomes_6878b124f71d34e241ebf8f3.xlsx

# Export expenses theo forUserId + tháng/năm
curl -X GET "http://localhost:5001/api/expenses/export-excel/6878b124f71d34e241ebf8f3?month=7&year=2025" \
  -H "Accept: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" \
  --output expenses_6878b124f71d34e241ebf8f3_2025_07.xlsx

# Export incomes theo forUserId + tháng/năm
curl -X GET "http://localhost:5001/api/incomes/export-excel/6878b124f71d34e241ebf8f3?month=7&year=2025" \
  -H "Accept: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" \
  --output incomes_6878b124f71d34e241ebf8f3_2025_07.xlsx
```

## Lưu ý

1. **EPPlus License**: Sử dụng license NonCommercial cho EPPlus
2. **File Format**: Xuất ra file Excel (.xlsx) với định dạng chuẩn
3. **Headers**: Các cột header được format với font bold và background màu xám
4. **Auto-fit**: Các cột tự động điều chỉnh độ rộng theo nội dung
5. **Error Handling**: Có xử lý lỗi và trả về response phù hợp
6. **CORS**: Đảm bảo CORS được cấu hình đúng để frontend có thể gọi API

## Dependencies

- **EPPlus**: Version 7.0.5 - Thư viện tạo file Excel
- **MongoDB.Driver**: Version 2.21.0 - Truy vấn dữ liệu
- **.NET 8**: Framework chính 