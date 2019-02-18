using System;
using System.Collections.Generic;
using System.Text;
using Test.Data.Entities;
using Test.Model.Models;

namespace Test.Model.Services.Abstract
{
    public interface IParseService
    {
        List<Vacancy> Parse(string urls);
        List<Vacancy> GetVacancies();
    }
}
