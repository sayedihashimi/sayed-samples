using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ContactManager.Common;

namespace ContactManager.Service {
    [ServiceContract]
    public interface IContactService {

        [OperationContract]
        List<Contact> GetAllContacts();

        [OperationContract]
        Contact UpdateContact(Contact contact);

        [OperationContract]
        Contact AddContact(Contact contact);

        [OperationContract]
        List<State> GetAllStates();

        [OperationContract]
        Contact GetContactById(long id);
    }
}
