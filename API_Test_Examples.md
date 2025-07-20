# API Test Examples

## 1. Test Expenses APIs

### Get all expenses
```bash
curl -X GET "http://localhost:5001/api/expenses" \
  -H "Content-Type: application/json"
```

### Get expenses by forUserId
```bash
curl -X GET "http://localhost:5001/api/expenses/for-user/6878b124f71d34e241ebf8f3" \
  -H "Content-Type: application/json"
```

### Get expenses by forUserId + month/year
```bash
curl -X GET "http://localhost:5001/api/expenses/for-user/6878b124f71d34e241ebf8f3?month=7&year=2025" \
  -H "Content-Type: application/json"
```

### Create new expense
```bash
curl -X POST "http://localhost:5001/api/expenses" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Test Expense",
    "amount": 150000,
    "description": "Test expense description",
    "forUserId": "6878b124f71d34e241ebf8f3",
    "categoryId": "507f1f77bcf86cd799439011",
    "statusId": "507f1f77bcf86cd799439012"
  }'
```

### Update expense
```bash
curl -X PUT "http://localhost:5001/api/expenses/507f1f77bcf86cd799439013" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Updated Test Expense",
    "amount": 200000,
    "description": "Updated description",
    "forUserId": "6878b124f71d34e241ebf8f3",
    "categoryId": "507f1f77bcf86cd799439011",
    "statusId": "507f1f77bcf86cd799439012"
  }'
```

### Delete expense
```bash
curl -X DELETE "http://localhost:5001/api/expenses/507f1f77bcf86cd799439013" \
  -H "Content-Type: application/json"
```

## 2. Test Incomes APIs

### Get all incomes
```bash
curl -X GET "http://localhost:5001/api/incomes" \
  -H "Content-Type: application/json"
```

### Get incomes by forUserId
```bash
curl -X GET "http://localhost:5001/api/incomes/for-user/6878b124f71d34e241ebf8f3" \
  -H "Content-Type: application/json"
```

### Get incomes by forUserId + month/year
```bash
curl -X GET "http://localhost:5001/api/incomes/for-user/6878b124f71d34e241ebf8f3?month=7&year=2025" \
  -H "Content-Type: application/json"
```

### Create new income
```bash
curl -X POST "http://localhost:5001/api/incomes" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Test Income",
    "amount": 5000000,
    "description": "Test income description",
    "forUserId": "6878b124f71d34e241ebf8f3"
  }'
```

### Update income
```bash
curl -X PUT "http://localhost:5001/api/incomes/507f1f77bcf86cd799439014" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Updated Test Income",
    "amount": 6000000,
    "description": "Updated income description",
    "forUserId": "6878b124f71d34e241ebf8f3"
  }'
```

### Delete income
```bash
curl -X DELETE "http://localhost:5001/api/incomes/507f1f77bcf86cd799439014" \
  -H "Content-Type: application/json"
```

## 3. Test Export Excel APIs

### Export all expenses to Excel
```bash
curl -X GET "http://localhost:5001/api/expenses/export-excel" \
  -H "Accept: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" \
  --output expenses_all.xlsx
```

### Export expenses by forUserId
```bash
curl -X GET "http://localhost:5001/api/expenses/export-excel/6878b124f71d34e241ebf8f3" \
  -H "Accept: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" \
  --output expenses_user.xlsx
```

### Export expenses by forUserId + month/year
```bash
curl -X GET "http://localhost:5001/api/expenses/export-excel/6878b124f71d34e241ebf8f3?month=7&year=2025" \
  -H "Accept: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" \
  --output expenses_user_2025_07.xlsx
```

### Export all incomes to Excel
```bash
curl -X GET "http://localhost:5001/api/incomes/export-excel" \
  -H "Accept: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" \
  --output incomes_all.xlsx
```

### Export incomes by forUserId
```bash
curl -X GET "http://localhost:5001/api/incomes/export-excel/6878b124f71d34e241ebf8f3" \
  -H "Accept: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" \
  --output incomes_user.xlsx
```

### Export incomes by forUserId + month/year
```bash
curl -X GET "http://localhost:5001/api/incomes/export-excel/6878b124f71d34e241ebf8f3?month=7&year=2025" \
  -H "Accept: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" \
  --output incomes_user_2025_07.xlsx
```

## 4. Test Categories APIs

### Get all categories
```bash
curl -X GET "http://localhost:5001/api/categories" \
  -H "Content-Type: application/json"
```

### Create new category
```bash
curl -X POST "http://localhost:5001/api/categories" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Food & Dining",
    "description": "Expenses for food and dining"
  }'
```

## 5. Test Statuses APIs

### Get all statuses
```bash
curl -X GET "http://localhost:5001/api/statuses" \
  -H "Content-Type: application/json"
```

### Create new status
```bash
curl -X POST "http://localhost:5001/api/statuses" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Pending",
    "description": "Expense is pending approval"
  }'
```

## 6. Test Users APIs

