using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Emarketing.Helper
{
    public static class StringExtensions
    {
        public static bool Contains(this string target, string value, StringComparison comparison)
        {
            return target.IndexOf(value, comparison) >= 0;
        }

        public static string Format(this string s, params object[] args)
        {
            return string.Format(s, args);
        }

        /// <summary>
        /// Adds a char to end of given string if it does not ends with the char.
        /// </summary>
        public static string EnsureEndsWith(this string str, char c)
        {
            return EnsureEndsWith(str, c, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Adds a char to end of given string if it does not ends with the char.
        /// </summary>
        public static string EnsureEndsWith(this string str, char c, StringComparison comparisonType)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.EndsWith(c.ToString(CultureInfo.InvariantCulture), comparisonType))
            {
                return str;
            }

            return str + c;
        }

        /// <summary>
        /// Adds a char to end of given string if it does not ends with the char.
        /// </summary>
        public static string EnsureEndsWith(this string str, char c, bool ignoreCase, CultureInfo culture)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.EndsWith(c.ToString(culture), ignoreCase, culture))
            {
                return str;
            }

            return str + c;
        }

        /// <summary>
        /// Adds a char to beginning of given string if it does not starts with the char.
        /// </summary>
        public static string EnsureStartsWith(this string str, char c)
        {
            return EnsureStartsWith(str, c, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Adds a char to beginning of given string if it does not starts with the char.
        /// </summary>
        public static string EnsureStartsWith(this string str, char c, StringComparison comparisonType)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.StartsWith(c.ToString(CultureInfo.InvariantCulture), comparisonType))
            {
                return str;
            }

            return c + str;
        }

        /// <summary>
        /// Adds a char to beginning of given string if it does not starts with the char.
        /// </summary>
        public static string EnsureStartsWith(this string str, char c, bool ignoreCase, CultureInfo culture)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.StartsWith(c.ToString(culture), ignoreCase, culture))
            {
                return str;
            }

            return c + str;
        }

        /// <summary>
        /// Indicates whether this string is null or an System.String.Empty string.
        /// </summary>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// indicates whether this string is null, empty, or consists only of white-space characters.
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="len"/> is bigger that string's length</exception>
        public static string Left(this string str, int len)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(0, len);
        }

        /// <summary>
        /// Converts line endings in the string to <see cref="Environment.NewLine"/>.
        /// </summary>
        public static string NormalizeLineEndings(this string str)
        {
            return str.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);
        }

        /// <summary>
        /// Gets index of nth occurence of a char in a string.
        /// </summary>
        /// <param name="str">source string to be searched</param>
        /// <param name="c">Char to search in <see cref="str"/></param>
        /// <param name="n">Count of the occurence</param>
        public static int NthIndexOf(this string str, char c, int n)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            var count = 0;
            for (var i = 0; i < str.Length; i++)
            {
                if (str[i] != c)
                {
                    continue;
                }

                if ((++count) == n)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Gets a substring of a string from end of the string.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="len"/> is bigger that string's length</exception>
        public static string Right(this string str, int len)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(str.Length - len, len);
        }

        /// <summary>
        /// Uses string.Split method to split given string by given separator.
        /// </summary>
        public static string[] Split(this string str, string separator)
        {
            return str.Split(new[] { separator }, StringSplitOptions.None);
        }

        /// <summary>
        /// Uses string.Split method to split given string by given separator.
        /// </summary>
        public static string[] Split(this string str, string separator, StringSplitOptions options)
        {
            return str.Split(new[] { separator }, options);
        }

        /// <summary>
        /// Uses string.Split method to split given string by <see cref="Environment.NewLine"/>.
        /// </summary>
        public static string[] SplitToLines(this string str)
        {
            return str.Split(Environment.NewLine);
        }

        /// <summary>
        /// Uses string.Split method to split given string by <see cref="Environment.NewLine"/>.
        /// </summary>
        public static string[] SplitToLines(this string str, StringSplitOptions options)
        {
            return str.Split(Environment.NewLine, options);
        }

        /// <summary>
        /// Converts PascalCase string to camelCase string.
        /// </summary>
        /// <param name="str">String to convert</param>
        /// <returns>camelCase of the string</returns>
        public static string ToCamelCase(this string str)
        {
            return str.ToCamelCase(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts PascalCase string to camelCase string in specified culture.
        /// </summary>
        /// <param name="str">String to convert</param>
        /// <param name="culture">An object that supplies culture-specific casing rules</param>
        /// <returns>camelCase of the string</returns>
        public static string ToCamelCase(this string str, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (str.Length == 1)
            {
                return str.ToLower(culture);
            }

            return char.ToLower(str[0], culture) + str.Substring(1);
        }

        /// <summary>
        /// Converts string to enum value.
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">String value to convert</param>
        /// <returns>Returns enum object</returns>
        public static T ToEnum<T>(this string value)
            where T : struct
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// Converts string to enum value.
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">String value to convert</param>
        /// <param name="ignoreCase">Ignore case</param>
        /// <returns>Returns enum object</returns>
        public static T ToEnum<T>(this string value, bool ignoreCase)
            where T : struct
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

        /// <summary>
        /// Converts camelCase string to PascalCase string.
        /// </summary>
        /// <param name="str">String to convert</param>
        /// <returns>PascalCase of the string</returns>
        public static string ToPascalCase(this string str)
        {
            return str.ToPascalCase(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts camelCase string to PascalCase string in specified culture.
        /// </summary>
        /// <param name="str">String to convert</param>
        /// <param name="culture">An object that supplies culture-specific casing rules</param>
        /// <returns>PascalCase of the string</returns>
        public static string ToPascalCase(this string str, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (str.Length == 1)
            {
                return str.ToUpper(culture);
            }

            return char.ToUpper(str[0], culture) + str.Substring(1);
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string if it exceeds maximum length.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        public static string Truncate(this string str, int maxLength)
        {
            if (str == null)
            {
                return null;
            }

            if (str.Length <= maxLength)
            {
                return str;
            }

            return str.Left(maxLength);
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string if it exceeds maximum length.
        /// It adds a "..." postfix to end of the string if it's truncated.
        /// Returning string can not be longer than maxLength.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        public static string TruncateWithPostfix(this string str, int maxLength)
        {
            return TruncateWithPostfix(str, maxLength, "...");
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string if it exceeds maximum length.
        /// It adds given <paramref name="postfix"/> to end of the string if it's truncated.
        /// Returning string can not be longer than maxLength.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        public static string TruncateWithPostfix(this string str, int maxLength, string postfix)
        {
            if (str == null)
            {
                return null;
            }

            if (str == string.Empty || maxLength == 0)
            {
                return string.Empty;
            }

            if (str.Length <= maxLength)
            {
                return str;
            }

            if (maxLength <= postfix.Length)
            {
                return postfix.Left(maxLength);
            }

            return str.Left(maxLength - postfix.Length) + postfix;
        }

        public static string SellutionUrlEncode(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            text = Uri.EscapeDataString(text);
            text = HttpUtility.UrlEncode(text);

            return text;
        }

        public static string SellutionEscapeDataString(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            text = Uri.EscapeDataString(text);

            return text;
        }

        public static string AppendId(this string text, int id)
        {
            return text.Contains("~") ? text : $"{text}~{id}";
        }

        public static string RemoveId(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            var startIndex = text.IndexOf("~");

            if (startIndex == -1)
                return text;

            return text.Substring(0, startIndex);
        }

        public static DateTime GetStartOfDay(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
        }

        public static DateTime GetStartOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0);
        }

        public static DateTime GetLastDateOfMonth(this DateTime dateTime)
        {
            var lastDate = dateTime.AddMonths(1).AddDays(-1);
            return new DateTime(lastDate.Year, lastDate.Month, lastDate.Day, 24, 59, 59);
        }


        public static DateTime? GetEndOfDayNullable(this DateTime? dateTime)
        {
            if (dateTime == null)
                return null;

            return GetEndOfDay(dateTime.Value);
        }

        public static DateTime GetEndOfDay(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
        }


        /// <summary>
        /// Gets the subdomain portion of a url, given a known "root" domain
        /// http://stackoverflow.com/questions/19734769/get-specific-subdomain-from-url-in-foo-bar-car-com
        /// </summary>
        public static string GetSubdomain(this string url, string domain = null)
        {
            var subdomain = url;
            if (subdomain != null)
            {
                if (domain == null)
                {
                    // Since we were not provided with a known domain, assume that second-to-last period divides the subdomain from the domain.
                    var nodes = url.Split('.');
                    var lastNodeIndex = nodes.Length - 1;
                    if (lastNodeIndex > 0)
                        domain = nodes[lastNodeIndex - 1] + "." + nodes[lastNodeIndex];
                }

                // Verify that what we think is the domain is truly the ending of the hostname... otherwise we're hooped.
                if (!subdomain.EndsWith(domain))
                    throw new ArgumentException("Site was not loaded from the expected domain");

                // Quash the domain portion, which should leave us with the subdomain and a trailing dot IF there is a subdomain.
                subdomain = subdomain.Replace(domain, "");
                // Check if we have anything left.  If we don't, there was no subdomain, the request was directly to the root domain:
                if (string.IsNullOrWhiteSpace(subdomain))
                    return null;

                // Quash any trailing periods
                subdomain = subdomain.TrimEnd(new[] { '.' });
            }

            return subdomain;
        }


        public static bool IsNullOrEmptyOrWhiteSpace(this string str)
        {
            return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
        }


        /// <summary>
        /// https://developer.citrixonline.com/time-zones
        /// </summary>
        /// <param name="systemTimeZoneById">Pakistan Standard Time</param>
        /// <returns></returns>
        public static string ToCitrixOnlineTimeZone(this string systemTimeZoneById)
        {
            return ToCitrixOnlineTimeZone(TimeZoneInfo.FindSystemTimeZoneById(id: systemTimeZoneById));
        }

        /// <summary>
        /// https://developer.citrixonline.com/time-zones
        /// </summary>
        /// <param name="timeZoneInfo"></param>
        /// <returns></returns>
        public static string ToCitrixOnlineTimeZone(this TimeZoneInfo timeZoneInfo)
        {
            #region CitrixOnlineTimeZones

            var citrixOnlineTimeZones = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("GMT+13:00", "Pacific/Tongatapu"),
                new KeyValuePair<string, string>("GMT+12:00", "Pacific/Fiji"),
                new KeyValuePair<string, string>("GMT+12:00", "Pacific/Auckland"),
                new KeyValuePair<string, string>("GMT+11:00", "Asia/Magadan"),
                new KeyValuePair<string, string>("GMT+10:00", "Asia/Vladivostok"),
                new KeyValuePair<string, string>("GMT+10:00", "Australia/Hobart"),
                new KeyValuePair<string, string>("GMT+10:00", "Pacific/Guam"),
                new KeyValuePair<string, string>("GMT+10:00", "Australia/Sydney"),
                new KeyValuePair<string, string>("GMT+10:00", "Australia/Brisbane"),
                new KeyValuePair<string, string>("GMT+09:30", "Australia/Darwin"),
                new KeyValuePair<string, string>("GMT+09:30", "Australia/Adelaide"),
                new KeyValuePair<string, string>("GMT+09:00", "Asia/Yakutsk"),
                new KeyValuePair<string, string>("GMT+09:00", "Asia/Seoul"),
                new KeyValuePair<string, string>("GMT+09:00", "Asia/Tokyo"),
                new KeyValuePair<string, string>("GMT+08:00", "Asia/Taipei"),
                new KeyValuePair<string, string>("GMT+08:00", "Australia/Perth"),
                new KeyValuePair<string, string>("GMT+08:00", "Asia/Singapore"),
                new KeyValuePair<string, string>("GMT+08:00", "Asia/Irkutsk"),
                new KeyValuePair<string, string>("GMT+08:00", "Asia/Shanghai"),
                new KeyValuePair<string, string>("GMT+07:00", "Asia/Krasnoyarsk"),
                new KeyValuePair<string, string>("GMT+07:00", "Asia/Bangkok"),
                new KeyValuePair<string, string>("GMT+07:00", "Asia/Jakarta"),
                new KeyValuePair<string, string>("GMT+06:30", "Asia/Rangoon"),
                new KeyValuePair<string, string>("GMT+06:00", "Asia/Colombo"),
                new KeyValuePair<string, string>("GMT+06:00", "Asia/Dhaka"),
                new KeyValuePair<string, string>("GMT+06:00", "Asia/Novosibirsk"),
                new KeyValuePair<string, string>("GMT+05:45", "Asia/Katmandu"),
                new KeyValuePair<string, string>("GMT+05:30", "Asia/Calcutta"),
                new KeyValuePair<string, string>("GMT+05:00", "Asia/Karachi"),
                new KeyValuePair<string, string>("GMT+05:00", "Asia/Yekaterinburg"),
                new KeyValuePair<string, string>("GMT+04:30", "Asia/Kabul"),
                new KeyValuePair<string, string>("GMT+04:00", "Asia/Tbilisi"),
                new KeyValuePair<string, string>("GMT+04:00", "Asia/Muscat"),
                new KeyValuePair<string, string>("GMT+03:30", "Asia/Tehran"),
                new KeyValuePair<string, string>("GMT+03:00", "Africa/Nairobi"),
                new KeyValuePair<string, string>("GMT+03:00", "Europe/Moscow"),
                new KeyValuePair<string, string>("GMT+03:00", "Asia/Kuwait"),
                new KeyValuePair<string, string>("GMT+03:00", "Asia/Baghdad"),
                new KeyValuePair<string, string>("GMT+02:00", "Asia/Jerusalem"),
                new KeyValuePair<string, string>("GMT+02:00", "Europe/Helsinki"),
                new KeyValuePair<string, string>("GMT+02:00", "Africa/Harare"),
                new KeyValuePair<string, string>("GMT+02:00", "Africa/Cairo"),
                new KeyValuePair<string, string>("GMT+02:00", "Europe/Bucharest"),
                new KeyValuePair<string, string>("GMT+02:00", "Europe/Athens"),
                new KeyValuePair<string, string>("GMT+01:00", "Africa/Malabo"),
                new KeyValuePair<string, string>("GMT+01:00", "Europe/Warsaw"),
                new KeyValuePair<string, string>("GMT+01:00", "Europe/Brussels"),
                new KeyValuePair<string, string>("GMT+01:00", "Europe/Prague"),
                new KeyValuePair<string, string>("GMT+01:00", "Europe/Amsterdam"),
                new KeyValuePair<string, string>("GMT", "GMT"),
                new KeyValuePair<string, string>("GMT", "Europe/London"),
                new KeyValuePair<string, string>("GMT", "Africa/Casablanca"),
                new KeyValuePair<string, string>("GMT-01:00", "Atlantic/Cape_Verde"),
                new KeyValuePair<string, string>("GMT-01:00", "Atlantic/Cape_Verde"),
                new KeyValuePair<string, string>("GMT-01:00", "Atlantic/Azores"),
                new KeyValuePair<string, string>("GMT-03:00", "America/Buenos_Aires"),
                new KeyValuePair<string, string>("GMT-03:00", "America/Sao_Paulo"),
                new KeyValuePair<string, string>("GMT-03:30", "America/St_Johns"),
                new KeyValuePair<string, string>("GMT-04:00", "America/Santiago"),
                new KeyValuePair<string, string>("GMT-04:00", "America/Caracas"),
                new KeyValuePair<string, string>("GMT-04:00", "America/Halifax"),
                new KeyValuePair<string, string>("GMT-05:00", "America/Indianapolis"),
                new KeyValuePair<string, string>("GMT-05:00", "America/New_York"),
                new KeyValuePair<string, string>("GMT-05:00", "America/Bogota"),
                new KeyValuePair<string, string>("GMT-06:00", "America/Mexico_City"),
                new KeyValuePair<string, string>("GMT-06:00", "America/Chicago"),
                new KeyValuePair<string, string>("GMT-07:00", "America/Denver"),
                new KeyValuePair<string, string>("GMT-07:00", "America/Phoenix"),
                new KeyValuePair<string, string>("GMT-08:00", "America/Los_Angeles"),
                new KeyValuePair<string, string>("GMT-09:00", "America/Anchorage"),
                new KeyValuePair<string, string>("GMT-10:00", "Pacific/Honolulu"),
                new KeyValuePair<string, string>("GMT-11:00", "MIT"),
            };

            #endregion

            var timeZoneInfoString = timeZoneInfo.BaseUtcOffset.ToString();
            timeZoneInfoString = timeZoneInfoString.Substring(0, timeZoneInfoString.Length - 3);
            timeZoneInfoString = "GMT+" + timeZoneInfoString;
            timeZoneInfoString = timeZoneInfoString.Replace("+00:00", string.Empty);
            timeZoneInfoString = timeZoneInfoString.Replace("+-", "-");
            var timeZoneInfoCities = timeZoneInfo.DisplayName.Split(' ').ToList();
            timeZoneInfoCities = timeZoneInfoCities.Select(x => x.Replace(",", string.Empty).Trim()).ToList();

            var finalCitrixOnlineTimeZones = string.Empty;
            var selectedcitrixOnlineTimeZones = citrixOnlineTimeZones.Where(x => x.Key == timeZoneInfoString).ToList();
            foreach (var selectedcitrixOnlineTimeZone in selectedcitrixOnlineTimeZones)
            {
                var citrixOnlineTimeZoneCities = selectedcitrixOnlineTimeZone.Value.Split('/').ToList();
                citrixOnlineTimeZoneCities = citrixOnlineTimeZoneCities.Select(x => x.Replace(",", string.Empty).Trim())
                    .ToList();

                var isAny = citrixOnlineTimeZoneCities.Any(x => timeZoneInfoCities.Contains(x));
                if (isAny)
                {
                    finalCitrixOnlineTimeZones = selectedcitrixOnlineTimeZone.Value;
                    break;
                }
            }

            if (string.IsNullOrEmpty(finalCitrixOnlineTimeZones) && selectedcitrixOnlineTimeZones.Count > 0)
                return selectedcitrixOnlineTimeZones.FirstOrDefault().Value;

            return finalCitrixOnlineTimeZones;
        }


        public static string GetFormattedCardNumber(this string cardNumber)
        {
            var cardNumberSubstring = cardNumber.Substring(0, cardNumber.Length - 4);
            var pattern = string.Empty;
            for (int i = 0; i < cardNumberSubstring.Length; i++)
            {
                pattern += '*';
            }

            var formattedCardNumber = cardNumber.Replace(cardNumberSubstring, pattern);
            return formattedCardNumber;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="appInputForm">(1)(111) 111-1111</param>
        /// <returns></returns>
        public static string ToTwilioNumberFormat(this string appInputForm)
        {
            if (string.IsNullOrEmpty(appInputForm))
                return string.Empty;


            appInputForm = string.Join("", appInputForm.ToCharArray().Where(char.IsDigit));
            appInputForm = "+" + appInputForm;
            return appInputForm;
        }

        //public static IdentityResult IsValidPassword(this string password,
        //    SystemListDataItemEnumsCore.PasswordComplexityType passwordComplexityType, int minLength)
        //{
        //    var passwordValidator = new PasswordValidator
        //    {
        //        RequiredLength = minLength,
        //        RequireNonLetterOrDigit = passwordComplexityType ==
        //                                  SystemListDataItemEnumsCore.PasswordComplexityType.MixAlphaNumeric,
        //        RequireDigit = true,
        //        RequireLowercase = true,
        //        RequireUppercase = true,
        //    };
        //    return passwordValidator.ValidateAsync(password).Result;
        //}

        public static string FormatHttp(this string httpInput)
        {
            return httpInput.Replace("http", "https");
        }

        public static string FilterStringForOnlyIntegerValue(this string s)
        {
            return new string(s.Where(char.IsDigit).ToArray());
        }

        public static string AppendUrlProtocol(this string url)
        {
            if (!url.StartsWith("https://") && !url.StartsWith("http://"))
                return $"https://{url}";
            else if (url.StartsWith("https://"))
                return url;
            else if (url.StartsWith("http://"))
                return url;
            return url;
        }

        public static string RemoveSpecialCharacters(this string s)
        {
            s = Regex.Replace(s, "[^0-9a-zA-Z]+ ", "");
            return s;
            //return new string(s.Where(char.IsLetterOrDigit).ToArray());
        }


        
        public static string FormatTimeFromSeconds(this long seconds)
        {
            //if (!seconds.HasValue)
            //    return string.Empty;

            var time = TimeSpan.FromSeconds(seconds);
            return time.ToString(@"hh\:mm\:ss");
        }

        public static string FormatTimeFromHour(this long hours)
        {
            //if (!seconds.HasValue)
            //    return string.Empty;

            var time = TimeSpan.FromHours(hours);
            return time.ToString(@"dd\:hh\:mm\:ss");
        }

        public static string ReplaceBodyTags(this string s, string firstName, string lastName, string fullName,
            string companyName,
            string email, string userName, string userFirstName, string userLastName, string userEmail, string address)
        {
            s = s.Contains("%FirstName%") ? s.Replace("%FirstName%", firstName) : s;
            s = s.Contains("%LastName%") ? s.Replace("%LastName%", lastName) : s;
            s = s.Contains("%FullName%") ? s.Replace("%FullName%", fullName) : s;
            s = s.Contains("%Company%") ? s.Replace("%Company%", companyName) : s;
            s = s.Contains("%Email%") ? s.Replace("%Email%", email) : s;
            s = s.Contains("%UserName%") ? s.Replace("%UserName%", userName) : s;
            s = s.Contains("%UserFirstName%") ? s.Replace("%UserFirstName%", userFirstName) : s;
            s = s.Contains("%UserLastName%") ? s.Replace("%UserLastName%", userLastName) : s;
            s = s.Contains("%UserEmail%") ? s.Replace("%UserEmail%", userEmail) : s;
            s = s.Contains("%Address%") ? s.Replace("%Address%", address) : s;


            return s;
        }

        
         

        public static MemoryStream ToStream(this string text)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(text);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
 
        public static string ToTimeFormate(this int seconds)
        {
            TimeSpan t = TimeSpan.FromSeconds(seconds);
            return string.Format("{0:00}:{1:00}:{2:00}", ((int)t.TotalHours), t.Minutes, t.Seconds);
        }

        public static string AddOrUpdateScriptTag(this string content)
        {
            content = HttpUtility.HtmlDecode(content);
            if (content.IndexOf("<script>", StringComparison.CurrentCultureIgnoreCase) >= 0
                && content.IndexOf("</script>", StringComparison.CurrentCultureIgnoreCase) < 0
            ) // Not include </script> 
            {
                content = $"{content}</script>";
            }
            else if (content.IndexOf("<script>", StringComparison.CurrentCultureIgnoreCase) < 0
                     && content.IndexOf("</script>", StringComparison.CurrentCultureIgnoreCase) >= 0
            ) // Not include <script>
            {
                content = $"<script>{content}";
            }
            else if (content.IndexOf("<script>", StringComparison.CurrentCultureIgnoreCase) < 0
                     && content.IndexOf("</script>", StringComparison.CurrentCultureIgnoreCase) < 0
            ) // Not include <script> and </script> 
            {
                content = $"<script>{content}</script>";
            }

            return content;
        }

        public static string AddOrUpdateStyleTag(this string content)
        {
            content = HttpUtility.HtmlDecode(content);
            if (content.IndexOf("<style>", StringComparison.CurrentCultureIgnoreCase) >= 0
                && content.IndexOf("</style>", StringComparison.CurrentCultureIgnoreCase) < 0) // Not include </script> 
            {
                content = $"{content}</style>";
            }
            else if (content.IndexOf("<style>", StringComparison.CurrentCultureIgnoreCase) < 0
                     && content.IndexOf("</style>", StringComparison.CurrentCultureIgnoreCase) >= 0
            ) // Not include <script>
            {
                content = $"<style>{content}";
            }
            else if (content.IndexOf("<style>", StringComparison.CurrentCultureIgnoreCase) < 0
                     && content.IndexOf("</style>", StringComparison.CurrentCultureIgnoreCase) < 0
            ) // Not include <script> and </script> 
            {
                content = $"<style>{content}</style>";
            }

            return content;
        }

        public static string GetFileExtension(this string content)
        {
            var extension = string.Empty;
            var data = content.Split('.');
            extension = data[data.Length - 1];
            return extension;
        }

        public static string Base64Decode(this string encodedData) //Decode    
        {
            try
            {
                //var encoder = new System.Text.UTF8Encoding();
                //System.Text.Decoder utf8Decode = encoder.GetDecoder();
                //byte[] todecodeByte = Convert.FromBase64String(encodedData);
                //int charCount = utf8Decode.GetCharCount(todecodeByte, 0, todecodeByte.Length);
                //char[] decodedChar = new char[charCount];
                //utf8Decode.GetChars(todecodeByte, 0, todecodeByte.Length, decodedChar, 0);
                //string result = new String(decodedChar);
                //return result;
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(encodedData);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Decode" + ex.Message);
            }
        }

        public static string ToPasswordFormat(this string content) //Decode    
        {
            if (content.IsNullOrEmptyOrWhiteSpace()) return string.Empty;

            var pattern = "*";
            var formattedContent = string.Empty;
            var charList = content.ToCharArray();
            foreach (var i in charList)
            {
                formattedContent += pattern;
            }

            return formattedContent;
        }

        public static string fwHash(this string mHashString)
        {
            string mHashedString;
            SHA256Managed mHashProvider;
            byte[] mHashBytes;
            mHashBytes = System.Text.Encoding.Unicode.GetBytes(mHashString);
            mHashProvider = new SHA256Managed();
            mHashProvider.Initialize();
            mHashBytes = mHashProvider.ComputeHash(mHashBytes);
            mHashedString = Convert.ToBase64String(mHashBytes);
            return mHashedString;
        }
    }

    public static class RandomNumberGenerator
    {
        // Generate a random number between two numbers    
        public static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        // Generate a random string with a given size and case.   
        // If second parameter is true, the return string is lowercase  
        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        // Generate a random password of a given length (optional)  
        public static string RandomPassword(int size = 8)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(2, true));
            builder.Append(RandomNumber(3, 9999));
            builder.Append(RandomString(3, false));
            return builder.ToString();
        }
    }
}
