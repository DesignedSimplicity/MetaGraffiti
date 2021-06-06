using MetaGraffiti.Base.Modules.Geo.Info;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaGraffiti.Web.Core.Models
{
	public class SimpleFilterModel
	{
		public SimpleFilterModel(string field, string name, IEnumerable<string> options, string selected = null, string cssClass = "geo-color")
		{
			Name = name;
			Field = field;
			Options = options;
			SelectedOption = selected;
			CssClass = cssClass;
		}

		public IEnumerable<string> Options { get; set; }
		public string SelectedOption { get; set; }
		public string CssClass { get; set; }
		public string Field { get; set; }
		public string Name { get; set; }


		public bool IsSelected(string option)
		{
			if (String.IsNullOrWhiteSpace(option) || String.IsNullOrWhiteSpace(SelectedOption)) return false;
			return (String.Compare(option, SelectedOption, true) == 0);
		}

		public string GetOptionID(string option)
		{
			return Field + "_" + option.Replace(" ", "_");
		}
	}
}