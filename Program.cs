using System;
using System.Globalization;

namespace FileMeta
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Testing valid values.");
                foreach (var testCase in CreateTestCases())
                {
                    Console.WriteLine(testCase.ToString());
                    testCase.PerformTest();
                }

                Console.WriteLine();
                Console.WriteLine("Testing expected parse failures.");

                // Test parse failures
                foreach (var s in s_failureCases)
                {
                    Console.WriteLine(s);
                    DateTag result;
                    if (DateTag.TryParse(s, out result))
                    {
                        throw new ApplicationException("Succeeded in parsing case that should have failed.");
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Testing detect precision.");
                TestDetectPrecision("2019-01-23T10:00:00", DateTag.PrecisionSecond);
                TestDetectPrecision("2019-01-23T10:00:00.000", DateTag.PrecisionSecond);
                TestDetectPrecision("2019-01-23T10:00:00.100", DateTag.PrecisionMillisecond);
                TestDetectPrecision("2019-01-23T10:00:00.120", DateTag.PrecisionMillisecond);
                TestDetectPrecision("2019-01-23T10:00:00.123", DateTag.PrecisionMillisecond);
                TestDetectPrecision("2019-01-23T10:00:00.123000", DateTag.PrecisionMillisecond);
                TestDetectPrecision("2019-01-23T10:00:00.1234", DateTag.PrecisionMicrosecond);
                TestDetectPrecision("2019-01-23T10:00:00.12345", DateTag.PrecisionMicrosecond);
                TestDetectPrecision("2019-01-23T10:00:00.123456", DateTag.PrecisionMicrosecond);
                TestDetectPrecision("2019-01-23T10:00:00.1234560", DateTag.PrecisionMicrosecond);
                TestDetectPrecision("2019-01-23T10:00:00.12345600", DateTag.PrecisionMicrosecond);
                TestDetectPrecision("2019-01-23T10:00:00.12345600", DateTag.PrecisionMicrosecond);
                TestDetectPrecision("2019-01-23T10:00:00.123456000", DateTag.PrecisionMicrosecond);
                TestDetectPrecision("2019-01-23T10:00:00.123456700", DateTag.PrecisionTick);
                TestDetectPrecision("2019-01-23T10:00:00.123456740", DateTag.PrecisionTick);
                TestDetectPrecision("2019-01-23T10:00:00.123456744", DateTag.PrecisionTick);
                TestDetectPrecision("2019-01-23T10:00:00.1234567444", DateTag.PrecisionTick);
                
                Console.WriteLine();
                Console.WriteLine("Testing AdaptFormatString.");
                Console.WriteLine(CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern + " " + CultureInfo.InvariantCulture.DateTimeFormat.ShortTimePattern);
                // c_fullDate = "dddd, MMMM d, yyyy h:mm:ss.FFFFFFF tt"
                TestAdaptFormatString(c_fullDate, DateTag.PrecisionTick + 10, c_fullDate);
                TestAdaptFormatString(c_fullDate, DateTag.PrecisionTick + 1, c_fullDate);
                TestAdaptFormatString(c_fullDate, DateTag.PrecisionTick, c_fullDate);
                TestAdaptFormatString(c_fullDate, DateTag.PrecisionMicrosecond, "dddd, MMMM d, yyyy h:mm:ss.FFFFFF tt");
                TestAdaptFormatString(c_fullDate, DateTag.PrecisionMillisecond, "dddd, MMMM d, yyyy h:mm:ss.FFF tt");
                TestAdaptFormatString(c_fullDate, DateTag.PrecisionSecond+1, "dddd, MMMM d, yyyy h:mm:ss.F tt");
                TestAdaptFormatString(c_fullDate, DateTag.PrecisionSecond, "dddd, MMMM d, yyyy h:mm:ss tt");
                TestAdaptFormatString(c_fullDate, DateTag.PrecisionMinute, "dddd, MMMM d, yyyy h:mm tt");
                TestAdaptFormatString(c_fullDate, DateTag.PrecisionHour, "dddd, MMMM d, yyyy h tt");
                TestAdaptFormatString(c_fullDate, DateTag.PrecisionDay, "dddd, MMMM d, yyyy");
                TestAdaptFormatString(c_fullDate, DateTag.PrecisionMonth, "MMMM yyyy");
                TestAdaptFormatString(c_fullDate, DateTag.PrecisionYear, "yyyy");
                TestAdaptFormatString(c_fullDate, 0, "yyyy");
                // c_shortDate = "MM/dd/yyyy HH:mm"
                TestAdaptFormatString(c_shortDate, DateTag.PrecisionMillisecond, c_shortDate);
                TestAdaptFormatString(c_shortDate, DateTag.PrecisionSecond + 1, c_shortDate);
                TestAdaptFormatString(c_shortDate, DateTag.PrecisionSecond, c_shortDate);
                TestAdaptFormatString(c_shortDate, DateTag.PrecisionMinute, c_shortDate);
                TestAdaptFormatString(c_shortDate, DateTag.PrecisionHour, "MM/dd/yyyy HH");
                TestAdaptFormatString(c_shortDate, DateTag.PrecisionDay, "MM/dd/yyyy");
                TestAdaptFormatString(c_shortDate, DateTag.PrecisionMonth, "MM/yyyy");
                TestAdaptFormatString(c_shortDate, DateTag.PrecisionYear, "yyyy");
                TestAdaptFormatString(c_shortDate, 0, "yyyy");
                // c_8601Date = "yyyy-MM-ddTHH:mm:ss.fffffff";
                TestAdaptFormatString(c_8601Date, DateTag.PrecisionTick + 10, c_8601Date);
                TestAdaptFormatString(c_8601Date, DateTag.PrecisionTick + 1, c_8601Date);
                TestAdaptFormatString(c_8601Date, DateTag.PrecisionTick, c_8601Date);
                TestAdaptFormatString(c_8601Date, DateTag.PrecisionMicrosecond, "yyyy-MM-ddTHH:mm:ss.ffffff");
                TestAdaptFormatString(c_8601Date, DateTag.PrecisionMillisecond, "yyyy-MM-ddTHH:mm:ss.fff");
                TestAdaptFormatString(c_8601Date, DateTag.PrecisionSecond + 1, "yyyy-MM-ddTHH:mm:ss.f");
                TestAdaptFormatString(c_8601Date, DateTag.PrecisionSecond, "yyyy-MM-ddTHH:mm:ss");
                TestAdaptFormatString(c_8601Date, DateTag.PrecisionMinute, "yyyy-MM-ddTHH:mm");
                TestAdaptFormatString(c_8601Date, DateTag.PrecisionHour, "yyyy-MM-ddTHH");
                TestAdaptFormatString(c_8601Date, DateTag.PrecisionDay, "yyyy-MM-dd");
                TestAdaptFormatString(c_8601Date, DateTag.PrecisionMonth, "yyyy-MM");
                TestAdaptFormatString(c_8601Date, DateTag.PrecisionYear, "yyyy");
                TestAdaptFormatString(c_8601Date, 0, "yyyy");
                // c_8601DateTz = "yyyy-MM-ddTHH:mm:ss.fffffffK"
                TestAdaptFormatString(c_8601DateTz, DateTag.PrecisionTick, c_8601DateTz);
                TestAdaptFormatString(c_8601DateTz, DateTag.PrecisionMicrosecond, "yyyy-MM-ddTHH:mm:ss.ffffffK");
                TestAdaptFormatString(c_8601DateTz, DateTag.PrecisionMillisecond, "yyyy-MM-ddTHH:mm:ss.fffK");
                TestAdaptFormatString(c_8601DateTz, DateTag.PrecisionSecond + 1, "yyyy-MM-ddTHH:mm:ss.fK");
                TestAdaptFormatString(c_8601DateTz, DateTag.PrecisionSecond, "yyyy-MM-ddTHH:mm:ssK");
                TestAdaptFormatString(c_8601DateTz, DateTag.PrecisionMinute, "yyyy-MM-ddTHH:mmK");
                TestAdaptFormatString(c_8601DateTz, DateTag.PrecisionHour, "yyyy-MM-ddTHHK");
                TestAdaptFormatString(c_8601DateTz, DateTag.PrecisionDay, "yyyy-MM-dd");
                TestAdaptFormatString(c_8601DateTz, DateTag.PrecisionMonth, "yyyy-MM");
                TestAdaptFormatString(c_8601DateTz, DateTag.PrecisionYear, "yyyy");
                // Boundary cases (e.g. empty string, just one component, etc.)
                TestAdaptFormatString("HH:mm", DateTag.PrecisionYear, string.Empty);
                TestAdaptFormatString("MM", DateTag.PrecisionMillisecond, "MM");

                Console.WriteLine();
                Console.WriteLine("Testing ToString()");
                TestToString("2018-12-28T10:59:45.1234567-05:00", c_fullDate, false, "Friday, December 28, 2018 10:59:45.1234567 AM");
                TestToString("2018-12-28T10:59:45.123456-05:00", c_fullDate, false, "Friday, December 28, 2018 10:59:45.123456 AM");
                TestToString("2018-12-28T10:59:45.12345-05:00", c_fullDate, false, "Friday, December 28, 2018 10:59:45.12345 AM");
                TestToString("2018-12-28T10:59:45.1234-05:00", c_fullDate, false, "Friday, December 28, 2018 10:59:45.1234 AM");
                TestToString("2018-12-28T10:59:45.123-05:00", c_fullDate, false, "Friday, December 28, 2018 10:59:45.123 AM");
                TestToString("2018-12-28T10:59:45.12-05:00", c_fullDate, false, "Friday, December 28, 2018 10:59:45.12 AM");
                TestToString("2018-12-28T10:59:45.1-05:00", c_fullDate, false, "Friday, December 28, 2018 10:59:45.1 AM");
                TestToString("2018-12-28T10:59:45-05:00", c_fullDate, false, "Friday, December 28, 2018 10:59:45 AM");
                TestToString("2018-12-28T10:59-05:00", c_fullDate, false, "Friday, December 28, 2018 10:59 AM");
                TestToString("2018-12-28T10-05:00", c_fullDate, false, "Friday, December 28, 2018 10 AM");
                TestToString("2018-12-28-05:00", c_fullDate, false, "Friday, December 28, 2018");
                TestToString("2018-12", c_fullDate, false, "December 2018");
                TestToString("2018", c_fullDate, false, "2018");
                TestToString("2018-12-28T10:59:45.1234567-05:00", c_fullDate, true, "Friday, December 28, 2018 3:59:45.1234567 PM");
                TestToString("2018-12-28T10:59:45.123456-05:00", c_fullDate, true, "Friday, December 28, 2018 3:59:45.123456 PM");
                TestToString("2018-12-28T10:59:45.12345-05:00", c_fullDate, true, "Friday, December 28, 2018 3:59:45.12345 PM");
                TestToString("2018-12-28T10:59:45.1234-05:00", c_fullDate, true, "Friday, December 28, 2018 3:59:45.1234 PM");
                TestToString("2018-12-28T10:59:45.123-05:00", c_fullDate, true, "Friday, December 28, 2018 3:59:45.123 PM");
                TestToString("2018-12-28T10:59:45.12-05:00", c_fullDate, true, "Friday, December 28, 2018 3:59:45.12 PM");
                TestToString("2018-12-28T10:59:45.1-05:00", c_fullDate, true, "Friday, December 28, 2018 3:59:45.1 PM");
                TestToString("2018-12-28T10:59:45-05:00", c_fullDate, true, "Friday, December 28, 2018 3:59:45 PM");
                TestToString("2018-12-28T10:59-05:00", c_fullDate, true, "Friday, December 28, 2018 3:59 PM");
                TestToString("2018-12-28T10-05:00", c_fullDate, true, "Friday, December 28, 2018 3 PM");
                TestToString("2018-12-28-05:00", c_fullDate, true, "Friday, December 28, 2018");
                TestToString("2018-12", c_fullDate, true, "December 2018");
                TestToString("2018", c_fullDate, true, "2018");
                TestToString("2018-12-28T21:59:45-05:00", c_shortDateTz, false, "12/28/2018 21:59-05:00");
                TestToString("2018-12-28T21-05:00", c_shortDateTz, false, "12/28/2018 21-05:00");
                TestToString("2018-12-28-14:00", c_shortDateTz, false, "12/28/2018");
                TestToString("2018-12-28T21:59:45-05:00", c_shortDateTz, true, "12/29/2018 02:59Z");
                TestToString("2018-12-28T21-05:00", c_shortDateTz, true, "12/29/2018 02Z");
                TestToString("2018-12-28-14:00", c_shortDateTz, true, "12/29/2018");

                Console.WriteLine();
                Console.WriteLine("All tests succeeded.");
            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
                Console.WriteLine();
            }

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
#endif
        }

        const string c_fullDate = "dddd, MMMM d, yyyy h:mm:ss.FFFFFFF tt";
        const string c_shortDate = "MM/dd/yyyy HH:mm";
        const string c_shortDateTz = "MM/dd/yyyy HH:mmK";
        const string c_8601Date = "yyyy-MM-ddTHH:mm:ss.fffffff";
        const string c_8601DateTz = "yyyy-MM-ddTHH:mm:ss.fffffffK";

        static readonly TimeZoneInfo s_tzPlusFourFive =
    TimeZoneInfo.CreateCustomTimeZone("Custom", new TimeSpan(4, 0, 0), "Custom", "Custom Standard", "Custom Daylight",
        new TimeZoneInfo.AdjustmentRule[] {
                        TimeZoneInfo.AdjustmentRule.CreateAdjustmentRule(DateTime.MinValue, new DateTime(9999, 1, 1), new TimeSpan(1, 0, 0),
                        TimeZoneInfo.TransitionTime.CreateFloatingDateRule(new DateTime(1, 1, 1, 2, 0, 0), 4, 2, DayOfWeek.Sunday),
                        TimeZoneInfo.TransitionTime.CreateFloatingDateRule(new DateTime(1, 1, 1, 2, 0, 0), 11, 1, DayOfWeek.Sunday))
        });

        static string[] s_failureCases = new string[]
        {
            "",
            "1",
            "12",
            "123",
            "ABCD",
            "0000",
            "0001-00",
            "0001-13",
            "0001-01-32",
            "0001-02-29",
            "2018-02-29",
            "2020-02-30",
            "0001-03-32",
            "0001-04-31",
            "0001-05-32",
            "0001-06-31",
            "0001-07-32",
            "0001-08-32",
            "0001-09-31",
            "0001-10-32",
            "0001-11-31",
            "0001-12-32",
            "0001-01-01T24",
            "0001-01-01T24:00:00",
            "0001-01-01T00:60",
            "0001-01-01T00:00:60",
            "0001-01-01T00:00:00-14:01",
            "0001-01-01T00:00:00-15",
            "0001-01-01T00:00:00+14:01",
            "0001-01-01T00:00:00+15",
            "0001-01-01T00:00:00-12:60",
            "0001-01-01T00:00:00+12:60"
        };

        // We use a function to create these rather than a static initializer so that we can control when
        // the exception would be thrown if there's a parse error.
        static TestCase[] CreateTestCases()
        {
            return new TestCase[]
            {
                new TestCase("2018", "2018-01-01T12:00:00", "0", 4),
                new TestCase("2018-12", "2018-12-01T12:00:00", "0", 6),
                new TestCase("2018-12-28", "2018-12-28T12:00:00", "0", 8),
                new TestCase("2018-12-28T10", "2018-12-28T10:00:00", "0", 10),
                new TestCase("2018-12-28T10-05:00", "2018-12-28T10:00:00", "-5", 10),
                new TestCase("2018-12-28T10+08:00", "2018-12-28T10:00:00", "+8", 10),
                new TestCase("2018-12-28T10:59", "2018-12-28T10:59:00", "0", 12),
                new TestCase("2018-12-28T10:59-05:00", "2018-12-28T10:59:00", "-5", 12),
                new TestCase("2018-12-28T10:59+08:00", "2018-12-28T10:59:00", "+8", 12),
                new TestCase("2018-12-28T10:59:45", "2018-12-28T10:59:45", "0", 14),
                new TestCase("2018-12-28T10:59:45-05:00", "2018-12-28T10:59:45", "-5", 14),
                new TestCase("2018-12-28T10:59:45+08:00", "2018-12-28T10:59:45", "+8", 14),
                new TestCase("2018-12-28T10:59:45.123", "2018-12-28T10:59:45.123", "0", 17),
                new TestCase("2018-12-28T10:59:45.123-05:00", "2018-12-28T10:59:45.123", "-5", 17),
                new TestCase("2018-12-28T10:59:45.123+08:00", "2018-12-28T10:59:45.123", "+8", 17),
                new TestCase("2018-12-28T10:59:45.1", "2018-12-28T10:59:45.1", "0", 15),
                new TestCase("2018-12-28T10:59:45.12", "2018-12-28T10:59:45.12", "0", 16),
                new TestCase("2018-12-28T10:59:45.123", "2018-12-28T10:59:45.123", "0", 17),
                new TestCase("2018-12-28T10:59:45.1234", "2018-12-28T10:59:45.1234", "0", 18),
                new TestCase("2018-12-28T10:59:45.12345", "2018-12-28T10:59:45.12345", "0", 19),
                new TestCase("2018-12-28T10:59:45.123456", "2018-12-28T10:59:45.123456", "0", 20),
                new TestCase("2018-12-28T10:59:45.1234567", "2018-12-28T10:59:45.1234567", "0", 21),

                // Inbound value is acceptable but not canonical
                new TestCase("2018-12-28T10:59-05", "2018-12-28T10:59-05:00", "2018-12-28T10:59:00", "-5", 12),
                new TestCase("2018-12-28T10:59+08", "2018-12-28T10:59+08:00", "2018-12-28T10:59:00", "+8", 12),
                new TestCase("2018-12-28T10:59:45-5", "2018-12-28T10:59:45-05:00", "2018-12-28T10:59:45", "-5", 14),
                new TestCase("2018-12-28T10:59:45+8", "2018-12-28T10:59:45+08:00", "2018-12-28T10:59:45", "+8", 14),
                new TestCase("2018-12-28T10:59:45.12345678", "2018-12-28T10:59:45.1234567", "2018-12-28T10:59:45.1234567", "0", 21),
                new TestCase("2018-12-28T10:59:45.12345678+08:23", "2018-12-28T10:59:45.1234567+08:23", "2018-12-28T10:59:45.1234567", "+08:23", 21),
            
                // UTC
                new TestCase("2018-12-28T23Z", "2018-12-28T23:00:00", "Z", 10),
                new TestCase("2018-12-28T23:59Z", "2018-12-28T23:59:00", "Z", 12),
                new TestCase("2018-12-28T23:59:45Z", "2018-12-28T23:59:45", "Z", 14),
                new TestCase("2018-12-28T23:59:59Z", "2018-12-28T23:59:59", "Z", 14),
                new TestCase("2018-12-28T23:59:59.9Z", "2018-12-28T23:59:59.9", "Z", 15),
                new TestCase("2018-12-28T23:59:59.99Z", "2018-12-28T23:59:59.99", "Z", 16),
                new TestCase("2018-12-28T23:59:59.999Z", "2018-12-28T23:59:59.999", "Z", 17),
                new TestCase("2018-12-28T23:59:59.9999Z", "2018-12-28T23:59:59.9999", "Z", 18),
                new TestCase("2018-12-28T23:59:59.99999Z", "2018-12-28T23:59:59.99999", "Z", 19),
                new TestCase("2018-12-28T23:59:59.999999Z", "2018-12-28T23:59:59.999999", "Z", 20),
                new TestCase("2018-12-28T23:59:59.9999999Z", "2018-12-28T23:59:59.9999999", "Z", 21),
                new TestCase("2018-12-28T23:59:59.99999999Z", "2018-12-28T23:59:59.9999999Z", "2018-12-28T23:59:59.9999999", "Z", 21),

                // Summer ForceLocal
                //  Summer has a different timezone offset in the test TimeZoneInfo due to Daylight
                //  savings. This affects ResolveTimezone related tests.
                new TestCase("2018-07-28T23", "2018-07-28T23:00:00", "0", 10),
                new TestCase("2018-07-28T23:59", "2018-07-28T23:59:00", "0", 12),
                new TestCase("2018-07-28T23:59:45", "2018-07-28T23:59:45", "0", 14),
                new TestCase("2018-07-28T23:59:59", "2018-07-28T23:59:59", "0", 14),
                new TestCase("2018-07-28T23:59:59.9", "2018-07-28T23:59:59.9", "0", 15),
                new TestCase("2018-07-28T23:59:59.99", "2018-07-28T23:59:59.99", "0", 16),
                new TestCase("2018-07-28T23:59:59.999", "2018-07-28T23:59:59.999", "0", 17),
                new TestCase("2018-07-28T23:59:59.9999", "2018-07-28T23:59:59.9999", "0", 18),
                new TestCase("2018-07-28T23:59:59.99999", "2018-07-28T23:59:59.99999", "0", 19),
                new TestCase("2018-07-28T23:59:59.999999", "2018-07-28T23:59:59.999999", "0", 20),
                new TestCase("2018-07-28T23:59:59.9999999", "2018-07-28T23:59:59.9999999", "0", 21),
                new TestCase("2018-07-28T23:59:59.99999999", "2018-07-28T23:59:59.9999999", "2018-07-28T23:59:59.9999999", "0", 21),

                // Summer ForceUtc
                new TestCase("2018-07-28T23Z", "2018-07-28T23:00:00", "Z", 10),
                new TestCase("2018-07-28T23:59Z", "2018-07-28T23:59:00", "Z", 12),
                new TestCase("2018-07-28T23:59:45Z", "2018-07-28T23:59:45", "Z", 14),
                new TestCase("2018-07-28T23:59:59Z", "2018-07-28T23:59:59", "Z", 14),
                new TestCase("2018-07-28T23:59:59.9Z", "2018-07-28T23:59:59.9", "Z", 15),
                new TestCase("2018-07-28T23:59:59.99Z", "2018-07-28T23:59:59.99", "Z", 16),
                new TestCase("2018-07-28T23:59:59.999Z", "2018-07-28T23:59:59.999", "Z", 17),
                new TestCase("2018-07-28T23:59:59.9999Z", "2018-07-28T23:59:59.9999", "Z", 18),
                new TestCase("2018-07-28T23:59:59.99999Z", "2018-07-28T23:59:59.99999", "Z", 19),
                new TestCase("2018-07-28T23:59:59.999999Z", "2018-07-28T23:59:59.999999", "Z", 20),
                new TestCase("2018-07-28T23:59:59.9999999Z", "2018-07-28T23:59:59.9999999", "Z", 21),
                new TestCase("2018-07-28T23:59:59.99999999Z", "2018-07-28T23:59:59.9999999Z", "2018-07-28T23:59:59.9999999", "Z", 21),
                // Extremes
                new TestCase("0001", "0001-01-01T12:00:00", "0", 4),
                new TestCase("0001-01-02T00:00:00-13:59", "0001-01-02T00:00:00", "-13:59", 14),
                new TestCase("0001-01-02T00:00:00-14:00", "0001-01-02T00:00:00", "-14:00", 14),
                new TestCase("0001-01-01T04:00:00-00:00", "0001-01-01T04:00:00+00:00", "0001-01-01T04:00:00", "+00:00", 14),
                new TestCase("0001-01-01T04:00:00+00:00", "0001-01-01T04:00:00", "+00:00", 14), // The timezone being used in the test has a four-hour winter offset. This is the lowest possible value.
                new TestCase("0001-01-01T04:00:00Z", "0001-01-01T04:00:00", "Z", 14),
                new TestCase("9999-12-31T12:59:59.9999999+00:00", "9999-12-31T12:59:59.9999999", "+00:00", 21),
                new TestCase("9999-12-31T12:59:59.9999999+13:59", "9999-12-31T12:59:59.9999999", "+13:59", 21),
                new TestCase("9999-12-31T12:59:59.9999999+14:00", "9999-12-31T12:59:59.9999999", "+14:00", 21),
                new TestCase("9999-12-31T12:59:59.9999999Z", "9999-12-31T12:59:59.9999999", "Z", 21),

                // Month Maximums
                new TestCase("2018-01-31", "2018-01-31T12:00:00", "0", 8),
                new TestCase("2018-02-28", "2018-02-28T12:00:00", "0", 8),
                new TestCase("2020-02-29", "2020-02-29T12:00:00", "0", 8),
                new TestCase("2018-03-31", "2018-03-31T12:00:00", "0", 8),
                new TestCase("2018-04-30", "2018-04-30T12:00:00", "0", 8),
                new TestCase("2018-05-31", "2018-05-31T12:00:00", "0", 8),
                new TestCase("2018-06-30", "2018-06-30T12:00:00", "0", 8),
                new TestCase("2018-07-31", "2018-07-31T12:00:00", "0", 8),
                new TestCase("2018-08-31", "2018-08-31T12:00:00", "0", 8),
                new TestCase("2018-09-30", "2018-09-30T12:00:00", "0", 8),
                new TestCase("2018-10-31", "2018-10-31T12:00:00", "0", 8),
                new TestCase("2018-11-30", "2018-11-30T12:00:00", "0", 8),
                new TestCase("2018-12-31", "2018-12-31T12:00:00", "0", 8)
            };
        }

        static void TestDetectPrecision(string dtStr, int expectedPrecision)
        {
            Console.WriteLine(dtStr);
            DateTime dt = DateTime.Parse(dtStr, CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault | DateTimeStyles.RoundtripKind);
            if (DateTag.DetectPrecision(dt) != expectedPrecision)
            {
                throw new ApplicationException("Failed DetectPrecision.");
            }
        }

        static void TestAdaptFormatString(string format, int precision, string expected)
        {
            string val = DateTag.PrecisionAdaptFormatString(precision, format);
            if (!string.Equals(val, expected, StringComparison.Ordinal))
            {
                throw new ApplicationException("Failed AdaptFormatString: " + val);
            }

            // Write to screen to ensure formatting doesn't throw an exception and to give format examples.
            var now = new DateTag(DateTime.UtcNow, s_tzPlusFourFive, precision);
            Console.WriteLine($"{expected} = {now.ToString(format, CultureInfo.CurrentCulture)} {now.ToString(format, CultureInfo.CurrentCulture, true)}");
        }

        static void TestToString(string val, string format, bool utcDefault, string expected)
        {
            Console.WriteLine($"{val} = {expected}");
            var dt = DateTag.Parse(val);
            var value = dt.ToString(format, CultureInfo.InvariantCulture, utcDefault);
            if (!value.Equals(expected, StringComparison.Ordinal))
            {
                throw new ApplicationException("Failed ToString(format): " + value);
            }
        }

        class TestCase
        {
            string m_srcTag;
            string m_canonicalTag;
            DateTime m_dt;
            TimeZoneTag m_tz;
            int m_precision;

            public TestCase(string srcTag, string canonicalTag, string dt, string tz, int precision)
            {
                m_srcTag = srcTag;
                m_canonicalTag = canonicalTag;
                m_dt = DateTime.SpecifyKind(
                    DateTime.Parse(dt, CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault | DateTimeStyles.RoundtripKind),
                    DateTimeKind.Local);
                m_tz = TimeZoneTag.Parse(tz);
                m_precision = precision;
            }

            public TestCase(string srcTag, string dt, string tz, int precision)
                : this(srcTag, srcTag, dt, tz, precision)
            {
            }

            public void PerformTest()
            {
                DateTag dtg;
                if (!DateTag.TryParse(m_srcTag, out dtg))
                {
                    throw new ApplicationException("Failed TryParse");
                }

                if (!dtg.ToString().Equals(m_canonicalTag))
                {
                    throw new ApplicationException("Failed ToString");
                }

                if (!Equals(dtg.Date, m_dt))
                {
                    throw new ApplicationException("Failed Date");
                }

                if (!dtg.TimeZone.Equals(m_tz))
                {
                    throw new ApplicationException("Failed TimeZone");
                }

                if (dtg.Precision != m_precision)
                {
                    throw new ApplicationException("Failed Precision");
                }

                DateTag dtg2 = DateTag.Parse(m_canonicalTag);
                if (!dtg.Equals(dtg2))
                {
                    throw new ApplicationException("Failed Equals");
                }

                if (!dtg.Equals((object)dtg2))
                {
                    throw new ApplicationException("Failed Equals Obj");
                }

                if (dtg.GetHashCode() != dtg2.GetHashCode())
                {
                    throw new ApplicationException("Failed GetHashCode");
                }

                if (dtg.Date.Kind != DateTimeKind.Local)
                {
                    throw new ApplicationException("Failed kind");
                }

                var dt = new DateTime(dtg.Date.Ticks - dtg.TimeZone.UtcOffsetTicks, DateTimeKind.Utc);
                if (!Equals(dtg.DateUtc, dt))
                {
                    throw new ApplicationException("Failed DateUtc");
                }

                // Constructors

                dtg2 = new DateTag(dtg.Date, dtg.TimeZone, dtg.Precision);
                if (!dtg.Equals(dtg2))
                {
                    throw new ApplicationException("Failed Constructor");
                }

                dtg2 = new DateTag(dtg.DateUtc, dtg.TimeZone, dtg.Precision);
                if (!dtg.Equals(dtg2))
                {
                    throw new ApplicationException("Failed Constructor on UTC");
                }

                TimeSpan offset = s_tzPlusFourFive.GetUtcOffset(dtg.Date);
                var dtgComp = new DateTag(dtg.Date, new TimeZoneTag(offset), dtg.Precision);

                dtg2 = new DateTag(dtg.Date, s_tzPlusFourFive, dtg.Precision);
                if (!dtg2.Equals(dtgComp) || !dtg2.TimeZone.UtcOffset.Equals(offset))
                {
                    throw new ApplicationException("Failed TimeZoneInfo constructor.");
                }

                dtg2 = new DateTag(new DateTimeOffset(dtg.Ticks, offset), dtg.Precision);
                if (!dtg2.Equals(dtgComp))
                {
                    throw new ApplicationException("Failed DateTimeOffset constructor.");
                }

                // Verify export to DateTimeOffset
                var dto = new DateTimeOffset(dtg.Ticks, offset);
                if (!Equals(dto, dtg2.ToDateTimeOffset()))
                {
                    throw new ApplicationException("Failed ToDateTimeOffset");
                }

                // Detect timezone and precision UTC

                dtg2 = new DateTag(dtg.DateUtc);
                if (!dtg2.Date.Equals(dtg.DateUtc) || !dtg2.DateUtc.Equals(dtg.DateUtc))
                {
                    throw new ApplicationException("Failed Constructor UTC Default");
                }

                if (!dtg2.TimeZone.Equals(TimeZoneTag.ForceUtc))
                {
                    throw new ApplicationException("Failed Constructor UTC Default Timezone");
                }

                if (dtg2.Precision != DateTag.DetectPrecision(dtg.Date))
                {
                    throw new ApplicationException("Failed Constructor UTC Default Precision");
                }

                // Detect timezone and precision Local

                dtg2 = new DateTag(dtg.Date);
                if (!dtg2.Date.Equals(dtg.Date) || !dtg2.DateUtc.Equals(dtg.Date))
                {
                    throw new ApplicationException("Failed Constructor Local Default");
                }

                if (!dtg2.TimeZone.Equals(TimeZoneTag.ForceLocal))
                {
                    throw new ApplicationException("Failed Constructor Local Default Timezone");
                }

                if (dtg2.Precision != DateTag.DetectPrecision(dtg.Date))
                {
                    throw new ApplicationException("Failed Constructor Local Default Precision");
                }

                // When ResolveTimeZone is called on a ForceLocal, then Date should remain the same
                // while DateUtc should change. When called on a ForceUtc then DateUtc should change while
                // Date remains the same.

                dtg2 = dtg.ResolveTimeZone(s_tzPlusFourFive);

                if (dtg.TimeZone.Kind == TimeZoneKind.Normal)
                {
                    if (!dtg2.TimeZone.Equals(dtg.TimeZone))
                    {
                        throw new ApplicationException("Failed ResolveTimeZone Normal");
                    }
                }
                else
                {
                    if (dtg2.TimeZone.Kind != TimeZoneKind.Normal)
                    {
                        throw new ApplicationException("Failed ResolveTimeZone Kind");
                    }
                   
                    if (!dtg2.TimeZone.UtcOffset.Equals(offset))
                    {
                        throw new ApplicationException("Failed ResolveTimeZone");
                    }

                    if (dtg.TimeZone.Kind == TimeZoneKind.ForceLocal)
                    {
                        if (!Equals(dtg.Date, dtg2.Date))
                        {
                            throw new ApplicationException("Failed ResolveTimezone Date match");
                        }

                        if (Equals(dtg.DateUtc, dtg2.Date))
                        {
                            throw new ApplicationException("Failed ResolveTimezone DateUtc match");
                        }

                        if (dtg2.Date.Ticks - dtg2.DateUtc.Ticks != offset.Ticks)
                        {
                            throw new ApplicationException("Failed ResolveTimezone offset");
                        }
                    }

                    else
                    {
                        if (dtg.TimeZone.Kind != TimeZoneKind.ForceUtc)
                        {
                            throw new ApplicationException("Unexpected TimeZone kind");
                        }

                        if (!Equals(dtg.DateUtc, dtg2.DateUtc))
                        {
                            throw new ApplicationException("Failed ResolveTimezone DateUtc match");
                        }

                        if (Equals(dtg.Date, dtg2.Date))
                        {
                            throw new ApplicationException("Failed ResolveTimezone Date match");
                        }

                        if (dtg2.Date.Ticks - dtg2.DateUtc.Ticks != offset.Ticks)
                        {
                            throw new ApplicationException("Failed ResolveTimezone offset");
                        }
                    }
                }
            }

            public override string ToString()
            {
                return m_srcTag;
            }

            // DateTime.Equals only checks the date and time value, it doesn't include Kind in the comparison
            // This function checks both.
            static bool Equals(DateTime a, DateTime b)
            {
                return a.Ticks == b.Ticks && a.Kind == b.Kind;
            }

            // DateTimeOffset checks that the two values represent the "same point in time". Therefore, they could be
            // different times and offsets so long as they resolve to the same UTC time.
            // This function checks that they are identical (same time and offset).
            static bool Equals(DateTimeOffset a, DateTimeOffset b)
            {
                return a.Ticks == b.Ticks && a.Offset.Ticks == b.Offset.Ticks;
            }
        }
    }
}
