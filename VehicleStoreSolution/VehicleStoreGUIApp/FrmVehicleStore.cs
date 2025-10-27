
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using VehicleClassLibrary.Models;
using VehicleClassLibrary.Services.BusinessLogicLayer;

namespace VehicleStoreGUIApp
{
    // Thomas Pineda, CST-250 – Simple GUI built entirely in code (no designer file)
    public class FrmVehicleStore : Form
    {
        private readonly StoreLogic _storeLogic;

        private readonly ListBox lstInventory = new();
        private readonly ListBox lstCart = new();
        private readonly Button btnAddToCart = new() { Text = "Add →" };
        private readonly Button btnCheckout = new() { Text = "Checkout" };
        private readonly Label lblTotal = new() { Text = "Total: $0.00", AutoSize = true };

        private readonly ComboBox cboType = new();
        private readonly TextBox txtMake = new();
        private readonly TextBox txtModel = new();
        private readonly NumericUpDown numYear = new() { Minimum = 1900, Maximum = 2100, Value = 2020 };
        private readonly NumericUpDown numPrice = new() { DecimalPlaces = 2, Maximum = 1000000, Increment = 100 };
        private readonly NumericUpDown numWheels = new() { Minimum = 1, Maximum = 18, Value = 4 };
        private readonly CheckBox chkBool = new() { Text = "Specialty Bool" };
        private readonly NumericUpDown numDec = new() { DecimalPlaces = 2, Maximum = 1000 };
        private readonly Button btnCreate = new() { Text = "Create Vehicle" };
        private readonly Button btnSave = new() { Text = "Save Inv" };
        private readonly Button btnLoad = new() { Text = "Load Inv" };

        private readonly BindingSource _invBinding = new();
        private readonly BindingSource _cartBinding = new();

        public FrmVehicleStore()
        {
            Text = "Vehicle Store";
            Width = 900;
            Height = 520;
            StartPosition = FormStartPosition.CenterScreen;

            _storeLogic = new StoreLogic();

            // Layout controls quickly (TableLayout)
            var table = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 2 };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
            Controls.Add(table);

            // Left: Inventory
            var pnlLeft = new Panel { Dock = DockStyle.Fill };
            lstInventory.Dock = DockStyle.Fill;
            pnlLeft.Controls.Add(lstInventory);
            table.Controls.Add(pnlLeft, 0, 0);

            // Middle buttons
            var pnlMid = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown };
            pnlMid.Controls.Add(btnAddToCart);
            pnlMid.Controls.Add(btnCheckout);
            pnlMid.Controls.Add(lblTotal);
            pnlMid.Controls.Add(btnSave);
            pnlMid.Controls.Add(btnLoad);
            table.Controls.Add(pnlMid, 1, 0);

            // Right: Cart
            var pnlRight = new Panel { Dock = DockStyle.Fill };
            lstCart.Dock = DockStyle.Fill;
            pnlRight.Controls.Add(lstCart);
            table.Controls.Add(pnlRight, 2, 0);

            // Bottom: Create section
            var bottom = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 8 };
            for (int i = 0; i < 8; i++) bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5f));
            bottom.Controls.Add(new Label { Text = "Type", AutoSize = true }, 0, 0);
            bottom.Controls.Add(cboType, 1, 0);
            cboType.Items.AddRange(new[] { "Vehicle", "Car", "Motorcycle", "Pickup" });
            cboType.SelectedIndex = 0;

            bottom.Controls.Add(new Label { Text = "Make", AutoSize = true }, 0, 1);
            bottom.Controls.Add(txtMake, 1, 1);
            bottom.Controls.Add(new Label { Text = "Model", AutoSize = true }, 2, 1);
            bottom.Controls.Add(txtModel, 3, 1);
            bottom.Controls.Add(new Label { Text = "Year", AutoSize = true }, 4, 1);
            bottom.Controls.Add(numYear, 5, 1);
            bottom.Controls.Add(new Label { Text = "Price", AutoSize = true }, 6, 1);
            bottom.Controls.Add(numPrice, 7, 1);

            bottom.Controls.Add(new Label { Text = "Wheels", AutoSize = true }, 0, 2);
            bottom.Controls.Add(numWheels, 1, 2);
            bottom.Controls.Add(chkBool, 2, 2);
            bottom.Controls.Add(new Label { Text = "Specialty Dec", AutoSize = true }, 4, 2);
            bottom.Controls.Add(numDec, 5, 2);
            bottom.Controls.Add(btnCreate, 7, 2);

            table.Controls.Add(bottom, 0, 1);
            table.SetColumnSpan(bottom, 3);

            // Data binding
            _invBinding.DataSource = _storeLogic.GetInventory();
            _cartBinding.DataSource = _storeLogic.GetShoppingCart();
            lstInventory.DataSource = _invBinding;
            lstCart.DataSource = _cartBinding;

            // Events
            btnCreate.Click += BtnCreate_Click;
            btnAddToCart.Click += BtnAddToCart_Click;
            btnCheckout.Click += BtnCheckout_Click;
            btnSave.Click += (s, e) => MessageBox.Show(_storeLogic.WriteInventory() ? "Saved" : "Save failed");
            btnLoad.Click += (s, e) => { _storeLogic.ReadInventory(); _invBinding.ResetBindings(false); };

            // Seed a couple of items so the UI isn't empty
            _storeLogic.AddVehicleToInventory(new VehicleModel(0, "Toyota", "Corolla", 2020, 12000m, 4));
            _storeLogic.AddVehicleToInventory(new CarModel(0, "Mazda", "MX-5", 2022, 27000m, 4, true, 5.5m));
            _invBinding.ResetBindings(false);
        }

        private void BtnAddToCart_Click(object? sender, EventArgs e)
        {
            if (lstInventory.SelectedItem is VehicleModel v)
            {
                _storeLogic.AddVehicleToCart(v.Id);
                _cartBinding.ResetBindings(false);
            }
        }

        private void BtnCheckout_Click(object? sender, EventArgs e)
        {
            var total = _storeLogic.Checkout();
            _cartBinding.ResetBindings(false);
            lblTotal.Text = $"Total: {total:C2}";
        }

        private void BtnCreate_Click(object? sender, EventArgs e)
        {
            string make = txtMake.Text.Trim();
            string model = txtModel.Text.Trim();
            int year = (int)numYear.Value;
            decimal price = numPrice.Value;
            int wheels = (int)numWheels.Value;
            bool b = chkBool.Checked;
            decimal d = numDec.Value;

            VehicleModel v;
            switch (cboType.SelectedItem?.ToString())
            {
                case "Car":
                    v = new CarModel(0, make, model, year, price, wheels, b, d);
                    break;
                case "Motorcycle":
                    v = new MotorcycleModel(0, make, model, year, price, wheels, b, d);
                    break;
                case "Pickup":
                    v = new PickupModel(0, make, model, year, price, wheels, b, d);
                    break;
                default:
                    v = new VehicleModel(0, make, model, year, price, wheels);
                    break;
            }

            _storeLogic.AddVehicleToInventory(v);
            _invBinding.ResetBindings(false);
            ClearInputs();
        }

        private void ClearInputs()
        {
            txtMake.Text = "";
            txtModel.Text = "";
            numYear.Value = 2020;
            numPrice.Value = 0;
            numWheels.Value = 4;
            chkBool.Checked = false;
            numDec.Value = 0;
            cboType.SelectedIndex = 0;
        }
    }
}
