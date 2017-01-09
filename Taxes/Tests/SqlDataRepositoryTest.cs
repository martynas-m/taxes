using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;
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
	}
}