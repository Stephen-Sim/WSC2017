//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Module1
{
    using System;
    using System.Collections.Generic;
    
    public partial class Log
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public System.DateTime LogInTime { get; set; }
        public Nullable<System.DateTime> LogoutTime { get; set; }
        public string NotProperLogoutReason { get; set; }
        public Nullable<int> CrashType { get; set; }
    
        public virtual CrashType CrashType1 { get; set; }
        public virtual User User { get; set; }
    }
}
