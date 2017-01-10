using System;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Taxes.Importer
{
	internal class YamlImporter
	{
		private readonly ITaxRepository _repository;

		public YamlImporter(ITaxRepository repository)
		{
			_repository = repository;
		}

		public void Import(TextReader yaml)
		{
			var deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());
			var data = deserializer.Deserialize<YamlStructure>(yaml);

			foreach (var tax in data.Daily)
				_repository.AddTax(data.Municipality, TaxType.Daily, tax.Tax, tax.Date, tax.Date);

			foreach (var tax in data.Weekly)
				_repository.AddTax(data.Municipality, TaxType.Weekly, tax.Tax, tax.Date, tax.Date.AddDays(7));

			var year = DateTime.Today.Year;
			foreach (var tax in data.Monthly)
			{
				var start = new DateTime(year, tax.Month, 1);
				_repository.AddTax(data.Municipality, TaxType.Monthly, tax.Tax, start,
					start.AddMonths(1).AddDays(-1));
			}

			foreach (var tax in data.Yearly)
				_repository.AddTax(data.Municipality, TaxType.Yearly, tax.Tax,
					new DateTime(tax.Year, 1, 1),
					new DateTime(tax.Year, 12, 31));
		}
	}
}