
// Thomas Pineda, CST-250 – StoreLogic (Business Logic Layer).
// Thin wrapper over DAO to keep presentation separate from data access.
using VehicleClassLibrary.Models;
using VehicleClassLibrary.Services.DataAccessLayer;

namespace VehicleClassLibrary.Services.BusinessLogicLayer;

public class StoreLogic
{
    private readonly StoreDAO _storeDAO;

    public StoreLogic()
    {
        _storeDAO = new StoreDAO();
    }

    public List<VehicleModel> GetInventory() => _storeDAO.GetInventory();
    public List<VehicleModel> GetShoppingCart() => _storeDAO.GetShoppingCart();

    public int AddVehicleToInventory(VehicleModel vehicle) => _storeDAO.AddVehicleToInventory(vehicle);
    public int AddVehicleToCart(int id) => _storeDAO.AddVehicleToCart(id);

    public bool WriteInventory() => _storeDAO.WriteInventory();
    public List<VehicleModel> ReadInventory() => _storeDAO.ReadInventory();

    public decimal Checkout() => _storeDAO.Checkout();
}
