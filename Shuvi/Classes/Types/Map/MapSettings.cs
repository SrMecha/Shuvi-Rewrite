using Shuvi.Classes.Data.Map;
using Shuvi.Interfaces.Map;

namespace Shuvi.Classes.Types.Map
{
    public class MapSettings : IMapSettings
    {
        public string PictureURL { get; init; }

        public MapSettings(MapSettingsData data)
        {
            PictureURL = data.PictureURL;
        }
    }
}
