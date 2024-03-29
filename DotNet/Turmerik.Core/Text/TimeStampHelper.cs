﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turmerik.Collections;
using Turmerik.I18n;

namespace Turmerik.Text
{
    public enum TimeStamp
    {
        None = 0,
        Minutes = 1,
        Seconds = 2,
        Millis = 3,
        Ticks = 4
    }

    public class TmStmpMtbl
    {
        public bool HasDate { get; set; }
        public TimeStamp TimeStamp { get; set; }
        public bool HasTimeZone { get; set; }
        public bool IsForFileName { get; set; }
        public bool StartsWithDayOfWeek { get; set; }
    }

    public readonly struct TmStmp
    {
        public readonly bool HasDate;
        public readonly TimeStamp TimeStamp;
        public readonly bool HasTimeZone;
        public readonly bool IsForFileName;
        public readonly bool StartsWithDayOfWeek;

        public TmStmp(TmStmpMtbl src) : this()
        {
            HasDate = src.HasDate;
            TimeStamp = src.TimeStamp;
            HasTimeZone = src.HasTimeZone;
            IsForFileName = src.IsForFileName;
            StartsWithDayOfWeek = src.StartsWithDayOfWeek;
        }

        public TmStmp(
            bool hasDate,
            TimeStamp timeStamp,
            bool hasTimeZone,
            bool isForFileName,
            bool startsWithDayOfWeek)
        {
            HasDate = hasDate;
            TimeStamp = timeStamp;
            HasTimeZone = hasTimeZone;
            IsForFileName = isForFileName;
            StartsWithDayOfWeek = startsWithDayOfWeek;
        }
    }

    public class TmStmpEqualityComparer : IEqualityComparer<TmStmp>
    {
        public bool Equals(TmStmp x, TmStmp y)
        {
            bool retVal = x.HasDate == y.HasDate;
            retVal = retVal && x.TimeStamp == y.TimeStamp;

            retVal = retVal && x.HasTimeZone == y.HasTimeZone;
            retVal = retVal && x.IsForFileName == y.IsForFileName;

            retVal = retVal && x.StartsWithDayOfWeek == y.StartsWithDayOfWeek;
            return retVal;
        }

        public int GetHashCode(TmStmp obj)
        {
            int pow = (int)obj.TimeStamp;
            int hashCode = (int)Math.Pow(2, pow);

            if (obj.HasDate)
            {
                hashCode += (int)Math.Pow(2, 5);
            }

            if (obj.HasTimeZone)
            {
                hashCode += (int)Math.Pow(2, 6);
            }

            if (obj.IsForFileName)
            {
                hashCode += (int)Math.Pow(2, 7);
            }

            if (obj.StartsWithDayOfWeek)
            {
                hashCode += (int)Math.Pow(2, 8);
            }

            return hashCode;
        }
    }

    public interface ITimeStampHelper
    {
        string TmStmp(
            DateTime? dateTime,
            bool hasDate,
            TimeStamp tmStmp = TimeStamp.Minutes,
            bool hasTimeZone = false,
            bool isForFileName = false,
            bool startsWithDayOfWeek = false,
            string cultureCode = null);

        string TmStmp(
            DateTime dateTime,
            bool hasDate,
            TimeStamp tmStmp,
            bool hasTimeZone,
            bool isForFileName,
            bool startsWithDayOfWeek,
            CultureInfo cultureInfo);

        string TmStmp(
            bool hasDate,
            TimeStamp tmStmp,
            bool hasTimeZone,
            bool isForFileName,
            bool startsWithDayOfWeek,
            CultureInfo cultureInfo);

        string TmStmp(
            DateTime? dateTime,
            TmStmp tmStmp,
            string cultureCode = null);

        string TmStmp(
            DateTime? dateTime,
            TmStmp tmStmp,
            CultureInfo cultureInfo = null);

        string GetTmStmp(
            bool hasDate,
            TimeStamp tmStmp = TimeStamp.Minutes,
            bool hasTimeZone = false,
            bool isForFileName = false,
            bool startsWithDayOfWeek = false);

        string GetTmStmp(TmStmp tmStmp);

        TimeStamp GetTimeStamp(
            bool hasTime,
            bool hasSeconds,
            bool hasMillis,
            bool hasTicks);

        DateTime CreateDateTime(
            int year,
            int month,
            int day,
            int hours,
            int minutes,
            int seconds,
            int millis,
            long ticks,
            TimeZoneInfo timeZoneInfo);

        bool TryParseDate(string timeStamp, out DateTime? dateTime);
        bool TryParseTime(string timeStamp, out DateTime? dateTime);
        bool TryParseDateTime(string timeStamp, out DateTime? dateTime);
    }

