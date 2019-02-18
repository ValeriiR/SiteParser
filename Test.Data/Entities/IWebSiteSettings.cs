

using System.Collections.Generic;

namespace Test.Data.Entities
{
    public interface IWebSiteSettings
    {
        string Domain { get; set; }
        List<Vacancy> Parse(string urls);
    }
}
