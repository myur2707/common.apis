using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using System.Web.Http.WebHost; //for HttpControllerHandler
using System.Web.SessionState; //for IRequiresSessionState
using System.Web.Routing; //for RouteData

namespace WebApiCampusConcierge
{
    public class SessionableControllerHandler : HttpControllerHandler, IRequiresSessionState 
    {
        //
        // GET: /SessionableControllerHandler/

        public SessionableControllerHandler(RouteData routeData)
            : base(routeData)
        { }

    }
}
