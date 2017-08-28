using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing; //for IRouteHandler

namespace WebApiCampusConcierge
{
    public class SessionStateRouteHandler : IRouteHandler 
    {
        IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext)
        {
            return new SessionableControllerHandler(requestContext.RouteData);
        }

    }
}
