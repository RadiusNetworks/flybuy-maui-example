using System.Collections.ObjectModel;
using FlyBuy;
using FlyBuy.Data;
using Java.Time;

namespace FlybuyExample;

public class FlybuyService : IFlybuyService
{
    static ObservableCollection<Site> Sites { get; set; }
    static ObservableCollection<Order> Orders { get; set; }
    private CustomerCallback CustomerCallback;
    private OrderCallback OrderCallback;

    public FlybuyService()
    {
        CustomerCallback = new CustomerCallback();
        OrderCallback = new OrderCallback();
        Sites = new ObservableCollection<Site>();
        Orders = new ObservableCollection<Order>();
    }

    static FlyBuy.Data.CustomerInfo CustomerInfo(Customer customer) => new FlyBuy.Data.CustomerInfo(
            customer.Name, customer.Phone,
            customer.CarType, customer.CarColor, customer.CarLicense);

    public void CreateCustomer(Customer customer)
    {
        Core.Customer.Create(
            CustomerInfo(customer),
            true, true,
            null, null,
            CustomerCallback);
    }

    public void UpdateCustomer(Customer customer)
    {
        Core.Customer.Update(
            CustomerInfo(customer),
            CustomerCallback);
    }

    public void CreateOrder(Order order, Customer customer)
    {
        DateTime epoch = DateTime.UnixEpoch;
        TimeSpan ts = order.PickupStart.Subtract(epoch);
        Instant i = Instant.OfEpochMilli((long)ts.TotalMilliseconds);
        var pickupWindow = new FlyBuy.Data.PickupWindow(i);

        var builder = new OrderOptions.Builder(customer.Name);
        builder.SetCustomerPhone(customer.Phone).SetCustomerCarColor(customer.CarColor)
            .SetCustomerCarType(customer.CarType).SetCustomerCarPlate(customer.CarLicense)
            .SetPartnerIdentifier(order.Number).SetPickupWindow(pickupWindow)
            .SetState("created").SetPickupType(order.PickupType);
        Core.Orders.Create(order.SiteId(),
            builder.Build(),
            OrderCallback);
    }

    public void FetchOrder(string code)
    {
        Core.Orders.Fetch(
            code,
            OrderCallback);
    }

    public void ClaimOrder(Order order, Customer customer)
    {
        var builder = new OrderOptions.Builder(customer.Name).SetCustomerPhone(customer.Phone)
            .SetCustomerCarColor(customer.CarColor).SetCustomerCarType(customer.CarType)
            .SetCustomerCarPlate(customer.CarLicense).SetPickupType(order.PickupType);

        Core.Orders.Claim(order.Code, builder.Build(), OrderCallback);
    }

    public void UpdateOrder(Order order, string customerState)
    {
        Core.Orders.UpdateCustomerState(
            order.Id, customerState,
            OrderCallback);
    }

    public void FetchOrders()
    {
        Core.Orders.Fetch(OrderCallback);
    }

    public Customer CurrentCustomer()
    {
        FlyBuy.Data.Customer customer = Core.Customer.Current;

        if (customer == null)
        {
            return null;
        }
        else
        {
            return new Customer(
                customer.Name, customer.Phone,
                customer.CarType, customer.CarColor, customer.LicensePlate,
                customer.ApiToken);
        }
    }

    public ObservableCollection<Order> GetOrders()
    {
        Orders.Clear();
        foreach (FlyBuy.Data.Order order in Core.Orders.Open)
        {
            var flybuySite = order.Site;
            var site = new Site(
                flybuySite.Id,
                flybuySite.PartnerIdentifier,
                flybuySite.Name,
                flybuySite.Description);

            Orders.Add(
                new Order(
                    (int)order.Id, site,
                    order.PartnerIdentifier,
                    order.PickupType));
        }

        return Orders;
    }

    public ObservableCollection<Site> GetSites()
    {
        if (Sites.Count == 0)
        {
            foreach (FlyBuy.Data.Site site in Core.Sites.All)
            {
                Sites.Add(new Site(site.Id, site.PartnerIdentifier, site.Name, site.Description));
            }
        }
        return Sites;
    }

    public void OnMessageReceived(IDictionary<string, object> data)
    {
        IDictionary<string, string> q = new Dictionary<string, string>();
        foreach (var p in data)
        {
            System.Diagnostics.Debug.WriteLine($"{p.Key} : {p.Value}");
            q.Add(p.Key, p.Value.ToString());
        }
        Core.OnMessageReceived(q, new Callback());
    }
}

class CustomerCallback : Java.Lang.Object, Kotlin.Jvm.Functions.IFunction2
{
    public Java.Lang.Object Invoke(Java.Lang.Object p0, Java.Lang.Object p1)
    {
        var customer = p0 as FlyBuy.Data.Customer;
        var error = p1 as SdkError;

        if (error != null)
        {
            Console.WriteLine("Customer callback error: " + error.UserError());
        }
        else
        {
            Console.WriteLine("Customer [" + customer.Name + "] callback success");
        }

        return null;
    }
}

class OrderCallback : Java.Lang.Object, Kotlin.Jvm.Functions.IFunction2
{
    public Java.Lang.Object Invoke(Java.Lang.Object p0, Java.Lang.Object p1)
    {
        var order = p0 as FlyBuy.Data.Order;
        var error = p1 as FlyBuy.Data.SdkError;

        if (error != null)
        {
            Console.WriteLine("Order callback error: " + error.UserError());
        }
        else
        {
            if (order != null)
            {
                string s = order.CustomerState;
                Console.WriteLine("Order [" + order.PartnerIdentifier + "] callback success");
            }
            else
            {
                //LiveData x = Core.orders.GetOrder(order.Id);
                //x.Observe(this, null);

                Console.WriteLine("Order callback success");
            }
        }

        return null!;
    }
}

class SitesCallback : Java.Lang.Object, Kotlin.Jvm.Functions.IFunction2
{
    public Java.Lang.Object Invoke(Java.Lang.Object p0, Java.Lang.Object p1)
    {
        var error = p1 as FlyBuy.Data.SdkError;

        if (error != null)
        {
            Console.WriteLine("Sites callback error: " + error.UserError());
        }

        return null!;
    }
}

class Callback : Java.Lang.Object, Kotlin.Jvm.Functions.IFunction1
{
    public Java.Lang.Object Invoke(Java.Lang.Object p0)
    {
        var error = p0 as FlyBuy.Data.SdkError;

        if (error != null)
        {
            Console.WriteLine("Sites callback error: " + error.UserError());
        }

        return null!;
    }
}