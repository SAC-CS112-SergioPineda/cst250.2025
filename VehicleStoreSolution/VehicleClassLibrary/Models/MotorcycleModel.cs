
// Thomas Pineda, CST-250 – MotorcycleModel extends VehicleModel
namespace VehicleClassLibrary.Models;

public class MotorcycleModel : VehicleModel
{
    public bool HasSideCar { get; set; }
    public decimal SeatHeight { get; set; }

    public MotorcycleModel() : base() { }

    public MotorcycleModel(int id, string make, string model, int year, decimal price, int numWheels,
                           bool hasSideCar, decimal seatHeight)
        : base(id, make, model, year, price, numWheels)
    {
        HasSideCar = hasSideCar;
        SeatHeight = seatHeight;
    }

    public override string ToString()
        => base.ToString() + $" | SideCar: {(HasSideCar ? "Yes" : "No")}, Seat: {SeatHeight}";
}
