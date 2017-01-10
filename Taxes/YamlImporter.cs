using System.IO;
using YamlDotNet.Serialization;

namespace Taxes.Tests
{
	public class YamlImporter
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