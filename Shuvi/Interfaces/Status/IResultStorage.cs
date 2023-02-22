namespace Shuvi.Interfaces.Status
{
    public interface IResultStorage
    {
        public List<IActionResult> Results { get; }

        public string GetDescriptions();
        public void Add(IActionResult? result);
        public void Add(IResultStorage storage);
        public void Clear();
    }
}
