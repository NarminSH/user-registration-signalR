using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace RegisterMVC.Attributes
{
    public class CheckSignInAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string connectionId = context.HttpContext.Session.GetString("_connectionId");
            string username = context.HttpContext.Session.GetString("_username");
            if (!string.IsNullOrWhiteSpace(connectionId) && !string.IsNullOrWhiteSpace(username))
            {
                base.OnActionExecuting(context);    
            }
            else
            {
            context.Result = new NotFoundResult();
            }
        }
    }
}

