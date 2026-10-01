namespace feen;

using Microsoft.Data.Sqlite;
using System.Globalization;

public static class Database
{
    private static SqliteConnection OpenConnection ()
    {
        var connection = new SqliteConnection("Data Source=feen_db.db");
        connection.Open();
        return connection;
    }

    public static void Start()
    {
        using var connection = OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = """
            CREATE TABLE IF NOT EXISTS records (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                created_at TEXT DEFAULT CURRENT_TIMESTAMP,
                amount INTEGER NOT NULL,
                note TEXT
            );
            """;
        cmd.ExecuteNonQuery();
    }

    public static void AddRecord(int amount, string? note )
    {
        using var connection = OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = """
            INSERT INTO records (amount, note) VALUES ($amount, $note)
            """;
        cmd.Parameters.AddWithValue("$amount", amount);
        cmd.Parameters.AddWithValue("$note", note ?? "");
        cmd.ExecuteNonQuery();
    }

    public static List<Record> GetAllRecords()
    {
        using var connection = OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = """
            SELECT id, created_at, amount, note FROM records 
            """;
        using var reader = cmd.ExecuteReader();
        List<Record> records = [];
        while (reader.Read())
        {
            records.Add( new Record (
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetInt32(2),
                reader.GetString(3)
                ));
        }
        return records;
    }

    public static void DeleteRecord(int id)
    {
        using var connection = OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = """
            DELETE FROM records WHERE id = $id
            """;
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }

    public static void UpdateRecord(int id, int? amount = null, string? note = null)
    {
        using var connection = OpenConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = """
            UPDATE records SET 
            amount = COALESCE($amount, amount), 
            note = COALESCE($note, note) 
            WHERE id = $id
            """;
        cmd.Parameters.AddWithValue("$amount", (object?)amount ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$note", (object?)note ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }
}
