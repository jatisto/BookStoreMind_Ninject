using Domain.Entities;

namespace Domain.Abstract
{
    public interface IOrderProcessor
    {
        void ProcessOrder(Card card, ShippingDetails shippingDetails);
    }
}