using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Task.DB;

namespace Task
{
    public class OrderDetailSerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext streamingContext)
        {
            var orderDetail = (Order_Detail)obj;
            info.AddValue("Discount", orderDetail.Discount);
            info.AddValue("OrderID", orderDetail.OrderID);
            info.AddValue("ProductID", orderDetail.ProductID);
            info.AddValue("UnitPrice", orderDetail.UnitPrice);
            info.AddValue("Quantity", orderDetail.Quantity);
            if (streamingContext.Context is SerializationContext context && context.SerializingType == typeof(Order_Detail))
            {
                context.ObjectContextAdapter.ObjectContext.LoadProperty(orderDetail, oD => oD.Product);
                context.ObjectContextAdapter.ObjectContext.LoadProperty(orderDetail, oD => oD.Order);
            }
            info.AddValue("Order", orderDetail.Order);
            info.AddValue("Product", orderDetail.Product);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext streamingContext, ISurrogateSelector selector)
        {
            var orderDetail = (Order_Detail)obj;
            orderDetail.Discount = info.GetSingle("Discount");
            orderDetail.OrderID = info.GetInt32("OrderID");
            orderDetail.ProductID = info.GetInt32("ProductID");
            orderDetail.UnitPrice = info.GetDecimal("UnitPrice");
            orderDetail.Quantity = info.GetInt16("Quantity");
            orderDetail.Order = info.GetValue("Order", typeof(Order)) as Order;
            orderDetail.Product = info.GetValue("Product", typeof(Product)) as Product;
            return orderDetail;
        }
    }
}
