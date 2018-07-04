using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace TabControlTest
{
    public sealed class ParameterDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate StringParameterTemplate { get; set; }
        public DataTemplate IntegerParameterTemplate { get; set; }
        public DataTemplate DateParameterTemplate { get; set; }
        public DataTemplate TimeParameterTemplate { get; set; }
        public DataTemplate ComboParameterTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate template = base.SelectTemplate(item, container);

            var vm = item as ReportParameterViewModel;
            
            if (vm != null)
            {
                switch (vm.ParameterType)
                {
                    case ReportParameterType.String:
                        template = StringParameterTemplate;
                        break;
                    case ReportParameterType.Numeric:
                        template = IntegerParameterTemplate;
                        break;
                    case ReportParameterType.Date:
                        template = DateParameterTemplate;
                        break;
                    case ReportParameterType.Time:
                        template = TimeParameterTemplate;
                        break;
                    case ReportParameterType.Combo:
                        template = ComboParameterTemplate;
                        break;
                }
            }

            return template;
        }
    }
}
