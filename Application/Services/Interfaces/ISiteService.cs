using DataLayer.Entities.Site;
using System;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface ISiteService : IAsyncDisposable
    {
        Task<SiteSetting> GetDefaultSiteSettingAsync();
    }
}
