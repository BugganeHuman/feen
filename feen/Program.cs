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
            .AddChoices("Do Record", "Actions with Records" , "Escape")
    );
    if (choice == "Escape")
    {
        break;
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
    else if (choice == "Actions with Records")
    {
        while (true)
        {
            string choice_action= AnsiConsole.Prompt(
               new SelectionPrompt<string>()
               .Title("Choose the action: ")
               .HighlightStyle(new Style(Color.SeaGreen1))
               .AddChoices("Show Records", "Update Record","Delete Record", "Get Back"));

            if (choice_action == "Get Back") { break; }
            
            else if (choice_action == "Show Records")
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

            else if (choice_action == "Update Record")
            {
                int id = AnsiConsole.Ask<int>("id of record: ");
                int? amount = AnsiConsole.Prompt(
                    new TextPrompt<int?>("amount, or Enter to skip: ")
                    .DefaultValue(null)
                    .HideDefaultValue()
                    );
                string? note = AnsiConsole.Prompt(
                    new TextPrompt<string?>("note, or Enter to skip: ")
                    .DefaultValue(null)
                    .HideDefaultValue()
                    );
                Database.UpdateRecord(id, amount, note);
                AnsiConsole.MarkupLine("\n[green]DONE[/]\n");
                
            }

            else if (choice_action == "Delete Record")
            {
                int id = AnsiConsole.Ask<int>("id of record: ");
                string isSure = AnsiConsole.Ask<string>("\nAre you sure?\n" +
                                                        "Write yes to delete\n" +
                                                        "Or no to get back: ");
                if (isSure == "yes".ToLower())
                {
                    Database.DeleteRecord(id);
                    AnsiConsole.MarkupLine("\n[green]DONE[/]\n");
                }
                else
                {
                    AnsiConsole.MarkupLine("\n[red]REJECTING[/]\n");
                }
            }

        }
    }


}