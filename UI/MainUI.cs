using Personal_Finance_Tracker.Models;
using Personal_Finance_Tracker.Services;
using Spectre.Console;
using System.Text;

namespace Personal_Finance_Tracker.UI
{
    public class MainUI
    {
        private readonly UserService _userService;
        private readonly FinanceService _financeService;
        private User? _currentUser;

        public MainUI()
        {
            _userService = new UserService();
            _financeService = new FinanceService();
        }

        public void Start()
        {
            Console.OutputEncoding = Encoding.UTF8;
            AnsiConsole.Profile.Capabilities.Ansi = true;
            AnsiConsole.Profile.Capabilities.Unicode = true;
            AnsiConsole.Clear();

            //AnsiConsole.MarkupLine("[bold yellow]PERSONAL FINANCE TRACKER[/]");
            AnsiConsole.Write(new FigletText("Personal Finance Tracker") { Color = Color.Yellow, Justification = Justify.Center });

            //Login
            while (_currentUser == null)
            {
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Welcome! Please select an option : ")
                    .AddChoices("Login", "Register", "Exit"));

                switch (choice)
                {
                    case "Login":
                        Login();
                        break;
                    case "Register":
                        Register();
                        break;
                    case "Exit":
                        Environment.Exit(0);
                        break;
                }
            }

            ShowMainMenu();
        }

        private void Login()
        {
            string username = AnsiConsole.Ask<string>("Enter [green]Username[/] : ");
            string password = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter [green]Password[/] : ")
                .PromptStyle("red")
                .Secret());

            AnsiConsole.Status().Start("Verifying Username & Password...", ctx =>
            {
                ctx.Spinner(Spinner.Known.Arc);
                Thread.Sleep(1000);
                _currentUser = _userService.Login(username, password);
            });

            if (_currentUser == null)
                AnsiConsole.MarkupLine("[bold red]Invalid username or password![/]");
            else
                AnsiConsole.MarkupLineInterpolated($"[green]Successfully logged in as {_currentUser.Username}![/]");
        }

        private void Register()
        {
            string username = AnsiConsole.Ask<string>("Enter a [green]Username[/] : ");
            string password = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter a [green]Password[/] : ")
                .PromptStyle("red")
                .Secret());

            bool isSuccess = _userService.AddUser(username, password);

            if (isSuccess)
                AnsiConsole.MarkupLine("[green]Account created successfully! Please Login.[/]");
            else
                AnsiConsole.MarkupLine("[red]Username already exists. Try another.[/]");
        }

        private void ShowMainMenu()
        {
            const string ADD = "Add New Transaction";
            const string VIEW = "View Transaction History";
            const string STAT = "Show Statistics";
            const string CLEAR = "Clear data";
            const string EXIT = "Logout";

            while (true)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(new FigletText("Personal Finance Tracker") { Color = Color.Yellow, Justification = Justify.Center });
                AnsiConsole.Write(new Rule($"[cyan]Dashboard : [bold blue]{_currentUser.Username}[/][/]"));

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .AddChoices(ADD, VIEW, STAT, CLEAR, EXIT));

                switch (choice)
                {
                    case ADD:
                        AddTransactionScreen();
                        break;
                    case VIEW:
                        ViewHistory();
                        break;
                    case STAT:
                        ShowStats();
                        break;
                    case CLEAR:
                        ClearDataScreen();
                        break;
                    case EXIT:
                        _currentUser = null!;
                        AnsiConsole.Status().Start("Logging out...", ctx =>
                        {
                            ctx.Spinner(Spinner.Known.Arc);
                            Thread.Sleep(500);
                        });
                        Start();
                        return;
                }
            }
        }

        private void AddTransactionScreen()
        {
            decimal amount = AnsiConsole.Prompt(
                new TextPrompt<decimal>("Enter Amount : ")
                .Validate(amount => (amount > 0) ? ValidationResult.Success() : ValidationResult.Error("[red]Amount must be positive[/]")));

            TransactionType type = AnsiConsole.Prompt(
                new SelectionPrompt<TransactionType>()
                .Title("What is the transaction type : ")
                .UseConverter(type => type.ToString())
                .AddChoices(TransactionType.Income, TransactionType.Expense));

            string[] categories = (type == TransactionType.Income)
                ? new[] { "Salary", "Freelance", "Gift", "Other" }
                : new[] { "Food", "Rent", "Utilities", "Entertainment", "Other" };

            string category = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[bold]Select Category[/]")
                .AddChoices(categories));

            _financeService.AddTransaction(_currentUser.Id, amount, category, type);

            AnsiConsole.MarkupLine("[green]Transaction Saved![/]");
            AnsiConsole.MarkupLine("Press any key to continue");
            Console.ReadKey();
        }

        private void ViewHistory()
        {
            List<Transaction> transactions = _financeService.GetTransactionsByUser(_currentUser.Id);

            Table table = new Table();
            table.Border(TableBorder.Rounded);
            table.AddColumn("Date", col => col.Width(30));
            table.AddColumn("Category", col => col.Width(50));
            table.AddColumn("Amount", col => col.Width(70).RightAligned());

            foreach (Transaction t in transactions)
            {
                var color = (t.Type == TransactionType.Income) ? "green" : "red";
                var sign = (t.Type == TransactionType.Income) ? '+' : '-';

                table.AddRow(
                    t.Date.ToShortDateString(),
                    t.Category,
                    $"[{color}]{sign}${t.Amount}[/]");
            }

            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine("Press any key to continue");
            Console.ReadKey();
        }

        private void ShowStats()
        {
            var (income, expense) = _financeService.GetStats(_currentUser.Id);
            decimal savings = income - expense;

            AnsiConsole.Write(new BarChart()
                .Width(150)
                .Label("[yellow bold]Financial Overview[/]")
                .CenterLabel()
                .AddItem("Total Income", (double)income, Color.Green)
                .AddItem("Total Expense", (double)expense, Color.Red)
                .AddItem("Net Saving", (double)savings, (savings >= 0) ? Color.Blue : Color.Red));

            AnsiConsole.MarkupLine("Press any key to continue");
            Console.ReadKey();
        }

        private void ClearDataScreen()
        {
            bool confirm = AnsiConsole.Confirm("[bold]Are you sure you want to clear your data?[/]");
            if (confirm)
            {
                _financeService.ClearTransactions(_currentUser.Id);
                AnsiConsole.MarkupLineInterpolated($"[green]All transactions of [bold]{_currentUser.Username}[/] were removed successfully![/]");
            }
            else
                AnsiConsole.MarkupLine("[green]Delete process aborted...[/]");

            AnsiConsole.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    }
}
