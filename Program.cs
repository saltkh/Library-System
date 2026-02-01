using System;


class Program
{
    static void Main(string[] args)

    {
        Library library = new Library();

        var item1 = LibraryFactory.CreateLibraryItem("Book", "001", "The Great Gatsby", "F. Scott Fitzgerald", "Fiction");
        if (item1 != null) library.AddItem(item1);

        var item2 = LibraryFactory.CreateLibraryItem("Book", "005", "lord of rings", "F. Scott Fitzgerald", "Fiction");
        if (item2 != null) library.AddItem(item2);

        var item3 = LibraryFactory.CreateLibraryItem("Magazine", "002", "National Geographic", "Various Authors", "Science");
        if (item3 != null) library.AddItem(item3);

        var item4 = LibraryFactory.CreateLibraryItem("Magazine", "003", "The New york time", "Various Authors", "drama");
        if (item4 != null) library.AddItem(item4);

        var item5 = LibraryFactory.CreateLibraryItem("ResearchPaper", "004", "Quantum Physics", "Albert Einstein", "Physics");
        if (item5 != null) library.AddItem(item5);

        Register libraryRegister = new Register();
        Borrow borrowSystem = new Borrow();

        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("-------------Welcome to the Library System-----------");
            Console.WriteLine("1.Admin Login"); // verify them with password , username, be able to add, remove, search items in library , view members details . 
            Console.WriteLine("2.User Login");
            Console.WriteLine("3.Exit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine() ?? string.Empty;

            switch (choice)
            {
                case "1":
                    bool adminExit = false;
                    while (!adminExit)
                    {
                        Console.WriteLine("----------Welcome to the Library System-----------");
                        Console.WriteLine("1. Add a new item to the library");
                        Console.WriteLine("2. Search for an item in the library");
                        Console.WriteLine("3. Remove an item from the library");
                        Console.WriteLine("4. View all items in the library");
                        Console.WriteLine("5. Exit");
                        Console.Write("Enter your choice: ");
                        string choice2 = Console.ReadLine()?.Trim() ?? string.Empty;

                        switch (choice2)
                        {
                            case "1":
                                Console.WriteLine("Enter the type of item (Book, Magazine, ResearchPaper): ");
                                string type = Console.ReadLine() ?? string.Empty;
                                Console.WriteLine("Enter the item number: ");
                                string number = Console.ReadLine() ?? string.Empty;
                                Console.WriteLine("Enter the item title: ");
                                string title = Console.ReadLine() ?? string.Empty;
                                Console.WriteLine("Enter the author: ");
                                string author = Console.ReadLine() ?? string.Empty;
                                Console.WriteLine("Enter the genre: ");
                                string genre = Console.ReadLine() ?? string.Empty;

                                LibraryItem? newItem = LibraryFactory.CreateLibraryItem(type, number, title, author, genre);
                                if (newItem != null)
                                {
                                    library.AddItem(newItem);
                                }
                                else
                                {
                                    Console.WriteLine("Failed to create library item. Please check your input.");
                                }
                                break;

                            case "2":
                                Console.Write("Enter the key word of the library item to search: ");
                                string searchKeyWord = Console.ReadLine() ?? string.Empty;
                                library.SearchItem(searchKeyWord);
                                break;

                            case "3":
                                Console.Write("Enter the item number to remove: ");
                                string removeNumber = Console.ReadLine() ?? string.Empty;

                                break;

                            case "4":
                                Console.WriteLine("All items in the library:");
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
                        Console.WriteLine();
                        Console.WriteLine("----------Welcome to the Library System-----------");
                        Console.WriteLine("1. Register a new member");
                        Console.WriteLine("2. Search for a member");
                        Console.WriteLine("3. Borrow an item");
                        Console.WriteLine("4. Return an item");
                        Console.WriteLine("5. Fees");
                        Console.WriteLine("6. Exit");
                        Console.Write("Enter your choice: ");
                        string choice1 = Console.ReadLine()?.Trim() ?? string.Empty;

                        switch (choice1)
                        {
                            case "1":
                                libraryRegister.RegisterMember();
                                break;

                            case "2":
                                Console.Write("Enter the member ID to search: ");
                                string searchId = Console.ReadLine() ?? string.Empty;
                                libraryRegister.SearchMember(searchId);
                                break;

                            case "3":
                                Console.Write("Enter your member ID: ");
                                string borrowMemberId = Console.ReadLine() ?? string.Empty;

                                Member ? borrowMember = libraryRegister.SearchMember(borrowMemberId);
                                if (borrowMember != null)
                                {
                                    Console.Write("Enter the key word of the library item to borrow: ");
                                    string itemTitle = Console.ReadLine() ?? string.Empty;

                                    LibraryItem ? itemToBorrow = library.SearchItem(itemTitle);

                                    if (itemToBorrow != null)
                                    {
                                        borrowSystem.BorrowedItems(borrowMember, itemToBorrow);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Item not found.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Member wasnot found.");
                                }

                                break;

                            case "4":
                                Console.Write("Enter your member ID: ");
                                string returnMemberId = Console.ReadLine() ?? string.Empty;

                                Member ?returnMember = libraryRegister.SearchMember(returnMemberId);
                                if (returnMember != null)
                                {
                                    Console.Write("Enter the key word of the library item to return: ");
                                    string returnItemTitle = Console.ReadLine() ?? string.Empty;

                                    LibraryItem ?titleofReturnItem = library.SearchItem(returnItemTitle);

                                    borrowSystem.ReturnItem(returnMember, titleofReturnItem!);
                                }
                                else
                                {
                                    Console.WriteLine("Member not found.");
                                }
                                break;

                            case "5":
                                Console.Write("Enter your member ID: ");
                                string feeMemberId = Console.ReadLine() ?? string.Empty;
                                Member ? memberFee = libraryRegister.SearchMember(feeMemberId);

                                if (memberFee != null)
                                {
                                    Console.WriteLine("Your fee is: " + borrowSystem.CalculateFee(memberFee));
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
                    Console.WriteLine("Exiting the program. Goodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }


        }

    }
}



