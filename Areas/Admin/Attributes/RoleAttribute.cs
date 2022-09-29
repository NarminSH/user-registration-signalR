using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RegisterMVC.Areas.Admin.Attributes
{
    public class RoleAttribute: ActionFilterAttribute
    {
        public string Role { get; set; }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var role = context.HttpContext.Session.GetString("_role");
            if (role!=null)
            {
            var allRoles = role.Split(",");
            if (!allRoles.Contains(Role))
            {
                context.HttpContext.Response.Redirect("http://localhost:5074/");
            }
            base.OnActionExecuting(context);
            }
        }
    }
}

