using System;

namespace Taxes
{
	public interface ITaxRepository
	{
		float? FindDailyTax(string municipality, DateTime dateFor);
		float? FindWeeklyTax(string municipality, DateTime dateFor);
		void AddDailyTax(string vilnius, DateTime today, float tax);
	}
}