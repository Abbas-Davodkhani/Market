using DataLayer.DTOs;
using DataLayer.DTOs.Common;
using DataLayer.DTOs.Store;
using DataLayer.DTOs.Stores;
using DataLayer.Entities;
using System;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IStoreService : IAsyncDisposable
    {
        #region RequestStore
        Task<RequestStorePanelResult> AddNewStoreRequest(RequestStorePanelDTO requestStore , long userId);
        Task<FilterRequestStoreDTo> FilterStores(FilterRequestStoreDTo filter);
        Task<EditRequestStoreDTO> GetEditStoreRequestForEdit(long id , long currentUserId);
        Task<EditRequestStoreResult> EditStoreRequest(EditRequestStoreDTO request , long currentUserId);
        Task<bool> AcceptSellerRequest(long requestId);
        Task<bool> RejectSellerRequest(RejectItemDTO reject);
        Task<Store> GetLastActiveSellerByUserId(long userId);
        #endregion
    }
}
