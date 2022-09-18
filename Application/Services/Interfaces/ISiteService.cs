using DataLayer.Entities.Site;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface ISiteService : IAsyncDisposable
    {
        Task<SiteSetting> GetDefaultSiteSettingAsync();
        Task<List<Slider>> GetAllActiveSlidrsAsync();
        Task<List<SiteBanner>> GetSiteBannersByPlacementAsync(List<BannerPlacement> placements);
    }
}
