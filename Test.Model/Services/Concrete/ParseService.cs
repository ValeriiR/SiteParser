using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Test.Data.Entities;
using Test.Data.Repositories.Abstract;
using Test.Model.Models;
using Test.Model.Services.Abstract;
using Test.Data.Repositories.Concrete;
using HtmlAgilityPack;

namespace Test.Model.Services.Concrete
{
    public class ParseService : IParseService
    {
        private IVacancyRepository _vacancyRepository;
        private IWebSiteSettingsRepository _webSiteSettingsRepository;

        public ParseService()
        {
            _vacancyRepository = new VacancyRepository();
            _webSiteSettingsRepository = new WebSiteSettingsRepository();
        }

        //public ParseService(IVacancyRepository vacancyRepository)
        //{
        //    _vacancyRepository = vacancyRepository;
        //}



        public List<Vacancy> Parse(string url)
        {
            try
            {
                var siteSettingsList = _webSiteSettingsRepository.GetWebSitesSettings();

                IWebSiteSettings webSiteSettings = null;

                foreach (var setting in siteSettingsList)
                {
                    if (url.Contains(setting.Domain))
                        webSiteSettings = setting;
                }

                if (webSiteSettings != null)
                {
                   List<Vacancy> vacancies= webSiteSettings.Parse(url);
                   _vacancyRepository.AddVacancies(vacancies);
                                 
                }
                return _vacancyRepository.GetVacancies();

              

            }

            catch (Exception ex)
            {
                return new List<Vacancy>();
            }

        }
     

        public List<Vacancy> GetVacancies()
        {
            var vacancies = _vacancyRepository.GetVacancies();
            return vacancies;
        }
    }
}












// var htmlDoc = web.Load(html);
//HtmlDocument htmlDoc = web.Load(url);

//// HtmlAgilityPack.HtmlNode node = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='t125__title t-title']");
//var nodeVacancyInfo = htmlDoc.DocumentNode.SelectNodes("//div[@class='t125__title t-title']");
//var nodeSalaryInfo =
//    htmlDoc.DocumentNode.SelectNodes("//div[@class='t125__descr t-descr t-descr_xxs']");


//for (int i = 0; i < nodeVacancyInfo.Count; ++i)
//{
//    //   string[] separatingChars = { "Зарплата от ", " р." };
//    string text = nodeSalaryInfo[i].InnerText;
//    string salary = text.Split(new string[] { "Зарплата от ", " р." },
//        System.StringSplitOptions.RemoveEmptyEntries).Single();



//    text = nodeVacancyInfo[i].InnerText;

//    string city = text.Split(new char[] { '.' }, System.StringSplitOptions.RemoveEmptyEntries)
//        .FirstOrDefault();
//    text = text.Substring(city.Length + 1);

//    string vacancy = text.Split(new char[] { '.', ' ' }, System.StringSplitOptions.RemoveEmptyEntries)
//        .FirstOrDefault();
//    text = text.Substring(vacancy.Length + 1);

//    string description = text;


//    _vacancyRepository.AddVacancy(new Vacancy
//    { Country = "", City = city, Description = description, Salary = salary, VacancyName = vacancy });
//}




//  var nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id='rec23515634']/div/div/div/div");


//*[@id="rec23515634"]/div
//*[@id="rec33230308"]/div


//for (int i = 0; i < nodes.Count; ++i)
//{
//    if (i % 2 == 0)
//    {
//        string[] separatingChars = { "Зарплата от ", " р." };

//        string text = nodes[i + 1].InnerText;

//        string salary = text.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries).Single();

//        text = nodes[i].InnerText;


//        string city = text.Split(new char[] { '.' }, System.StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

//        text = text.Substring(city.Length+1);
//        string vacancy = text.Split(new char[] { '.', ' ' }, System.StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

//        text = text.Substring(vacancy.Length + 1);

//      //  string description = text.Split(new char[] { '.' }, System.StringSplitOptions.RemoveEmptyEntries).Last();

//      string description = text;

//        _vacancyRepository.AddVacancy(new Vacancy { Country = "", City = city, Description = description, Salary = salary, VacancyName = vacancy });


//    }

//}