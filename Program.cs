class Program
{
    static List<Employee> employees = new List<Employee>();
    static Random random = new Random();

    static void Main(string[] args)
    {
        LoadEmployees(); // Load employees from file at startup

        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Create a new employee");
            Console.WriteLine("2. View all employees");
            Console.WriteLine("3. Search for an employee by ID");
            Console.WriteLine("4. Search for employees by name");
            Console.WriteLine("5. Delete an employee by their Employee ID");
            Console.WriteLine("6. Update an employee by their Employee ID");
            Console.WriteLine("7. Exit");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        CreateEmployee();
                        break;
                    case 2:
                        ViewEmployees();
                        break;
                    case 3:
                        SearchByID();
                        break;
                    case 4:
                        SearchByName();
                        break;
                    case 5:
                        DeleteEmployee();
                        break;
                    case 6:
                        UpdateEmployee();
                        break;
                    case 7:
                        SaveEmployees(); // Save employees to file before exiting
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Please enter a valid number.");
            }
        }
    }

    private static DateTime ReadValidDate(string prompt)
    {
        DateTime dateValue;
        Console.Write(prompt);
        while (!DateTime.TryParse(Console.ReadLine(), out dateValue))
        {
            Console.Write("Invalid date, please re-enter (MM/dd/yyyy): ");
        }
        return dateValue;
    }

        static void CreateEmployee()
    {
        Console.Write("Enter Employee's name: ");
        string name = Console.ReadLine() ?? "";
        Console.Write("Enter Employee's title: ");
        string title = Console.ReadLine() ?? "";

        DateTime startDate = ReadValidDate("Enter Employee's start date (MM/dd/yyyy): ");

        int employeeId = GenerateUniqueEmployeeID();
        Employee employee = new(name, employeeId, title, startDate);
        employees.Add(employee);

        Console.WriteLine("Employee created with ID: " + employeeId);
    }

        private static void ViewEmployees()
    {
        if (!employees.Any())
        {
            Console.WriteLine("No employees to display.");
            return;
        }

        foreach (var employee in employees)
        {
            Console.WriteLine($"ID: {employee.EmployeeID}, Name: {employee.Name}, Title: {employee.Title}, Start Date: {employee.StartDate:MM/dd/yyyy}");
        }
    }

    private static void SearchByID()
    {
        Console.Write("Enter Employee ID: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var employee = employees.FirstOrDefault(e => e.EmployeeID == id);
            if (employee != null)
            {
                Console.WriteLine($"Found Employee: ID: {employee.EmployeeID}, Name: {employee.Name}, Title: {employee.Title}, Start Date: {employee.StartDate:MM/dd/yyyy}");
            }
            else
            {
                Console.WriteLine("No employee found with that ID.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }
    }

        private static void SearchByName()
    {
        Console.Write("Enter Employee Name: ");
        string name = Console.ReadLine();
        var foundEmployees = employees.Where(e => e.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

        if (foundEmployees.Any())
        {
            foreach (var employee in foundEmployees)
            {
                Console.WriteLine($"Found Employee: ID: {employee.EmployeeID}, Name: {employee.Name}, Title: {employee.Title}, Start Date: {employee.StartDate:MM/dd/yyyy}");
            }
        }
        else
        {
            Console.WriteLine("No employees found with that name.");
        }
    }

        private static void DeleteEmployee()
    {
            Console.Write("Enter Employee ID to delete: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var employee = employees.FirstOrDefault(e => e.EmployeeID == id);
            if (employee != null)
            {
                Console.WriteLine($"Are you sure you want to delete {employee.Name}? (Y/N)");
                string confirmation = Console.ReadLine().ToUpper();
                if (confirmation == "Y")
                {
                    employees.Remove(employee);
                    Console.WriteLine("Employee deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Deletion canceled.");
                }
            }
            else
            {
                Console.WriteLine("No employee found with that ID.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }
    }

        private static void UpdateEmployee()
    {
        Console.Write("Enter Employee ID to update: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var employee = employees.FirstOrDefault(e => e.EmployeeID == id);
            if (employee != null)
            {
                Console.WriteLine("Leave blank and press enter to keep current value.");

                Console.Write($"Enter new name or leave blank (Current: {employee.Name}): ");
                string newName = Console.ReadLine();
                if (!string.IsNullOrEmpty(newName))
                {
                    employee.Name = newName;
                }

                Console.Write($"Enter new title or leave blank (Current: {employee.Title}): ");
                string newTitle = Console.ReadLine();
                if (!string.IsNullOrEmpty(newTitle))
                {
                    employee.Title = newTitle;
                }

                Console.Write($"Enter new start date (MM/dd/yyyy) or leave blank (Current: {employee.StartDate:MM/dd/yyyy}): ");
                string newDateInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(newDateInput))
                {
                    DateTime newStartDate = ReadValidDate($"Enter new start date (MM/dd/yyyy): ");
                    employee.StartDate = newStartDate;
                }

                Console.WriteLine("Employee updated successfully.");
            }
            else
            {
                Console.WriteLine("No employee found with that ID.");
            }
        } 
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }
    }

    private static void SaveEmployees()
    {
        string filePath = "employees.txt";
        string tempFilePath = "employees_temp.txt";
        string backupFilePath = "employees_backup.txt";

        var lines = employees.Select(e => $"{e.Name},{e.EmployeeID},{e.Title},{e.StartDate:MM/dd/yyyy}");

        try
        {
            // Write to a temporary file first
            File.WriteAllLines(tempFilePath, lines);

            // Backup the current file before replacing it
            if (File.Exists(filePath))
            {
                File.Copy(filePath, backupFilePath, overwrite: true);
            }

            // Replace the current file with the new data
            File.Delete(filePath);
            File.Move(tempFilePath, filePath);
        }
        catch (IOException ex)
        {
            Console.WriteLine($"An error occurred while saving the file: {ex.Message}");
            // Attempt to restore the backup
            if (File.Exists(backupFilePath))
            {
                File.Copy(backupFilePath, filePath, overwrite: true);
                Console.WriteLine("The original data was restored from the backup.");
            }
        }
    }
    private static void LoadEmployees()
    {
        string filePath = "employees.txt";
        try
        {
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var data = line.Split(',');
                    if (data.Length == 4 && DateTime.TryParse(data[3], out DateTime startDate))
                    {
                        try
                        {
                            var employee = new Employee(data[0], int.Parse(data[1]), data[2], startDate);
                            employees.Add(employee);
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine($"Error parsing employee data: {line}");
                        }
                    }
                }
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
        }
    }
    static int GenerateUniqueEmployeeID()
    {
        int id;
        do
        {
            id = random.Next(1000, 10000); // Generates a 4-digit number
        }
        while (employees.Any(e => e.EmployeeID == id));
        return id;
    }

}

