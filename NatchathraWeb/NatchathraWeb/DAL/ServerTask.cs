//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NatchathraWeb.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class ServerTask
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public System.DateTimeOffset QueueTime { get; set; }
        public Nullable<System.DateTimeOffset> StartTime { get; set; }
        public Nullable<System.DateTimeOffset> CompletedTime { get; set; }
        public string ErrorMessage { get; set; }
        public string ConcurrencyTag { get; set; }
        public string State { get; set; }
        public bool HasPendingInterruptions { get; set; }
        public bool HasWarningsOrErrors { get; set; }
        public string ServerNode { get; set; }
        public string ProjectId { get; set; }
        public string EnvironmentId { get; set; }
        public string JSON { get; set; }
    }
}
