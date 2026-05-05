using System;

class Program
{
    static void Main(string[] args)
    {
        // ── Start database (creates library.db file if it doesn't exist) ──
        Database db = new Database();

        // ── Load saved data from database ──
        Library library = new Library();
        foreach (var item in db.LoadAllItems())
            library.AddItem(item);

        Register libraryRegister = new Register();
        foreach (var member in db.LoadAllMembers())
            libraryRegister.LoadMember(member);

        Borrow borrowSystem = new Borrow(); // Borrow does not need DB dependency here

        // ── Only add default items if database is empty ──
        if (!library.HasItems())
        {
            var defaults = new[]
            {
                LibraryFactory.CreateLibraryItem("Book",          "001", "The Great Gatsby",   "F. Scott Fitzgerald", "Fiction"),
                LibraryFactory.CreateLibraryItem("Book",          "005", "Lord of the Rings",  "J.R.R. Tolkien",      "Fiction"),
                LibraryFactory.CreateLibraryItem("Magazine",      "002", "National Geographic","Various Authors",     "Science"),
                LibraryFactory.CreateLibraryItem("Magazine",      "003", "The New York Times", "Various Authors",     "Drama"),
                LibraryFactory.CreateLibraryItem("ResearchPaper", "004", "Quantum Physics",    "Albert Einstein",     "Physics"),
            };
            foreach (var item in defaults)
            {
                if (item != null)
                {
                    library.AddItem(item);
                    db.SaveItem(item);
                }
            }
        }

        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\n-------------Welcome to the Library System-----------");
            Console.WriteLine("1. Admin Login");
            Console.WriteLine("2. User Login");
            Console.WriteLine("3. Exit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine() ?? string.Empty;

            switch (choice)
            {
                case "1":
                    // ── Simple admin login ──
                    Console.Write("Enter admin username: ");
                    string username = Console.ReadLine() ?? string.Empty;
                    Console.Write("Enter admin password: ");
                    string password = Console.ReadLine() ?? string.Empty;

                    if (username != "admin" || password != "1234")
                    {
                        Console.WriteLine("Invalid credentials. Returning to main menu.");
                        break;
                    }

                    bool adminExit = false;
                    while (!adminExit)
                    {
                        Console.WriteLine("\n----------Admin Menu-----------");
                        Console.WriteLine("1. Add a new item");
                        Console.WriteLine("2. Search for an item");
                        Console.WriteLine("3. Remove an item");
                        Console.WriteLine("4. View all items");
                        Console.WriteLine("5. Exit");
                        Console.Write("Enter your choice: ");
                        string choice2 = Console.ReadLine()?.Trim() ?? string.Empty;

                        switch (choice2)
                        {
                            case "1":
                                Console.Write("Enter the type (Book, Magazine, ResearchPaper): ");
                                string type = Console.ReadLine() ?? string.Empty;
                                Console.Write("Enter the item number: ");
                                string number = Console.ReadLine() ?? string.Empty;
                                Console.Write("Enter the title: ");
                                string title = Console.ReadLine() ?? string.Empty;
                                Console.Write("Enter the author: ");
                                string author = Console.ReadLine() ?? string.Empty;
                                Console.Write("Enter the genre: ");
                                string genre = Console.ReadLine() ?? string.Empty;

                                LibraryItem? newItem = LibraryFactory.CreateLibraryItem(type, number, title, author, genre);
                                if (newItem != null)
                                {
                                    library.AddItem(newItem);
                                    db.SaveItem(newItem);         // 💾 save to database
                                    Console.WriteLine("Item added and saved.");
                                }
                                else
                                {
                                    Console.WriteLine("Failed to create item. Check your input.");
                                }
                                break;

                            case "2":
                                Console.Write("Enter keyword to search: ");
                                string searchKeyWord = Console.ReadLine() ?? string.Empty;
                                library.SearchItem(searchKeyWord);
                                break;

                            case "3":
                                Console.Write("Enter the item number to remove: ");
                                string removeNumber = Console.ReadLine() ?? string.Empty;
                                if (library.RemoveItem(removeNumber))
                                {
                                    db.DeleteItem(removeNumber);  // 💾 delete from database
                                    Console.WriteLine("Item removed.");
                                }
                                else
                                {
                                    Console.WriteLine("Item not found.");
                                }
                                break;

                            case "4":
                                library.ViewAllItems();
                                break;

                            case "5":
                                adminExit = true;
                                Console.WriteLine("Returning to main menu.");
                                break;

                            default:
                                Console.WriteLine("Invalid choice. Please try again.");
                                break;
                        }
                    }
                    break;

                case "2":
                    bool userExit = false;
                    while (!userExit)
                    {
                        Console.WriteLine("\n----------User Menu-----------");
                        Console.WriteLine("1. Register as a new member");
                        Console.WriteLine("2. Search for a member");
                        Console.WriteLine("3. Borrow an item");
                        Console.WriteLine("4. Return an item");
                        Console.WriteLine("5. Check fees");
                        Console.WriteLine("6. Exit");
                        Console.Write("Enter your choice: ");
                        string choice1 = Console.ReadLine()?.Trim() ?? string.Empty;

                        switch (choice1)
                        {
                            case "1":
                                Member? newMember = libraryRegister.RegisterMember();
                                if (newMember != null)
                                {
                                    db.SaveMember(newMember);     // 💾 save to database
                                    Console.WriteLine("Member registered and saved.");
                                }
                                break;

                            case "2":
                                Console.Write("Enter member ID to search: ");
                                string searchId = Console.ReadLine() ?? string.Empty;
                                libraryRegister.SearchMember(searchId);
                                break;

                            case "3":
                                Console.Write("Enter your member ID: ");
                                string borrowMemberId = Console.ReadLine() ?? string.Empty;
                                Member? borrowMember = libraryRegister.SearchMember(borrowMemberId);
                                if (borrowMember != null)
                                {
                                    Console.Write("Enter keyword of item to borrow: ");
                                    string itemTitle = Console.ReadLine() ?? string.Empty;
                                    LibraryItem? itemToBorrow = library.SearchItem(itemTitle);
                                    if (itemToBorrow != null)
                                    {
                                        borrowSystem.BorrowedItems(borrowMember, itemToBorrow);

                                        // record in DB only if borrow succeeded (item present in member's borrowed list)
                                        if (borrowMember.BorrowedBooks.Contains(itemToBorrow))
                                        {
                                            db.SaveBorrow(borrowMember, itemToBorrow, DateTime.UtcNow);
                                            Console.WriteLine("Borrow recorded in database.");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Borrow failed (checks blocked borrowing).");
                                        }
                                    }
                                    else
                                        Console.WriteLine("Item not found.");
                                }
                                else
                                {
                                    Console.WriteLine("Member not found.");
                                }
                                break;

                            case "4":
                                Console.Write("Enter your member ID: ");
                                string returnMemberId = Console.ReadLine() ?? string.Empty;
                                Member? returnMember = libraryRegister.SearchMember(returnMemberId);
                                if (returnMember != null)
                                {
                                    Console.Write("Enter keyword of item to return: ");
                                    string returnItemTitle = Console.ReadLine() ?? string.Empty;
                                    LibraryItem? itemToReturn = library.SearchItem(returnItemTitle);
                                    if (itemToReturn != null)
                                    {
                                        // call ReturnItem (it will validate and remove from in-memory structures)
                                        borrowSystem.ReturnItem(returnMember, itemToReturn);

                                        // if the member no longer has the item in their borrowed list, remove DB borrow row
                                        if (!returnMember.BorrowedBooks.Contains(itemToReturn))
                                        {
                                            db.DeleteBorrow(returnMember, itemToReturn);
                                            Console.WriteLine("Return recorded in database.");
                                        }
                                    }
                                    else
                                        Console.WriteLine("Item not found.");
                                }
                                else
                                {
                                    Console.WriteLine("Member not found.");
                                }
                                break;

                            case "5":
                                Console.Write("Enter your member ID: ");
                                string feeMemberId = Console.ReadLine() ?? string.Empty;
                                Member? memberFee = libraryRegister.SearchMember(feeMemberId);
                                if (memberFee == null)
                                {
                                    Console.WriteLine("Member not found.");
                                    break;
                                }

                                Console.WriteLine("1. Show total fees for all borrowed items");
                                Console.WriteLine("2. Show fee for a specific item");
                                Console.Write("Choose option (1 or 2): ");
                                string feeChoice = Console.ReadLine()?.Trim() ?? string.Empty;

                                if (feeChoice == "1")
                                {
                                    decimal total = 0m;
                                    foreach (var it in borrowSystem[memberFee])
                                        total += borrowSystem.CalculateFee(memberFee, it);
                                    Console.WriteLine($"Your total fee is: {total:C}");
                                }
                                else if (feeChoice == "2")
                                {
                                    Console.Write("Enter the item title to check fee: ");
                                    var title = Console.ReadLine() ?? string.Empty;
                                    var it = library.SearchItem(title);
                                    if (it != null)
                                        Console.WriteLine($"Fee for \"{it.Title}\": {borrowSystem.CalculateFee(memberFee, it):C}");
                                    else
                                        Console.WriteLine("Item not found.");
                                }
                                else
                                {
                                    Console.WriteLine("Invalid option.");
                                }
                                break;

                            case "6":
                                userExit = true;
                                Console.WriteLine("Returning to main menu.");
                                break;

                            default:
                                Console.WriteLine("Invalid choice. Please try again.");
                                break;
                        }
                    }
                    break;

                case "3":
                    exit = true;
                    Console.WriteLine("Exiting. Goodbye!");
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}
