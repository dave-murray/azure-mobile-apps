﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Microsoft.Azure.Mobile.Server.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class RResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal RResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Microsoft.Azure.Mobile.Server.Properties.RResources", typeof(RResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Authentication failed with error: {0}..
        /// </summary>
        internal static string Authentication_Error {
            get {
                return ResourceManager.GetString("Authentication_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Authentication failed due to invalid security identity type. Expected an &apos;{0}&apos; of type &apos;{1}&apos; but received &apos;{2}&apos;..
        /// </summary>
        internal static string Authentication_InvalidIdentity {
            get {
                return ResourceManager.GetString("Authentication_InvalidIdentity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Authentication failed due to an invalid token..
        /// </summary>
        internal static string Authentication_InvalidToken {
            get {
                return ResourceManager.GetString("Authentication_InvalidToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to GetIdentity failed because the authentication header was missing or had an invalid value..
        /// </summary>
        internal static string GetIdentities_MissingHeader {
            get {
                return ResourceManager.GetString("GetIdentities_MissingHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The IPrincipal&apos;s Claims must contain an &apos;iss&apos; Claim..
        /// </summary>
        internal static string GetIdentity_ClaimsMustHaveIssuer {
            get {
                return ResourceManager.GetString("GetIdentity_ClaimsMustHaveIssuer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There was an unexpected error retrieving the credentials: &apos;{0}&apos;.
        /// </summary>
        internal static string GetIdentity_HttpError {
            get {
                return ResourceManager.GetString("GetIdentity_HttpError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The &apos;{0}&apos; parameter must be of type &apos;{1}&apos;..
        /// </summary>
        internal static string ParameterMustBeOfType {
            get {
                return ResourceManager.GetString("ParameterMustBeOfType", resourceCulture);
            }
        }
    }
}
