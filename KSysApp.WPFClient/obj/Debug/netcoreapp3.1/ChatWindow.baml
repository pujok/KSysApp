   M S B A M L   `   `   `  ����L  HKSysApp.WPFClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:KSysApp.WPFClientKSysApp.WPFClient  5   	   "    KSysApp.WPFClient.ChatWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:KSysApp.WPFClient     5   	     � Title$  KSysChat��5   	    � SizeToContent$ WidthAndHeight=�6   $	��810��69   $	��960��6I   .��5	      � .+�5
   
   �� -   5        $��
MessageBox��5
   &   $��
10,0,75,10q�6    �� TextWrapping$
 Wrap=�68   $��60��6m   $��Bottom=�6y   $	��275��6�   �� 5   
   -   5        $��
SendButton��5   G   $
��Send��6   $��	0,0,10,10q�6    $��60��6Y   $��Bottom=�6e   $��60��6�   $��Right=�6�   �� 5   
   -   5        $��MessageScrollViewer��5   O   $��10,36,10,75q�6   $
��Auto=�6,   .��5      �� -   5        $��MessageBlock��5      $
��Wrap=�6,   $	��300��6@   5      �� 5   
   $��	Messages:��6   $
��Left=�6$   $��10,10,10,10q�6?   $	��Top=�6T   5      5      