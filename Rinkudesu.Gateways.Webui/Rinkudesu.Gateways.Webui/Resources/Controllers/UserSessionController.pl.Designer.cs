﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rinkudesu.Gateways.Webui.Resources.Controllers {
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class UserSessionController_pl {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal UserSessionController_pl() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("Rinkudesu.Gateways.Webui.Resources.Controllers.UserSessionController_pl", typeof(UserSessionController_pl).Assembly);
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
        
        internal static string login_failed {
            get {
                return ResourceManager.GetString("login-failed", resourceCulture);
            }
        }
        
        internal static string forgotPasswordSubject {
            get {
                return ResourceManager.GetString("forgotPasswordSubject", resourceCulture);
            }
        }
        
        internal static string forgotPasswordIntro {
            get {
                return ResourceManager.GetString("forgotPasswordIntro", resourceCulture);
            }
        }
        
        internal static string forgotPasswordClick {
            get {
                return ResourceManager.GetString("forgotPasswordClick", resourceCulture);
            }
        }
        
        internal static string passwordMismatch {
            get {
                return ResourceManager.GetString("passwordMismatch", resourceCulture);
            }
        }
    }
}
