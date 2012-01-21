namespace ContactManager.Common {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Runtime.Serialization;

    [DataContract]
    [Serializable]
    [KnownType(typeof(Address))]
    [KnownType(typeof(State))]
    public class Contact {
        public Contact() {
            this.Address = new Address();
        }
        [DataMember]
        public long ID { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public Address Address { get; set; }
    }
}
