using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace WebApiJwtAngular.Web.Api.Util
{
    public class VerbConstraint : IHttpRouteConstraint
    {
        private HttpMethod _method;

        public VerbConstraint(HttpMethod method)
        {
            _method = method;
        }

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            return routeDirection == HttpRouteDirection.UriGeneration || request.Method == _method;
        }
    }

    public class VerbRouteAttribute : RouteFactoryAttribute, IActionHttpMethodProvider
    {
        private HttpMethod _method;

        public VerbRouteAttribute(string template, string verb) : base(template)
        {
            _method = new HttpMethod(verb);
        }

        public Collection<HttpMethod> HttpMethods
        {
            get
            {
                return new Collection<HttpMethod> { _method };
            }
        }

        public override IDictionary<string, object> Constraints
        {
            get
            {
                var constraints = new HttpRouteValueDictionary();
                constraints.Add("verb", new VerbConstraint(_method));
                return constraints;
            }
        }
    }

    public class GetRouteAttribute : VerbRouteAttribute
    {
        public GetRouteAttribute(string template) : base(template, "GET")
        {
        }
    }

    public class PostRouteAttribute : VerbRouteAttribute
    {
        public PostRouteAttribute(string template) : base(template, "POST")
        {
        }
    }

    public class PutRouteAttribute : VerbRouteAttribute
    {
        public PutRouteAttribute(string template) : base(template, "PUT")
        {
        }
    }

    public class DeleteRouteAttribute : VerbRouteAttribute
    {
        public DeleteRouteAttribute(string template) : base(template, "DELETE")
        {
        }
    }
}