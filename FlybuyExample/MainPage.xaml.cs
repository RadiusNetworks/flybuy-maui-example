using System.Collections.ObjectModel;

namespace FlybuyExample;

public partial class MainPage : ContentPage
{
    private IFlybuyService _flybuy;
    private readonly Customer _customer;
    private Order _order = null!;
    private bool _createCust = false;

    public MainPage(IFlybuyService flybuy)
    {
        InitializeComponent();

        this._flybuy = flybuy;

        _customer = flybuy.CurrentCustomer();
        if (_customer == null)
        {
            _createCust = true;
            _customer = new Customer();
        }
        else
        {
            _createCust = false;
            CustName.Text = _customer.Name;
            CustPhone.Text = _customer.Phone;
        }

        ObservableCollection<Order> orders = flybuy.GetOrders();
        if (orders.Count > 0)
        {
            _order = orders.First();
        }
    }

    private void OnCustomerClicked(object sender, EventArgs e)
    {
        _customer.Name = CustName.Text;
        _customer.Phone = CustPhone.Text;

        if (_createCust)
        {
            _flybuy.CreateCustomer(_customer);
        }
        else
        {
            _flybuy.UpdateCustomer(_customer);
        }        
    }

    private void OnOrderRedeem(object sender, EventArgs e)
    {
        _order = new Order();
        _order.Code = RedeemCode.Text;
        if (_order.Code != null)
        {
            _flybuy.ClaimOrder(_order, _customer);
        }
    }

    private void OnEnRouteClicked(object sender, EventArgs e)
    {
        if (_order.Id > 0)
        {
            _flybuy.UpdateOrder(_order, "en_route");
            Console.WriteLine("Customer En Route");
        }
    }

    private void OnImHereClicked(object sender, EventArgs e)
    {
        if (_order.Id > 0)
        {
            _flybuy.UpdateOrder(_order, "waiting");
            Console.WriteLine("Customer Here");
        }
    }
}