using Shuvi.Enums.Actions;

namespace Shuvi.Interfaces.Actions
{
    public interface IFightActions
    {
        public int LightAttack { get; set; }
        public int HeavyAttack { get; set; }
        public int Dodge { get; set; }
        public int Defense { get; set; }
        public int Spell { get; set; }

        public void SetAction(FightAction action, int rate);
        public int GetAction(FightAction action);
        public FightAction GetRandomAction();
    }
}
