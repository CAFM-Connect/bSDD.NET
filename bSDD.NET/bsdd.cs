using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using RestSharp;
using bsDD.NET.Model.Objects;
using System.IO;

namespace bsDD.NET
{
    public class bsdd
    {

        private string Protocol;
        private string Domain;
        public string BaseUrl { get; }

        private RestClient restclient;
        public IfdAPISession Session { get; }

        public IfdLanguageList Languages { get; set; }

        public bsdd(string email, string password, bool testserver = false)
        {
            Protocol= "http";
            if (testserver)
                Domain= "test.bsdd.buildingsmart.org";
            else
                Domain = "bsdd.buildingsmart.org";
            BaseUrl = Protocol+"://"+ Domain +"/api/4.0";
            restclient = new RestClient(BaseUrl);
            Session = Login(email, password);
            GetLanguages();
        }

        private IfdAPISession Login(string email, string password)
            {
            var request = new RestRequest("/session/login", Method.POST);
            request.AddParameter("email", email); 
            request.AddParameter("password", password); 

            var response = restclient.Execute<IfdAPISession>(request);
            IfdAPISession session = response.Data;
            return session;
        }

        private void GetLanguages()
        {
            var request = new RestRequest("/IfdLanguage", Method.GET);
            var response = restclient.Execute<IfdLanguageList>(request);
            Languages = response.Data;
        }

        public IfdNameList SearchNames(string searchstring)
        {
            var request = new RestRequest("/IfdName/search/" + searchstring, Method.GET);
            var response = restclient.Execute<IfdNameList>(request);     
            return response.Data;
        }

        public IfdNameList SearchNamesByLanguage(string searchstring, string language)
        {
            string languageguid = Languages.IfdLanguage.Where(x => x.LanguageCode == language).FirstOrDefault().Guid;
            var request = new RestRequest("/IfdName/search/filter/language/" + languageguid + "/" + searchstring, Method.GET);
            var response = restclient.Execute<IfdNameList>(request);
            return response.Data;
        }

        public IfdConceptList SearchConcepts(string searchstring)
        {
            var request = new RestRequest("/IfdConcept/search/" + searchstring, Method.GET);
            var response = restclient.Execute<IfdConceptList>(request);
            return response.Data;
        }

    }
}
