using System;

namespace MetaGraffiti.Base.Common
{
	public class TypeConvert
	{
		//================================================================================
		#region Integer conversions

		/// <summary>
		/// Converts object to integer – defaults to 0 on error.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static int ToInt(object o, int defaultValue = 0)
		{
			if (o != null)
			{
				int r;
				return int.TryParse(o.ToString(), out r) ? r : defaultValue;
			}
			return defaultValue;
		}


		/// <summary>
		/// Converts object to integer – defaults to null on error.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static int? ToIntNull(object o)
		{
			int? value = null;

			//check for null
			if (o != null)
			{
				int r;
				if (int.TryParse(o.ToString(), out r)) return r;
			}

			return value;
		}

		#endregion
		//================================================================================

		//================================================================================
		#region DateTime conversions

		public static TimeSpan ToTimeSpan(object o)
		{
			TimeSpan value;

			try
			{
				string time = o.ToString();
				if (!time.Contains(":"))
				{
					int hours = TypeConvert.ToInt(o);
					if (hours > 23)
						time = String.Format("{0:0}:{1:0}:{2:0}", Math.Floor(hours / 24.0), hours % 24, 0);
					else
						time += ":0";
				}
				value = TimeSpan.Parse(time);
			}
			catch
			{
				value = new TimeSpan(0);
			}

			return value;
		}
		
		/// <summary>
		/// Converts object to DateTime – defaults to MinDate on error.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static DateTime ToDateTime(object o)
		{
			if (o == null)
				return DateTime.MinValue;
			else
			{
				DateTime d;
				return DateTime.TryParse(o.ToString(), out d)
					? d
					: DateTime.MinValue;
			}
		}

		/// <summary>
		/// Converts object to DateTime – defaults to specified date on error.
		/// </summary>
		/// <param name="o"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static DateTime ToDateTime(object o, DateTime defaultValue)
		{
			if (o == null)
				return defaultValue;
			else
			{
				DateTime d;
				return DateTime.TryParse(o.ToString(), out d)
					? d
					: defaultValue;
			}
		}

		/// <summary>
		/// Converts object to DateTime – defaults to null on error.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static DateTime? ToDateTimeNull(object o)
		{
			if (o == null)
				return null;
			else
			{
				DateTime d;
				if (DateTime.TryParse(o.ToString(), out d))
					return d;
				else
					return null;
			}
		}

		public static DateTime? ToUTCDateTimeNull(object o)
		{
			if (o == null)
				return null;
			else
			{
				var d = ToDateTimeNull(o);
				return (d.HasValue ? d.Value.ToUniversalTime() : d);
			}
		}
		#endregion
		//================================================================================

		//================================================================================
		#region Decimal conversions
		/// <summary>
		/// Converts object to decimal – defaults to 0 on error.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static decimal ToDecimal(object o)
		{
			if (o == null)
				return 0;
			else
			{
				decimal d;
				if (decimal.TryParse(o.ToString(), out d))
					return d;
				else
					return 0;
			}
		}

		/// <summary>
		/// Converts object to decimal – defaults to 0 on error.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static decimal? ToDecimalNull(object o)
		{
			if (o == null)
				return null;
			else
			{
				decimal d;
				if (decimal.TryParse(o.ToString(), out d))
					return d;
				else
					return null;
			}
		}
		#endregion
		//================================================================================

		//================================================================================
		#region Double conversions
		/// <summary>
		/// Converts object to double – defaults to 0 on error.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static double ToDouble(object o)
		{
			if (o == null)
				return 0;
			else
			{
				double d;
				if (double.TryParse(o.ToString(), out d))
					return d;
				else
					return 0;
			}
		}

		/// <summary>
		/// Converts object to double – defaults to 0 on error.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static double? ToDoubleNull(object o)
		{
			if (o == null)
				return null;
			else
			{
				double d;
				if (double.TryParse(o.ToString(), out d))
					return d;
				else
					return null;
			}
		}
		#endregion
		//================================================================================

		//================================================================================
		#region Currency conversions
		public static decimal ToMoney(object o)
		{
			string strValue;
			decimal dValue;

			//check for null
			if (o == null)
			{
				dValue = 0;
			}
			else
			{
				strValue = o.ToString();

				//strip legal characters
				strValue = strValue.Replace("$", "");
				strValue = strValue.Replace(",", "");

				try
				{
					dValue = Convert.ToDecimal(strValue);
				}
				catch //(InvalidCastException)
				{
					dValue = 0;
				}
			}

			return dValue;
		}

		public static decimal? ToMoneyNull(object o)
		{
			string strValue;
			decimal? dValue;

			//check for null
			if (o == null)
			{
				dValue = null;
			}
			else
			{
				strValue = o.ToString();

				//strip legal characters
				strValue = strValue.Replace("$", "");
				strValue = strValue.Replace(",", "");

				try
				{
					dValue = Convert.ToDecimal(strValue);
				}
				catch //(InvalidCastException)
				{
					dValue = null;
				}
			}

			return dValue;
		}
		#endregion
		//================================================================================

		//================================================================================
		#region Boolean conversions

		/// <summary>
		/// Returns boolean represenation of object or false on error
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static bool ToBoolean(object o)
		{
			if (o == null)
				return false;
			else
			{
				bool b;
				if (bool.TryParse(o.ToString(), out b))
					return b;
				else
					return false;
			}
		}

		/// <summary>
		/// Converts a flag column to a boolean
		/// </summary>
		/// <param name="input">Assumes null on null, false on 0 and true on 1</param>
		/// <returns></returns>
		public static bool? ToBooleanNull(object o)
		{
			if (o == null)
				return null;
			else
			{
				bool b;
				if (bool.TryParse(o.ToString(), out b))
					return b;
				else
					return null;
			}
		}

		#endregion
		//================================================================================

		//================================================================================
		#region String conversions

		public static string ToString(object s)
		{
			if (s == null)
				return "";
			else
				return s.ToString();
		}

		#endregion
		//================================================================================
	}
}
