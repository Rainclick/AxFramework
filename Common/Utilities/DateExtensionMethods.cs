using System;
using System.Globalization;
using System.Reflection;

namespace Common.Utilities
{
    public static class DateExtensionMethods
    {
        private static CultureInfo _culture;
        public static CultureInfo GetPersianCulture()
        {
            if (_culture == null)
            {
                _culture = new CultureInfo("fa-IR");
                DateTimeFormatInfo formatInfo = _culture.DateTimeFormat;
                formatInfo.AbbreviatedDayNames = new[] { "ی", "د", "س", "چ", "پ", "ج", "ش" };
                formatInfo.DayNames = new[] { "یکشنبه", "دوشنبه", "سه شنبه", "چهار شنبه", "پنجشنبه", "جمعه", "شنبه" };
                var monthNames = new[]
                {
                    "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن",
                    "اسفند",
                    ""
                };
                formatInfo.AbbreviatedMonthNames =
                    formatInfo.MonthNames =
                    formatInfo.MonthGenitiveNames = formatInfo.AbbreviatedMonthGenitiveNames = monthNames;
                formatInfo.AMDesignator = "ق.ظ";
                formatInfo.PMDesignator = "ب.ظ";
                formatInfo.ShortDatePattern = "yyyy/MM/dd";
                formatInfo.LongDatePattern = "dddd, dd MMMM,yyyy";
                formatInfo.FirstDayOfWeek = DayOfWeek.Saturday;
                Calendar cal = new PersianCalendar();

                FieldInfo fieldInfo = _culture.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo != null)
                    fieldInfo.SetValue(_culture, cal);

                FieldInfo info = formatInfo.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
                if (info != null)
                    info.SetValue(formatInfo, cal);

                _culture.NumberFormat.NumberDecimalSeparator = "/";
                _culture.NumberFormat.DigitSubstitution = DigitShapes.NativeNational;
                _culture.NumberFormat.NumberNegativePattern = 0;
            }
            return _culture;
        }

        public static string ToPerDateString(this DateTime date, string format = null)
        {
            if (string.IsNullOrWhiteSpace(format))
                format = "yyyy/MM/dd";
            return date.ToString(format, GetPersianCulture());
        }

        public static string ToPerDateTimeString(this DateTime date, string format = null)
        {
            if (string.IsNullOrWhiteSpace(format))
                format = "yyyy/MM/dd HH:mm:ss";
            return date.ToString(format, GetPersianCulture());
        }

        public static DateTime? GetMiladiDate(string input)
        {
            try
            {
                var pd = new PersianCalendar();
                var str = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string[] arrTemp = str[0].Split(new[] { '-', '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
                if (arrTemp.Length == 3)
                {
                    return pd.ToDateTime(int.Parse(arrTemp[0]), int.Parse(arrTemp[1]), int.Parse(arrTemp[2]), 0, 0, 0, 0);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public static DateTime UnixTimeStampToDateTime(this long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

    }
}
