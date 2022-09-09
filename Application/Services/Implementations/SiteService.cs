using Application.Services.Interfaces;
using DataLayer.Entities.Site;
using DataLayer.Repositories.GenericRepostitory;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class SiteService : ISiteService
    {
        #region Constructor
        private readonly IGenericRepository<SiteSetting> _siteSettingRepository;
        public SiteService(IGenericRepository<SiteSetting> siteSettingRepository)
        {
            _siteSettingRepository = siteSettingRepository;
        }

        #endregion
        #region SiteSetting
        public async Task<SiteSetting> GetDefaultSiteSettingAsync()
        {
            return await _siteSettingRepository.GetQuery().AsQueryable().
                SingleOrDefaultAsync(x => x.IsDefault && !x.IsDeleted);
        }
        #endregion
        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _siteSettingRepository.DisposeAsync();  
        }
        #endregion
    }
}
