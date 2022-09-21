using System.IO;

namespace Application.Utils
{
    public static class PathExtension
    {
        #region DefaultImages

        public static string DefaultAvatar = "/img/defaults/avatar.jpg";

        #endregion
        #region UserAvatar
        public static string UserAvatarOrigin = "/Content/Images/UserAvatar/origin/";
        public static string UserAvatarOriginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Images/UserAvatar/origin/");

        public static string UserAvatarThumb = "/Content/Images/UserAvatar/Thumb/";
        public static string UserAvatarThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Images/UserAvatar/Thumb/");
        #endregion
        #region UploadImage
        public static string UploadImageOrigin = "/img/Upload/";
        public static string UploadImageOriginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Upload/");
        #endregion
        #region slider

        public static string SliderOrigin = "/img/slider/";

        #endregion
        #region Banner

        public static string BannerOrigin = "/img/bg/";

        #endregion
    }
}
