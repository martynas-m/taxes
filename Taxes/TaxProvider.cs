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
			foreach (TaxType type in Enum.GetValues(typeof(TaxType)))
			{
				var tax = _taxRepo.FindTax(muninicipality, type, date);
				if (tax.HasValue)
					return tax;
			}
			return null;
		}
	}
}