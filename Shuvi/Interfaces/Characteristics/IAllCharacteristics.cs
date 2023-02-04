namespace Shuvi.Interfaces.Characteristics
{
    public interface IAllCharacteristics
    {
        public IDynamicCharacteristics Dynamic { get; init; }
        public IStaticCharacteristics Static { get; init; }
    }
}
