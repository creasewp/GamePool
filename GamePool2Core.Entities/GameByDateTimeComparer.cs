using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePool2Core.Entities
{
    public class GameByDateTimeComparer : IComparer<Game>
    {
        public int Compare(Game x, Game y)
        {
            DateTime datex;
            DateTime.TryParse(x.GameDateTime, out datex);
            DateTime datey;
            DateTime.TryParse(y.GameDateTime, out datey);

            return datex.CompareTo(datey);
        }
    }

    public class GameByDateTimeComparerASC : IComparer<Game>
    {
        public int Compare(Game x, Game y)
        {
            DateTime datex;
            DateTime.TryParse(x.GameDateTime, out datex);
            DateTime datey;
            DateTime.TryParse(y.GameDateTime, out datey);

            return datey.CompareTo(datex);
        }
    }

}
