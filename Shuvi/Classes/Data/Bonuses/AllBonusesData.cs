using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shuvi.Classes.Data.Bonuses
{
    public class AllBonusesData : FightBonusesData
    {
        public int Health { get; set; } = 0;
        public int Mana { get; set; } = 0;
        public int Energy { get; set; } = 0;
    }
}
