using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ProductServiceLibrary
{
  [ServiceContract]
  public interface IProductService
  {
    [OperationContract]
    Product GetProduct(int productId);
  }

  [DataContract]
  public class Product
  {
    [DataMember]
    public int ProductId { get; set; }

    [DataMember]
    public string ProductName { get; set; }

    [DataMember]
    public decimal UnitPrice { get; set; }
  }
}
