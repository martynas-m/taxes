using System;
using NSubstitute;
using NUnit.Framework;

namespace Taxes.Tests
{
	[TestFixture]
	class TaxesTests
	{
		private TaxProvider _sut;
		private ITaxRepository _taxRepo;
		private const string Vilnius = "Vilnius";
		private readonly DateTime _dailyTaxDate = new DateTime(2016, 5, 13);
		private readonly DateTime _weeklyTaxDate = new DateTime(2016, 6, 13);
		private readonly DateTime _monthlyTaxDate = new DateTime(2016, 7, 13);
		private readonly DateTime _yearlyTaxDate = new DateTime(2016, 8, 13);
		private const float DailyTax = 0.1f;
		private const float WeeklyTax = 0.15f;
		private const float MonthlyTax = 0.2f;
		private const float YearlyTax = 0.3f;

		[SetUp]
		public void SetUp()
		{
			_taxRepo = Substitute.For<ITaxRepository>();
			_sut = new TaxProvider(_taxRepo);
			Init_tax_repo_with_default_values();
		}
		[Test]
		public void Returns_dayly_tax_for_municipality_by_date_when_daily_tax_defined_for_date()
		{
			Assert.That(_sut.GetForDate(Vilnius, _dailyTaxDate), Is.EqualTo(DailyTax));
		}

		[Test]
		public void Returns_weekly_tax_if_no_daily_tax_defined_for_that_day()
		{
			Assert.That(_sut.GetForDate(Vilnius, _weeklyTaxDate), Is.EqualTo(WeeklyTax));
		}

		[Test]
		public void Returns_monthy_tax_if_no_weekly_tax_exist()
		{
			Assert.That(_sut.GetForDate(Vilnius, _monthlyTaxDate), Is.EqualTo(MonthlyTax));
		}

		[Test]
		public void Returns_yearly_tax_if_no_monthly_tax_exist()
		{
			Assert.That(_sut.GetForDate(Vilnius, _yearlyTaxDate), Is.EqualTo(YearlyTax));
		}

		private void Init_tax_repo_with_default_values()
		{
			_taxRepo.FindTax(Vilnius, TaxType.Daily, _dailyTaxDate).Returns(DailyTax);
			_taxRepo.FindTax(Vilnius, TaxType.Weekly, _weeklyTaxDate).Returns(WeeklyTax);
			_taxRepo.FindTax(Vilnius, TaxType.Monthly, _monthlyTaxDate).Returns(MonthlyTax);
			_taxRepo.FindTax(Vilnius, TaxType.Yearly, _yearlyTaxDate).Returns(YearlyTax);
		}
	}
}