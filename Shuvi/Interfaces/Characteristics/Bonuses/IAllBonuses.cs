namespace Shuvi.Interfaces.Characteristics.Bonuses
{
    public interface IAllBonuses : IFightBonuses, IDynamicBonuses
    {
        public void Add(IAllBonuses bonuses);
    }
}