### Get all users
```bash
curl -X GET "http://localhost:5001/api/users" \
  -H "Content-Type: application/json"
```

### Create new user
```bash
curl -X POST "http://localhost:5001/api/users" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "email": "test@example.com",
    "fullName": "Test User"
  }'
```

### Login user
```bash
curl -X POST "http://localhost:5001/api/users/login" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser"
  }'
```

## 7. JavaScript Examples

### Test API calls with fetch
```javascript
// Test get expenses by forUserId + month/year
const testGetExpenses = async () => {
  try {
    const response = await fetch('http://localhost:5001/api/expenses/for-user/6878b124f71d34e241ebf8f3?month=7&year=2025');
    const data = await response.json();
    console.log('Expenses:', data);
  } catch (error) {
    console.error('Error:', error);
  }
};

// Test export expenses to Excel
const testExportExpenses = async () => {
  try {
    const response = await fetch('http://localhost:5001/api/expenses/export-excel/6878b124f71d34e241ebf8f3?month=7&year=2025', {
      headers: {
        'Accept': 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
      }
    });
    
    if (response.ok) {
      const blob = await response.blob();
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = 'test_expenses_2025_07.xlsx';
      a.click();
      window.URL.revokeObjectURL(url);
    }
  } catch (error) {
    console.error('Export failed:', error);
  }
};

// Test create expense
const testCreateExpense = async () => {
  try {
    const response = await fetch('http://localhost:5001/api/expenses', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        title: 'Test Expense',
        amount: 150000,
        description: 'Test expense description',
        forUserId: '6878b124f71d34e241ebf8f3',
        categoryId: '507f1f77bcf86cd799439011',
        statusId: '507f1f77bcf86cd799439012'
      })
    });
    
    const data = await response.json();
    console.log('Created expense:', data);
  } catch (error) {
    console.error('Error:', error);
  }
};
```

## 8. Postman Collection

### Import vào Postman
```json
{
  "info": {
    "name": "Expense Management API",
    "schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
  },
  "item": [
    {
      "name": "Get All Expenses",
      "request": {
        "method": "GET",
        "header": [],
        "url": {
          "raw": "http://localhost:5001/api/expenses",
          "protocol": "http",
          "host": ["localhost"],
          "port": "5001",
          "path": ["api", "expenses"]
        }
      }
    },
    {
      "name": "Get Expenses by User",
      "request": {
        "method": "GET",
        "header": [],
        "url": {
          "raw": "http://localhost:5001/api/expenses/for-user/6878b124f71d34e241ebf8f3?month=7&year=2025",
          "protocol": "http",
          "host": ["localhost"],
          "port": "5001",
          "path": ["api", "expenses", "for-user", "6878b124f71d34e241ebf8f3"],
          "query": [
            {
              "key": "month",
              "value": "7"
            },
            {
              "key": "year", 
              "value": "2025"
            }
          ]
        }
      }
    },
    {
      "name": "Export Expenses Excel",
      "request": {
        "method": "GET",
        "header": [
          {
            "key": "Accept",
            "value": "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
          }
        ],
        "url": {
          "raw": "http://localhost:5001/api/expenses/export-excel/6878b124f71d34e241ebf8f3?month=7&year=2025",
          "protocol": "http",
          "host": ["localhost"],
          "port": "5001",
          "path": ["api", "expenses", "export-excel", "6878b124f71d34e241ebf8f3"],
          "query": [
            {
              "key": "month",
              "value": "7"
            },
            {
              "key": "year",
              "value": "2025"
            }
          ]
        }
      }
    }
  ]
}
```

## 9. Test Data

### Sample Expense Data
```json
{
  "title": "Lunch at Restaurant",
  "amount": 150000,
  "description": "Lunch with colleagues",
  "forUserId": "6878b124f71d34e241ebf8f3",
  "categoryId": "507f1f77bcf86cd799439011",
  "statusId": "507f1f77bcf86cd799439012"
}
```

### Sample Income Data
```json
{
  "title": "Salary",
  "amount": 5000000,
  "description": "Monthly salary",
  "forUserId": "6878b124f71d34e241ebf8f3"
}
```

### Sample Category Data
```json
{
  "name": "Food & Dining",
  "description": "Expenses for food and dining"
}
```

### Sample Status Data
```json
{
  "name": "Approved",
  "description": "Expense has been approved"
}
```

## 10. Expected Responses

### Success Response
```json
{
  "success": true,
  "message": "Operation completed successfully",
  "data": {
    // Response data here
  }
}
```

### Error Response
```json
{
  "success": false,
  "message": "Error description",
  "data": null
}
```

## 11. Testing Tips

1. **Start with basic CRUD operations** before testing export features
2. **Create test data first** using POST endpoints
3. **Test filtering** with different month/year combinations
4. **Verify Excel files** are downloaded correctly
5. **Check file names** match the expected format
6. **Test with different forUserId values** to ensure proper filtering
7. **Verify CORS** if testing from frontend
8. **Check MongoDB connection** if getting database errors 