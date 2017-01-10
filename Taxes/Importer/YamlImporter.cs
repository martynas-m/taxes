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
			var builder = new DeserializerBuilder();
			var deserializer = builder.WithNamingConvention(new CamelCaseNamingConvention()).Build();
			var data = deserializer.Deserialize<YamlStructure>(yaml);

			ParseDaily(data);
			ParseWeekly(data);
			ParseMonthly(data);
			ParseYearly(data);
		}

		private void ParseYearly(YamlStructure data)
		{
			if (null == data.Yearly) return;

			foreach (var tax in data.Yearly)
				_repository.AddTax(data.Municipality, TaxType.Yearly, tax.Tax,
					new DateTime(tax.Year, 1, 1),
					new DateTime(tax.Year, 12, 31));
		}

		private void ParseMonthly(YamlStructure data)
		{
			if (null == data.Monthly) return;

			var year = DateTime.Today.Year;
			foreach (var tax in data.Monthly)
			{
				var start = new DateTime(year, tax.Month, 1);
				_repository.AddTax(data.Municipality, TaxType.Monthly, tax.Tax, start,
					start.AddMonths(1).AddDays(-1));
			}
		}

		private void ParseWeekly(YamlStructure data)
		{
			if (null == data.Weekly) return;

			foreach (var tax in data.Weekly)
				_repository.AddTax(data.Municipality, TaxType.Weekly, tax.Tax, tax.Date, tax.Date.AddDays(7));
		}

		private void ParseDaily(YamlStructure data)
		{
			if (null == data.Daily) return;

			foreach (var tax in data.Daily)
				_repository.AddTax(data.Municipality, TaxType.Daily, tax.Tax, tax.Date, tax.Date);
		}
	}
}