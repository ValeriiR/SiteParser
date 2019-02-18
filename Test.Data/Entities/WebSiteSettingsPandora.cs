using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

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


            HtmlNodeCollection citiesDiv = htmlDoc.DocumentNode.SelectNodes("//div[@class='t650']//a");


            List<string> urls = new List<string>();
            foreach (var div in citiesDiv)
            {
                var str = div.GetAttributeValue("href", "");
                urls.Add(str);

            }
            return urls;
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
