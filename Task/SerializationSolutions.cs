using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Task.DB;
using Task.TestHelpers;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace Task
{
    [TestClass]
    public class SerializationSolutions
    {
        Northwind dbContext;

        [TestInitialize]
        public void Initialize()
        {
            dbContext = new Northwind();
        }

        [TestMethod]
        public void SerializationCallbacks()
        {
            dbContext.Configuration.ProxyCreationEnabled = false;
            var objectContext = (dbContext as IObjectContextAdapter).ObjectContext;
            var categories = dbContext.Categories.ToList();
            var c = categories.Last();

            var tester = new XmlDataContractSerializerTester<Category>(
                new NetDataContractSerializer(
                    new StreamingContext(
                        StreamingContextStates.All,
                        new SerializationContext
                        {
                            ObjectContextAdapter = objectContext,
                            SerializingType = typeof(Category)
                        })
                    ),
                true
            );

            tester.SerializeAndDeserialize(c);
        }

        [TestMethod]
        public void ISerializable()
        {
            dbContext.Configuration.ProxyCreationEnabled = false;
            var objectContext = (dbContext as IObjectContextAdapter).ObjectContext;
            var products = dbContext.Products.ToList();
            var p = products.Last();

            var tester = new XmlDataContractSerializerTester<Product>(
                new NetDataContractSerializer(
                    new StreamingContext(
                        StreamingContextStates.All,
                        new SerializationContext
                        {
                            ObjectContextAdapter = objectContext,
                            SerializingType = p.GetType()
                        })
                    ),
                true
                );

            tester.SerializeAndDeserialize(p);
        }


        [TestMethod]
        public void ISerializationSurrogate()
        {
            dbContext.Configuration.ProxyCreationEnabled = false;
            var objectContext = (dbContext as IObjectContextAdapter).ObjectContext;

            var orderDetail = dbContext.Order_Details.ToList();
            var od = orderDetail.Last();

            var serializationContext = new SerializationContext()
            {
                ObjectContextAdapter = objectContext,
                SerializingType = od.GetType()
            };
            var serializer = new NetDataContractSerializer()
            {
                Context = new StreamingContext(
                  StreamingContextStates.All,
                  serializationContext
                ),
                SurrogateSelector = new SurrogateSelector()
            };
            (serializer.SurrogateSelector as SurrogateSelector).AddSurrogate(
                od.GetType(), 
                serializer.Context, 
                new OrderDetailSerializationSurrogate()
            );
            var tester = new XmlDataContractSerializerTester<Order_Detail>(serializer, true);

            tester.SerializeAndDeserialize(od);
        }

        [TestMethod]
        public void IDataContractSurrogate()
        {
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;

            var orders = dbContext.Orders.ToList();

            var objectContext = (dbContext as IObjectContextAdapter).ObjectContext;
            foreach (var item in orders)
            {
                objectContext.LoadProperty(item, p => p.Customer);
                objectContext.LoadProperty(item, p => p.Employee);
                objectContext.LoadProperty(item, p => p.Shipper);
            }

            var o = orders.First();
            var serializer = new DataContractSerializer(
                typeof(Order),
                new DataContractSerializerSettings()
                {
                    DataContractSurrogate = new SerializationDataContractSurrogate()
                }
            );
            var tester = new XmlDataContractSerializerTester<Order>(serializer, true);
            
            tester.SerializeAndDeserialize(o);
        }
    }
}
