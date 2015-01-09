using System;
using System.Collections.Generic;

namespace GamePool2Core.Entities
{
    public class UserGameByDateTimeComparer : IComparer<UserGame>
    {
        public int Compare(UserGame x, UserGame y)
        {
            DateTime datex;
            DateTime.TryParse(x.Game.GameDateTime, out datex);
            DateTime datey;
            DateTime.TryParse(y.Game.GameDateTime, out datey);

            return datex.CompareTo(datey);
        }
    }
}
