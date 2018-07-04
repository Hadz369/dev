using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using SecureServiceLibrary;

namespace ConsoleHost
{
  class Program
  {
    static void Main(string[] args)
    {
      ServiceHost host = null;

      try
      {
        // Create a new instance of the ServiceHost class
        host = new ServiceHost(typeof(SecureService));
        host.Faulted += new EventHandler(Host_Faulted);
        // Start listening for messages
        host.Open();

        Console.WriteLine(
          "The service is running and is listening on:");
        foreach (ServiceEndpoint endpoint in
         host.Description.Endpoints)
        {
          Console.WriteLine("{0} ({1})",
            endpoint.Address.ToString(), endpoint.Binding.Name);
        }
        Console.WriteLine();
        Console.WriteLine("Press any key to stop the service.");
        Console.ReadKey();
      }
      finally
      {
        if (host.State == CommunicationState.Faulted)
        {
          host.Abort();
        }
        else
        {
          host.Close();
        }
      }
    }

    static void Host_Faulted(object sender, EventArgs e)
    {
      Console.WriteLine("The SecureService host has faulted.");
    }

  }
}
