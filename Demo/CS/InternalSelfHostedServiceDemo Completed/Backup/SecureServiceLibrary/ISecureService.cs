using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SecureServiceLibrary
{
  [ServiceContract]
  public interface ISecureService
  {
    [OperationContract]
    string SayHello();

    [OperationContract]
    decimal ReportSales();
  }
}
