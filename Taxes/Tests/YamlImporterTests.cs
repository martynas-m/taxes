using System;
using System.IO;
using NSubstitute;
using NUnit.Framework;
using Taxes.Importer;

namespace Taxes.Tests
{
	[TestFixture]
	internal class YamlImporterTests
	{
		private ITaxRepository _repository;
		private YamlImporter _sut;

		private const string YamlDataMin =
@"
municipality: Vilnius
";
		private const string YamlData = 
@"
municipality: Vilnius
daily:
  - date: 2017-03-11
    tax: 0.1
  - date: 2017-07-06
    tax: 0.13
weekly:
  - date: 2017-08-11
    tax: 0.3
monthly:
  - month: 9
    tax: 0.4
yearly:
  - year: 2017
    tax: 0.11
";

		[SetUp]
		public void Setup()
		{
			_repository = Substitute.For<ITaxRepository>();
			_sut = new YamlImporter(_repository);
			_sut.Import(new StringReader(YamlData));
		}

		[Test]
		public void Can_import_daily_tax()
		{
			_repository.Received().AddTax("Vilnius", TaxType.Daily, 0.1f, new DateTime(2017, 3, 11), new DateTime(2017,3,11));
			_repository.Received().AddTax("Vilnius", TaxType.Daily, 0.13f, new DateTime(2017, 7, 6), new DateTime(2017,7,6));
		}

		[Test]
		public void While_importing_weekly_tax_sets_end_after_seven_days()
		{
			_repository.Received().AddTax("Vilnius", TaxType.Weekly, 0.3f, new DateTime(2017, 8, 11), new DateTime(2017, 8, 18));
		}

		[Test]
		public void Importing_monthly_tax_converts_month_to_date()
		{
			_repository.Received().AddTax("Vilnius", TaxType.Monthly, 0.4f, new DateTime(2017, 9, 1), new DateTime(2017, 9, 30));
		}

		[Test]
		public void Importing_yearly_tax_converts_year_to_date()
		{
			_repository.Received().AddTax("Vilnius", TaxType.Yearly, 0.11f, new DateTime(2017, 1, 1), new DateTime(2017, 12, 31));
		}

		[Test]
		public void Elements_should_be_not_required()
		{
			_sut.Import(new StringReader(YamlDataMin));
		}
	}
}