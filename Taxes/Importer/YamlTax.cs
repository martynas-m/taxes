using System;

namespace Taxes.Importer
{
	internal class YamlTax
	{
		public DateTime Date { get; set; }
		public int Month { get; set; }
		public int Year { get; set; }
		public float Tax { get; set; }
	}
}