using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dragablz;


namespace TabControlTest
{
    public class MainInterTabClient : IInterTabClient
    {
        public Dragablz.INewTabHost<System.Windows.Window> GetNewHost(Dragablz.IInterTabClient interTabClient, object partition, Dragablz.TabablzControl source)
        {
            var view = new TabHostWindow();
            return new NewTabHost<TabHostWindow>(view, view.TabsContainer);
        }

        public Dragablz.TabEmptiedResponse TabEmptiedHandler(Dragablz.TabablzControl tabControl, System.Windows.Window window)
        {
            throw new NotImplementedException();
        }
    }
}
