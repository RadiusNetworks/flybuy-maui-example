using System.Collections.ObjectModel;

namespace FlybuyExample;

public interface IFlybuyService
{
        public Customer CurrentCustomer();
        public void CreateCustomer(Customer customer);

        public void UpdateCustomer(Customer customer);

        public void FetchOrder(string code);

        public void ClaimOrder(Order order, Customer customer);

        public void CreateOrder(Order order, Customer customer);

        public void UpdateOrder(Order order, string customerState);

        public void FetchOrders();

        public ObservableCollection<Order> GetOrders();

        public ObservableCollection<Site> GetSites();

        public void OnMessageReceived(IDictionary<string, object> data);
}