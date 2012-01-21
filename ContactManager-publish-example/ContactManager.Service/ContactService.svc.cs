namespace ContactManager.Service {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.Text;
    using ContactManager.Common;

    public class ContactService : IContactService {
        #region Contact related
        public Contact GetContactById(long id) {
            return new ContactManager().GetContactById(id);
        }
        public List<Contact> GetAllContacts() {            
            return new ContactManager().GetAllContacts();
        }

        public Contact AddContact(Contact contact) {
            Contact result = new ContactManager().AddContact(contact);

            return result;
        }

        public Contact UpdateContact(Contact contact) {
            throw new NotImplementedException();
        }
        #endregion

        public List<State> GetAllStates() {
            return new ContactManager().GetAllStates();
        }


    }
}
