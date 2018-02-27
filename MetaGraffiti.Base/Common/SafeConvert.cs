using System;

namespace MetaGraffiti.Base.Common
{
	public class SafeConvert
	{
		//================================================================================
		#region Integer conversions

		/// <summary>
		/// Converts object to integer – defaults to 0 on error.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static int ToInt(object o, int d = 0)
		{
			if (o != null)
			{
				try { return Convert.ToInt32(o); }
				catch { }
			}
			return d;
		}


		/// <summary>
		/// Converts object to integer – defaults to null on error.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static int? ToIntNull(object o)
		{
			int? value;

			//check for null
			if (o == null)
			{
				value = null;
			}
			else
			{
				try
				{
					value = Convert.ToInt32(o);

				}
				catch //(InvalidCastException) //TODO RICH remove the InvalidCastException from eveywhere in common
				{
					value = null;
				}
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
					int hours = SafeConvert.ToInt(o);
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
			DateTime value;

			//check for null
			if (o == null)
			{
				value = DateTime.MinValue;
			}
			else
			{
				try
				{
					value = Convert.ToDateTime(o);

				}
				catch //(InvalidCastException)
				{
					value = DateTime.MinValue;
				}
			}

			return value;
		}

		/// <summary>
		/// Converts object to DateTime – defaults to specified date on error.
		/// </summary>
		/// <param name="o"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static DateTime ToDateTime(object o, DateTime defaultValue)
		{
			DateTime value;

			//check for null
			if (o == null)
			{
				value = defaultValue;
			}
			else
			{
				try
				{
					value = Convert.ToDateTime(o);

				}
				catch //(InvalidCastException)
				{
					value = defaultValue;
				}
			}

			return value;
		}

		/// <summary>
		/// Converts object to DateTime – defaults to null on error.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static DateTime? ToDateTimeNull(object o)
		{
			DateTime? value;

			//check for null
			if (o == null)
			{
				value = null;
			}
			else
			{
				try
				{
					value = Convert.ToDateTime(o);
				}
				catch //(InvalidCastException)
				{
					value = null;
				}
			}

			return value;
		}

		public static DateTime? ToUTCDateTimeNull(object o)
		{
			DateTime? value;

			//check for null
			if (o == null)
			{
				value = null;
			}
			else
			{
				try
				{
					value = Convert.ToDateTime(o).ToUniversalTime();
				}
				catch //(InvalidCastException)
				{
					value = null;
				}
			}

			return value;
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
			decimal value;

			//check for null
			if (o == null)
			{
				value = 0;
			}
			else
			{
				try
				{
					value = Convert.ToDecimal(o);
				}
				catch //(InvalidCastException)
				{
					value = 0;
				}
			}

			return value;
		}

		/// <summary>
		/// Converts object to decimal – defaults to 0 on error.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static decimal? ToDecimalNull(object o)
		{
			Decimal? value;

			//check for null
			if (o == null)
			{
				value = null;
			}
			else
			{
				try
				{
					value = Convert.ToDecimal(o);

				}
				catch //(InvalidCastException)
				{
					value = null;
				}
			}

			return value;
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
			double value;

			//check for null
			if (o == null)
			{
				value = 0;
			}
			else
			{
				try
				{
					value = Convert.ToDouble(o);
				}
				catch //(InvalidCastException)
				{
					value = 0;
				}
			}

			return value;
		}

		/// <summary>
		/// Converts object to double – defaults to 0 on error.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static double? ToDoubleNull(object o)
		{
			Double? value;

			//check for null
			if (o == null)
			{
				value = null;
			}
			else
			{
				try
				{
					value = Convert.ToDouble(o);

				}
				catch //(InvalidCastException)
				{
					value = null;
				}
			}

			return value;
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
			try { return Convert.ToBoolean(o); }
			catch { return false; }
		}

		/// <summary>
		/// Converts a flag column to a boolean
		/// </summary>
		/// <param name="input">Assumes null on null, false on 0 and true on 1</param>
		/// <returns></returns>
		public static bool? ToBooleanNull(object o)
		{
			try
			{
				if (o == null)
					return null;
				else
					return Convert.ToBoolean(o);
			}
			catch { return null; }
		}

		#endregion
		//================================================================================

		//================================================================================
		#region String conversions

		public static string ToString(object s)
		{
			try { return Convert.ToString(s); }
			catch { return ""; }
		}

		#endregion
		//================================================================================
	}
}
