﻿#pragma checksum "..\..\..\..\..\Decorator\Users\FunctionsWindows\SendPersonnelWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "F57C28D34605D26E36939560A962DC58C861EA028BB0FC863920EBABB2A0917F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
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


namespace APPZ.Decorator.Users.FunctionsWindows {
    
    
    /// <summary>
    /// SendPersonnelWindow
    /// </summary>
    public partial class SendPersonnelWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 6 "..\..\..\..\..\Decorator\Users\FunctionsWindows\SendPersonnelWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal APPZ.Decorator.Users.FunctionsWindows.SendPersonnelWindow WAddUser;
        
        #line default
        #line hidden
        
        
        #line 8 "..\..\..\..\..\Decorator\Users\FunctionsWindows\SendPersonnelWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid ParentGrid;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\..\..\Decorator\Users\FunctionsWindows\SendPersonnelWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label LblHeader;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\..\..\Decorator\Users\FunctionsWindows\SendPersonnelWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label LblSecondHeader;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\..\..\Decorator\Users\FunctionsWindows\SendPersonnelWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid GridInfo;
        
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
            System.Uri resourceLocater = new System.Uri("/APPZ;component/decorator/users/functionswindows/sendpersonnelwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Decorator\Users\FunctionsWindows\SendPersonnelWindow.xaml"
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
            this.WAddUser = ((APPZ.Decorator.Users.FunctionsWindows.SendPersonnelWindow)(target));
            return;
            case 2:
            this.ParentGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.LblHeader = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.LblSecondHeader = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.GridInfo = ((System.Windows.Controls.Grid)(target));
            return;
            case 6:
            
            #line 46 "..\..\..\..\..\Decorator\Users\FunctionsWindows\SendPersonnelWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.BtnAddRequest_OnClick);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 54 "..\..\..\..\..\Decorator\Users\FunctionsWindows\SendPersonnelWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.BtnExit_OnClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
