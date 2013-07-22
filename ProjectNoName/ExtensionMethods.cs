using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuikTODO
{
    public static class ExtensionMethods
    {
        public static int GetWeekNumber(this DateTime date)
        {
            return (int)(date.GetDayNumber() / 7M);
        }

        public static int GetDayNumber(this DateTime date)
        {
            var firstWeek = new DateTime(DateTime.Today.Year, 1, 1);
            var totalDays = (date.Date - firstWeek.Date).Days + 1;

            return (int)totalDays;
        }

        public static bool IsThisWeek(this DateTime date)
        {
            if (DateTime.Today.GetWeekNumber() == date.GetWeekNumber())
            {
                return true;
            }
            return false;
        }

        public static bool IsPreviousWeek(this DateTime date)
        {
            if (DateTime.Today.GetWeekNumber() - date.GetWeekNumber() == 1)
            {
                return true;
            }
            return false;
        }

        public static bool IsNextWeek(this DateTime date)
        {
            if (date.GetWeekNumber() - DateTime.Today.GetWeekNumber() == 1)
            {
                return true;
            }
            return false;
        }

        public static bool IsFutureDate(this DateTime date)
        {
            if (date.GetDayNumber() - DateTime.Today.GetDayNumber() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
