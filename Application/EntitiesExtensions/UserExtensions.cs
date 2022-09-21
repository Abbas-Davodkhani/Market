using DataLayer.Entities.Account;

namespace Application.EntitiesExtensions
{
    public static class UserExtensions
    {
        public static string GetUserFullName(this User user)
        {
            if (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName))
            {
                return $"{user.FirstName} {user.LastName}";
            }
            return user.Mobile;
        }
    }
}
