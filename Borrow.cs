public class Borrow
{
    private Dictionary<string, DateTime> borrowDates = new ();
    private Dictionary<Member, List<LibraryItem>> borrowedItems = new ();

    // Indexer — lets Program.cs do borrowSystem[member] to get their items
    public List<LibraryItem> this[Member member]
    {
        get
        {
            return borrowedItems.ContainsKey(member) ? borrowedItems[member] : new List<LibraryItem>();
        }
    }

    public void BorrowedItems<T>(Member member, T item) where T : LibraryItem
    {
        if (member.BorrowedBooks.Contains(item))
        {
            Console.WriteLine("You have already borrowed this item.");
            return;
        }
        if (member.BorrowedBooks.Count >= member.BorroweLimit)
        {
            Console.WriteLine("You have reached your borrow limit.");
            return;
        }

        DateTime borrowDate = DateTime.Now;
        member.BorrowedBooks.Add(item);
        borrowDates[item.Number] = borrowDate;

        if (!borrowedItems.ContainsKey(member))
            borrowedItems[member] = new List<LibraryItem>();
        borrowedItems[member].Add(item);

        Console.WriteLine($"'{item.Title}' borrowed. Return by: {borrowDate.AddDays(member.MaxBorrowDuration):dd MMM yyyy}");
    }

    public void ReturnItem<T>(Member member, T item) where T : LibraryItem
    {
        if (!borrowedItems.ContainsKey(member) || !borrowedItems[member].Contains(item))
        {
            Console.WriteLine("You have not borrowed this item.");
            return;
        }

        DateTime borrowDate = borrowDates.ContainsKey(item.Number)
            ? borrowDates[item.Number]
            : DateTime.Now;

        DateTime dueDate = borrowDate.AddDays(member.MaxBorrowDuration);

        // Always remove, no matter late or not
        borrowedItems[member].Remove(item);
        member.BorrowedBooks.Remove(item);
        borrowDates.Remove(item.Number);

        int lateDays = (DateTime.Now - dueDate).Days; //fix: no longer blocks other members if smb else had late fees
        if (lateDays > 0)
        {
            decimal fee = lateDays * member.LateReturnFee;
            Console.WriteLine($"'{item.Title}' returned {lateDays} day(s) late. Fee: {fee:C}");
        }
        else
        {
            Console.WriteLine($"'{item.Title}' returned successfully. Thank you!");
        }
    }

    public decimal CalculateFee(Member member, LibraryItem item)
    {
        if (!borrowDates.ContainsKey(item.Number)) return 0;
        DateTime dueDate = borrowDates[item.Number].AddDays(member.MaxBorrowDuration);
        int lateDays = (DateTime.Now - dueDate).Days;
        return lateDays > 0 ? lateDays * member.LateReturnFee : 0;
    }
}