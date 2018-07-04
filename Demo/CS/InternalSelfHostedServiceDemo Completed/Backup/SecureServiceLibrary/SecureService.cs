using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Security.Permissions;
using System.Security;
using System.Security.Principal;

namespace SecureServiceLibrary
{
  public class SecureService : ISecureService
  {
    public string SayHello()
    {
      return string.Format("Hello {0}", 
        System.Threading.Thread.CurrentPrincipal.Identity.Name);
    }

    //[PrincipalPermission(SecurityAction.Demand, 
    //  Role="BUILTIN\\BackupOperators")]
    public decimal ReportSales()
    {
      var currentUser = new WindowsPrincipal((WindowsIdentity)
        System.Threading.Thread.CurrentPrincipal.Identity);
      if (currentUser.IsInRole(WindowsBuiltInRole.BackupOperator))
      {
        return 10000M;
      }
      else
      {
        return -1M;
      }
    }
  }
}
