using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nKanban.Controllers;
using nKanban.Services;
using nKanban;
using nKanban.Shared;

namespace Specs.nKanban.ControllerSpecs.AbstractBase
{
    public class StubController : AbstractBaseController
    {
        public void AddServiceErrorsPub(IEnumerable<ServiceError> errors)
        {
            this.AddServiceErrors(errors);
        }

        public void SetRedirectMessagePub(MessageType messageType, string message)
        {
            this.SetRedirectMessage(messageType, message);
        }

        public nKanbanPrincipal CurrentUserPub
        {
            get
            {
                return this.CurrentUser;
            }
        }

        public bool IsLoggedInPub
        {
            get
            {
                return this.IsLoggedIn;
            }
        }
    }
}
