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
    
    public partial class Deployment
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public System.DateTimeOffset Created { get; set; }
        public string EnvironmentId { get; set; }
        public string ProjectId { get; set; }
        public string ReleaseId { get; set; }
        public string ProjectGroupId { get; set; }
        public string TaskId { get; set; }
        public string JSON { get; set; }
    }
}
