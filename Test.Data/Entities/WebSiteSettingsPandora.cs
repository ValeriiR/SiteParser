using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using Test.Data.Repositories.Abstract;

namespace Test.Data.Entities
{
    public class WebSiteSettingsPandora : IWebSiteSettings
    {
        public  string Domain { get; set; }

        private List<string> Domains = new List<string>();

        public WebSiteSettingsPandora()
        {
            Domain = "pandorajob.ru";

            Domains.Add("http://pandorajob.ru/office");

        }

        public List<Vacancy> Parse(string urlss)
        {
            List<Vacancy> vacancies = new List<Vacancy>();

            var urls = ParsePandoraRegions();

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
                            //Regex regex = new Regex(@"туп(\w*)");
                            string text = nodeSalaryInfo[i].InnerText;

                            string salary = text.Split(new string[] { "Зарплата от ", " р." },
                                System.StringSplitOptions.RemoveEmptyEntries).Single();


                            text = nodeVacancyInfo[i].InnerText;

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


        private List<string> ParsePandoraRegions()
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load("http://pandorajob.ru/regions");

            var citiesDiv = htmlDoc.DocumentNode.SelectNodes("//div[@class='t650']//a");


            List<string> urls = new List<string>();
            foreach (var div in citiesDiv)
            {
                var str = div.GetAttributeValue("href", "");
                urls.Add(str);

            }
            return urls;
        }
    }
}
