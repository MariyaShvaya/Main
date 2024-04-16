public class Employee
{
    private static Random random = new Random();  // Shared Random instance

    public string Name { get; set; }
    public int EmployeeID { get; set; }
    public string Title { get; set; }
    public DateTime StartDate { get; set; }

    public Employee(string name, string title, DateTime startDate)
    {
        Name = name;
        Title = title;
        StartDate = startDate;
        EmployeeID = GenerateUniqueID();
    }

    public Employee(string name, int employeeId, string title, DateTime startDate)
    {
        Name = name;
        EmployeeID = employeeId;
        Title = title;
        StartDate = startDate;
    }

    private int GenerateUniqueID()
    {
        lock (random) // Ensures thread safety
        {
            return random.Next(1000, 10000); // Proper range for a 4-digit ID
        }
    }
    
}

