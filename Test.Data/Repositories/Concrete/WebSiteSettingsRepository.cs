using System.Collections.Generic;
using Test.Data.Entities;
using Test.Data.Repositories.Abstract;

namespace Test.Data.Repositories.Concrete
{
    public class WebSiteSettingsRepository : IWebSiteSettingsRepository
    {
        List<IWebSiteSettings> _webSiteSettingses=new List<IWebSiteSettings>
        {
            new WebSiteSettingsPandora()
        };


        public List<IWebSiteSettings> GetWebSitesSettings()
        {
            return _webSiteSettingses;
        }
    }
}
