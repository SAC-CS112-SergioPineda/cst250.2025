
// Thomas Pineda, CST-250, Vehicle Store Activity – VehicleModel (Model)
// Constructor methods are called when an instance is created. Two constructors are provided:
// 1) default (sets safe defaults), 2) parameterized (uses provided values).
// ToString is overridden so printing a VehicleModel shows its data instead of memory location.
namespace VehicleClassLibrary.Models;

public class VehicleModel
{
    public int Id { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal Price { get; set; }
    public int NumWheels { get; set; }

    // Default constructor (no inputs). Useful for creating a blank vehicle.
    public VehicleModel()
    {
        Price = 0m;  // 'm' means decimal literal in C#
        NumWheels = 4;
    }

    // Parameterized constructor (uses inputs).
    public VehicleModel(int id, string make, string model, int year, decimal price, int numWheels)
    {
        Id = id;
        Make = make;
        Model = model;
        Year = year;
        Price = price;
        NumWheels = numWheels;
    }

    public override string ToString()
        => $"{Id}: {Year} {Make} {Model} ({NumWheels} wheels) - {Price:C2}";
}
