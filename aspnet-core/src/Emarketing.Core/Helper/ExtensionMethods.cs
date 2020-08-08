using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Emarketing.Helper
{
    public static class ExtensionMethods
    {
        public static string DefaultIfNull(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return EmarketingConsts.ReplacementString;
            return value;
        }

        public static int? DefaultIfNull(this int? value)
        {
            if (value == null)
                return EmarketingConsts.ReplacementInt;
            return value;
        }


        public static long? DefaultIfNull(this long? value)
        {
            return value ?? EmarketingConsts.ReplacementInt;
        }

        public static string FormatDate(this DateTime? value, string format = EmarketingConsts.DateTimeFormat)
        {
            return value?.ToString(format, CultureInfo.InvariantCulture) ?? "";
        }
        public static string FormatTime(this TimeSpan value, string format = EmarketingConsts.TimeFormat)
        {
            var time = DateTime.Today.Add(value);
            return time.ToString(format, CultureInfo.InvariantCulture);
        }
        public static string FormatTime(this TimeSpan? value, string format = EmarketingConsts.TimeFormat)
        {
            return value.HasValue ? FormatTime(value: value.Value, format: format) : "";
        }


        public static string FormatDate(this DateTime value, string format = EmarketingConsts.DateTimeFormat)
        {
            return value.ToString(format, CultureInfo.InvariantCulture);
        }


        public static DateTime ToLocalTime(this DateTime utcDateTime, string timeZoneById)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneById);
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc), timeZoneInfo);
            return localTime;
        }

        public static DateTime? ToLocalTime(this DateTime? utcDateTime, string timeZoneById)
        {
            if (utcDateTime.HasValue)
                return ToLocalTime(utcDateTime.Value, timeZoneById);
            else
                return null;
        }

        public static TimeSpan? ToUtcTime(this TimeSpan? localTimeSpan, string timeZoneById)
        {
            if (localTimeSpan.HasValue)
                return ToUtcTime(localTimeSpan.Value, timeZoneById);
            else
                return null;
        }

        public static DateTime ToTimeSpan(this string timeSpan)
        {
            return DateTime.ParseExact(timeSpan, "hh:mm tt", CultureInfo.InvariantCulture);
        }
        public static DateTime ToTimeSpan(this string timeSpan, string format)
        {
            if (format.IsNullOrEmptyOrWhiteSpace())
                format = "hh:mm:ss";

            return DateTime.ParseExact(timeSpan, format, CultureInfo.InvariantCulture);
        }

        public static TimeSpan ToUtcTime(this TimeSpan localTimeSpan, string timeZoneById)
        {
            var currentDate = new DateTime(2016, 1, 1).Add(localTimeSpan);
            var localTime = currentDate.ToUtcTime(timeZoneById);
            return localTime.TimeOfDay;
        }


        public static TimeSpan? ToLocalTime(this TimeSpan? utcTimeSpan, string timeZoneById)
        {
            if (utcTimeSpan.HasValue)
                return ToLocalTime(utcTimeSpan.Value, timeZoneById);
            else
                return null;
        }

        public static TimeSpan ToLocalTime(this TimeSpan utcTimeSpan, string timeZoneById)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneById);
            var currentDate = new DateTime(2016, 1, 1).Add(utcTimeSpan);
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(currentDate, timeZoneInfo);
            return localTime.TimeOfDay;
        }

        public static DateTime? ToUtcTime(this DateTime? localDateTime, string timeZoneById)
        {
            if (localDateTime.HasValue)
                return ToUtcTime(localDateTime.Value, timeZoneById);
            else
                return null;
        }
        public static DateTime ToUtcTime(this DateTime utcDateTime, string timeZoneById)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneById);
            var localTime = TimeZoneInfo.ConvertTimeToUtc(utcDateTime, timeZoneInfo);
            return localTime;
        }

        public static DateTime ToUtcTime(this DateTime utcDateTime, TimeSpan offsetTimeSpan)
        {
            return new DateTimeOffset(utcDateTime).ToOffset(offsetTimeSpan).DateTime;
        }



        public static string FixSqlSingleQuote(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            return text.Replace("'", "''");
        }


        public static string ConvertToFriendlyMessaage(this string efExceptionMessage)
        {
            try
            {
                #region Sample
                //var msg = "The INSERT statement conflicted with the FOREIGN KEY constraint" +
                //            " \"FK_dbo.GeneralContactGroups_dbo.ListDataItems_GroupId\"." +
                //            " The conflict occurred in database \"SmartDesk_V4\", table \"dbo.ListDataItems\", column 'Id'.\r\n" +
                //            "The statement has been terminated."; 
                #endregion

                if (string.IsNullOrEmpty(efExceptionMessage))
                    return string.Empty;

                if (efExceptionMessage.Contains("The INSERT statement conflicted with the FOREIGN KEY ") == false)
                    return efExceptionMessage;

                var startIndex = efExceptionMessage.IndexOf("\"", StringComparison.Ordinal);
                var endIndex = efExceptionMessage.IndexOf("\"", startIndex + 1, StringComparison.Ordinal);

                efExceptionMessage = "Invalid Paramater : " + "'" + efExceptionMessage.Substring(startIndex + 1, endIndex - startIndex - 1) + "'";
                efExceptionMessage = efExceptionMessage.Replace("FK_dbo.", string.Empty);

                return efExceptionMessage;
            }
            catch (Exception)
            {
                return efExceptionMessage;
            }
        }


        public static byte[] ToBytesArray(this System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        public static void AddProperty(this ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }

        public static decimal RoundOff(this decimal d, int digit = 2)
        {
            return Math.Round(d, 2);
        }

        public static double RoundOff(this double d, int digit = 2)
        {
            return Math.Round(d, 2);
        }

         
        public static decimal CalculatePartnerPercentage(this decimal amount)
        {
            return amount * EmarketingConsts.PartnersPercentage / 100;
        }

        public static decimal CalculatePercentage(this int value, decimal totalValue)
        {
            var percentage = Math.Round(value / totalValue * 100, 2);
            return percentage;
        }

        public static byte[] ToByteArray<T>(this T obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static T FromByteArray<T>(this byte[] data)
        {
            if (data == null)
                return default(T);
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(data))
            {
                object obj = bf.Deserialize(ms);
                return (T)obj;
            }
        }
    }
     
}
