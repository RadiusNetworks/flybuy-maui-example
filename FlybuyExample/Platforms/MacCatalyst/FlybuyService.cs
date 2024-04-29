using System.Collections.ObjectModel;

namespace FlybuyExample;

public class FlybuyService : IFlybuyService
{
    public FlybuyService()
    {
        
    }
    
    public Customer CurrentCustomer()
    {
        return new Customer();
        
    }

    public void CreateCustomer(Customer customer)
    {
        
    }

    public void UpdateCustomer(Customer customer)
    {
        
    }

    public void FetchOrder(string code)
    {
        
    }

    public void ClaimOrder(Order order, Customer customer)
    {
        
    }

    public void CreateOrder(Order order, Customer customer)
    {
        
    }

    public void UpdateOrder(Order order, string customerState)
    {
        
    }

    public void FetchOrders()
    {
        
    }

    public ObservableCollection<Order> GetOrders()
    {
        return null;
    }

    public ObservableCollection<Site> GetSites()
    {
        return null;
    }

    public void OnMessageReceived(IDictionary<string, object> data)
    {
        
    }
}