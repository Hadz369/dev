   M S B A M L   `   `   `  ����I  ETabControlTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:TabControlTestTabControlTest  5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35n jSAPBusinessObjects.WPF.ViewerShared, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089    booleanToVisibilityConverter  TabItemBorder  �� Color  TabItemBackground  TabItemSelectedBackground  TabItemMouseOverBackground  TabControlBackground  IgButtonBackgroundBrush �� 
StartPoint �� EndPoint � Offset � Color �� A �� R �� G �� B  IgTabHeaderBackgroundBrush)    #TabControlTest.IgTabItemCloseButton	 �� 
TargetType
 �� Property �� Value �� 
TargetType ��Name �� Triggers P� Property P� Value"   TabControlTest.IgImageButton �� CornerRadius �� Orientation   Source!   TabControlTest.IgTabControl   TabControlTest.IgTabItem �� Padding   Name �� RelativeSource �� Path  	boolToVis �� 	Converter ��Name �� Triggers �� 
TargetName P� 
SourceName�� 5      H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      :x,http://schemas.microsoft.com/winfx/2006/xaml     5      &localclr-namespace:TabControlTest  5      %�  &	        &	      &	 1     &	 R     &	 u     &	 �     &	 �     &	 �    '   0    '  �    7  '  �    7  '  �    7  7  7  7   7  �� 5      6#   �� 5      $
  Gray��6,   �� 5	      $  #B1B1B1��60   �� 5
      $  	LightGray��68   �� 5      $  	#80E0ECF9��69   �� 5      $  LightSteelBlue��63   �� 5"      $ 0.5,0&�6:   $ 0.5,1&�6M   .��5#   
   � $
 0.00Q�6    5$      �� 5%      $	 255��6   $	 237��6    $	 237��6(   $	 237��60   5'      � 5(   
   $
 1.00Q�6    5)      �� 5*      $	 255��6   $	 141��6    $	 168��6(   $	 197��60   5,      5-      �� 5<      $ 0.5,0&�6=   $ 0.5,1&�6P   .��5=   
   � $
 0.00Q�6    5>      �� 5?      $	 255��6   $	 237��6    $	 237��6(   $	 237��60   5A      � 5B   
   $
 1.00Q�6    5C      �� 5D      $	 255��6   $	 141��6    $	 168��6(   $	 197��60   5F      5G      �� 5I      "	   6   .�5J   
   �� 
 � �� 5K      �� 5L      "   6"   .R�5M      �� $��0q�6   
������ 
������ ._�5N      �� $ CloseBtnImage��6    ��5O      �� 5P   "   "	 ��6(   .�5Q   &   �� 
 � ��$ /Image/BlueDelete_D.png��6?    5R   &   P� 5S   *    �@J�IsMouseOver . . �5T   .   �� 
 � ��$ /Image/BlueDelete.png��6G   5U   +   5W   #   5Y      5Z      5[      5]      5^      �� 5`      "	  6   .�5a   
   �� 
 � �� 5b      �� 5c      "  6"   .R�5d      �� $ 2��6   $��1q�6.   
�������8��  ._�5e      �� $ 
Horizontal=�6%   $��Center=�6>   $��Stretch=�6Y   $��3q�6w   .+�5f      �� #��z 6$   �� 5g      $
��Left=�6/   $��3,0,0,0q�6E   #��z��5h      #��z��5i      #��z��5j      #��z��5k      5l      5m      5n      5p      5q      �� 5s      "	  6   .�5t   
   �� 
 � ��8   6'   5u      �� 5w      "	  6   .�5x   
   �� 
 � ��$ Stretch=�67   �� 5y   
   
 � ��$ Stretch=�65   �� 5z   
   
 � �� 5{      �� 5|      "  6"   .R�5}      � .+�5~      �� $��Border��6!   $ 3,0,0,0��6/   $��20��6F   $��1,1,1,0q�6R   8��  8�� $ 3,0,3,0q�6�   ._�5      ]� ��. $��Stretch=�6=   $��60��6\   .+�5�   "      $ 
PART_Close��6=   $��Right=�6Q   $��12��6h   $��12��6t   $��5,0,0,0q�6   $��0q�6  y�6�   ��  �� *TemplatedParent+$ ShowCloseButton�8  6  �� 5�   "   $��Image��6(   $
��Left=�65   $��16��6K   $��16��6W   y�5�   (   ��  �� *TemplatedParent+$ 	ShowImage�8  ��5�   (   ��  �� *TemplatedParent+$ Image��� 5�   "   $ IgTabItemContent��63   $��3,0,0,0q�6M   $��Header��6^   $��Center=�6u   ��. 6�   5�      5�      5�       5�      P� 5�       �@��
IsSelected . . �5�      �� $ Border��6%   
 � ��
 �����6O   5�      P� 5�      $ IgTabItemContent��6"    �@J�IsMouseOver . . �5�      �� $ Border��6%   8  
 � ��6O   5�      5�      5�      5�      5�      