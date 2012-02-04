namespace ContactManager.Mvc.Models {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using ContactManager.Mvc.ContactServiceReference;

    public class AddContactPageModel {
        public AddContactPageModel(IList<State> states) {
            if (states == null) { throw new ArgumentNullException("states"); }

            this.States = new List<SelectListItem>();
            foreach (State state in states) {
                this.States.Add(new SelectListItem { Value = state.StateCode, Text = state.Name });
            }
        }

        public List<SelectListItem> States { get; set; }
    }
}