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
    public class JournalistController : ApiController
    {
        //ADD NEWS
        public HttpResponseMessage AddNews(News news)
        {
            try //try this:
            {
                //1. Connect to database (XML)
                XElement newsXML = XElement.Load(GetFilePath());

                //2. Add new information to XML
                newsXML.Add(
                    new XElement("news",
                        new XElement("id", news.Id),
                        new XElement("title", news.Title),
                        new XElement("date", news.Date),
                        new XElement("category", news.Category),
                        new XElement("text", news.Text)
                    )
                );

                //3. Save
                newsXML.Save(GetFilePath());

                //4. Return news
                return Request.CreateResponse(HttpStatusCode.Created, news);
            }
            catch //If "try" fails
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Kunne ikkje opprette ny nyhetsartikkel.");
            }

        }

        //UPDATE NEWS
        public HttpResponseMessage PutNews(News _news)
        {
            try
            {
                //1. Connect to database (XML)
                XElement newsXML = XElement.Load(GetFilePath());

                //2. Linq-query (where-statement to get 1 news by id)
                var selectedNews = (from news in newsXML.Descendants("news")
                                    where (int)news.Element("id") == _news.Id
                                    select news).SingleOrDefault();//retur one objekt or null 

                //3. Change XML-object
                selectedNews.SetElementValue("title", _news.Title);
                selectedNews.SetElementValue("date", _news.Date);
                selectedNews.SetElementValue("category", _news.Category);
                selectedNews.SetElementValue("text", _news.Text);

                //4. Save
                newsXML.Save(GetFilePath());

                //5. Return news
                return Request.CreateResponse(HttpStatusCode.OK, selectedNews);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Kunne ikkje oppdatere nyhetsartikkelen.");
            }

        }

        //GET NEWS
        public HttpResponseMessage GetNews()
        {
            try
            {
                //1. Connect to database (XML)
                XElement newsXML = XElement.Load(GetFilePath());

                //2. Linq-query (where-statement to get 1 news by id)
                var newsList = from news in newsXML.Descendants("news")
                               select news;//retur one objekt or null 

                //3. Return news
                return Request.CreateResponse(HttpStatusCode.OK, newsList);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Kunne ikkje hente nyhetsartikkel.");
            }

        }

        //GET NEWS BY ID
        public HttpResponseMessage GetNewsId(int? id)
        {
            if (id != null)
            {
                //1. Connect to database (XML)
                XElement newsXML = XElement.Load(GetFilePath());

                //2. Linq-query (where-statement to get 1 news by id)
                var selectedNews = (from news in newsXML.Descendants("news")
                                    where (int)news.Element("id") == id
                                    select news).SingleOrDefault();//retur one objekt or null 

                //3. return news
                return Request.CreateResponse(HttpStatusCode.OK, selectedNews);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Kunne ikkje hente id.");
            }
        }

        //DELETE NEWS
        public HttpResponseMessage DeleteNews(int id) //void går også i stede for HttpResponseMessage
        {
            try
            {
                //1. Connect to database (XML)
                XElement newsXML = XElement.Load(GetFilePath());

                //2. Linq-query (where-statement to get 1 news by id)
                var selectedNews = (from news in newsXML.Descendants("news")
                                    where (int)news.Element("id") == id
                                    select news).SingleOrDefault();//retur one objekt or null 

                //3. Remove selectedNews form XML
                selectedNews.Remove();

                //4. Save deleted news in XML
                newsXML.Save(GetFilePath());

                //5. Return selectedNews
                return Request.CreateResponse(HttpStatusCode.OK, selectedNews);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Nyhetsartikkelen blei IKKJE sletta.");
            }

        }

        //GET FILEPATH TO XML
        public String GetFilePath() //Get the path to the XML
        {
            return System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/newsArticles.xml");
        }
    }
}
