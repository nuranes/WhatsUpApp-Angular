using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;
using WhatsUpApp.Models;

namespace WhatsUpApp.Controllers
{
    public class CustomerController : ApiController
    {

        public HttpResponseMessage GetNews()
        {
            try //try this:
            {
                //1. Connect to database (XML)
                XElement newsXML = XElement.Load(GetFilePath());

                //2. Add new information to XML
                var newsList = from news in newsXML.Descendants("news")
                               select news;

                //3. Return news
                return Request.CreateResponse(HttpStatusCode.OK, newsList);
            }
            catch //If try fails:
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Kunne ikkje hente nyhetsartikkel.");
            }
        }

        public String GetFilePath() //Get the pathe to the XML
        {
            return System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/newsArticles.xml");
        }
    }
}
