using System;
using NUnit.Framework;

namespace Taxes.Tests
{
	[TestFixture]
	class TaxesTests
	{
		private TaxProvider _sut;

		[SetUp]
		public void SetUp()
		{
			_sut = new TaxProvider();
		}
		[Test]
		public void Returns_daily_tax_if_day_matche()
		{
			Assert.That(_sut.GetForDate(new DateTime(2016, 5, 13)), Is.EqualTo(0.3f));
		}
	}

	public class TaxProvider
	{
		public float GetForDate(DateTime date)
		{
			return 0.3f;
		}
	}
}