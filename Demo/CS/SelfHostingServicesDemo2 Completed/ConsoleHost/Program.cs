using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using InventoryServiceLibrary;
using ProductServiceLibrary;

namespace ConsoleHost
{
  class Program
  {
    static void Main(string[] args)
    {
      ServiceHost inventoryHost = null;
      ServiceHost productHost = null;

      try
      {
        productHost = new ServiceHost(typeof(ProductService));
        productHost.Faulted += new EventHandler(ProductHost_Faulted);
        productHost.Open();

        Console.WriteLine(
          "\nThe Product service is running and is listening on:");
        foreach (ServiceEndpoint endpoint in
          productHost.Description.Endpoints)
        {
          Console.WriteLine("{0} ({1})",
            endpoint.Address.ToString(), endpoint.Binding.Name);
        }

        inventoryHost = new ServiceHost(typeof(InventoryService));
        inventoryHost.Faulted += new EventHandler(InventoryHost_Faulted);
        inventoryHost.Open();

        Console.WriteLine(
          "\nThe Inventory service is running and is listening on:");
        foreach (ServiceEndpoint endpoint in
          inventoryHost.Description.Endpoints)
        {
          Console.WriteLine("{0} ({1})",
            endpoint.Address.ToString(), endpoint.Binding.Name);
        }

        Console.WriteLine("\nPress any key to stop the service.");
        Console.ReadKey();
      }
      finally
      {
        if (productHost.State == CommunicationState.Faulted)
        {
          productHost.Abort();
        }
        else
        {
          productHost.Close();
        }
        if (inventoryHost.State == CommunicationState.Faulted)
        {
          inventoryHost.Abort();
        }
        else
        {
          inventoryHost.Close();
        }
      }
    }

    static void ProductHost_Faulted(object sender, EventArgs e)
    {
      Console.WriteLine("The ProductService host has faulted.");
    }

    static void InventoryHost_Faulted(object sender, EventArgs e)
    {
      Console.WriteLine("The InventoryService host has faulted.");
    }
  }
}
