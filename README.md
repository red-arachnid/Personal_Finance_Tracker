# Personal Finance Tracker | A Simple CLI Project

A console-based Personal Finance Tracker application built with .NET 9. This application allows users to securely manage their financial transaction, track income and expenses, and visualise their financial health through rich command-line interface.

## Key Features
- **Secure Authentication:** User registration and login system with password hashing using `BCrypt` Library
- **Transaction Management:**
	- Add Income
	- Add Expenses
	- Smart Validation
- **Rich User Interface:** Uses `Spectre.Console` for an aesthetic CLI experience.
- **Data Visualization:** Displays a Bar Chart comparing Total Income, Total Expenses, and Net Savings.
- **Data Persistence:** Automatically saves and loads all user data and transaction to a local `data.json` file.
- **User Isolation:** Data is filtered by the logged-in user; users cannot see each other's transactions.

## Tech Stack
- **Language:** C#
- **Frameword:** .NET 9.0
- **Libraries:**
	- **[Spectre.Console](https://spectreconsole.net/):** For rendering tables, charts, and interactive prompts.
	- **[BCrypt.Net-Next](https://github.com/BcryptNet/bcrypt.net):** For secure password hashing and verification.
	- **System.Text.Json:** For data serialization and storage.

## What I Learned From This
Building this project provided hands-on experience with several core software engineering concepts:
1. **Seperation of Concern:** The project is structured into folders (`Models`, `Services`, `Data`, `UI`), separating the business logic from the user interface and data access layers.
2. **LINQ (Language Integrated Query):** Used extensively to filter transactions by user ID, calculate sums, and order data by date.
3. **Security Best Practices:** Learned why passwords should never be stored in plain text and how to implement `BCrypt` for hashing.
4. **File I/O & Serialization:** Handling data persistence by reading/writing objects to JSON files.

## How to Use It

### Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) installed on your machine.

### Installation & Run
1. **Clone the repository** (or download the source code):
	```bash
	git clone https://github.com/red-arachnid/Personal_Finance_Tracker
	cd Personal_Finance_Tracker

2. **Restore Dependencies:** This downloads `Spectre.Console` and `BCrypt.Net-Next`.
	```bash
	dotnet restore

3. **Run the Application:**
	```bash
	dotnet run

### Usage Guide
1. **Register:** Select "Register" to create a new account.

2. **Login:** Use your credentials to access the dashboard.

3. **Add Transaction:** Input an amount and category. (Note: You cannot add an expense if your savings are 0).

4. **View History:** See a table of your past financial activity.

5. **Show Statistics:** View a bar chart summary of your finances.

6. **Logout:** Securely exit to the main screen.
	

