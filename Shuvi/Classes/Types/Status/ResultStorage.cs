using Shuvi.Interfaces.Status;

namespace Shuvi.Classes.Types.Status
{
    public class ResultStorage : IResultStorage
    {
        public List<IActionResult> Results { get; protected set; } = new();


        public void Add(IActionResult? result)
        {
            if (result is not null)
                Results.Add(result);
        }
        public void Add(IResultStorage storage)
        {
            Results.AddRange(storage.Results);
        }
        public void Clear()
        {
            Results.Clear();
        }
        public string GetDescriptions()
        {
            return string.Join("\n", Results.Select(x => x.Description));
        }
    }
}
