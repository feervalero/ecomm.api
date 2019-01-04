using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;
using Newtonsoft.Json.Serialization;
using WebApiTest1.Controllers;
using WebApiTest1.Models;
namespace WebApiTest1
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Order>("Orders");
            builder.EntitySet<Payment>("Payments");
            builder.EntitySet<OrderDetail>("OrderDetails");
            builder.EntitySet<Price>("Prices");
            builder.EntitySet<Audience>("Audiences");
            builder.EntitySet<FeatureType>("FeatureTypes");
            builder.EntitySet<Inventory>("Inventories");
            builder.EntitySet<Module>("Modules");
            builder.EntitySet<OrderDetail>("OrderDetails");
            builder.EntitySet<PaymentMethod>("PaymentMethods");
            builder.EntitySet<Price>("Prices");
            builder.EntitySet<PriceType>("PriceTypes");
            builder.EntitySet<ProductFeature>("ProductFeatures");
            builder.EntitySet<Product>("Products");
            builder.EntitySet<Promotion>("Promotions");
            builder.EntitySet<PromotionType>("PromotionTypes");
            builder.EntitySet<Resource>("Resources");
            builder.EntitySet<ResourceType>("ResourceTypes");
            builder.EntitySet<Role>("Roles");
            builder.EntitySet<StatusType>("StatusTypes");
            builder.EntitySet<User>("Users");
            builder.EntitySet<UserType>("UserTypes");


            builder.EntitySet<vwInventoryBySku>("GetInventoryBySku");
                


            config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            
        }
    }
}
