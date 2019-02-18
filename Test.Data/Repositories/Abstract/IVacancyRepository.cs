

using System.Collections.Generic;
using Test.Data.Entities;

namespace Test.Data.Repositories.Abstract
{
   public interface IVacancyRepository
   {
       List<Vacancy> GetVacancies();

       void AddVacancy(Vacancy vacancy);

       void AddVacancies(List<Vacancy> vacancies);
   }
}
