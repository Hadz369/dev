   M S B A M L   `   `   `  ����T  PSampleProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=292626629a736ae0-clr-namespace:SampleProjectSampleProject  5   	   $    SampleProject.NoTabCloseWindow   5   	   \ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      M IFabTab, Version=1.0.0.0, Culture=neutral, PublicKeyToken=292626629a736ae0$tabhttp://fabtab.codeplex.com  5   	   'localclr-namespace:SampleProject  5   	     � Title$  NoTabCloseWindow��5      $	��600��6   $	��800��6+    � 	Resources 5        FabTab.FabTabItem �� 
TargetType �� Property �� Value �� Path �� Orientation��%�   '        �� 5   
   "  6   .�5	      ��  � �� 6'   �� $ 	LabelText��� 5
       � �� 5      �� 5      .R�5      �� $ 
Horizontal=�6%   .+�5      �� ��6$   �� 5      5      5      5      5      .��5      ]� .+�5   
   �� $ Vertical=�6   $��Right=�6,   .+�5      �� $��Add Tab��6   5        FabTab.FabTabControl  5   
   -   5          Name$ 
tabControl��5   2   $
��Left=�6   $	��300��6F      ShowTabCloseButtons .  	   ShowToolTipImages	 .  @�5        5      $��	My View 1��6!   .��5         SampleProject.SampleView  
   	LabelText$
 View 1��6'   5        5       $��	My View 2��6!   .��5!        $
 View 2��6'   5"        5#      $��	My View 3��6!   .��5$        $
 View 3��6'   5%        5&      $
 Oddball View��6#   5)      5-      5.      