﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rinkudesu.Gateways.Webui.Resources.Views.AccountAdmin {
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class IndexContent {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal IndexContent() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("Rinkudesu.Gateways.Webui.Resources.Views.AccountAdmin.IndexContent", typeof(IndexContent).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        internal static string accountLocked {
            get {
                return ResourceManager.GetString("accountLocked", resourceCulture);
            }
        }
        
        internal static string twoFactorNotEnabled {
            get {
                return ResourceManager.GetString("twoFactorNotEnabled", resourceCulture);
            }
        }
        
        internal static string emailConfirmed {
            get {
                return ResourceManager.GetString("emailConfirmed", resourceCulture);
            }
        }
        
        internal static string emailNotConfirmed {
            get {
                return ResourceManager.GetString("emailNotConfirmed", resourceCulture);
            }
        }
        
        internal static string twoFactorEnabled {
            get {
                return ResourceManager.GetString("twoFactorEnabled", resourceCulture);
            }
        }
        
        internal static string admin {
            get {
                return ResourceManager.GetString("admin", resourceCulture);
            }
        }
    }
}
