using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Task.DB;

namespace Task
{
    public class SerializationDataContractSurrogate : IDataContractSurrogate
    {
        public object GetObjectToSerialize(object obj, Type targetType)
        {
            if (obj is Order order)
            {           
                return order;
            }
            if (obj is Customer customer)
            {
                customer.Orders = null;
                customer.CustomerDemographics = null;
                return customer;
            }
            if (obj is Employee employee)
            {
                employee.Orders = null;
                employee.Territories = null;
                employee.Employees1 = null;
                employee.Employee1 = null;
                return employee;
            }
            if (obj is Shipper shipper)
            {
                shipper.Orders = null;
                return shipper;
            }
            if (obj is Order_Detail orderDetail)
            {
                orderDetail.Order = null;
                orderDetail.Product = null;
                return orderDetail;
            }
            return obj;
        }

        public object GetDeserializedObject(object obj, Type targetType)
        {
            return obj;
        }

        public Type GetDataContractType(Type type)
        {
            return type;
        }
        #region Garbage
        public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
        {
            throw new NotImplementedException();
        }

        public object GetCustomDataToExport(Type clrType, Type dataContractType)
        {
            throw new NotImplementedException();
        }

        public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
        {
            throw new NotImplementedException();
        }

        public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
        {
            throw new NotImplementedException();
        }

        public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
