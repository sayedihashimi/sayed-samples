namespace ContactManager.Common {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data.Entity;

    public class ContactManager {
        //public void UpdateContact(Contact contact) {
        //    if (contact == null) { throw new ArgumentNullException("contact"); }

        //    using (ContactManagerEntities ctx = new ContactManagerEntities()) {
        //        // If the contact already exist update that entry otherwise insert a new contact
        //    }
        //}

        public void UpdateContact(Contact contact) {
            throw new NotImplementedException();
        }

        public Contact GetContactById(long id) {
            Contact contact = null;
            using (ContactManagerContext ctx = new ContactManagerContext()) {
                contact = (from c in ctx.Contacts
                           .Include("Address")
                           where c.ID == id
                           select c).SingleOrDefault();
            }
            return contact;
        }

        public List<Contact> GetAllContacts() {
            List<Contact> contacts = null;
            using (ContactManagerContext ctx = new ContactManagerContext()) {
                contacts = (from c in ctx.Contacts
                            .Include("Address")
                            select c).ToList();
            }
            return contacts;
        }
        
        public Contact AddContact(Contact contact) {
            if (contact == null) { throw new ArgumentNullException("contact"); }

            using (ContactManagerContext ctx = new ContactManagerContext()) {
                ctx.Contacts.Add(contact);

                ctx.SaveChanges();
            }

            return contact;
        }

        public List<State> GetAllStates() {
            using (ContactManagerContext ctx = new ContactManagerContext()) {
                var states = from s in ctx.States
                             select s;

                return states.ToList();
            }
        }
    }
}
