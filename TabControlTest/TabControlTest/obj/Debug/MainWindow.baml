   M S B A M L   `   `   `  ����n  jGrapeCity.ActiveReports.Viewer.Wpf.v8, Version=8.0.133.0, Culture=neutral, PublicKeyToken=cc4967777c49a3ff�_clr-namespace:GrapeCity.ActiveReports.Viewer.Wpf;assembly=GrapeCity.ActiveReports.Viewer.Wpf.v8"GrapeCity.ActiveReports.Viewer.Wpf  5   	   I ETabControlTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:TabControlTestTabControlTest 5   	   E ADragablz, Version=0.0.3.176, Culture=neutral, PublicKeyToken=null5(clr-namespace:Dragablz;assembly=DragablzDragablz 5   	   G1clr-namespace:Dragablz.Dockablz;assembly=DragablzDragablz.Dockablz 5	   	      TabControlTest.MainWindow   5   	   e aInfragisticsWPF4.v10.3, Version=10.3.20103.2337, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cbo kInfragisticsWPF4.Reporting.v10.3, Version=10.3.20103.2337, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb5igReporting!http://infragistics.com/Reporting   x tInfragisticsWPF4.Controls.Schedules.v10.3, Version=10.3.20103.2337, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb/ig$http://schemas.infragistics.com/xaml   5   	   kWpf_clr-namespace:GrapeCity.ActiveReports.Viewer.Wpf;assembly=GrapeCity.ActiveReports.Viewer.Wpf.v8   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35n jSAPBusinessObjects.WPF.ViewerShared, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304^	 ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W
 SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    	 
 5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml    
 5   	   .localclr-namespace:TabControlTest    
 5   	   =dragablz(clr-namespace:Dragablz;assembly=Dragablz    
 5   	   Fdockablz1clr-namespace:Dragablz.Dockablz;assembly=Dragablz    
 5	   	   -   5          � Title$  
MainWindow��5
   	   $	��600��6   $	��800��6)   $��Segoe UI)�65   $��14(�6K    � 	Resources 5       �� MergedDictionaries �� Source    booleanToVisibilityConverter�� 5   
    5      �� 5      $' !Common/DataTemplateSelectors.xaml?�6%   %   &	        �� 5      6+   5      .��5      � $��3,3,2,3q�6    � ColumnDefinitions 5   
   �� 5      $	��250 �6   �� 5      .+�5   
   � $��0��6    � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      .+�5!      �� $��0��6   $��2,2,2,1q�6"   
�������$��1q�6K    �� CornerRadius$ 5��6_   ._�5"      �� $��Actions��6)   $��5,2,5,2q�6:   .��5#      �� $��5q�6!   .+�5$      �� $��Execute��6!   
������ $
��Left=�6L   $��0q�6n   $��5,2,2,2q�6�   �� 5%      $��Cancel��6!   
������ $
��Left=�6K   $��0q�6m   $��5,2,2,2q�6�   5&      5'      5(      �� 5*      $��1��6   $��2,1,2,2q�6"   
�������$��1q�6K   $ 5��6_   ._�5+      �� $��Reports��6)   $��5,2,5,2q�6:   .��5,      R� $��0q�6   $��2q�6S   ��63   �� *	Reports+��5-      �� 5.       �� 
TargetType" Q�6$   .�5/   "   ��  �� Property �@Q�
IsExpanded	 �� Value	 6?   �� *
IsExpanded+50      ��52      �� 53      
 �� ItemsSource
 67   �� *
Children+.R�54   "   ��  �� Orientation$ 
Horizontal=�6-   $��2q�6F   .+�55   &   �� $��Images\Folder-icon.png��6,   $��16��6L   $��16��6W    J� SnapsToDevicePixels . 6c   �� 56   &   $	��5,0q�6L   ��60   ��  �� Path$ Title�6L   57   #   58      5:      5;      5<      5=      � 5?   
   $��1��6   $��2,3,3,3q�6    5@      �� 5A      $
��Auto �6    �� 5B      $
��Auto �6    �� 5C      $��2* �6    .+�5F      �� $��3��6   $��1��6&   $��Stretch=�63     Dragablz.TabablzControl  5H      $��2��6'     tabItemDataTemplateSelector �� ContentTemplateSelector# [ 5J      ��5I      �� *Items+ �� SelectedItem 6/   �� *SelectedItem+   InterTabController 5K      !  Dragablz.InterTabController  5L         	Partition$	 ABC��5M         InterTabClient 5N      �� *InterTabClient+   HeaderItemTemplate 5P      �� 5Q      .R�5R      �� $ 
Horizontal=�6%   .+�5S      �� ��6(   �� *Name+�� 5T      $��20��6%   $��20��60   $��X��6<   
������  J� IsHitTestVisible . ��5U   8   �� *CloseTabCommand+5V      5W      5Y      9� 5[      $��Report Parameters��6   $��Stretch=�62   $��Stretch=�6P    9� 
IsExpanded .  9� ExpandDirection$
 Down=�5\      .��5]      �� $��0��6   $��3q�6W   ��6/   �� *ParameterGroups+��5^      �� 5_      .R�5`      �� $ 
Horizontal=�6)   5a      ��5c      �� 5d      .R�5e      �� ��6'   �� *Name+.��5f   "   ��  � IsSharedSizeScope . $��1��5g   %   $��3q�5h   H    ! parameterDataTemplateSelector#��[ 5i   %   ��5h   %   �� *
Parameters+5j   #   5k      5l      5n      5o      5p      5q      5r      