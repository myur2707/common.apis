using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Routing;
using System.Net.Http.Headers;
using System.Text;

namespace WebApiCampusConcierge
{
    public class ExecutionTimeFilterAttribute : ActionFilterAttribute
    {
        //this the globle filter 
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {


            base.OnActionExecuting(actionContext);
            actionContext.Request.Properties[actionContext.ActionDescriptor.ActionName] = Stopwatch.StartNew();

            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
            else
            {
                string authToken = actionContext.Request.Headers.Authorization.Parameter;
                string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
                string username = decodedToken.Substring(0, decodedToken.IndexOf(":"));
                string password = decodedToken.Substring(decodedToken.IndexOf(":") + 1);

               
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            Log(((((System.Net.Http.ObjectContent)(actionExecutedContext.Response.Content))).Value).ToString());
            base.OnActionExecuted(actionExecutedContext);
            Stopwatch stopwatch = (Stopwatch)actionExecutedContext.Request.Properties[actionExecutedContext.ActionContext.ActionDescriptor.ActionName];
            Trace.WriteLine(actionExecutedContext.ActionContext.ActionDescriptor.ActionName + "-elapsed=" + stopwatch.Elapsed);
            ;
        }

        private void Log(string p)
        {

            Debug.WriteLine(p, "Action Filter Log");
        }

        private void Log(System.Web.Http.Routing.IHttpRouteData httpRouteData)
        {
            var controllerName = httpRouteData.Values["controller"];

            var actionName = httpRouteData.Values["action"];

            var message = String.Format("controller:{0}, action:{1}", controllerName, actionName);

            Debug.WriteLine(message, "Action Filter Log");
        }
    }
}