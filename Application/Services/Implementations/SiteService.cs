using Application.Services.Interfaces;
using DataLayer.Entities.Site;
using DataLayer.Repositories.GenericRepostitory;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class SiteService : ISiteService
    {
        #region Constructor
        private readonly IGenericRepository<SiteSetting> _siteSettingRepository;
        private readonly IGenericRepository<Slider> _sliderRepository;
        private readonly IGenericRepository<SiteBanner> _siteBannerRepository;
        //private ra
        public SiteService(IGenericRepository<SiteSetting> siteSettingRepository, IGenericRepository<Slider> sliderRepository
            , IGenericRepository<SiteBanner> siteBannerRepository)
        {
            _siteSettingRepository = siteSettingRepository;
            _sliderRepository = sliderRepository;   
            _siteBannerRepository = siteBannerRepository;
        }

        #endregion
        #region SiteSetting
        public async Task<SiteSetting> GetDefaultSiteSettingAsync()
        {
            return await _siteSettingRepository.GetQuery().AsQueryable().
                SingleOrDefaultAsync(x => x.IsDefault && !x.IsDeleted);
        }
        #endregion
        #region Slider
        public async Task<List<Slider>> GetAllActiveSlidrsAsync()
        {
            return await _sliderRepository.GetQuery().AsQueryable().Where(x => x.IsActive && !x.IsDeleted).ToListAsync();
        }
        #endregion
        #region Banner
        public async Task<List<SiteBanner>> GetSiteBannersByPlacementAsync(List<BannerPlacement> placements)
        {
            return await _siteBannerRepository.GetQuery().AsQueryable().
                Where(x => placements.Contains(x.BannerPlacement)).ToListAsync();
        }
        #endregion
        #region Dispose
        public async ValueTask DisposeAsync()
        {
            if(_siteSettingRepository != null ) await _siteSettingRepository.DisposeAsync();
            if (_sliderRepository != null) await _sliderRepository.DisposeAsync();
            if (_siteBannerRepository != null) await _siteBannerRepository.DisposeAsync();
        }
        #endregion
    }
}
