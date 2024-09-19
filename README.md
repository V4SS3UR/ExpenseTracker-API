# ExpenseTracker API

This project is an API developed using **ASP.NET 8 Web API** as part of a technical test. Its goal is to manage expenses for users, including creating and retrieving expenses.

## Features

### POST:
- **Create expenses** for a user with extensive data validation.

### GET:
- **List expenses** by user, with sorting options available.

## Database

To simplify project execution and sharing, I chose to use an **InMemoryDatabase**, which is recreated each time the API starts. This avoids the complexity of using a persistent database for integration and testing purposes.

### Tables:
- **Users**: Stores user information.
- **Expenses**: Stores expenses associated with users.
- **ExpenseTypes**: Defines possible types of expenses.

### Initial Data:
- **2 Users**:
  - *Anthony Stark* (USD)
  - *Natasha Romanova* (RUB)
- **3 Expense Types**:
  - *Restaurant*
  - *Hotel*
  - *Misc*

## Project Execution

1. Open the solution in Visual Studio or another compatible IDE.
2. Start the **ExpenseTracker** project, which will launch the API with a **Swagger** interface.
3. Access the Swagger interface at: [https://localhost:7095/swagger/index.html](https://localhost:7095/swagger/index.html).
4. Accept the IIS certificate in your browser if prompted.

## API Usage

### Add an Expense

To add an expense, make a **POST** request to:  
`https://localhost:7095/api/Expenses`

Example JSON payload:

```json
{
  "date": "2024-09-19T00:00:00.000Z",
  "amount": 10,
  "currency": "USD",
  "comment": "test",
  "userId": 1,
  "expenseTypeId": 1
}
```

#### Validation:
- **Date**: Cannot be in the future.
- **Date**: Cannot be older than 3 months.
- **Comment**: Required.
- **Currency**: Required and must follow the ISO 4217 format.
- **Duplicate**: No duplicate expenses for the same date and amount for a user.
- **ExpenseType**: Must exist in the database.
- **User**: Must exist in the database.
- **Currency**: Must match the user's currency.

If validation fails, an `AggregateException` is thrown with a list of detailed errors.

### List Expenses

To list a user's expenses, make a **GET** request to:  
`https://localhost:7095/api/Expenses`

#### Parameters:
- `userId` (int): The user's ID.
  - 1 for *Anthony Stark*.
  - 2 for *Natasha Romanova*.
- `sort` (string): Sort field.
  - `"date"`: Sort by date.
  - `"amount"`: Sort by amount.

### Create and Retrieve Users

Due to initial issues connecting to a local database, I added a controller to **create** and **retrieve** users, which helped with debugging.

### Other Services

#### ExpenseTypeService
- Allows **creating** and **retrieving** expense types.
- **Validation**:
  - Name is required and must be unique.

#### UserService
- Allows **creating** and **retrieving** users.
- **Validation**:
  - `firstName` is required.
  - `lastName` is required.
  - `currency` is required and must follow ISO 4217 format.

## Unit Tests

Unit tests are located in the **ExpenseTrackerTests** project and follow the **Arrange-Act-Assert** pattern.

### Test Strategy
Initially, I used mocks to simulate repository operations but switched to **InMemoryDatabase** for more realistic behavior testing. Each test uses a separate database to ensure isolation and prevent interference between tests.

### Test List:

#### CreateExpense:
- ShouldSucceed_IfValid  
- ShouldThrowException_IfCommentIsMissing  
- ShouldThrowException_IfCurrencyDoesNotMatchUserCurrency  
- ShouldThrowException_IfCurrencyIsInvalid  
- ShouldThrowException_IfCurrencyIsMissing  
- ShouldThrowException_IfDateIsInFuture  
- ShouldThrowException_IfDateIsOlderThanThreeMonths  
- ShouldThrowException_IfExpenseIsDuplicate  
- ShouldThrowException_IfExpenseTypeDoesNotExist  
- ShouldThrowException_IfUserDoesNotExist  

#### GetExpensesByUserAsync:
- ShouldReturnEmptyList_WhenNoExpenses  
- ShouldReturnExpenses_WhenValidUserId  
- ShouldThrowException_WhenInvalidSortField  

#### CreateUserAsync:
- ShouldReturnUser_WhenValidUserIsProvided  
- ShouldThrowException_WhenCurrencyIsMissing  
- ShouldThrowException_WhenFirstNameIsMissing  
- ShouldThrowException_WhenInvalidCurrency  
- ShouldThrowException_WhenLastNameIsMissing  

#### GetAllUsersAsync:
- ShouldReturnSortedUsers  
- ShouldThrowException_WhenInvalidSortField  

#### CreateExpenseTypeAsync:
- ShouldCreateExpenseType_WhenValid  
- ShouldThrowException_WhenExpenseTypeExists  
- ShouldThrowException_WhenNameIsEmpty  

#### GetAllExpensesTypeAsync:
- ShouldReturnAllExpenseTypes  

## Technical Choices

### Packages Used:
- **EntityFramework (8.0.8)**: Used for managing entities and database operations. InMemoryDatabase allows fast testing without database configuration.
- **Moq**: Initially used for unit tests but later replaced by real tests using InMemoryDatabase.
- **ISO.4217.CurrencyCodes**: Used to validate currency formats according to ISO 4217.

### DTOs:
To avoid exposing unnecessary or sensitive information, I implemented **DTOs** for **GET** and **POST** operations:
- **ExpenseCreateDTO**: Used for creating an expense.
- **ExpenseGetDTO**: Used for retrieving expenses, including the user's full name (`FirstName LastName`).

## Potential Improvements

- **OrderBy expenses**: Add the option to sort expenses in ascending or descending order.
- **Custom error**: Create a custom exception to handle and return validation errors in a clearer and more structured way.