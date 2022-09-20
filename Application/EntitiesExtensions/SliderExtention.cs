using Application.Utils;
using DataLayer.Entities.Site;

namespace Application.EntitiesExtentions
{
    public static class SliderExtention
    {
        public static string GetSliderImageAddress(this Slider slider)
        {
            return PathExtension.SliderOrigin + slider.ImageName;
        }
    }
}
