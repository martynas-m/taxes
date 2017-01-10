using System.Collections.Generic;

namespace Taxes.Importer
{
	internal class YamlStructure
	{
		public string Municipality { get; set; }
		public IEnumerable<YamlTax> Daily { get; set; }
		public IEnumerable<YamlTax> Weekly { get; set; }
		public IEnumerable<YamlTax> Monthly { get; set; }
		public IEnumerable<YamlTax> Yearly { get; set; }
	}
}