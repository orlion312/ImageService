   M S B A M L   `   `   `  ����  InageServiceGui9    3BasicMVVMQuickstart_Desktop.Views.QuestionnaireView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .��5	      � -   5          �Name$  
LayoutRoot��5	       � RowDefinitions 5
   
   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��3��5      $��Stretch=�5      .��5      �  5      �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   .+�5      �� $��0��6      HeaderedBorderStyle#��[  5      ._�5      �  5      �� 5      $
��Auto �6,   �� 5       $
��Auto �6,    5"      �� 5#      $
��Auto �6/   �� 5$      $
��Auto �6/   .+�5&      �� $��0��6$   $��0��5'   $   $��Bottom=�5(   $   �� 6@   $
�Name��6D   6T   �� 5)      -   5         ��Name$ NameTextBox��5)   "   $��1��5*   "   $��0��5+   "   $	��150��5-   "   $��2q�5.   "   4  .System.Windows.Automation.AutomationProperties   AutomationId$ NameText��6-   ��5,   "   �� *Questionnaire.Name+ �� Mode$ TwoWay=�5.   -   �� 5/      $��0��6$   $��1��50   $   $��Bottom=�51   $   �� 6@   $	�Age��6D   6S   �� 52      -   5        $ 
AgeTextBox��52   "   $��1��53   "   $��1��54   "   $
��Left=�55   "    �� 	MaxLength$ 3��57   "   $��40��58   "   $��2q�59   "   $ AgeText��6-   ��56   "   �� *Questionnaire.Age+$ TwoWay=�59   -   5:      5;      � 5<      $��1��6   $��5q�5=       5>      �� 5?      $
��Auto �6(   �� 5@      $
��Auto �6(    5B      �� 5C      $��* �6+   .+�5E      �� $��0��6    $��What is your quest?��5F         QuestionLabelStyle#��[ 5G       �� 5H      -   5        $ Question1Response��5H      $��1��5I      $ Question1Text��5K      ��5J      �� *Questionnaire.Quest+$ TwoWay=�5K      5L      � 5M      $��2��6   $��5q�5N       5O      �� 5P      $��* �6+    5R      �� 5S      $
��Auto �6(   �� 5T      $
��Auto �6(   .+�5V      �� $��0��6    #��[ 5W       �� 5X      $�What is your favorite ��6   �� 6?   -   5         ��Name$ ColorRun��5X   C   $�color��5Y   C   ��5Z   C   �� *Questionnaire.FavoriteColor+ �� TargetNullValue$ Black���� 6�   $�?��6�   5[      �� 5\      $��1��6     BorderBrush#��[ 5]      ._�5^      �� -   5        	 ��Name$	 Colors��5^   "   ��.  $��5q�5a   "   ��5`   "   �� *	AllColors+
 �� SelectedItem
 5b   "   �� *Questionnaire.FavoriteColor+$ TwoWay=�5c      5d      5e      5f      5g      5h      5i      