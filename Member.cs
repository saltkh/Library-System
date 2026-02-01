public abstract class Member
{
    public string Id;
    private string FullName;
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
    public void RegisterMember()
    {
        Member standartMember = new StandartMember("1", "Standart Member");
        Member premiumMember = new PremiumMember("2", "Premium Member");

        Console.WriteLine("---------Member Registration---------");
        Console.WriteLine("Enter your Id");
        string id = Console.ReadLine()?? string.Empty;
        if(members.Exists(m => m.Id == id))
        {
            Console.WriteLine("Member with this id already exists. Please try again.");
            return;
        }

        Console.WriteLine("Enter your full name");
        string fullname = Console.ReadLine() ?? string.Empty;


        Console.WriteLine("Choose membership type:");

        Console.WriteLine("1. Premium Membership");
        premiumMember.MembershipDetails();
        Console.WriteLine(" 2. Standard Membership");
        standartMember.MembershipDetails();


        Console.Write("Enter your choice (1 or 2): ");
        string membershipChoice = Console.ReadLine() ?? string.Empty;

        while (membershipChoice != "1" && membershipChoice != "2")
        {
            Console.WriteLine("Invalid choice. Please try again.");
            Console.Write("Enter your choice (1 or 2): ");
            membershipChoice = Console.ReadLine() ?? string.Empty;
        }
        if (membershipChoice == "1")
        {
            Member newMember = new PremiumMember(id, fullname);
            members.Add(newMember);
            Console.WriteLine("You have successfully registered as a Premium Member.");
        }
        else if (membershipChoice == "2")
        {
            Member newMember = new StandartMember(id, fullname);
            members.Add(newMember);
            Console.WriteLine("You have successfully registered as a Standard Member.");
        }
    }
     public Member? SearchMember(string id)
        {
            
            if (members.Exists(m => m.Id == id))
            {
                Member? member = members.FirstOrDefault(m => m.Id == id);

                if (member != null)
                {
                    member.Details();
                }
                return member;
            }
            else
            {
                Console.WriteLine("Member not found.");
                return null;
            }
        }
}
      