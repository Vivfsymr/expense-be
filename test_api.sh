#!/bin/bash

# API Test Script for Expense Management Backend
# Usage: ./test_api.sh

BASE_URL="http://localhost:5001"
TEST_USER_ID="6878b124f71d34e241ebf8f3"
TEST_CATEGORY_ID="507f1f77bcf86cd799439011"
TEST_STATUS_ID="507f1f77bcf86cd799439012"

echo "🚀 Starting API Tests..."
echo "================================"

# Test 1: Get all expenses
echo "📋 Test 1: Get all expenses"
curl -X GET "$BASE_URL/api/expenses" \
  -H "Content-Type: application/json" \
  -w "\nHTTP Status: %{http_code}\nTime: %{time_total}s\n" \
  -s

echo -e "\n"

# Test 2: Get expenses by forUserId
echo "📋 Test 2: Get expenses by forUserId"
curl -X GET "$BASE_URL/api/expenses/for-user/$TEST_USER_ID" \
  -H "Content-Type: application/json" \
  -w "\nHTTP Status: %{http_code}\nTime: %{time_total}s\n" \
  -s

echo -e "\n"

# Test 3: Get expenses by forUserId + month/year
echo "📋 Test 3: Get expenses by forUserId + month/year"
curl -X GET "$BASE_URL/api/expenses/for-user/$TEST_USER_ID?month=7&year=2025" \
  -H "Content-Type: application/json" \
  -w "\nHTTP Status: %{http_code}\nTime: %{time_total}s\n" \
  -s

echo -e "\n"

# Test 4: Create new expense
echo "📋 Test 4: Create new expense"
curl -X POST "$BASE_URL/api/expenses" \
  -H "Content-Type: application/json" \
  -d "{
    \"title\": \"Test Expense $(date +%s)\",
    \"amount\": 150000,
    \"description\": \"Test expense description\",
    \"forUserId\": \"$TEST_USER_ID\",
    \"categoryId\": \"$TEST_CATEGORY_ID\",
    \"statusId\": \"$TEST_STATUS_ID\"
  }" \
  -w "\nHTTP Status: %{http_code}\nTime: %{time_total}s\n" \
  -s

echo -e "\n"

# Test 5: Get all incomes
echo "📋 Test 5: Get all incomes"
curl -X GET "$BASE_URL/api/incomes" \
  -H "Content-Type: application/json" \
  -w "\nHTTP Status: %{http_code}\nTime: %{time_total}s\n" \
  -s

echo -e "\n"

# Test 6: Get incomes by forUserId
echo "📋 Test 6: Get incomes by forUserId"
curl -X GET "$BASE_URL/api/incomes/for-user/$TEST_USER_ID" \
  -H "Content-Type: application/json" \
  -w "\nHTTP Status: %{http_code}\nTime: %{time_total}s\n" \
  -s

echo -e "\n"

# Test 7: Get incomes by forUserId + month/year
echo "📋 Test 7: Get incomes by forUserId + month/year"
curl -X GET "$BASE_URL/api/incomes/for-user/$TEST_USER_ID?month=7&year=2025" \
  -H "Content-Type: application/json" \
  -w "\nHTTP Status: %{http_code}\nTime: %{time_total}s\n" \
  -s

echo -e "\n"

# Test 8: Create new income
echo "📋 Test 8: Create new income"
curl -X POST "$BASE_URL/api/incomes" \
  -H "Content-Type: application/json" \
  -d "{
    \"title\": \"Test Income $(date +%s)\",
    \"amount\": 5000000,
    \"description\": \"Test income description\",
    \"forUserId\": \"$TEST_USER_ID\"
  }" \
  -w "\nHTTP Status: %{http_code}\nTime: %{time_total}s\n" \
  -s

echo -e "\n"

# Test 9: Get all categories
echo "📋 Test 9: Get all categories"
curl -X GET "$BASE_URL/api/categories" \
  -H "Content-Type: application/json" \
  -w "\nHTTP Status: %{http_code}\nTime: %{time_total}s\n" \
  -s

echo -e "\n"

# Test 10: Get all statuses
echo "📋 Test 10: Get all statuses"
curl -X GET "$BASE_URL/api/statuses" \
  -H "Content-Type: application/json" \
  -w "\nHTTP Status: %{http_code}\nTime: %{time_total}s\n" \
  -s

echo -e "\n"

# Test 11: Get all users
echo "📋 Test 11: Get all users"
curl -X GET "$BASE_URL/api/users" \
  -H "Content-Type: application/json" \
  -w "\nHTTP Status: %{http_code}\nTime: %{time_total}s\n" \
  -s

echo -e "\n"

# Test 12: Export expenses to Excel (without download)
echo "📋 Test 12: Export expenses to Excel"
curl -X GET "$BASE_URL/api/expenses/export-excel/$TEST_USER_ID?month=7&year=2025" \
  -H "Accept: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" \
  -w "\nHTTP Status: %{http_code}\nTime: %{time_total}s\nContent-Type: %{content_type}\n" \
  -s \
  -I

echo -e "\n"

# Test 13: Export incomes to Excel (without download)
echo "📋 Test 13: Export incomes to Excel"
curl -X GET "$BASE_URL/api/incomes/export-excel/$TEST_USER_ID?month=7&year=2025" \
  -H "Accept: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" \
  -w "\nHTTP Status: %{http_code}\nTime: %{time_total}s\nContent-Type: %{content_type}\n" \
  -s \
  -I

echo -e "\n"

echo "✅ API Tests completed!"
echo "================================"
echo "📝 Check the responses above for any errors"
echo "💡 Use the detailed examples in API_Test_Examples.md for more testing" 