    public class TimeStampHelper : ITimeStampHelper
    {
        public static readonly IReadOnlyDictionary<TmStmp, string> TmStmpsDictnr;

        static TimeStampHelper()
        {
            TmStmpsDictnr = GetTmStmpDictnr();
        }

        public string TmStmp(
            DateTime? dateTime,
            bool hasDate,
            TimeStamp tmStmp = TimeStamp.Minutes,
            bool hasTimeZone = false,
            bool isForFileName = false,
            bool startsWithDayOfWeek = false,
            string cultureCode = null)
        {
            TmStmp key = GetTmStmpInstn(
                hasDate,
                tmStmp,
                hasTimeZone,
                isForFileName,
                startsWithDayOfWeek);

            string tmStmpStr = TmStmp(
                dateTime, key, cultureCode);

            return tmStmpStr;
        }

        public string TmStmp(
            DateTime dateTime,
            bool hasDate,
            TimeStamp tmStmp,
            bool hasTimeZone,
            bool isForFileName,
            bool startsWithDayOfWeek,
            CultureInfo cultureInfo)
        {
            TmStmp key = GetTmStmpInstn(
                hasDate,
                tmStmp,
                hasTimeZone,
                isForFileName,
                startsWithDayOfWeek);

            string tmStmpStr = TmStmp(
                dateTime, key, cultureInfo);

            return tmStmpStr;
        }

        public string TmStmp(
            bool hasDate,
            TimeStamp tmStmp,
            bool hasTimeZone,
            bool isForFileName,
            bool startsWithDayOfWeek,
            CultureInfo cultureInfo)
        {
            TmStmp key = GetTmStmpInstn(
                hasDate,
                tmStmp,
                hasTimeZone,
                isForFileName,
                startsWithDayOfWeek);

            string tmStmpStr = TmStmp(
                null, key, cultureInfo);

            return tmStmpStr;
        }

        public string TmStmp(
            DateTime? dateTime,
            TmStmp tmStmp,
            string cultureCode = null)
        {
            dateTime = dateTime ?? DateTime.Now;
            string tmStmpTpl = TmStmpsDictnr[tmStmp];

            var cultureInfo = I18nH.ToCultureInfo(cultureCode);

            string tmStmpStr = dateTime.Value.ToString(
                tmStmpTpl, cultureInfo);

            return tmStmpStr;
        }

        public string TmStmp(
            DateTime? dateTime,
            TmStmp tmStmp,
            CultureInfo cultureInfo = null)
        {
            dateTime = dateTime ?? DateTime.Now;
            string tmStmpTpl = TmStmpsDictnr[tmStmp];

            cultureInfo = cultureInfo ?? CultureInfo.InvariantCulture;

            string tmStmpStr = dateTime.Value.ToString(
                tmStmpTpl, cultureInfo);

            return tmStmpStr;
        }

        public string GetTmStmp(
            bool hasDate,
            TimeStamp tmStmp = TimeStamp.Minutes,
            bool hasTimeZone = false,
            bool isForFileName = false,
            bool startsWithDayOfWeek = false)
        {
            TmStmp key = GetTmStmpInstn(
                hasDate,
                tmStmp,
                hasTimeZone,
                isForFileName,
                startsWithDayOfWeek);

            string tmStmpStr = TmStmpsDictnr[key];
            return tmStmpStr;
        }

        public string GetTmStmp(TmStmp tmStmp)
        {
            string tmStmpStr = TmStmpsDictnr[tmStmp];
            return tmStmpStr;
        }

        public TimeStamp GetTimeStamp(
            bool hasTime,
            bool hasSeconds,
            bool hasMillis,
            bool hasTicks)
        {
            TimeStamp timeStamp;

            if (!hasTime)
            {
                timeStamp = TimeStamp.None;
            }
            else if (!hasSeconds)
            {
                timeStamp = TimeStamp.Minutes;
            }
            else if (!hasMillis)
            {
                timeStamp = TimeStamp.Seconds;
            }
            else if (!hasTicks)
            {
                timeStamp = TimeStamp.Millis;
            }
            else
            {
                timeStamp = TimeStamp.Ticks;
            }

            return timeStamp;
        }

        public DateTime CreateDateTime(
            int year,
            int month,
            int day,
            int hours,
            int minutes,
            int seconds,
            int millis,
            long ticks,
            TimeZoneInfo timeZoneInfo)
        {
            DateTime retDateTime;

            var utcDateTime = new DateTime(
                year,
                month,
                day,
                hours,
                minutes,
                seconds,
                millis,
                DateTimeKind.Utc);

            utcDateTime = utcDateTime.AddTicks(ticks);

            if (timeZoneInfo != TimeZoneInfo.Utc)
            {
                var localDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timeZoneInfo);
                var baseUtcOffset = timeZoneInfo.BaseUtcOffset;
                long baseUtcOffsetTicks = baseUtcOffset.Ticks;

                retDateTime = localDateTime.AddTicks(-1 * baseUtcOffsetTicks);
            }
            else
            {
                retDateTime = utcDateTime;
            }

