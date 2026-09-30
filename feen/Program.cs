using feen;
using Spectre.Console;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("Hi I am Feen, your simple fin-tracker\n");
Database.Start();
while (true)
{
    string choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Choose the action: ")
            .HighlightStyle(new Style(Color.SeaGreen1))
            .AddChoices("Do Record", "Show Records", "Escape")
    );
    if (choice == "Escape")
    {
        break;
    }
    else if (choice == "Show Records")
    {
        List<Record> records = Database.GetAllRecords();
        var table = new Table();
        table.AddColumn("id"); 
        table.AddColumn("created_at");
        table.AddColumn("amount");
        table.AddColumn("note");
        foreach (Record record in records)
        {
            table.AddRow(record.id.ToString(), record.created_at,
                         record.amount.ToString(), Markup.Escape(record.note));
        }
        AnsiConsole.Write(table);
    }
    else if (choice == "Do Record")
    {
        while (true)
        {
            Console.Write("\nWrite the amount: ");
            bool isAmountInt = int.TryParse(Console.ReadLine(), out int amount);
            if (!isAmountInt)
            {
                continue;
            }
            Console.Write("\nWrite the note: ");
            string? note = Console.ReadLine();
            Database.AddRecord(amount, note);
            break;
        }
    }
}