using System;
using System.Data.SqlClient;
using NUnit.Framework;
using Taxes.Repositories;

namespace Taxes.Tests
{
	[TestFixture]
	internal class SqlDataRepositoryTest
	{
		private ITaxRepository _sut;
		private SqlConnection _cnn;
		private SqlTransaction _tran;
		private const string ConnectionString = "Server=.;Database=Taxes;Integrated Security=true;";

		[SetUp]
		public void Setup()
		{
			_cnn = new SqlConnection(ConnectionString);
			_cnn.Open();
			_tran = _cnn.BeginTransaction();

			_sut = new SqlTaxRepository(_cnn, _tran);
		}

		[TearDown]
		public void TiearDown()
		{
			_tran.Rollback();
			_cnn.Dispose();
		}

		[Test]
		public void Can_add_and_then_find_tax_by_type()
		{
			_sut.AddTax("Vilnius", TaxType.Daily, .1f, DateTime.Today, DateTime.Today);
			Assert.That(_sut.FindTax("Vilnius", TaxType.Daily, DateTime.Today), Is.Not.Null.And.EqualTo(0.1f));
		}

		[Test]
		public void Returns_null_if_tax_not_defined_for_date()
		{
			Assert.That(_sut.FindTax("Vilnius", TaxType.Daily, DateTime.Today), Is.Null);
		}

		[Test]
		public void Can_find_tax_when_only_type_different()
		{
			_sut.AddTax("Vilnius", TaxType.Daily, .1f, DateTime.Today, DateTime.Today);
			_sut.AddTax("Vilnius", TaxType.Weekly, .2f, DateTime.Today, DateTime.Today.AddDays(7));
			
			Assert.That(_sut.FindTax("Vilnius", TaxType.Daily, DateTime.Today), Is.EqualTo(.1f));
			Assert.That(_sut.FindTax("Vilnius", TaxType.Weekly, DateTime.Today), Is.EqualTo(.2f));
		}
	}
}