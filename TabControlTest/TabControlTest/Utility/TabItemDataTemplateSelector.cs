using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace TabControlTest
{
    public sealed class TabItemDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ReportViewerTemplate { get; set; }
        public DataTemplate CrystalTemplate { get; set; }
        public DataTemplate DataGridTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate template = base.SelectTemplate(item, container);

            var vm = item as TabItemViewModel ;

            if (vm != null)
            {
                switch (vm.ContentType)
                {
                    case TabContentType.ReportViewer:
                        template = ReportViewerTemplate;
                        break;
                    case TabContentType.Crystal:
                        template = CrystalTemplate;
                        break;
                    case TabContentType.DataGrid:
                        template = DataGridTemplate;
                        break;
                }
            }

            return template;
        }
    }
}
