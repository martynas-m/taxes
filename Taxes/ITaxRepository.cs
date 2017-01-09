using System;

namespace Taxes
{
	public interface ITaxRepository
	{
		void AddTax(string municipality, TaxType taxType, float tax, DateTime start, DateTime end);
		float? FindTax(string municipality, TaxType taxType, DateTime effeciteDate);
	}

	public enum TaxType
	{
		Daily = 1,
		Weekly = 2,
	}
}