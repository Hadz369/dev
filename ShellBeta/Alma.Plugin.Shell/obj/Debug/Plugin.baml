   M S B A M L   `   `   `  ����H  DAlma.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullD2clr-namespace:Alma.Controls;assembly=Alma.ControlsAlma.Controls  5       Alma.Plugin.Shell   Alma.Plugin.ShellPlugin   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      GControls2clr-namespace:Alma.Controls;assembly=Alma.Controls     5        � 	Resources  5	          SectionGroupHeaderBackground �� Color   SectionGroupHeaderForeground  ToolBarSeparatorBrush   Alma.Controls.dhWidget �� 
TargetType �� Property �� Value �� 
TargetType  QuickSearchTextBoxStyle �� BasedOn ��Name J� SnapsToDevicePixels	 ��Name
 J� Opacity �� Triggers ]� 
Conditions �� Property �� Value �� 
TargetName��%�  &	        &	 #     &	 B     '  `     7   7  &	 +    7���� 5
   
   $ 	LightGray��6?   �� 5   
   $ Black��6?   �� 5   
   $
 Gray��68   �� 5   
   "  6   .�5      ��  � ��8   6+   �� 5       � ��8  6+   �� 5       � �� 5      �� 5      "  6&   .R�5      ]� ��. .+�5      ]� $	��Top=�6(   #��z��6=   .+�5   "   �� $��Right=�63   $��HeaderContent��6J   $��25��6h   �� 5   "   $��Header��63   $
��Left=�6J   $��Center=�6e   $��SemiBold"�5   +   $��5,3,3,3q�6K   5      �� 5      5      5      5      5      �� 5!   
   8   " ��5"      .�5#      ��  � �� 5$      �� 5%      " ��6&   .R�5&      � .+�5'      �� $ PART_ContentHost��6+   # z 5(   #   �� 5)      $	 	textBlock��6(   $
 0.345Q�5*       $��Quick Search��5+       $
��Wrap=�5,       $y�Hidden=�5-       5.       5/      ]� 50       51   "   �� 52   &    �@J�	IsFocused .  53   (   �� 54   &    � ��$  ��55   (   ./�57   "   �� $ 	textBlock��58   !    � y�$ Visible=�59   !   5:      5<      5>      5?      5A      .��5B      � $��5,5,5,0q�6    � ColumnDefinitions 5C   
   �� 5D      $	��200 �6   �� 5E      �� 5F      $	��200 �6   .+�5H   
   �� 
�������$��0,0,1,0q�6$    �� Padding$ 0,0,5,0q�6>   ._�5I      ]� $��0��6   .+�5J        $��Action��6$   $	��150��64   $	��Top=�6A     5K      $��Function��6$   $	��Top=�66   5L      5M      �� 5N   
   ��6   �� *CurrentPage+�� 5O   
   $��2��6   
�������$��1,0,0,0q�64   $ 5,0,0,0q�6N   ._�5P      �� $
��Auto=�6   $��2��6>   .��5Q      �� $
��Auto=�6   .+�5R        $��Widget1��6(   $	��150��69   6H     5S      $��Widget2��6(   $	��150��69   6H   5T      5U      5V      5X      5Y      