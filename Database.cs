
using Microsoft.Data.Sqlite;


public class Database
{
  
    private readonly string _connectionString;

    public Database(string dbPath = "library.db")
    {
        _connectionString = $"Data Source={dbPath}";
        InitialiseTables();
    }

    private SqliteConnection GetConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }

    // create table
    private void InitialiseTables()
    {
        using var connection = GetConnection();

        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Items (
                Number  TEXT PRIMARY KEY,
                Title   TEXT NOT NULL,
                Author  TEXT NOT NULL,
                Genre   TEXT NOT NULL,
                Type    TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Members (
                Id          TEXT PRIMARY KEY,
                FullName    TEXT NOT NULL,
                MemberType  TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Borrows (
                MemberId    TEXT NOT NULL,
                ItemNumber  TEXT NOT NULL,
                BorrowDate  TEXT NOT NULL,
                PRIMARY KEY (MemberId, ItemNumber)
            );
        ";
        cmd.ExecuteNonQuery();
    }

    // Items
    public void SaveItem(LibraryItem item)
    {
        using var connection = GetConnection();
        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            INSERT OR IGNORE INTO Items (Number, Title, Author, Genre, Type)
            VALUES ($number, $title, $author, $genre, $type);
        ";
        cmd.Parameters.AddWithValue("$number", item.Number);
        cmd.Parameters.AddWithValue("$title",  item.Title);
        cmd.Parameters.AddWithValue("$author", item.Author);
        cmd.Parameters.AddWithValue("$genre",  item.Genra);
        cmd.Parameters.AddWithValue("$type",   item.Type);
        cmd.ExecuteNonQuery();
    }

    public void DeleteItem(string number)
    {
        using var connection = GetConnection();
        var cmd = connection.CreateCommand();
        cmd.CommandText = "DELETE FROM Items WHERE Number = $number;";
        cmd.Parameters.AddWithValue("$number", number);
        cmd.ExecuteNonQuery();
    }

    public List<LibraryItem> LoadAllItems()
    {
        var items = new List<LibraryItem>();
        using var connection = GetConnection();
        var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT Number, Title, Author, Genre, Type FROM Items;";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            string number = reader.GetString(0);
            string title  = reader.GetString(1);
            string author = reader.GetString(2);
            string genre  = reader.GetString(3);
            string type   = reader.GetString(4);

            var item = LibraryFactory.CreateLibraryItem(type, number, title, author, genre);
            if (item != null) items.Add(item);
        }
        return items;
    }

    // Members
    public void SaveMember(Member member)
    {
        using var connection = GetConnection();
        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            INSERT OR IGNORE INTO Members (Id, FullName, MemberType)
            VALUES ($id, $fullname, $type);
        ";
        cmd.Parameters.AddWithValue("$id",       member.Id);
        cmd.Parameters.AddWithValue("$fullname",  member.FullName);
        cmd.Parameters.AddWithValue("$type",      member.MemberType);
        cmd.ExecuteNonQuery();
    }

    public List<Member> LoadAllMembers()
    {
        var members = new List<Member>();
        using var connection = GetConnection();
        var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT Id, FullName, MemberType FROM Members;";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            string id       = reader.GetString(0);
            string fullname = reader.GetString(1);
            string type     = reader.GetString(2);

            Member member = type == "Premium"
                ? new PremiumMember(id, fullname)
                : new StandartMember(id, fullname);

            members.Add(member);
        }
        return members;
    }

    // Borrrows
    public void SaveBorrow(Member member, LibraryItem item, DateTime borrowDate)
    {
        using var connection = GetConnection();
        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            INSERT OR IGNORE INTO Borrows (MemberId, ItemNumber, BorrowDate)
            VALUES ($memberId, $itemNumber, $borrowDate);
        ";
        cmd.Parameters.AddWithValue("$memberId",   member.Id);
        cmd.Parameters.AddWithValue("$itemNumber", item.Number);
        cmd.Parameters.AddWithValue("$borrowDate", borrowDate.ToString("o"));
        cmd.ExecuteNonQuery();
    }

    public void DeleteBorrow(Member member, LibraryItem item)
    {
        using var connection = GetConnection();
        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            DELETE FROM Borrows 
            WHERE MemberId = $memberId AND ItemNumber = $itemNumber;
        ";
        cmd.Parameters.AddWithValue("$memberId",   member.Id);
        cmd.Parameters.AddWithValue("$itemNumber", item.Number);
        cmd.ExecuteNonQuery();
    }

    public Dictionary<string, DateTime> LoadBorrowsForMember(string memberId)
    {
        var borrows = new Dictionary<string, DateTime>();
        using var connection = GetConnection();
        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            SELECT ItemNumber, BorrowDate FROM Borrows 
            WHERE MemberId = $memberId;
        ";
        cmd.Parameters.AddWithValue("$memberId", memberId);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            string itemNumber  = reader.GetString(0);
            DateTime borrowDate = DateTime.Parse(reader.GetString(1));
            borrows[itemNumber] = borrowDate;
        }
        return borrows;
    }
}
