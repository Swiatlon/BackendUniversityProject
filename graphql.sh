#!/bin/bash

# Base URL (update this to the correct URL and port if necessary)
BASE_URL="https://localhost:7054/graphql"
CURL_OPTS="-H \"Content-Type: application/json\" -H \"Accept: application/json\" -k"

# Function to send GraphQL queries and mutations
 send_request() {
    local query=$1
    curl -k -X POST "$BASE_URL" -H "Content-Type: application/json" -H "Accept: application/json" -d "$query"
 }

# EMPLOYEE--------------------------------------------

# Mutation: Add employee
echo "# Mutation: Add employee"
send_request '{
  "query": "mutation { employee { addEmployee(employeeDto: { id: \"8d9fcaeb-6e16-454d-a5ac-ca3ccb598c8d\", name: \"Jan\", pesel: \"85010112345\", surname: \"Nowak\", birthDate: \"1985-01-01\", gender: MALE }) { name } } }"
}'

# Mutation: Update Employee with Account and Address
echo "# Mutation: Update Employee with Account and Address"
send_request '{
  "query": "mutation { employee { updateEmployee(employeeDto: { id: \"8d9fcaeb-6e16-454d-a5ac-ca3ccb598c8d\", name: \"Maciej\", pesel: \"85010112345\", surname: \"Nowy Nowak\", birthDate: \"1985-01-01\", gender: MALE, account: { deactivationDate: \"2024-12-31\", isActive: true, email: \"jan.nowak@example.com\", username: \"jan.nowak\" }, address: { apartmentNumber: \"10\", city: \"Update warszawy\", country: \"Niemcy\", houseNumber: \"1\", postalCode: \"00-001\", street: \"Burstag\" } }) {  name address { country }  } } }"
}'

# Query: Get all employees
echo "# Query: Get all employees"
send_request '{
  "query": "query { employee { employees { birthDate gender name pesel surname } } }"
}'

# Query: Get employee by ID
echo "# Query: Get employee by ID"
send_request '{
    "query": "query { employee { employeeById(id: \"8d9fcaeb-6e16-454d-a5ac-ca3ccb598c8d\") { birthDate gender id name pesel surname } } }"
}'

# ADDRESSES--------------------------------------------

# Mutation: Add address
echo "# Mutation: Add address"
send_request '{
    "query": "mutation { address { addAddress(addressDto: { city: \"Warszawa\", country: \"Polska\", houseNumber: \"1\", id: \"8d9fcaeb-6e16-454d-a5ac-ca3ccb598c8d\", postalCode: \"00-001\", street: \"Główna\" }) { id city } } }"
}'

# Query: Get all addresses
echo "# Query: Get all addresses"
send_request '{
  "query": "query { address { addresses { city street houseNumber postalCode country } } }"
}'

# Mutation: Update Address
echo "# Mutation: Update Address"
send_request '{
  "query": "mutation { address { updateAddress(addressDto: { apartmentNumber: \"10\", city: \"Anglia\", country: \"Zupdatowana Polska\", houseNumber: \"1\", id: \"8d9fcaeb-6e16-454d-a5ac-ca3ccb598c8d\", postalCode: \"00-001\", street: \"Mikolaja\" }) { id city street } } }"
}'

# Query: Get address by ID
echo "# Query: Get address by ID"
send_request '{
  "query": "query { address { addressById(id: \"8d9fcaeb-6e16-454d-a5ac-ca3ccb598c8d\") { id city street houseNumber postalCode country } } }"
}'

# USER ACCOUNTS--------------------------------------------

# Mutation: Add user account
echo "# Mutation: Add user account"
send_request '{
  "query": "mutation { account { addAccount(accountDto: { id: \"8d9fcaeb-6e16-454d-a5ac-ca3ccb598c8d\", email: \"example.example@example.com\", username: \"example.example\", password: \"password123\", isActive: true }) { id username } } }"
}'

# Query: Get all user accounts
echo "# Query: Get all user accounts"
send_request '{
  "query": "query { account { accounts { username email isActive } } }"
}'

# Mutation: Update user account
echo "# Mutation: Update user account"
send_request '{
  "query": "mutation { account { updateAccount(accountDto: { id: \"8d9fcaeb-6e16-454d-a5ac-ca3ccb598c8d\", email: \"updated.email@example.com\", deactivationDate: \"2024-12-31\", isActive: true, password: \"newpassword123\", username: \"updated.username\" }) { id username email isActive deactivationDate } } }"
}'

# Query: Get user account by ID
echo "# Query: Get user account by ID"
send_request '{
  "query": "query { account { accountById(id: \"8d9fcaeb-6e16-454d-a5ac-ca3ccb598c8d\") { id username email isActive } } }"
}'

# ADVANCED UPDATES ---------------------------------------------------------------

# Mutation: Update Employee Address
echo "# Mutation: Update Employee Address"
send_request '{
    "query": "mutation { employee { updateEmployeeAddress(id: \"8d9fcaeb-6e16-454d-a5ac-ca3ccb598c8d\", addressDto: { apartmentNumber: \"10\", city: \"malo\", country: \"Polska\", houseNumber: \"1\", postalCode: \"00-001\", street: \"mieszkaniowa\" }) { city } } }"
}'

# Mutation: Update Employee Account
echo "# Mutation: Update Employee Account"
send_request '{
  "query": "mutation { employee { updateEmployeeAccount(id: \"8d9fcaeb-6e16-454d-a5ac-ca3ccb598c8d\", accountDto: { deactivationDate: \"2024-12-31\", email: \"updated.email@example.com\", isActive: true, username: \"updated.username\" }) { username email isActive deactivationDate } } }"
}'


# Deletions ------------------------------------------------------------------------

# Mutation: Delete employee
echo "# Mutation: Delete employee"
send_request '{
  "query": "mutation { employee { deleteEmployee(id: \"8d9fcaeb-6e16-454d-a5ac-ca3ccb598c8d\") } }"
}'

# Mutation: Delete address
echo "# Mutation: Delete address"
send_request '{
  "query": "mutation { address { deleteAddress(id: \"8d9fcaeb-6e16-454d-a5ac-ca3ccb598c8d\") } }"
}'

# Mutation: Delete user account
echo "# Mutation: Delete user account"
send_request '{
  "query": "mutation { account { deleteAccount(id: \"8d9fcaeb-6e16-454d-a5ac-ca3ccb598c8d\") } }"
}'