﻿#pragma checksum "..\..\MainWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A587455EF046AEDBB6F5189438A26968"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ProjectNoName {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 165 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label PriorityLabel;
        
        #line default
        #line hidden
        
        
        #line 172 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox ListBox;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ProjectNoName;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 13 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.PrintClick);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 14 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ExitClick);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 17 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenClick);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 18 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.SaveClick);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 19 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.SaveAsClick);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 20 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.PrintClick);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 21 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ExitClick);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 25 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.SortByHighestClick);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 29 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.HideCompletedClick);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 32 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ClearAllClick);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 73 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Grid)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.DragWindow);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 107 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Label)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.FileClick);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 118 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Label)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.EditClick);
            
            #line default
            #line hidden
            return;
            case 14:
            
            #line 126 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.MinimizeClick);
            
            #line default
            #line hidden
            return;
            case 15:
            
            #line 145 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBox)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.OnKeyDownEvent);
            
            #line default
            #line hidden
            
            #line 146 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBox)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.UnselectSelectedTask);
            
            #line default
            #line hidden
            return;
            case 16:
            
            #line 154 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.DatePicker)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.OnKeyDownEvent);
            
            #line default
            #line hidden
            return;
            case 17:
            this.PriorityLabel = ((System.Windows.Controls.Label)(target));
            
            #line 169 "..\..\MainWindow.xaml"
            this.PriorityLabel.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.PriorityLabelMouseClick);
            
            #line default
            #line hidden
            return;
            case 18:
            this.ListBox = ((System.Windows.Controls.ListBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 19:
            
            #line 186 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Grid)(target)).PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.OnKeyDownEvent);
            
            #line default
            #line hidden
            break;
            case 20:
            
            #line 198 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.CheckBox)(target)).GotFocus += new System.Windows.RoutedEventHandler(this.OnListBoxItemGotFocus);
            
            #line default
            #line hidden
            break;
            case 21:
            
            #line 203 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Label)(target)).MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.TaskMouseDoubleClick);
            
            #line default
            #line hidden
            break;
            case 22:
            
            #line 211 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBox)(target)).GotFocus += new System.Windows.RoutedEventHandler(this.OnListBoxItemGotFocus);
            
            #line default
            #line hidden
            
            #line 212 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBox)(target)).KeyDown += new System.Windows.Input.KeyEventHandler(this.EditTaskKeyDown);
            
            #line default
            #line hidden
            
            #line 213 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.TextBox)(target)).PreviewLostKeyboardFocus += new System.Windows.Input.KeyboardFocusChangedEventHandler(this.EditTaskPreviewLostFocus);
            
            #line default
            #line hidden
            break;
            case 23:
            
            #line 223 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Label)(target)).GotFocus += new System.Windows.RoutedEventHandler(this.OnListBoxItemGotFocus);
            
            #line default
            #line hidden
            break;
            case 24:
            
            #line 226 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Label)(target)).MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.ListBoxLabelMouseDoubleClick);
            
            #line default
            #line hidden
            break;
            case 25:
            
            #line 229 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.RemoveSelectedTaskClick);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

