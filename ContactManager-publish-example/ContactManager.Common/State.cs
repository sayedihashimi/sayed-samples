namespace ContactManager.Common {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    [Serializable]
    [KnownType(typeof(Address))]
    [KnownType(typeof(Contact))]
    public class State {
        [DataMember]
        [Key]
        public string StateCode { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}
