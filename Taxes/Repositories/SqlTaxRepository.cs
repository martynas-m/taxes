using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace Taxes.Repositories
{
	internal class SqlTaxRepository : ITaxRepository
	{
		private SqlConnection _cnn;
		private readonly SqlTransaction _tran;

		private const string InsertTaxSql = 
			"if(not exists(select 1 from Municipalities where Name = @m)) " +
			"  insert into Municipalities(Name) values(@m); " +
			"insert into Taxes(MunicipalityId, TaxType, Tax, Start, [End]) " +
			"select m.Id, @taxType, @tax, @start, @end " +
			"from Municipalities m where m.Name = @m";

		private const string FindTaxSql = 
			"select Tax from Taxes t " +
			"inner join Municipalities m on t.MunicipalityId = m.Id " +
			"where t.TaxType = @t and m.Name = @m and [Start] = @start and [End] = @end";

		private enum TaxType
		{
			Daily = 1
		};

		public SqlTaxRepository(SqlConnection cnn, SqlTransaction tran)
		{
			_cnn = cnn;
			_tran = tran;
		}

		public float? FindDailyTax(string municipality, DateTime dateFor)
		{
			var result = _cnn.Query<float>(FindTaxSql, 
				new
				{
					m = municipality,
					t = TaxType.Daily,
					start = dateFor,
					end = dateFor
				}, _tran)
			.FirstOrDefault();
			return result;
		}

		public float? FindWeeklyTax(string municipality, DateTime dateFor)
		{
			throw new NotImplementedException();
		}

		public void AddDailyTax(string municipality, DateTime effectiveDate, float tax)
		{
			_cnn.Execute(InsertTaxSql,
				new {m = municipality, taxType = TaxType.Daily, start = effectiveDate, end = effectiveDate, tax},
				_tran);
		}
	}
}