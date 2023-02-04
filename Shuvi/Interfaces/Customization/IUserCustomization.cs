using Discord;
using Shuvi.Enums.User;

namespace Shuvi.Interfaces.Customization
{
    public interface IUserCustomization
    {
        public Color Color { get; set; }
        public IUserCustomization Banner { get; set; }
        public UserBages Bages { get; set; }
    }
}
