#!/bin/bash

# Base URL
BASE_URL="https://localhost:7084"

# Function to send POST requests
send_request() {
  local endpoint=$1
  local data=$2
  curl -k -X POST "$BASE_URL/$endpoint" -H "Content-Type: application/json" -H "Accept: application/json" -d "$data"
}

# Authenticate and get JWT token
response=$(send_request "api/Auth/login" '{
  "username": "admin",
  "password": "admin"
}')

# Extract token from response using sed
token=$(echo $response | sed -n 's/.*"token":"\([^"]*\)".*/\1/p')

# Check if the token was extracted correctly
if [ -z "$token" ]; then
  echo "Authentication failed or token not found in response."
  exit 1
fi

echo "JWT Token: $token"

# Function to send authenticated requests
send_authenticated_request() {
  local method=$1
  local endpoint=$2
  local data=$3
  echo "Sending $method request to $endpoint"
  curl -k -X $method "$BASE_URL/$endpoint" -H "accept: application/json" -H "Authorization: Bearer $token" -H "Content-Type: application/json" -d "$data"
}

# ACCOUNT CONTROLLER ---------------------------------------------------

# GET all accounts
echo "# GET all accounts"
send_authenticated_request "GET" "api/Account" ""

# GET account by ID
echo "# GET account by ID"
send_authenticated_request "GET" "api/Account/by-id/1d9fcaeb-6e16-454d-a5ac-ca3ccb598c8a" ""

# GET account by username
echo "# GET account by username"
send_authenticated_request "GET" "api/Account/by-username/admin" ""

# POST add an account
echo "# POST add an account"
send_authenticated_request "POST" "api/Account" '{
  "id": "8d9fcaeb-6e16-454d-a5ac-ca3ccb598c8d",
  "email": "example.example@example.com",
  "username": "example.example",
  "password": "password123",
  "isActive": true
}'

# PUT update an account
echo "# PUT update an account"
send_authenticated_request "PUT" "api/Account/8d9fcaeb-6e16-454d-a5ac-ca3ccb598c8d" '{
  "id": "8d9fcaeb-6e16-454d-a5ac-ca3ccb598c8d",
  "email": "updated.email@example.com",
  "username": "updated.username",
  "password": "newpassword123",
  "isActive": false
}'

# DELETE an account
echo "# DELETE an account"
send_authenticated_request "DELETE" "api/Account/8d9fcaeb-6e16-454d-a5ac-ca3ccb598c8d" ""

# ADDRESS CONTROLLER ---------------------------------------------------

# GET all addresses
echo "# GET all addresses"
send_authenticated_request "GET" "api/Address" ""

# GET address by ID
echo "# GET address by ID"
send_authenticated_request "GET" "api/Address/8d9fcaeb-6e16-454d-a5ac-ca3ccb598c8a" ""

# POST add an address
echo "# POST add an address"
send_authenticated_request "POST" "api/Address" '{
  "id": "8d9fcaeb-6e16-454d-a7ac-ca3ccb598c8d",
  "city": "string",
  "street": "string",
  "houseNumber": "string",
  "apartmentNumber": "string",
  "postalCode": "string",
  "country": "string"
}'

# PUT update an address
echo "# PUT update an address"
send_authenticated_request "PUT" "api/Address/8d9fcaeb-6e16-454d-a7ac-ca3ccb598c8d" '{
  "id": "8d9fcaeb-6e16-454d-a7ac-ca3ccb598c8d",
  "city": "updatedString",
  "street": "updatedString",
  "houseNumber": "updatedString",
  "apartmentNumber": "updatedString",
  "postalCode": "updatedString",
  "country": "updatedString"
}'

# DELETE an address
echo "# DELETE an address"
send_authenticated_request "DELETE" "api/Address/8d9fcaeb-6e16-454d-a7ac-ca3ccb598c8d" ""


# EMPLOYEE CONTROLLER ---------------------------------------------------

# GET all employees
echo "# GET all employees"
send_authenticated_request "GET" "api/Employees" ""

# POST add an employee
echo "# POST add an employee"
send_authenticated_request "POST" "api/Employees" '{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa3",
  "name": "John",
  "surname": "Doe",
  "birthDate": "2024-06-09T15:36:01.329Z",
  "pesel": "12345678901",
  "gender": 0,
  "address": {
    "city": "Warszawa",
    "street": "Glowna",
    "houseNumber": "1",
    "apartmentNumber": "10",
    "postalCode": "00-001",
    "country": "Polska"
  },
  "account": {
    "username": "john.doe",
    "email": "john.doe@example.com",
    "isActive": true,
    "deactivationDate": "2024-12-31T15:36:01.329Z"
  }
}'

# GET employee by ID
echo "# GET employee by ID"
send_authenticated_request "GET" "api/Employees/3fa85f64-5717-4562-b3fc-2c963f66afa3" ""

# GET employee address by ID
echo "# GET employee address by ID"
send_authenticated_request "GET" "api/Employees/3fa85f64-5717-4562-b3fc-2c963f66afa3/address" ""

# PUT update employee address
echo "# PUT update employee address"
send_authenticated_request "PUT" "api/Employees/3fa85f64-5717-4562-b3fc-2c963f66afa3/address" '{
  "city": "Updated City",
  "street": "Updated Street",
  "houseNumber": "123",
  "apartmentNumber": "10",
  "postalCode": "12345",
  "country": "Updated Country"
}'

# PUT update employee account
echo "# PUT update employee account"
send_authenticated_request "PUT" "api/Employees/3d9fcaeb-6e16-454d-a5ac-ca3ccb598c8a/accounts" '{
  "username": "updated.username",
  "email": "updated.email@example.com",
  "isActive": true,
  "deactivationDate": "2024-12-31T15:36:01.329Z"
}'

# GET employee accounts by ID
echo "# GET employee accounts by ID"
send_authenticated_request "GET" "api/Employees/3d9fcaeb-6e16-454d-a5ac-ca3ccb598c8a/accounts" ""

# DELETE an employee
echo "# DELETE an employee"
send_authenticated_request "DELETE" "api/Employees/3fa85f64-5717-4562-b3fc-2c963f66afa3" ""