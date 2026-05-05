public abstract class Member
{
    public string Id;
    public string FullName; //database needs it 
    public  List<LibraryItem> BorrowedBooks;
    public string MemberType;

    public Member(string id, string fullname, string membertype)
    {
        Id = id;
        FullName = fullname;
        BorrowedBooks = new List<LibraryItem>();
        MemberType = membertype;
    }

    public void Details()
    {
        string borrowedBooks = "";
        if (BorrowedBooks.Count > 0)
        {
            foreach (var book in BorrowedBooks)
            {
                borrowedBooks += book.Title +"("+book.Number +")"+ ",";
                
            }
        }
        else
        {
            
        Console.WriteLine($"{Id}:{FullName} ({MemberType}) hasn't borrowed any books yet.");
        return;
        }

        Console.WriteLine($"{Id}:{FullName} ({MemberType}) has borrowed {borrowedBooks} )");
    }

    
    public decimal MembershipFee { get; protected set; }
    public int MaxBorrowDuration { get; protected set; }
    public decimal LateReturnFee { get; protected set; }
      public int BorroweLimit{get; protected set;}
    public bool CanAccessEveryItem { get; protected set; }
      public void MembershipDetails()
    {

        Console.WriteLine($"Membership Fee: {MembershipFee:C} , Max Borrow Duration: {MaxBorrowDuration} days , Late Return Fee: {LateReturnFee:C} per day , Access to All Items: {CanAccessEveryItem} ,Borrowed Book Limit: {BorroweLimit}");
    }

}
class PremiumMember : Member
{
    public PremiumMember(string id, string fullname) : base(id, fullname, "Premium")
    {
        BorroweLimit = 25;
        MembershipFee=30.0m;
        MaxBorrowDuration=30;
        LateReturnFee=0.5m;
        CanAccessEveryItem=true;
    }
    

}

class StandartMember : Member
{
    public StandartMember(string id, string fullname) : base(id, fullname, "Standard")
    {
        BorroweLimit = 10;
        MembershipFee=10.0m;
        MaxBorrowDuration=12;
        LateReturnFee=1.0m;
        CanAccessEveryItem=false;

    }

}


public class Register
{
    private List<Member> members = new List<Member>();

    public void LoadMember(Member member)
    {
        if (!members.Exists(m => m.Id == member.Id))
            members.Add(member);
    }
    
    public Member? RegisterMember()
        {
            Console.WriteLine("---------Member Registration---------");
            Console.Write("Enter your ID: ");
            string id = Console.ReadLine() ?? string.Empty;

            if (members.Exists(m => m.Id == id))
            {
                Console.WriteLine("Member with this ID already exists.");
                return null;
            }

            Console.Write("Enter your full name: ");
            string fullname = Console.ReadLine() ?? string.Empty;

            // Show membership options cleanly without dummy instances
            Console.WriteLine("\nChoose membership type:");
            Console.WriteLine("1. Premium  — £30/month, borrow up to 25 items, 30 days, £0.50/day late fee");
            Console.WriteLine("2. Standard — £10/month, borrow up to 10 items, 12 days, £1.00/day late fee");
            Console.Write("Enter your choice (1 or 2): ");

            string membershipChoice = Console.ReadLine() ?? string.Empty;
            while (membershipChoice != "1" && membershipChoice != "2")
            {
                Console.WriteLine("Invalid choice. Enter 1 or 2.");
                membershipChoice = Console.ReadLine() ?? string.Empty;
            }

            Member newMember = membershipChoice == "1"
                ? new PremiumMember(id, fullname)
                : new StandartMember(id, fullname);

            members.Add(newMember);
            Console.WriteLine($"Registered successfully as {newMember.MemberType} member.");
            return newMember;
        }

        public Member? SearchMember(string id)
        {
            Member? member = members.FirstOrDefault(m => m.Id == id);
            if (member != null)
                member.Details();
            else
                Console.WriteLine("Member not found.");
            return member;
        }
    }


