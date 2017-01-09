using System;

namespace Taxes
{
	public class TaxProvider
	{
		private readonly ITaxRepository _taxRepo;

		public TaxProvider(ITaxRepository taxRepo)
		{
			_taxRepo = taxRepo;
		}

		public float? GetForDate(string muninicipality, DateTime date)
		{
			return _taxRepo.FindDailyTax(muninicipality, date) ?? _taxRepo.FindWeeklyTax(muninicipality, date);
		}
	}
}