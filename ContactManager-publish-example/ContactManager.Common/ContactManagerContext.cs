namespace ContactManager.Common {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Data.Entity;

    public class ContactManagerContext :DbContext {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Contact> Contacts { get; set; }
    }
}
