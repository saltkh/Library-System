using System;

public interface IDetails
{
    void Details();
}
public interface ISearchable
{
    bool Search(string item);
}

public abstract class LibraryItem : IDetails, ISearchable
{
    public string Number { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Genra { get; set; }
    public string Type { get; set; }

    protected LibraryItem(string type, string number, string title, string author, string genra)
    {
        Number = number;
        Title = title;
        Author = author;
        Genra = genra;
        Type = type;
    }

    public bool Search(string item)
    {
        return Title.Contains(item, StringComparison.OrdinalIgnoreCase) ||
               Author.Contains(item, StringComparison.OrdinalIgnoreCase) ||
               Genra.Contains(item, StringComparison.OrdinalIgnoreCase) ||
               Type.Contains(item, StringComparison.OrdinalIgnoreCase) ||
               Number.Contains(item, StringComparison.OrdinalIgnoreCase);
    }
    public abstract void Details();
}


public class Book : LibraryItem
{
    public Book(string number, string title, string author, string genre) : base("book", number, title, author, genre)
    {}
    public override void Details()
    {
        Console.WriteLine($"Book: {Title} by {Author}(Book number: {Number})");
    }

}

public class Magazines : LibraryItem
{
    public Magazines(string number, string title, string author, string genre) : base("magazine", number, title, author, genre){

    }
    public override void Details()
    {
        Console.WriteLine($"Magazine: {Title} by {Author} (Magazine number: {Number})");
    }
}
public class ResearchPaper : LibraryItem
{
    public ResearchPaper(string number, string title, string author, string genre) : base("ResearchPaper", number, title, author, genre)
    {

    }
    public override void Details()
    {
        Console.WriteLine($"Research paper: {Title} by {Author} on {Genra}");
    }
}



public static class LibraryFactory
{
    public static LibraryItem? CreateLibraryItem(string type, string number, string title, string author, string genre)
    {
        try
        {
            switch (type)
            {
                case "Book":
                    return new Book(number, title, author, genre);
                case "Magazine":
                    return new Magazines(number, title, author, genre);
                case "ResearchPaper":
                    return new ResearchPaper(number, title, author, genre);
                default:
                    throw new ArgumentException("Invalid item type");
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null; 
        }
    }

}

public class Library
{
    private List<LibraryItem> items = new List<LibraryItem>();
    private Dictionary<string, LibraryItem> library = new Dictionary<string, LibraryItem>();
    public void AddItem(LibraryItem item) 
    {
        if (library.ContainsKey(item.Number))
        {
            Console.WriteLine($"Item with number {item.Number} already exists");
            return;
        }
        else
        {
            library.Add(item.Number, item);
        }
    }
    

//     public LibraryItem SearchItem(string keyWord) 
//     {
//         List<LibraryItem> foundItems = new List<LibraryItem>();
//         try 
//         {
//             if (string.IsNullOrEmpty(keyWord))
//             {
//                 throw new ArgumentException("Search item cannot be empty");
//             }
//             if (int.TryParse(keyWord, out int number) && number < 0)
//             {
//                 throw new ArgumentException("Search item cannot be a negative number");
//             }
//             bool IsAvailable = false;
//             foreach (var i in library.Values)
//             {
//                 if (i.Search(keyWord))
//                 {
//                     i.Details();
//                     foundItems.Add(i);
//                     IsAvailable = true;
//                 }
//             }
//             if (!IsAvailable)
//             {
//                 Console.WriteLine($"{keyWord} was not found");
//                 return null;
//             }
//             if (foundItems.Count > 1)
//             {
//                 Console.WriteLine("Number of item you want to choose: ");
//                 string NumberOfItem = Console.ReadLine();
//                 foreach (var num in foundItems)
//                 {
//                     if (num.Number == NumberOfItem)
//                     {
//                         num.Details();
//                         return num;
//                     }
//                 }
//             }
//             else if (foundItems.Count == 1)
//             {
//                 return foundItems[0];
//             }
//         }
//         catch (ArgumentException ex)
//         {
//             Console.WriteLine($"Error: {ex.Message}");
//             return null;
//         }
//         return null;
//     }
// }



public LibraryItem? SearchItem(string keyWord)
{
    List<LibraryItem> foundItems = new List<LibraryItem>();

    try
    {
        if (string.IsNullOrWhiteSpace(keyWord))
        {
            throw new ArgumentException("Search item cannot be empty");
        }

        if (int.TryParse(keyWord, out int number) && number < 0)
        {
            throw new ArgumentException("Search item cannot be a negative number");
        }

        keyWord = keyWord.Trim().ToLower();
        if (keyWord.EndsWith("s") && keyWord.Length > 1)
        {
            keyWord = keyWord.Substring(0, keyWord.Length - 1);
        }

        bool isAvailable = false;

        foreach (var item in library.Values)
        {
            if (item.Search(keyWord))
            {
                item.Details();
                foundItems.Add(item);
                isAvailable = true;
            }
        }

        if (!isAvailable)
        {
            Console.WriteLine($"{keyWord} was not found.");
            return null;
        }

        if (foundItems.Count > 1)
        {
            Console.Write("Enter the number of the item you want to choose: ");
            string numberOfItem = Console.ReadLine() ?? string.Empty;

            foreach (var selectedItem in foundItems)
            {
                if (selectedItem.Number == numberOfItem)
                {
                    selectedItem.Details();
                    return selectedItem;
                }
            }

            Console.WriteLine("No matching item number was selected.");
            return null;
        }
        else if (foundItems.Count == 1)
        {
            return foundItems[0];
        }
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        return null;
    }

    return null;
}
}
