using System;
using System.Globalization;

namespace PGADataScaper
{
	public static class WeekOfYearExtension
	{
		// Some reference documentation for this:
		// https://blogs.msdn.microsoft.com/shawnste/2006/01/24/iso-8601-week-of-year-format-in-microsoft-net/
		// http://stackoverflow.com/questions/11154673/get-the-correct-week-number-of-a-given-date

		public static int Iso8601WeekOfYear( this DateTime dt) {
			DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(dt);
			if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday) {
				dt = dt.AddDays(3);
			}

			// Return the week of our adjusted day
			return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(dt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
		}
	}
}
