   M S B A M L   `   `   `  ����   ProjectNoName    ProjectNoName.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  
Tasks TODO��$	��725��$	��700�� � AllowsTransparency . 
������  � WindowStyle$
 None=� � ContextMenu �� .@�m� -   $��Print��m� -   $��Close��.��� -    � Cursor$
 Hand�� � ColumnDefinitions �� $
��Auto ��� $	��400 ��� $	��100 ��� $��20 � � RowDefinitions �� $
��Auto ��� $
��Auto �.+��� $��0��$��0��$��+��$
 Hand������ *
AddCommand+�� -   $��0��$��1��$
��Bold"�
���<��$��0������ *	TaskToAdd+ �� UpdateSourceTrigger$ PropertyChanged=�(  "System.Windows.Controls.DatePicker  -   $��0��$��2��   SelectedDate �� *
TaskDate+�� -   	 ��Name$	 PriorityButton��$��0��$��3������ *PriorityColor+���� *ChangePriorityCommand+�� $��1��$��0��$��4��-  'System.Windows.Input.KeyboardNavigation
   	IsTabStop
 .     TabNavigation$
 None=����� *TaskCollection+ �� SelectedItem �� *SelectedTask+����  �� 
TargetType" ��.���  �� Property � �� �� Value �� .R��  �� $
��Auto ��� $	��200 ��� $	��100 ��� $��20 �.+��� -   $��0��$��Center=����� *DataContext.SortTaskCommand+ �� RelativeSource ��  �� AncestorType" �����  d� 	IsChecked �� *IsDone+�� -   $��1������ *
TaskName+y��� *
IsEditable+ "  inverseBoolVisibilityConverter �� 	Converter# [  �� -	   $��1��$
��Bold"�
���<������ *
TaskName+$ PropertyChanged=�y��� *
IsEditable+  boolVisibilityConverter# [ �� -
   $��2������ *
TaskDate+�� -   $��3������ *PriorityColor+