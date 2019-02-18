using System;
using System.Collections.Generic;
using System.Text;
using Test.Data.Entities;
using Test.Data.Repositories.Abstract;

namespace Test.Data.Repositories.Concrete
{
    public class VacancyRepository : IVacancyRepository
    {
       

        private List<Vacancy> _vacancies = new List<Vacancy>
        {
            new Vacancy
            {
                City = "Odessa", Country = "Ukraine", Description = "Good job", Salary = "100500",
                VacancyName = "Engineer", Id = 1
            }
        };
    
        public List<Vacancy> GetVacancies()
        {
            return _vacancies;
        }

        public void AddVacancy(Vacancy vacancy)
        {
            _vacancies.Add(vacancy);
        }

        public void AddVacancies(List<Vacancy> vacancies)
        {
            foreach (var vacancy in vacancies)
            {
                _vacancies.Add(vacancy);
            }
        }
    }
}
