using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MetaGraffiti.Base.Common
{
   public static class TextTranslate
    {
		public static string StripAccents(string text)
		{
			return String.Concat(text.Normalize(NormalizationForm.FormD).Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark));
		}
    }
}
