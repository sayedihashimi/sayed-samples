namespace ContactManager.Common {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Runtime.Serialization;

    [DataContract]
    [Serializable]
    [KnownType(typeof(Contact))]
    [KnownType(typeof(State))]
    public class Address {
        [DataMember]
        public long ID { get; set; }
        [DataMember]
        public string Street1 { get; set; }
        [DataMember]
        public string Street2 { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Zip { get; set; }
    }
}
