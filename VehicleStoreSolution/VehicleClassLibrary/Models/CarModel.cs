
// Thomas Pineda, CST-250 – CarModel extends VehicleModel (inheritance + polymorphism)
namespace VehicleClassLibrary.Models;

public class CarModel : VehicleModel
{
    public bool IsConvertible { get; set; }
    public decimal TrunkSize { get; set; }

    public CarModel() : base() { }

    public CarModel(int id, string make, string model, int year, decimal price, int numWheels,
                    bool isConvertible, decimal trunkSize)
        : base(id, make, model, year, price, numWheels)
    {
        IsConvertible = isConvertible;
        TrunkSize = trunkSize;
    }

    public override string ToString()
        => base.ToString() + $" | Convertible: {(IsConvertible ? "Yes" : "No")}, Trunk: {TrunkSize}";
}
