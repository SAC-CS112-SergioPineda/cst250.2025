
// Thomas Pineda, CST-250 – StoreDAO (Data Access Layer).
// Holds inventory and cart in-memory and reads/writes inventory to a text file.
using System.Text;
using VehicleClassLibrary.Models;

namespace VehicleClassLibrary.Services.DataAccessLayer;

public class StoreDAO
{
    private readonly List<VehicleModel> _inventory;
    private readonly List<VehicleModel> _shoppingCart;

    private readonly string _dataDir;
    private readonly string _filePath;

    public StoreDAO()
    {
        _inventory = new();
        _shoppingCart = new();
        _dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        _filePath = Path.Combine(_dataDir, "inventory.txt");
    }

    public List<VehicleModel> GetInventory() => _inventory;
    public List<VehicleModel> GetShoppingCart() => _shoppingCart;

    public int AddVehicleToInventory(VehicleModel vehicle)
    {
        // Assign next ID (starts at 1). No delete means count+1 is safe for a new unique ID.
        vehicle.Id = _inventory.Count + 1;
        _inventory.Add(vehicle);
        return vehicle.Id;
    }

    public int AddVehicleToCart(int id)
    {
        foreach (var v in _inventory)
        {
            if (v.Id == id)
            {
                _shoppingCart.Add(v);
                break;
            }
        }
        return _shoppingCart.Count;
    }

    public decimal Checkout()
    {
        decimal total = 0m;
        foreach (var v in _shoppingCart)
        {
            total += v.Price;
        }
        _shoppingCart.Clear();
        return total;
    }

    // Persist inventory to a text file (very simple line format)
    public bool WriteInventory()
    {
        try
        {
            Directory.CreateDirectory(_dataDir);
            using var sw = new StreamWriter(_filePath, false, Encoding.UTF8);
            foreach (var v in _inventory)
            {
                switch (v)
                {
                    case CarModel c:
                        sw.WriteLine($"Car|{c.Make}|{c.Model}|{c.Year}|{c.Price}|{c.NumWheels}|{c.IsConvertible}|{c.TrunkSize}");
                        break;
                    case MotorcycleModel m:
                        sw.WriteLine($"Motorcycle|{m.Make}|{m.Model}|{m.Year}|{m.Price}|{m.NumWheels}|{m.HasSideCar}|{m.SeatHeight}");
                        break;
                    case PickupModel p:
                        sw.WriteLine($"Pickup|{p.Make}|{p.Model}|{p.Year}|{p.Price}|{p.NumWheels}|{p.HasBedCover}|{p.BedSize}");
                        break;
                    default:
                        sw.WriteLine($"Vehicle|{v.Make}|{v.Model}|{v.Year}|{v.Price}|{v.NumWheels}");
                        break;
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    // Read inventory from file and replace current memory list
    public List<VehicleModel> ReadInventory()
    {
        _inventory.Clear();
        if (!File.Exists(_filePath))
            return _inventory;

        try
        {
            using var sr = new StreamReader(_filePath, Encoding.UTF8);
            string? line;
            while ((line = sr.ReadLine()) is not null)
            {
                var parts = line.Split('|');
                if (parts.Length < 6) continue;
                var type = parts[0];
                var make = parts[1];
                var model = parts[2];
                int year = ParseInteger(parts[3]);
                decimal price = ParseDecimal(parts[4]);
                int wheels = ParseInteger(parts[5]);

                VehicleModel v;
                switch (type)
                {
                    case "Car":
                        bool isConv = parts.Length > 6 && ParseBoolean(parts[6]);
                        decimal trunk = parts.Length > 7 ? ParseDecimal(parts[7]) : 0m;
                        v = new CarModel(0, make, model, year, price, wheels, isConv, trunk);
                        break;
                    case "Motorcycle":
                        bool hasSide = parts.Length > 6 && ParseBoolean(parts[6]);
                        decimal seat = parts.Length > 7 ? ParseDecimal(parts[7]) : 0m;
                        v = new MotorcycleModel(0, make, model, year, price, wheels, hasSide, seat);
                        break;
                    case "Pickup":
                        bool hasBed = parts.Length > 6 && ParseBoolean(parts[6]);
                        decimal bed = parts.Length > 7 ? ParseDecimal(parts[7]) : 0m;
                        v = new PickupModel(0, make, model, year, price, wheels, hasBed, bed);
                        break;
                    default:
                        v = new VehicleModel(0, make, model, year, price, wheels);
                        break;
                }
                AddVehicleToInventory(v); // assigns consistent Id
            }
        }
        catch
        {
            // ignore and return whatever we have
        }
        return _inventory;
    }

    // Safe parsers (each returns a default if parsing fails)
    private static int ParseInteger(string s)
        => int.TryParse(s, out var val) ? val : 0;

    private static decimal ParseDecimal(string s)
        => decimal.TryParse(s, out var val) ? val : 0m;

    private static bool ParseBoolean(string s)
        => bool.TryParse(s, out var val) ? val : false;
}
