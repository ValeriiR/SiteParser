using System;
using System.Collections.Generic;
using System.Text;
using Test.Data.Entities;

namespace Test.Data.Repositories.Abstract
{
    public interface IWebSiteSettingsRepository
    {
        List<IWebSiteSettings> GetWebSitesSettings();
    }
}
