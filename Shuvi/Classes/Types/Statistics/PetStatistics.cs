using Shuvi.Classes.Data.Statistics;
using Shuvi.Interfaces.Statistics;

namespace Shuvi.Classes.Types.Statistics
{
    public class PetStatistics : IPetStatistics
    {
        public long TamedAt { get; }

        public PetStatistics(long tamedAt)
        {
            TamedAt = tamedAt;
        }
        public PetStatistics(PetStatisticsData data)
        {
            TamedAt = data.TamedAt;
        }
    }
}