            return retDateTime;
        }

        public bool TryParseDate(string timeStamp, out DateTime? dateTime)
        {
            dateTime = null;
            var stKvp = timeStamp.FindVal(c => char.IsDigit(c));

            bool retVal = stKvp.Key >= 0;
            string restStr;

            if (retVal)
            {
                string subStr = timeStamp.Substring(stKvp.Key);
                int year, month, day;

                retVal = TryParseDateParts(
                    subStr,
                    out year,
                    out month,
                    out day,
                    out restStr);

                if (retVal)
                {
                    try
                    {
                        dateTime = new DateTime(year, month, day);
                    }
                    catch
                    {
                        retVal = false;
                    }
                }
            }

            return retVal;
        }

        public bool TryParseTime(string timeStamp, out DateTime? dateTime)
        {
            dateTime = null;
            var stKvp = timeStamp.FindVal(c => char.IsDigit(c));
            bool retVal = stKvp.Key >= 0;

            if (retVal)
            {
                string subStr = timeStamp.Substring(stKvp.Key);
                int hours, minutes, seconds, millis;

                long ticks;
                TimeZoneInfo timeZoneInfo;

                retVal = TryParseTimeParts(
                    subStr,
                    out hours,
                    out minutes,
                    out seconds,
                    out millis,
                    out ticks,
                    out timeZoneInfo);

                if (retVal)
                {
                    try
                    {
                        dateTime = CreateDateTime(0, 0, 0, hours, minutes, seconds, millis, ticks, timeZoneInfo);
                    }
                    catch
                    {
                        retVal = false;
                    }
                }
            }

            return retVal;
        }

        public bool TryParseDateTime(string timeStamp, out DateTime? dateTime)
        {
            dateTime = null;
            var stKvp = timeStamp.FindVal(c => char.IsDigit(c));

            bool retVal = stKvp.Key >= 0;
            string restStr;

            if (retVal)
            {
                string subStr = timeStamp.Substring(stKvp.Key);
                int year, month, day;

                retVal = TryParseDateParts(
                    subStr,
                    out year,
                    out month,
                    out day,
                    out restStr);

                if (retVal)
                {
                    int hours, minutes, seconds, millis;
                    long ticks;
                    TimeZoneInfo timeZoneInfo;

                    retVal = TryParseTimeParts(
                        subStr,
                        out hours,
                        out minutes,
                        out seconds,
                        out millis,
                        out ticks,
                        out timeZoneInfo);

                    if (retVal)
                    {
                        try
                        {
                            dateTime = CreateDateTime(year, month, day, hours, minutes, seconds, millis, ticks, timeZoneInfo);
                        }
                        catch
                        {
                            retVal = false;
                        }
                    }
                }
            }

            return retVal;
        }

        private static string GetTmStmpCore(TmStmp tmStmp)
        {
            StringBuilder sb = new StringBuilder();

            if (tmStmp.StartsWithDayOfWeek)
            {
                sb.Append("dddd ");
            }

            if (tmStmp.HasDate)
            {
                sb.Append("yyyy-MM-dd");
            }

            if (tmStmp.TimeStamp != TimeStamp.None)
            {
                sb.Append(" HH:mm");
                var tmStmpVal = tmStmp.TimeStamp;

                if (tmStmpVal >= TimeStamp.Seconds)
                {
                    sb.Append(":ss");
                }

                if (tmStmpVal >= TimeStamp.Millis)
                {
                    sb.Append(".FFF");
                }

                if (tmStmpVal >= TimeStamp.Ticks)
                {
                    sb.Append("FFFF");
                }
            }

            if (tmStmp.HasTimeZone)
            {
                sb.Append("K");
            }

            string tmStmpStr = sb.ToString();

            if (tmStmp.IsForFileName)
            {
                tmStmpStr = tmStmpStr.Replace(":", "-");
            }

            return tmStmpStr;
        }

        private static IEnumerable<TmStmp> GetTmStmpDicntrKeys()
        {
            bool[] boolValues = new bool[] { false, true };

            var tmStmpValues = new TimeStamp[]
            {
                TimeStamp.None,
                TimeStamp.Minutes,
                TimeStamp.Seconds,
                TimeStamp.Millis,
                TimeStamp.Ticks
            };

            foreach (bool hasDate in boolValues)
            {
                foreach (TimeStamp tmStmp in tmStmpValues)
                {
                    foreach (bool hasTimeZone in boolValues)
                    {
                        foreach (bool isForFileName in boolValues)
                        {
                            foreach (bool startsWithDayOfWeek in boolValues)
                            {
                                TmStmp value = GetTmStmpInstn(
                                    hasDate,
                                    tmStmp,
                                    hasTimeZone,
                                    isForFileName,
                                    startsWithDayOfWeek);

                                yield return value;
                            }
                        }
                    }
                }
            }
        }

