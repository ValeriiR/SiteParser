using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Test.Data.Repositories.Abstract;

namespace Test.Data.Entities
{
    public class WebSiteSettingsPandora : IWebSiteSettings
    {
        public string Domain { get; set; }

        private List<DomainsToParse> domainsToParse = new List<DomainsToParse>();

        public WebSiteSettingsPandora()
        {
            Domain = "pandorajob.ru";

            domainsToParse.Add(new DomainsToParse("http://pandorajob.ru/office"));

        }

        public List<Vacancy> Parse(string urlss)
        {
            //  GetDescriptionFromVacancyLink("http://pandorajob.ru/ekaterinburg");



            List<Vacancy> vacancies = new List<Vacancy>();

            List<string> urls = ParsePandoraRegions();

            HtmlWeb web = new HtmlWeb();

            List<HtmlDocument> htmlDocs = new List<HtmlDocument>();
            for (int i = 0; i < urls.Count; ++i)
            {
                var doc = web.Load(urls[i]);
                htmlDocs.Add(doc);

            }

            for (int k = 0; k < htmlDocs.Count; ++k)
            {
                var nodeVacancyInfo =
                    htmlDocs[k].DocumentNode.SelectNodes("//div[@class='t125__title t-title']");
                var nodeSalaryInfo = htmlDocs[k].DocumentNode
                    .SelectNodes("//div[@class='t125__descr t-descr t-descr_xxs']");

                if (nodeSalaryInfo != null && nodeVacancyInfo != null)
                {
                    for (int i = 0; i < nodeVacancyInfo.Count; ++i)
                    {
                        try
                        {

                            string desc = nodeSalaryInfo[i].InnerText;

                            string salary = GetSalaryFromDescription(desc);

                            //string salary = text.Split(new string[] { "Зарплата от ", " р." },
                            //    System.StringSplitOptions.RemoveEmptyEntries).Single();


                            string text = nodeVacancyInfo[i].InnerText;
                           
                            string city = text.Split(new char[] { '.' },
                                    System.StringSplitOptions.RemoveEmptyEntries)
                                .FirstOrDefault();
                            text = text.Substring(city.Length + 1);

                            string vacancy = text.Split(new char[] { '.', ' ' },
                                    System.StringSplitOptions.RemoveEmptyEntries)
                                .FirstOrDefault();
                            text = text.Substring(vacancy.Length + 1);

                            string description = text;


                            vacancies.Add(new Vacancy
                            {
                                Country = "",
                                City = city,
                                Description = description,
                                Salary = salary,
                                VacancyName = vacancy
                            });
                        }

                        catch
                        {
                            continue;
                        }
                    }
                }

            }

            return vacancies;
        }

        private List<string> GetInfoFromMainBlock(string text)
        {
            Regex regex = new Regex(@"(^(\w+)(\.))");

            var match = regex.Match(text);
            string str = match.Value;
            string cit = str.TrimEnd('.');



            regex = new Regex(@"((\. )(\w+)-(\w+))|((\. )(\w+))");
            match = regex.Match(text);
            str = match.Value;
            string vac = str.TrimStart('.');



            regex = new Regex(@"((\. )(\w+)-(\w+))|((\. )(\w+))");

            if (vac != "")
            {
                text = text.Substring(vac.Length + 1);

                string description = text;
            }

            return new List<string>();
        }

        private string GetSalaryFromDescription(string text)
        {
            Regex regex = new Regex(@" ((\d+)\s(\d+) (р\.))|((\d+)\s(\d+) (руб))");

            var match = regex.Match(text);
            string str = match.Value;
            str = str.TrimEnd('.');
            return str;
        }

        private List<string> ParsePandoraRegions()
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument htmlDoc = web.Load("http://pandorajob.ru/regions");


            //*[@id="rec23655290"]

            HtmlNodeCollection citiesDiv = htmlDoc.DocumentNode.SelectNodes("//div[@class='t650']//a");


            List<string> urls = new List<string>();
            foreach (var div in citiesDiv)
            {
                var str = div.GetAttributeValue("href", "");
                urls.Add(str);

            }
            return urls;
        }
        private string GetDescriptionFromVacancyLink(string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument htmlDoc = web.Load(url);

            //  HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='t650']//a");
            HtmlNodeCollection linksElements = htmlDoc.DocumentNode.SelectNodes("//div[@class='t125']//a");

            List<string> links = new List<string>();
            foreach (var div in linksElements)
            {
                var str = div.GetAttributeValue("href", "");
                string domain = "http://pandorajob.ru";
                domain = domain + str;
                links.Add(domain);

            }




            List<string> descs = new List<string>();

            foreach (var link in links)
            {
                htmlDoc = web.Load(link);

                HtmlNode description = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='t650']//a");

                descs.Add(description.InnerText);
            }





            //string description = nodes.InnerText;



            return "";
        }


        private class DomainsToParse
        {
            public string Domain { get; set; }

            public bool isParsed { get; set; }

            public DomainsToParse(string domain)
            {
                Domain = domain;
                isParsed = false;
            }
        }
    }



}
