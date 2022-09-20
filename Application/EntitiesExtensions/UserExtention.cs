using Application.Utils;
using DataLayer.DTOs.Account;

namespace Application.EntitiesExtentions
{
    public static class UserExtention
    {
        public static string GetUserAvatar(this EditUserProfileDTO profile)
        {
            return PathExtension.UserAvatarOrigin + profile.Avatar;
        }
    }
}