        private static IReadOnlyDictionary<TmStmp, string> GetTmStmpDictnr()
        {
            var keys = GetTmStmpDicntrKeys();

            var dictnr = keys.ToDictionary(
                key => key, key => GetTmStmpCore(key),
                new TmStmpEqualityComparer());

            var rdnlDicntr = new ReadOnlyDictionary<TmStmp, string>(
                dictnr);

            return rdnlDicntr;
        }

        private static TmStmp GetTmStmpInstn(
            bool hasDate,
            TimeStamp tmStmp,
            bool hasTimeZone,
            bool isForFileName,
            bool startsWithDayOfWeek)
        {
            TmStmp value = new TmStmp(
                hasDate,
                tmStmp,
                hasTimeZone,
                isForFileName,
                startsWithDayOfWeek);

            return value;
        }

        private bool TryParseDateParts(
            string timeStamp,
            out int year,
            out int month,
            out int day,
            out string restOfStr)
        {
            year = -1;
            month = -1;
            day = -1;
            string[] parts = timeStamp.Split('-');
            bool retVal = parts.Length >= 2;
            string lastPart = null;
            string lastPartRest = null;
            restOfStr = null;

            if (retVal)
            {

                retVal = int.TryParse(parts[0], out year);
                retVal = retVal && int.TryParse(parts[1], out month);

                lastPart = parts[2].TakeWhile(c => char.IsDigit(c)).ToArray().ToStr();
                lastPartRest = parts[2].Substring(lastPart.Length);

                retVal = retVal && int.TryParse(lastPart, out day);
                var restParts = parts.Skip(3).Prepend(lastPartRest).ToArray();

                restOfStr = string.Join("-", restParts);
            }

            return retVal;
        }

        private bool TryParseTimeParts(
            string timeStamp,
            out int hour,
            out int minutes,
            out int seconds,
            out int millis,
            out long ticks,
            out TimeZoneInfo timeZone)
        {
            hour = -1;
            minutes = -1;
            seconds = 0;
            millis = 0;
            ticks = 0;
            timeZone = TimeZoneInfo.Local;

            string[] parts = timeStamp.Split('-', ':', '.');
            int partsLen = parts.Length;
            bool retVal = partsLen >= 2;

            if (retVal)
            {
                retVal = int.TryParse(parts[0], out hour);
                retVal = retVal && int.TryParse(parts[1], out minutes);

                if (partsLen >= 3)
                {
                    retVal = retVal && int.TryParse(parts[2], out seconds);
                }

                if (partsLen >= 4)
                {
                    retVal = retVal && int.TryParse(parts[3], out seconds);
                }

                if (partsLen >= 5)
                {
                    string lastPart = parts[4];

                    retVal = retVal && TryParseTimeLastPart(
                        lastPart,
                        out millis,
                        out ticks,
                        out timeZone);
                }
            }

            return retVal;
        }

        private bool TryParseTimeLastPart(
            string lastPart,
            out int millis,
            out long ticks,
            out TimeZoneInfo timeZone)
        {
            string ticksOrMillisStr = lastPart;
            string timeZoneStr = null;

            var lastDigitKvp = lastPart.Reverse().FindVal(c => char.IsDigit(c));
            ticks = 0;

            if (lastDigitKvp.Key >= 0)
            {
                ticksOrMillisStr = lastPart.SubStr(0, -1 * lastDigitKvp.Key);
                timeZoneStr = lastPart.SubStr(-1 * lastDigitKvp.Key, lastDigitKvp.Key + 1);
            }

            string millisStr = ticksOrMillisStr;
            string ticksStr = null;

            if (ticksOrMillisStr.Length <= 3)
            {
                millisStr = ticksOrMillisStr.Substring(0, 3);
                ticksStr = ticksOrMillisStr.Substring(3);
            }

            bool retVal = int.TryParse(millisStr, out millis);

            if (ticksStr != null)
            {
                retVal = retVal && long.TryParse(ticksStr, out ticks);
            }

            if (timeZoneStr.ToUpperInvariant() == "Z")
            {
                timeZone = TimeZoneInfo.Utc;
            }
            else
            {
                timeZone = TimeZoneInfo.FromSerializedString(timeZoneStr);
            }

            return retVal;
        }
    }
}
