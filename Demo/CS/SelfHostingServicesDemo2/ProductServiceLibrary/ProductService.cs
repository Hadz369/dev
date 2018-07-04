using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ProductServiceLibrary
{
  public class ProductService : IProductService
  {
    #region IProductService Members

    public Product GetProduct(int productId)
    {
      var product = new Product();

      using (var cnn = new SqlConnection(
        Properties.Settings.Default.NorthwindConnectionString))
      {
        using (var cmd = new SqlCommand(
          "SELECT ProductName, UnitPrice FROM Products " +
          "WHERE ProductId = @productId", cnn))
        {
          cmd.Parameters.Add(new SqlParameter(
            "@productId", productId));

          cnn.Open();
          using (SqlDataReader dataReader = cmd.ExecuteReader())
          {
            while (dataReader.Read())
            {
              product.ProductId = productId;
              product.ProductName = dataReader.GetString(0);
              product.UnitPrice = dataReader.GetDecimal(1);
            }
          }
        }
      }
      return product;
    }

    #endregion
  }
}
