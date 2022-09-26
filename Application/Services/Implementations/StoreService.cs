using Application.Extensions;
using Application.Services.Interfaces;
using DataLayer.DTOs;
using DataLayer.DTOs.Common;
using DataLayer.DTOs.Paging;
using DataLayer.DTOs.Store;
using DataLayer.DTOs.Stores;
using DataLayer.Entities;
using DataLayer.Entities.Account;
using DataLayer.Repositories.GenericRepostitory;
using Microsoft.EntityFrameworkCore;
using System.Linq;

using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class StoreService : IStoreService
    {
        #region Constructor
        private readonly IGenericRepository<Store> _storeRepository;
        private readonly IGenericRepository<User> _userRespository;
        public StoreService(IGenericRepository<Store> storeRepository, IGenericRepository<User> userRepository)
        {
            _storeRepository = storeRepository;
            _userRespository = userRepository;  
        }
        #endregion
        #region RequestStore
        public async Task<RequestStorePanelResult> AddNewStoreRequest(RequestStorePanelDTO requestStore , long userId)
        {
            var isBlockedUser = await _userRespository.GetQuery().AsQueryable().AnyAsync(u => u.Id == userId && u.IsBlocked);
            if (isBlockedUser) return RequestStorePanelResult.HasNotPermission;

            var isHasUnderProgressRequest = await _storeRepository.GetQuery().AsQueryable().AnyAsync(s => s.SellerId == userId && s.StoreAcceptanceState == StoreAcceptanceState.UnderProgress);
            if (isHasUnderProgressRequest) return RequestStorePanelResult.HasUnderProgressRequest;


            var store = new Store
            {
                SellerId = userId,
                StoreName = requestStore.StoreName,
                Address = requestStore.Address,
                Phone = requestStore.Phone,
                StoreAcceptanceState = StoreAcceptanceState.UnderProgress
            };

            await _storeRepository.AddEntityAsync(store);
            await _userRespository.SaveChangesAsync();  

            return RequestStorePanelResult.Success;
        }

        public async Task<FilterRequestStoreDTo> FilterStores(FilterRequestStoreDTo filter)
        {
            var query = _storeRepository.GetQuery()
                .Include(s => s.Seller)
                .AsQueryable();

            #region State

            switch (filter.FilterStoreState)
            {
                case FilterStoreState.All:
                    query = query.Where(s => !s.IsDeleted);
                    break;
                case FilterStoreState.Accepted:
                    query = query.Where(s => s.StoreAcceptanceState == StoreAcceptanceState.Accepted && !s.IsDeleted);
                    break;

                case FilterStoreState.UnderProgress:
                    query = query.Where(s => s.StoreAcceptanceState == StoreAcceptanceState.UnderProgress && !s.IsDeleted);
                    break;
                case FilterStoreState.Rejected:
                    query = query.Where(s => s.StoreAcceptanceState == StoreAcceptanceState.Rejected && !s.IsDeleted);
                    break;
            }

            #endregion

            #region Filter

            if (filter.UserId != null && filter.UserId != 0)
                query = query.Where(s => s.SellerId == filter.UserId);

            if (!string.IsNullOrEmpty(filter.StoreName))
                query = query.Where(s => EF.Functions.Like(s.StoreName, $"%{filter.StoreName}%"));

            if (!string.IsNullOrEmpty(filter.Phone))
                query = query.Where(s => EF.Functions.Like(s.Phone, $"%{filter.Phone}%"));

            if (!string.IsNullOrEmpty(filter.Mobile))
                query = query.Where(s => EF.Functions.Like(s.Mobile, $"%{filter.Mobile}%"));

            if (!string.IsNullOrEmpty(filter.Address))
                query = query.Where(s => EF.Functions.Like(s.Address, $"%{filter.Address}%"));

            #endregion

            #region Paging

            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HomManyPageAfterAndBefore);
            var allEntities = await query.Paging(pager).ToListAsync();

            #endregion

            return filter.SetPaging(pager).SetStores(allEntities);
        }
        public async Task<EditRequestStoreDTO> GetEditStoreRequestForEdit(long id , long currentUserId)
        {
            var store = await _storeRepository.GetByIdAsync(id);
            if (store == null || store.SellerId != currentUserId) return null;
            return new EditRequestStoreDTO
            {
                StoreName = store.StoreName,
                Address = store.Address,
                Id = id,
                Phone = store.Phone,
            };
        }

        public async Task<EditRequestStoreResult> EditStoreRequest(EditRequestStoreDTO request, long currentUserId)
        {
            var store = await _storeRepository.GetByIdAsync(request.Id);
            if (store == null || store.SellerId != currentUserId) return EditRequestStoreResult.NotFound;

            store.Phone = request.Phone;
            store.Address = request.Address;
            store.StoreName = request.StoreName;
            store.StoreAcceptanceState = StoreAcceptanceState.UnderProgress;
            _storeRepository.UpdateEntity(store);
            await _storeRepository.SaveChangesAsync();

            return EditRequestStoreResult.Success;
        }
        public async Task<bool> AcceptSellerRequest(long requestId)
        {
            var sellerRequest = await _storeRepository.GetByIdAsync(requestId);
            if (sellerRequest != null)
            {
                sellerRequest.StoreAcceptanceState = StoreAcceptanceState.Accepted;
                _storeRepository.UpdateEntity(sellerRequest);
                await _storeRepository.SaveChangesAsync();

                return true;
            }

            return false;
        }
        public async Task<bool> RejectSellerRequest(RejectItemDTO reject)
        {
            var sellerRequest = await _storeRepository.GetByIdAsync(reject.Id);
            if (sellerRequest != null)
            {
                sellerRequest.StoreAcceptanceState = StoreAcceptanceState.Rejected;
                sellerRequest.StoreAcceptanceDescription = reject.RejectMessage;

                _storeRepository.UpdateEntity(sellerRequest);
                await _storeRepository.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<Store> GetLastActiveSellerByUserId(long userId)
        {
            return await _storeRepository.GetQuery()
                .OrderByDescending(s => s.CreatedDate)
                .FirstOrDefaultAsync(s =>
                    s.SellerId == userId && s.StoreAcceptanceState == StoreAcceptanceState.Accepted);
        }
        #endregion
        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _storeRepository.DisposeAsync();
            await _userRespository.DisposeAsync();
        }

        #endregion
    }
}
