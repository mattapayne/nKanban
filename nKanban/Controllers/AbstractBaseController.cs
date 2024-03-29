﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using nKanban.Services;
using nKanban.Shared;

namespace nKanban.Controllers
{
    public abstract class AbstractBaseController : Controller
    {
        protected void AddServiceErrors(IEnumerable<ServiceError> serviceErrors)
        {
            if (serviceErrors == null)
            {
                return;
            }

            foreach (var error in serviceErrors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }

        protected void SetRedirectMessage(MessageType messageType, string message)
        {
            if (String.IsNullOrEmpty(message))
            {
                return;
            }

            if(!TempData.ContainsKey(messageType.ToString()))
            {
                TempData.Add(messageType.ToString(), new List<string>());
            }

            ((List<string>)TempData[messageType.ToString()]).Add(message);
        }

        protected NKanbanPrincipal CurrentUser
        {
            get
            {
                if (HttpContext.Request.IsAuthenticated)
                {
                    return HttpContext.User as NKanbanPrincipal;
                }

                return null;
            }
        }

        protected bool IsLoggedIn
        {
            get
            {
                return CurrentUser != null;
            }
        }
    }
}
