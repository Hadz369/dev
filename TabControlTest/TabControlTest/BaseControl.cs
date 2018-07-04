using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TabControlTest
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:FlexiNet._value"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:FlexiNet._value;assembly=FlexiNet._value"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors\:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:BaseControl/>
    ///
    /// </summary>
    // Text controls and labels
    public class IgTextBox : TextBox { }
    public class IgTextBoxLV : TextBox { }
    public class IgTextBlock : TextBlock { }
    public class IgLabel : Label { }
    public class IgInfoLabel : Label { }

    [Obsolete]
    public class IgHeader : Label { }

    [Obsolete]
    public class IgSectionHeader : HeaderedContentControl { }
    
    [Obsolete]
    public class IgSectionGroupHeader : HeaderedContentControl { }

    public class IgSection : HeaderedContentControl 
    {
        public object Footer
        {
            get { return (object)GetValue(FooterProperty); }
            set { SetValue(FooterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FooterProperty =
            DependencyProperty.Register("Footer", typeof(object), 
            typeof(IgSection), new UIPropertyMetadata(null));

        public object HeaderContent
        {
            get { return (object)GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderContentProperty =
            DependencyProperty.Register("HeaderContent", typeof(object),
            typeof(IgSection), new UIPropertyMetadata(null));
    }

    public class IgSectionGroup : HeaderedContentControl
    {
        public object HeaderContent
        {
            get { return (object)GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderContentProperty =
            DependencyProperty.Register("HeaderContent", typeof(object),
            typeof(IgSectionGroup), new UIPropertyMetadata(null));
    }

    // Lists
    public class IgListBox : ListBox { }
    public class IgListView : ListView { }

    public class IgTreeView : TreeView { }
    public class IgTreeViewItem : TreeViewItem { }

    public class IgComboBox : ComboBox { }
    public class IgComboBoxLV : ComboBox { }

    public class IgExpander : Expander { }

    // Options
    public class IgCheckBox : CheckBox { }
    public class IgRadioButton : RadioButton { }

    // Panels and grouping
    public class IgGroupBox : HeaderedContentControl { }
    public class IgCheckedGroupBox : GroupBox
    {
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(IgCheckedGroupBox));

        public bool IsChecked
        {
            get { return (bool)base.GetValue(IsCheckedProperty); }
            set { base.SetValue(IsCheckedProperty, value); }
        }
    }

    // Buttons
    public class IgButton : Button { }
    public class IgButtonLV : Button { }

    public class IgImageButton : Button 
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(IgImageButton));
        
        public ImageSource Source
        {
            get { return base.GetValue(SourceProperty) as ImageSource; }
            set { base.SetValue(SourceProperty, value); }
        }
    }

    public class IgTabItemCloseButton : IgImageButton { }

    public class IgImageButtonNoText : IgImageButton { }

    public class IgTabHeaderButton : IgImageButton 
    {
        public static readonly DependencyProperty CloseProperty = DependencyProperty.Register("Close", typeof(bool), typeof(IgTabHeaderButton));
    }

    // Tab Control and Tab Items
    public class IgTabControl : TabControl { }

    public class IgTabItem : TabItem
    {
        public static readonly RoutedEvent CloseTabEvent = EventManager.RegisterRoutedEvent("CloseTab", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(IgTabItem));

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(IgTabItem), new UIPropertyMetadata(null));

        public static readonly DependencyProperty ShowCloseProperty = DependencyProperty.Register("ShowCloseButton", typeof(bool), typeof(IgTabItem));
        public static readonly DependencyProperty ShowImageProperty = DependencyProperty.Register("ShowImage", typeof(bool), typeof(IgTabItem));

        static IgTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IgTabItem),
                new FrameworkPropertyMetadata(typeof(IgTabItem)));
        }

        public bool ShowCloseButton
        {
            get { return (bool)base.GetValue(ShowCloseProperty); }
            set { base.SetValue(ShowCloseProperty, value); }
        }

        public bool ShowImage
        {
            get { return (bool)base.GetValue(ShowImageProperty); }
            set { base.SetValue(ShowImageProperty, value); }
        }
        
        public event RoutedEventHandler CloseTab
        {
            add { AddHandler(CloseTabEvent, value); }
            remove { RemoveHandler(CloseTabEvent, value); }
        }

        public ImageSource Image
        {
            get { return GetValue(ImageProperty) as ImageSource; }
            set { SetValue(ImageProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Button closeButton = base.GetTemplateChild("PART_Close") as Button;
            
            if (closeButton != null)
                closeButton.Click += new System.Windows.RoutedEventHandler(closeButton_Click);
        }

        void closeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(CloseTabEvent, this));
        }
    }
}