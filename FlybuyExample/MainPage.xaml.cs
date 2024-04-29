using System.Collections.ObjectModel;

namespace FlybuyExample;

public partial class MainPage : ContentPage
{
    private IFlybuyService flybuy;
    private Customer customer;
    private Order order = null!;
    private bool createCust = false;

    public MainPage(IFlybuyService flybuy)
    {
        InitializeComponent();

        this.flybuy = flybuy;

        customer = flybuy.CurrentCustomer();
        if (customer == null)
        {
            createCust = true;
            customer = new Customer();
        }
        else
        {
            CustName.Text = customer.Name;
            CustPhone.Text = customer.Phone;
        }

        ObservableCollection<Order> orders = flybuy.GetOrders();
        if (orders.Count > 0)
        {
            order = orders.First();
        }
    }

    private void OnCustomerClicked(object sender, EventArgs e)
    {
        customer.Name = CustName.Text;
        customer.Phone = CustPhone.Text;

        if (createCust)
        {
            flybuy.CreateCustomer(customer);
        }
        else
        {
            flybuy.UpdateCustomer(customer);
        }        
    }

    private void OnOrderRedeem(object sender, EventArgs e)
    {
        order = new Order();
        order.Code = RedeemCode.Text;
        if (order.Code != null)
        {
            flybuy.ClaimOrder(order, customer);
        }
    }
}