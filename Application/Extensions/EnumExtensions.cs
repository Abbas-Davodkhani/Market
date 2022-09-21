using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Application.Extensions
{
    public static class EnumExtensions
    {
        public static string GetEnumDisplayName(this Enum myEnum)
        {
            var displayEnumName = myEnum.GetType().GetMember(myEnum.ToString()).FirstOrDefault();
            if (displayEnumName != null)
            {
                return displayEnumName.GetCustomAttribute<DisplayAttribute>()?.GetName();
            }
            return "";
        }
    }
}
