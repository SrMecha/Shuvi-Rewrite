using Shuvi.Classes.Extensions;
using Shuvi.Classes.Types.Localization;
using Shuvi.Enums.Magic;
using Shuvi.Interfaces.Localization;
using Shuvi.Interfaces.Magic;
using Shuvi.Interfaces.Spell;
using Shuvi.Interfaces.User;

namespace Shuvi.Classes.Types.Magic
{
    public class MagicInfo : IMagicInfo
    {
        public MagicType MagicType { get; private set; }
        public ILocalizedInfo Info { get; private set; }

        public MagicInfo(MagicType magicType)
        {
            MagicType = magicType;
            Info = new CachedLocalizedInfo("magicTypes", magicType.GetLowerName());
        }
        public List<ISpell> GetAllSpells()
        {
            throw new NotImplementedException();
        }
        public List<ISpell> GetAvailableSpells(IDatabaseUser user)
        {
            throw new NotImplementedException();
        }
    }
}
