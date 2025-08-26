using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters; // untuk ActionFilterAttribute

namespace TheMealDBApp.Filters
{
    public class AuthFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context) 
        {
            var username = context.HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                // kalau belum login, redirect ke Auth/Login. RedirectToActionResult(action(nama fungsi), controller, routeValues(optional))
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
            base.OnActionExecuting(context);
        }
    }
}