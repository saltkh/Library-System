
public class Borrow
{
    public DateTime BorrowDate { get; set; }
    public DateTime ReturnDate { get; set; }

    public bool IsLate { get; set; }

    private Dictionary<Member, List<LibraryItem>> borrowedItems = new Dictionary<Member, List<LibraryItem>>();
    

    public List<LibraryItem> this[Member Id]
    {
        get
        {
            if (borrowedItems.ContainsKey(Id))
            {
                return borrowedItems[Id];
            }
            else

            {
                return new List<LibraryItem>();
            }
        }
        set
        {
            if (!borrowedItems.ContainsKey(Id))
            {
                borrowedItems[Id] = new List<LibraryItem>();
            }
            borrowedItems[Id] = value;
        }
    }



    public void BorrowedItems<T>(Member member, T item) where T : LibraryItem
    {
        BorrowDate = DateTime.Now;

        {
            if (IsLate)
            {
                Console.WriteLine("You have a late fee. Please pay it before borrowing more items.");
                return;
            }

            if (member.BorrowedBooks.Count >= member.MaxBorrowDuration)
            {
                Console.WriteLine("You have reached the maximum number of items you can borrow.");
                return;
            }

            if(member.BorrowedBooks.Contains(item))
            {
                Console.WriteLine("You have already borrowed this item.");
                return;
            }

            member.BorrowedBooks.Add(item);
            BorrowDate = DateTime.Now;
            if (!borrowedItems.ContainsKey(member))
            {
                borrowedItems[member] = new List<LibraryItem>();
            }

            borrowedItems[member].Add(item);
            Console.WriteLine($"{item.Title} was borrowed successfully. Please return it by {BorrowDate.AddDays(member.MaxBorrowDuration)}");
        }
    }

    public void ReturnItem<T>(Member member, T item) where T : LibraryItem
    {
        if ( !borrowedItems[member].Contains(item))
        {
            Console.WriteLine("You have not borrowed this items.");
            return;
        }

        ReturnDate = DateTime.Now;

        if (ReturnDate > BorrowDate.AddDays(member.MaxBorrowDuration))
        {
            IsLate = true;
            // LateFee = member.LateReturnFee();
            Console.WriteLine($"You have returned {item.Title} late. Please pay the late fee of {member.LateReturnFee}");
        }
        else
        {
            IsLate = false;
            // LateFee = 0;
            borrowedItems[member].Remove(item);
            member.BorrowedBooks.Remove(item);
            Console.WriteLine($"{item.Title} was returned successfully.");
        }
    }


    public decimal CalculateFee(Member member)
    {
        int LateDays= (ReturnDate - BorrowDate.AddDays(member.MaxBorrowDuration)).Days;
        if (LateDays>0)
        {
            return LateDays * member.LateReturnFee;
        }
        else
        {
            return 0;
        }
    }
}
