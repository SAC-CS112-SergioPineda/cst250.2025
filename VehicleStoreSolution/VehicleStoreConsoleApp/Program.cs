
// Thomas Pineda, CST-250 – Console App (top-level statements).
// This file contains local 'static' functions (often called functions instead of methods
// because they're not inside a class). Everything is written in simple terms.

using VehicleClassLibrary.Models;
using VehicleClassLibrary.Services.BusinessLogicLayer;

Console.WriteLine("Welcome to the Vehicle Store!");
Console.WriteLine("--------------------------------------------------");
ControlLoop();

// Reads a menu choice (int) safely from the user.
static int ReadChoice()
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("Menu:");
        Console.WriteLine("1) View Inventory");
        Console.WriteLine("2) View Shopping Cart");
        Console.WriteLine("3) Add Vehicle to Inventory");
        Console.WriteLine("4) Add Vehicle to Cart (by Id)");
        Console.WriteLine("5) Checkout");
        Console.WriteLine("6) Save Inventory to File");
        Console.WriteLine("7) Load Inventory from File");
        Console.WriteLine("0) Exit");
        Console.Write("Enter your choice: ");

        var input = Console.ReadLine();
        if (int.TryParse(input, out int choice) && choice >= 0 && choice <= 7)
            return choice;

        Console.WriteLine("Please enter a number 0-7.");
    }
}

// Main control loop that runs the store.
static void ControlLoop()
{
    var logic = new StoreLogic();

    while (true)
    {
        int choice = ReadChoice();
        Console.WriteLine();

        switch (choice)
        {
            case 0:
                Console.WriteLine("Goodbye!");
                return;

            case 1:
                Console.WriteLine("Inventory:");
                foreach (var v in logic.GetInventory())
                    Console.WriteLine(" - " + v);
                break;

            case 2:
                Console.WriteLine("Shopping Cart:");
                foreach (var v in logic.GetShoppingCart())
                    Console.WriteLine(" - " + v);
                break;

            case 3:
                AddVehicleFlow(logic);
                break;

            case 4:
                Console.Write("Enter Vehicle Id to add to cart: ");
                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    var count = logic.AddVehicleToCart(id);
                    Console.WriteLine($"Cart now has {count} item(s).");
                }
                else
                {
                    Console.WriteLine("Invalid Id.");
                }
                break;

            case 5:
                var total = logic.Checkout();
                Console.WriteLine($"Checkout complete. Total: {total:C2}.");
                break;

            case 6:
                Console.WriteLine(logic.WriteInventory()
                    ? "Inventory saved."
                    : "Failed to save inventory.");
                break;

            case 7:
                logic.ReadInventory();
                Console.WriteLine("Inventory loaded from file.");
                break;
        }
    }
}

// Helper to create and add a vehicle.
static void AddVehicleFlow(StoreLogic logic)
{
    Console.WriteLine("Select vehicle type:");
    Console.WriteLine("1) Vehicle (base)");
    Console.WriteLine("2) Car");
    Console.WriteLine("3) Motorcycle");
    Console.WriteLine("4) Pickup");
    Console.Write("Type: ");
    int type = int.TryParse(Console.ReadLine(), out var t) ? t : 1;

    Console.Write("Make: ");
    string make = Console.ReadLine() ?? "";
    Console.Write("Model: ");
    string model = Console.ReadLine() ?? "";
    Console.Write("Year: ");
    int year = int.TryParse(Console.ReadLine(), out var y) ? y : 0;
    Console.Write("Price: ");
    decimal price = decimal.TryParse(Console.ReadLine(), out var p) ? p : 0m;
    Console.Write("Wheels: ");
    int wheels = int.TryParse(Console.ReadLine(), out var w) ? w : 4;

    VehicleModel v;
    switch (type)
    {
        default:
        case 1:
            v = new VehicleModel(0, make, model, year, price, wheels);
            break;
        case 2:
            Console.Write("Is convertible (true/false): ");
            bool isConv = bool.TryParse(Console.ReadLine(), out var b1) ? b1 : false;
            Console.Write("Trunk size (decimal): ");
            decimal trunk = decimal.TryParse(Console.ReadLine(), out var d1) ? d1 : 0m;
            v = new CarModel(0, make, model, year, price, wheels, isConv, trunk);
            break;
        case 3:
            Console.Write("Has sidecar (true/false): ");
            bool side = bool.TryParse(Console.ReadLine(), out var b2) ? b2 : false;
            Console.Write("Seat height (decimal): ");
            decimal seat = decimal.TryParse(Console.ReadLine(), out var d2) ? d2 : 0m;
            v = new MotorcycleModel(0, make, model, year, price, wheels, side, seat);
            break;
        case 4:
            Console.Write("Has bed cover (true/false): ");
            bool bed = bool.TryParse(Console.ReadLine(), out var b3) ? b3 : false;
            Console.Write("Bed size (decimal): ");
            decimal bedSize = decimal.TryParse(Console.ReadLine(), out var d3) ? d3 : 0m;
            v = new PickupModel(0, make, model, year, price, wheels, bed, bedSize);
            break;
    }

    int newId = logic.AddVehicleToInventory(v);
    Console.WriteLine($"Added: {v} (assigned Id {newId}).");
}
