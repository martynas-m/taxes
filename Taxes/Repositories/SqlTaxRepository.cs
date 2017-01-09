using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace Taxes.Repositories
{
	internal class SqlTaxRepository : ITaxRepository
	{
		private readonly SqlConnection _cnn;
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
			"where t.TaxType = @taxType and m.Name = @m and @dateAt between [Start] and [End];";

		public SqlTaxRepository(SqlConnection cnn, SqlTransaction tran)
		{
			_cnn = cnn;
			_tran = tran;
		}

		public void AddTax(string municipality, TaxType taxType, float tax, DateTime start, DateTime end)
		{
			_cnn.Execute(InsertTaxSql,
				new {m = municipality, taxType, start, end, tax},
				_tran);
		}

		public float? FindTax(string municipality, TaxType taxType, DateTime dateAt)
		{
			var result = _cnn.Query<float>(FindTaxSql, 
				new
				{
					m = municipality,
					taxType,
					dateAt 
				}, _tran)
			.FirstOrDefault();
			return result;
		}
	}
}