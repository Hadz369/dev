using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace InventoryServiceLibrary
{
  [ServiceContract]
  public interface IInventoryService
  {
    [OperationContract]
    Inventory GetInventory(int productId);

    [OperationContract]
    bool UpdateInventory(Inventory inventory);
  }

  [DataContract]
  public class Inventory
  {
    [DataMember]
    public int ProductId { get; set; }

    [DataMember]
    public short UnitsInStock { get; set; }

    [DataMember]
    public short UnitsOnOrder { get; set; }
  }
}
