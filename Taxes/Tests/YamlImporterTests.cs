using System;
using System.IO;
using NSubstitute;
using NUnit.Framework;
using YamlDotNet.Serialization;

namespace Taxes.Tests
{
	[TestFixture]
	internal class YamlImporterTests
	{
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

		[Test]
		public void Can_parse_yaml()
		{
			var repository = Substitute.For<ITaxRepository>();
			var sut = new YamlImporter(repository);
			sut.Import(new StringReader(YamlData));

			repository.Received().AddTax("Vilnius", TaxType.Daily, 0.1f, new DateTime(2017, 3, 11), new DateTime(2017,3,11));
		}
	}

	internal class YamlImporter
	{
		private ITaxRepository _repository;

		public YamlImporter(ITaxRepository repository)
		{
			_repository = repository;
		}

		public void Import(TextReader yaml)
		{
			var deserializer = new Deserializer();
			var data = deserializer.Deserialize(yaml);
		}
	}
}