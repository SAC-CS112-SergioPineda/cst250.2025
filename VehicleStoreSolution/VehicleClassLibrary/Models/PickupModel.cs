
// Thomas Pineda, CST-250 – PickupModel extends VehicleModel
namespace VehicleClassLibrary.Models;

public class PickupModel : VehicleModel
{
    public bool HasBedCover { get; set; }
    public decimal BedSize { get; set; }

    public PickupModel() : base() { }

    public PickupModel(int id, string make, string model, int year, decimal price, int numWheels,
                       bool hasBedCover, decimal bedSize)
        : base(id, make, model, year, price, numWheels)
    {
        HasBedCover = hasBedCover;
        BedSize = bedSize;
    }

    public override string ToString()
        => base.ToString() + $" | BedCover: {(HasBedCover ? "Yes" : "No")}, Bed: {BedSize}";
}
