using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: AssemblyVersion("1.0.0")]

namespace EMS {
    using System;
    
    
    public class IDClass {
        
        /// ID used for reference purposes
        private string cim_ID;
        
        private const bool isIDMandatory = true;
        
        public virtual string ID {
            get {
                return this.cim_ID;
            }
            set {
                this.cim_ID = value;
            }
        }
        
        public virtual bool IsIDMandatory {
            get {
                return isIDMandatory;
            }
        }
    }
}
