using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using WindowsClient.InventoryService;
using WindowsClient.ProductService;
//using InventoryServiceLibrary;
//using ProductServiceLibrary;

namespace WindowsClient
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    private Product product = null;
    private Inventory inventory = null;
    private ProductServiceClient productProxy = null;
    private InventoryServiceClient inventoryProxy = null;
    //private IProductService productChannel = null;
    //private IInventoryService inventoryChannel = null;
    private decimal inStockValue = 0M;
    private decimal onOrderValue = 0M;

    private void Form1_Load(object sender, EventArgs e)
    {
      productProxy = new ProductServiceClient(
        "NetTcpBinding_IProductService");

      inventoryProxy = new InventoryServiceClient(
        "WSHttpBinding_IInventoryService");

      //productChannel = new ChannelFactory<IProductService>
      //  ("NetTcpBinding_IProductService").CreateChannel();

      //inventoryChannel = new ChannelFactory<IInventoryService>
      //  ("NetTcpBinding_IInventoryService").CreateChannel();

      //var productAddress = new EndpointAddress(
      //  "net.tcp://localhost:9010/ProductService");
      //productChannel = ChannelFactory<IProductService>.
      //  CreateChannel(new NetTcpBinding(), productAddress);

      //var inventoryAddress = new EndpointAddress(
      //  "net.tcp://localhost:9020/InventoryService");
      //inventoryChannel = ChannelFactory<IInventoryService>.
      //  CreateChannel(new NetTcpBinding(), inventoryAddress);
      //var inventoryAddress = new EndpointAddress(
      //  "http://localhost:8080/InventoryService");
      //inventoryChannel = ChannelFactory<IInventoryService>.
      //  CreateChannel(new WSHttpBinding(), inventoryAddress);
    }

    private void getProductButton_Click(object sender, EventArgs e)
    {
      product = new Product();
      product = productProxy.GetProduct(
        Convert.ToInt32(productIdTextBox.Text));
      //product = productChannel.GetProduct(
      //  Convert.ToInt32(productIdTextBox.Text));
      productNameLabel.Text = product.ProductName;
      unitPricelabel.Text = product.UnitPrice.ToString("C");

      inventory = new Inventory();
      inventory = inventoryProxy.GetInventory(
        Convert.ToInt32(productIdTextBox.Text));
      //inventory = inventoryChannel.GetInventory(
      //  Convert.ToInt32(productIdTextBox.Text));
      inStockTextBox.Text = inventory.UnitsInStock.ToString();
      inStockValue = product.UnitPrice *
        Convert.ToDecimal(inventory.UnitsInStock);
      inStockValueLabel.Text = inStockValue.ToString("C");
      onOrderTextBox.Text = inventory.UnitsOnOrder.ToString();
      onOrderValue = product.UnitPrice *
        Convert.ToDecimal(inventory.UnitsOnOrder);
      onOrderValueLabel.Text = onOrderValue.ToString("C");
    }

    private void updateProductButton_Click(object sender, EventArgs e)
    {
      inventory.UnitsInStock = Convert.ToInt16(inStockTextBox.Text);
      inventory.UnitsOnOrder = Convert.ToInt16(onOrderTextBox.Text);

      //if (inventoryChannel.UpdateInventory(inventory))
      if (inventoryProxy.UpdateInventory(inventory))
      {
        MessageBox.Show("Your changes were saved");
        inStockValue = product.UnitPrice *
          Convert.ToDecimal(inventory.UnitsInStock);
        inStockValueLabel.Text = inStockValue.ToString("C");
        onOrderValue = product.UnitPrice *
          Convert.ToDecimal(inventory.UnitsOnOrder);
        onOrderValueLabel.Text = onOrderValue.ToString("C");
      }
      else
      {
        MessageBox.Show("Your changes were not saved");
      }
    }

  }
}
