   M S B A M L   `   `   `  ����T  PSampleProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=292626629a736ae0-clr-namespace:SampleProjectSampleProject  5   	   R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35L5clr-namespace:System.Diagnostics;assembly=WindowsBaseSystem.Diagnostics 5   	       SampleProject.Window1   5   	   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      +localclr-namespace:SampleProject    5   	   Kdiagnostics5clr-namespace:System.Diagnostics;assembly=WindowsBase    5   	   M IFabTab, Version=1.0.0.0, Culture=neutral, PublicKeyToken=292626629a736ae0$tabhttp://fabtab.codeplex.com  5   	     � Title$  Window1��5      $	��600��6   $	��800��6"    � 	Resources 5        FabTab.FabTabItem �� 
TargetType �� Property �� Value �� Path1  +System.Diagnostics.PresentationTraceSources   
TraceLevel �� Orientation �� Triggers	 P� Property
 P� Value N� EnterActions"  FabTab.EasingMarginAnimation g� Duration t� From	 t� To �� TargetProperty   Equation��%6  '        �� 5
   
   "  6   .�5      ��  � �� 6'   �� $ 	LabelText�$
 High=��� 5       � �� 5      �� 5      .R�5      �� $ 
Horizontal=�6%   $��0,0,4,0q�6>   .+�5      �� $��CharacterEllipsis=�69   ��6(   �� 69   5      5      5       5      P� 5      	 �@��
IsSelected
 .  5      �� 5      .��5      �� .��5   "     $ 0:0:0.7D�6<   $ 	500,0,0,0X�6O   $ 0,0,0,0X�6`   $ Content.Margin�5   :   $ BounceEaseIn=�5   <   5!      5"      5$      5'      5(      .��5)      ]� .+�5*   
   �� -   5        $��Right=�5*      $��Add Tab��6=     FabTab.FabTabControl  5+   
   -   5          Name$ 
tabControl��5+      %   ShowDefaultTransitionAnimation .  $
��Left=�5,   #   ��5+   0   �� 5,   #   5.      5/      