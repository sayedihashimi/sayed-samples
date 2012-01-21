namespace ContactManager.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using ContactManager.Mvc.ContactServiceReference;
    using ContactManager.Mvc.Models;

    public class ContactsController : Controller
    {
        public ActionResult ViewContacts() {
            List<Contact> contacts = null;
            using (ContactServiceReference.ContactServiceClient client = new ContactServiceClient()) {
                contacts = client.GetAllContacts().ToList();
            }

            return View(contacts);
        }

        [HttpGet]
        public ActionResult AddContact() {
            IList<State> states = null;
            using (ContactServiceReference.ContactServiceClient client = new ContactServiceClient()) {
                states = client.GetAllStates();
            }
            return View(new AddContactPageModel(states));
        }

        [HttpPost]
        public ActionResult AddContact(Contact contact) {
            using (ContactServiceReference.ContactServiceClient client = new ContactServiceClient()) {
                Contact result = client.AddContact(contact);
            }

            return RedirectToAction("ViewContacts", "Contacts");
        }

        public ActionResult ViewContact(long id) {
            Contact contact = null;
            using (ContactServiceClient client = new ContactServiceClient()) {
                contact = client.GetContactById(id);
            }
            return ViewContact(contact);
        }

        private ActionResult ViewContact(Contact contact) {
            if (contact == null) { throw new ArgumentNullException("contact"); }

            return View(contact);
        }
    }
}
