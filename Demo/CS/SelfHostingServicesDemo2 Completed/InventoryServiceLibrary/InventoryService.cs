using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace InventoryServiceLibrary
{
  public class InventoryService : IInventoryService
  {
    #region IInventoryService Members

    public Inventory GetInventory(int productId)
    {
      var inventory = new Inventory();

      using (var cnn = new SqlConnection(
        Properties.Settings.Default.NorthwindConnectionString))
      {
        using (var cmd = new SqlCommand(
          "SELECT UnitsInStock, UnitsOnOrder FROM Products " +
          "WHERE ProductId = @productId", cnn))
        {
          cmd.Parameters.Add(new SqlParameter(
            "@productId", productId));

          cnn.Open();
          using (SqlDataReader dataReader = cmd.ExecuteReader())
          {
            while (dataReader.Read())
            {
              inventory.ProductId = productId;
              inventory.UnitsInStock = dataReader.GetInt16(0);
              inventory.UnitsOnOrder = dataReader.GetInt16(1);
            }
          }
        }
      }
      return inventory;
    }

    public bool UpdateInventory(Inventory inventory)
    {
      int rowsChanged = 0;

      using (var cnn = new SqlConnection(
        Properties.Settings.Default.NorthwindConnectionString))
      {
        using (var cmd = new SqlCommand(
          "UPDATE Products " +
          "SET UnitsInStock = @unitsInStock, " +
          "UnitsOnOrder = @unitsOnOrder " +
          "WHERE ProductId = @productId", cnn))
        {
          cmd.Parameters.Add(new SqlParameter(
            "@unitsInStock", inventory.UnitsInStock));
          cmd.Parameters.Add(new SqlParameter(
            "@unitsOnOrder", inventory.UnitsOnOrder));
          cmd.Parameters.Add(new SqlParameter(
            "@productId", inventory.ProductId));

          cnn.Open();
          rowsChanged = (int)cmd.ExecuteNonQuery();
        }
      }
      return (rowsChanged != 0);
    }

    #endregion
  }
}
