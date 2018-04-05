using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MetaGraffiti.Base.Common
{
	public static class TextMutate
	{
		private static TextInfo _textInfo = new CultureInfo("en-US", false).TextInfo;

		public static string StripAccents(string text)
		{
			return String.Concat(text.Normalize(NormalizationForm.FormD).Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark));
		}

		public static string TrimSafe(string text)
		{
			return (String.IsNullOrWhiteSpace(text) ? "" : text.Trim());
		}

		public static string ToTitleCase(string text)
		{
			return _textInfo.ToTitleCase(text);
		}


		public static string[] ExtractKeywords(string text, char split = ',')
		{
			if (String.IsNullOrWhiteSpace(text)) return new string[0];
			return text.Split(split).Select(p => p.Trim()).ToArray();
		}

		public static string FixKeywords(string text, char split = ',')
		{
			var keyworks = ExtractKeywords(text, split);
			return String.Join(split + " ", keyworks);
		}

		public static string FixUrl(string url)
		{
			if (String.IsNullOrWhiteSpace(url)) return "";
			if (url.StartsWith("http", StringComparison.InvariantCultureIgnoreCase)) return url.Trim();
			return "http://" + url.Trim();
		}
	}
}
