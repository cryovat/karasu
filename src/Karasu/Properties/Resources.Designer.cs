﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Karasu.Properties {
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
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Karasu.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to &lt;!DOCTYPE html&gt;
        ///&lt;html lang=&quot;en&quot;&gt;
        ///  &lt;head&gt;
        ///    &lt;meta charset=&quot;utf-8&quot;&gt;
        ///    &lt;title&gt;Karaoke Server&lt;/title&gt;
        ///    &lt;link rel=&quot;stylesheet&quot; href=&quot;style.css&quot;&gt;
        ///    &lt;style&gt;
        ///		body {{
        ///			background-color: AliceBlue;
        ///			font-family: sans-serif;
        ///			font-size: 2em;
        ///			text-align: center;
        ///		}}
        ///
        ///		img {{
        ///			max-width: 80%;
        ///		}}
        ///
        ///		p:first-of-type {{
        ///			margin-bottom: 2em;
        ///		}}
        ///
        ///		a {{
        ///			color: #3C598E;
        ///			text-decoration: none;
        ///		}}
        ///	&lt;/style&gt;
        ///  &lt;/head&gt;
        ///  &lt;body&gt;
        ///	&lt;p&gt;
        ///		&lt;img src=&quot;{0}&quot; alt=&quot;QR code [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string BlankPageTemplate {
            get {
                return ResourceManager.GetString("BlankPageTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Karasu Karaoke Server.
        /// </summary>
        internal static string KaraokeServerTitle {
            get {
                return ResourceManager.GetString("KaraokeServerTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to  * Listening on .
        /// </summary>
        internal static string ListeningOnText {
            get {
                return ResourceManager.GetString("ListeningOnText", resourceCulture);
            }
        }
    }
}
