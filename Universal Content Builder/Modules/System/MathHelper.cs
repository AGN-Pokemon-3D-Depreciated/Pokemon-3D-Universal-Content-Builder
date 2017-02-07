using System;
using System.Globalization;

namespace Modules.System
{
    public static class MathHelper
    {
        private static readonly string CurrentCulture = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

        public const double E = Math.E;
        public const double PI = Math.PI;

        public static decimal Abs(this decimal value)
        {
            return Math.Abs(value);
        }

        public static double Abs(this double value)
        {
            return Math.Abs(value);
        }

        public static float Abs(this float value)
        {
            return Math.Abs(value);
        }

        public static int Abs(this int value)
        {
            return Math.Abs(value);
        }

        public static long Abs(this long value)
        {
            return Math.Abs(value);
        }

        public static sbyte Abs(this sbyte value)
        {
            return Math.Abs(value);
        }

        public static short Abs(this short value)
        {
            return Math.Abs(value);
        }

        public static double Acos(this double d)
        {
            return Math.Acos(d);
        }

        public static double Asin(this double d)
        {
            return Math.Asin(d);
        }

        public static double Atan(this double d)
        {
            return Math.Atan(d);
        }

        public static double Atan2(this double y, double x)
        {
            return Math.Atan2(y, x);
        }

        public static long BigMul(this int a, int b)
        {
            return Math.BigMul(a, b);
        }

        public static decimal Ceiling(this decimal d)
        {
            return Math.Ceiling(d);
        }

        public static double Ceiling(this double a)
        {
            return Math.Ceiling(a);
        }

        public static double Cos(this double d)
        {
            return Math.Cos(d);
        }

        public static double Cosh(this double value)
        {
            return Math.Cosh(value);
        }

        public static int DivRem(this int a, int b, out int result)
        {
            Math.DivRem(a, b, out result);
            return result;
        }

        public static long DivRem(this long a, long b, out long result)
        {
            Math.DivRem(a, b, out result);
            return result;
        }

        public static double Exp(this double d)
        {
            return Math.Exp(d);
        }

        public static decimal Floor(this decimal d)
        {
            return Math.Floor(d);
        }

        public static double Floor(this double d)
        {
            return Math.Floor(d);
        }

        public static double IEEERemainder(this double x, double y)
        {
            return Math.IEEERemainder(x, y);
        }

        public static double Log(this double d)
        {
            return Math.Log(d);
        }

        public static double Log(this double a, double newBase)
        {
            return Math.Log(a, newBase);
        }

        public static double Log10(this double d)
        {
            return Math.Log10(d);
        }

        public static byte Max(this byte val1, byte val2)
        {
            return Math.Max(val1, val2);
        }

        public static decimal Max(this decimal val1, decimal val2)
        {
            return Math.Max(val1, val2);
        }

        public static double Max(this double val1, double val2)
        {
            return Math.Max(val1, val2);
        }

        public static float Max(this float val1, float val2)
        {
            return Math.Max(val1, val2);
        }

        public static int Max(this int val1, int val2)
        {
            return Math.Max(val1, val2);
        }

        public static long Max(this long val1, long val2)
        {
            return Math.Max(val1, val2);
        }

        public static sbyte Max(this sbyte val1, sbyte val2)
        {
            return Math.Max(val1, val2);
        }

        public static short Max(this short val1, short val2)
        {
            return Math.Max(val1, val2);
        }

        public static uint Max(this uint val1, uint val2)
        {
            return Math.Max(val1, val2);
        }

        public static ulong Max(this ulong val1, ulong val2)
        {
            return Math.Max(val1, val2);
        }

        public static ushort Max(this ushort val1, ushort val2)
        {
            return Math.Max(val1, val2);
        }

        public static byte Min(this byte val1, byte val2)
        {
            return Math.Min(val1, val2);
        }

        public static decimal Min(this decimal val1, decimal val2)
        {
            return Math.Min(val1, val2);
        }

        public static double Min(this double val1, double val2)
        {
            return Math.Min(val1, val2);
        }

        public static float Min(this float val1, float val2)
        {
            return Math.Min(val1, val2);
        }

        public static int Min(this int val1, int val2)
        {
            return Math.Min(val1, val2);
        }

        public static long Min(this long val1, long val2)
        {
            return Math.Min(val1, val2);
        }

        public static sbyte Min(this sbyte val1, sbyte val2)
        {
            return Math.Min(val1, val2);
        }

        public static short Min(this short val1, short val2)
        {
            return Math.Min(val1, val2);
        }

        public static uint Min(this uint val1, uint val2)
        {
            return Math.Min(val1, val2);
        }

        public static ulong Min(this ulong val1, ulong val2)
        {
            return Math.Min(val1, val2);
        }

        public static ushort Min(this ushort val1, ushort val2)
        {
            return Math.Min(val1, val2);
        }

        public static double Pow(this double x, double y)
        {
            return Math.Pow(x, y);
        }

        public static decimal Round(this decimal d)
        {
            return Math.Round(d);
        }

        public static double Round(this double d)
        {
            return Math.Round(d);
        }

        public static decimal Round(this decimal d, int decimals)
        {
            return Math.Round(d, decimals);
        }

        public static decimal Round(this decimal d, MidpointRounding mode)
        {
            return Math.Round(d, mode);
        }

        public static double Round(this double value, int digits)
        {
            return Math.Round(value, digits);
        }

        public static double Round(this double value, MidpointRounding mode)
        {
            return Math.Round(value, mode);
        }

        public static decimal Round(this decimal d, int decimals, MidpointRounding mode)
        {
            return Math.Round(d, decimals, mode);
        }

        public static double Round(this double value, int digits, MidpointRounding mode)
        {
            return Math.Round(value, digits, mode);
        }

        public static int Sign(this decimal value)
        {
            return Math.Sign(value);
        }

        public static int Sign(this double value)
        {
            return Math.Sign(value);
        }

        public static int Sign(this float value)
        {
            return Math.Sign(value);
        }

        public static int Sign(this int value)
        {
            return Math.Sign(value);
        }

        public static int Sign(this long value)
        {
            return Math.Sign(value);
        }

        public static int Sign(this sbyte value)
        {
            return Math.Sign(value);
        }

        public static int Sign(this short value)
        {
            return Math.Sign(value);
        }

        public static double Sin(this double a)
        {
            return Math.Sin(a);
        }

        public static double Sinh(this double value)
        {
            return Math.Sinh(value);
        }

        public static double Sqrt(this double d)
        {
            return Math.Sqrt(d);
        }

        public static double Tan(this double a)
        {
            return Math.Tan(a);
        }

        public static double Tanh(this double value)
        {
            return Math.Tanh(value);
        }

        public static decimal Truncate(this decimal d)
        {
            return Math.Truncate(d);
        }

        public static double Truncate(this double d)
        {
            return Math.Truncate(d);
        }

        public static string ConvertStringCulture(this string value, string separator = null)
        {
            if (separator == null)
                return value.Replace(".", CurrentCulture).Replace(",", CurrentCulture);
            else
                return value.Replace(".", separator).Replace(",", separator);
        }

        public static byte Clamp(this byte value, byte minValue, byte maxValue)
        {
            if (value < minValue)
                return minValue;
            else if (value > maxValue)
                return maxValue;
            else
                return value;
        }

        public static sbyte Clamp(this sbyte value, sbyte minValue, sbyte maxValue)
        {
            if (value < minValue)
                return minValue;
            else if (value > maxValue)
                return maxValue;
            else
                return value;
        }

        public static short Clamp(this short value, short minValue, short maxValue)
        {
            if (value < minValue)
                return minValue;
            else if (value > maxValue)
                return maxValue;
            else
                return value;
        }

        public static ushort Clamp(this ushort value, ushort minValue, ushort maxValue)
        {
            if (value < minValue)
                return minValue;
            else if (value > maxValue)
                return maxValue;
            else
                return value;
        }

        public static int Clamp(this int value, int minValue, int maxValue)
        {
            if (value < minValue)
                return minValue;
            else if (value > maxValue)
                return maxValue;
            else
                return value;
        }

        public static uint Clamp(this uint value, uint minValue, uint maxValue)
        {
            if (value < minValue)
                return minValue;
            else if (value > maxValue)
                return maxValue;
            else
                return value;
        }

        public static long Clamp(this long value, long minValue, long maxValue)
        {
            if (value < minValue)
                return minValue;
            else if (value > maxValue)
                return maxValue;
            else
                return value;
        }

        public static ulong Clamp(this ulong value, ulong minValue, ulong maxValue)
        {
            if (value < minValue)
                return minValue;
            else if (value > maxValue)
                return maxValue;
            else
                return value;
        }

        public static float Clamp(this float value, float minValue, float maxValue)
        {
            if (value < minValue)
                return minValue;
            else if (value > maxValue)
                return maxValue;
            else
                return value;
        }

        public static double Clamp(this double value, double minValue, double maxValue)
        {
            if (value < minValue)
                return minValue;
            else if (value > maxValue)
                return maxValue;
            else
                return value;
        }

        public static decimal Clamp(this decimal value, decimal minValue, decimal maxValue)
        {
            if (value < minValue)
                return minValue;
            else if (value > maxValue)
                return maxValue;
            else
                return value;
        }

        public static bool ToBool(this byte value)
        {
            return value > 0;
        }

        public static bool ToBool(this sbyte value)
        {
            return value > 0;
        }

        public static bool ToBool(this short value)
        {
            return value > 0;
        }

        public static bool ToBool(this ushort value)
        {
            return value > 0;
        }

        public static bool ToBool(this int value)
        {
            return value > 0;
        }

        public static bool ToBool(this uint value)
        {
            return value > 0;
        }

        public static bool ToBool(this long value)
        {
            return value > 0;
        }

        public static bool ToBool(this ulong value)
        {
            return value > 0;
        }

        public static bool ToBool(this float value)
        {
            return value > 0;
        }

        public static bool ToBool(this double value)
        {
            return value > 0;
        }

        public static bool ToBool(this decimal value)
        {
            return value > 0;
        }

        public static bool ToBool(this string value)
        {
            if (StringHelper.Equals(value, bool.TrueString, bool.FalseString))
                return bool.Parse(value);
            else
                return value.ToDouble().ToBool();
        }

        public static byte ToByte(this bool value)
        {
            return (byte)(value ? 1 : 0);
        }

        public static byte ToByte(this sbyte value)
        {
            return value < byte.MinValue ? byte.MinValue : (byte)value;
        }

        public static byte ToByte(this short value)
        {
            return value < byte.MinValue ? byte.MinValue : value > byte.MaxValue ? byte.MaxValue : (byte)value;
        }

        public static byte ToByte(this ushort value)
        {
            return value < byte.MinValue ? byte.MinValue : value > byte.MaxValue ? byte.MaxValue : (byte)value;
        }

        public static byte ToByte(this int value)
        {
            return value < byte.MinValue ? byte.MinValue : value > byte.MaxValue ? byte.MaxValue : (byte)value;
        }

        public static byte ToByte(this uint value)
        {
            return value < byte.MinValue ? byte.MinValue : value > byte.MaxValue ? byte.MaxValue : (byte)value;
        }

        public static byte ToByte(this long value)
        {
            return value < byte.MinValue ? byte.MinValue : value > byte.MaxValue ? byte.MaxValue : (byte)value;
        }

        public static byte ToByte(this ulong value)
        {
            return value < byte.MinValue ? byte.MinValue : value > byte.MaxValue ? byte.MaxValue : (byte)value;
        }

        public static byte ToByte(this float value)
        {
            return value < byte.MinValue ? byte.MinValue : value > byte.MaxValue ? byte.MaxValue : (byte)value;
        }

        public static byte ToByte(this double value)
        {
            return value < byte.MinValue ? byte.MinValue : value > byte.MaxValue ? byte.MaxValue : (byte)value;
        }

        public static byte ToByte(this decimal value)
        {
            return value < byte.MinValue ? byte.MinValue : value > byte.MaxValue ? byte.MaxValue : (byte)value;
        }

        public static byte ToByte(this string value)
        {
            return value.ToDouble().ToByte();
        }

        public static sbyte ToSbyte(this bool value)
        {
            return (sbyte)(value ? 1 : 0);
        }

        public static sbyte ToSbyte(this byte value)
        {
            return value > sbyte.MaxValue ? sbyte.MaxValue : (sbyte)value;
        }

        public static sbyte ToSbyte(this short value)
        {
            return value < sbyte.MinValue ? sbyte.MinValue : value > sbyte.MaxValue ? sbyte.MaxValue : (sbyte)value;
        }

        public static sbyte ToSbyte(this ushort value)
        {
            return value > sbyte.MaxValue ? sbyte.MaxValue : (sbyte)value;
        }

        public static sbyte ToSbyte(this int value)
        {
            return value < sbyte.MinValue ? sbyte.MinValue : value > sbyte.MaxValue ? sbyte.MaxValue : (sbyte)value;
        }

        public static sbyte ToSbyte(this uint value)
        {
            return value > sbyte.MaxValue ? sbyte.MaxValue : (sbyte)value;
        }

        public static sbyte ToSbyte(this long value)
        {
            return value < sbyte.MinValue ? sbyte.MinValue : value > sbyte.MaxValue ? sbyte.MaxValue : (sbyte)value;
        }

        public static sbyte ToSbyte(this ulong value)
        {
            return value > sbyte.MaxValue.ToUlong() ? sbyte.MaxValue : (sbyte)value;
        }

        public static sbyte ToSbyte(this float value)
        {
            return value < sbyte.MinValue ? sbyte.MinValue : value > sbyte.MaxValue ? sbyte.MaxValue : (sbyte)value;
        }

        public static sbyte ToSbyte(this double value)
        {
            return value < sbyte.MinValue ? sbyte.MinValue : value > sbyte.MaxValue ? sbyte.MaxValue : (sbyte)value;
        }

        public static sbyte ToSbyte(this decimal value)
        {
            return value < sbyte.MinValue ? sbyte.MinValue : value > sbyte.MaxValue ? sbyte.MaxValue : (sbyte)value;
        }

        public static sbyte ToSbyte(this string value)
        {
            return value.ToDouble().ToSbyte();
        }

        public static short ToShort(this bool value)
        {
            return (short)(value ? 1 : 0);
        }

        public static short ToShort(this byte value)
        {
            return value;
        }

        public static short ToShort(this sbyte value)
        {
            return value;
        }

        public static short ToShort(this ushort value)
        {
            return value > short.MaxValue ? short.MaxValue : (short)value;
        }

        public static short ToShort(this int value)
        {
            return value < short.MinValue ? short.MinValue : value > short.MaxValue ? short.MaxValue : (short)value;
        }

        public static short ToShort(this uint value)
        {
            return value > short.MaxValue ? short.MaxValue : (short)value;
        }

        public static short ToShort(this long value)
        {
            return value < short.MinValue ? short.MinValue : value > short.MaxValue ? short.MaxValue : (short)value;
        }

        public static short ToShort(this ulong value)
        {
            return value > short.MaxValue.ToUlong() ? short.MaxValue : (short)value;
        }

        public static short ToShort(this float value)
        {
            return value < short.MinValue ? short.MinValue : value > short.MaxValue ? short.MaxValue : (short)value;
        }

        public static short ToShort(this double value)
        {
            return value < short.MinValue ? short.MinValue : value > short.MaxValue ? short.MaxValue : (short)value;
        }

        public static short ToShort(this decimal value)
        {
            return value < short.MinValue ? short.MinValue : value > short.MaxValue ? short.MaxValue : (short)value;
        }

        public static short ToShort(this string value)
        {
            return value.ToDouble().ToShort();
        }

        public static ushort ToUshort(this bool value)
        {
            return (ushort)(value ? 1 : 0);
        }

        public static ushort ToUshort(this byte value)
        {
            return value;
        }

        public static ushort ToUshort(this sbyte value)
        {
            return value < ushort.MinValue ? ushort.MinValue : (ushort)value;
        }

        public static ushort ToUshort(this short value)
        {
            return value < ushort.MinValue ? ushort.MinValue : (ushort)value;
        }

        public static ushort ToUshort(this int value)
        {
            return value < ushort.MinValue ? ushort.MinValue : value > ushort.MaxValue ? ushort.MaxValue : (ushort)value;
        }

        public static ushort ToUshort(this uint value)
        {
            return value < ushort.MinValue ? ushort.MinValue : value > ushort.MaxValue ? ushort.MaxValue : (ushort)value;
        }

        public static ushort ToUshort(this long value)
        {
            return value < ushort.MinValue ? ushort.MinValue : value > ushort.MaxValue ? ushort.MaxValue : (ushort)value;
        }

        public static ushort ToUshort(this ulong value)
        {
            return value < ushort.MinValue ? ushort.MinValue : value > ushort.MaxValue ? ushort.MaxValue : (ushort)value;
        }

        public static ushort ToUshort(this float value)
        {
            return value < ushort.MinValue ? ushort.MinValue : value > ushort.MaxValue ? ushort.MaxValue : (ushort)value;
        }

        public static ushort ToUshort(this double value)
        {
            return value < ushort.MinValue ? ushort.MinValue : value > ushort.MaxValue ? ushort.MaxValue : (ushort)value;
        }

        public static ushort ToUshort(this decimal value)
        {
            return value < ushort.MinValue ? ushort.MinValue : value > ushort.MaxValue ? ushort.MaxValue : (ushort)value;
        }

        public static ushort ToUshort(this string value)
        {
            return value.ToDouble().ToUshort();
        }

        public static int ToInt(this bool value)
        {
            return value ? 1 : 0;
        }

        public static int ToInt(this byte value)
        {
            return value;
        }

        public static int ToInt(this sbyte value)
        {
            return value;
        }

        public static int ToInt(this short value)
        {
            return value;
        }

        public static int ToInt(this ushort value)
        {
            return value;
        }

        public static int ToInt(this uint value)
        {
            return value > int.MaxValue ? int.MaxValue : (int)value;
        }

        public static int ToInt(this long value)
        {
            return value < int.MinValue ? int.MinValue : value > int.MaxValue ? int.MaxValue : (int)value;
        }

        public static int ToInt(this ulong value)
        {
            return value > int.MaxValue ? int.MaxValue : (int)value;
        }

        public static int ToInt(this float value)
        {
            return value < int.MinValue ? int.MinValue : value > int.MaxValue ? int.MaxValue : (int)value;
        }

        public static int ToInt(this double value)
        {
            return value < int.MinValue ? int.MinValue : value > int.MaxValue ? int.MaxValue : (int)value;
        }

        public static int ToInt(this decimal value)
        {
            return value < int.MinValue ? int.MinValue : value > int.MaxValue ? int.MaxValue : (int)value;
        }

        public static int ToInt(this string value)
        {
            return value.ToDouble().ToInt();
        }

        public static uint ToUint(this bool value)
        {
            return (uint)(value ? 1 : 0);
        }

        public static uint ToUint(this byte value)
        {
            return value;
        }

        public static uint ToUint(this sbyte value)
        {
            return value < uint.MinValue ? uint.MinValue : (uint)value;
        }

        public static uint ToUint(this short value)
        {
            return value < uint.MinValue ? uint.MinValue : (uint)value;
        }

        public static uint ToUint(this ushort value)
        {
            return value;
        }

        public static uint ToUint(this int value)
        {
            return value < uint.MinValue ? uint.MinValue : (uint)value;
        }

        public static uint ToUint(this long value)
        {
            return value < uint.MinValue ? uint.MinValue : value > uint.MaxValue ? uint.MaxValue : (uint)value;
        }

        public static uint ToUint(this ulong value)
        {
            return value < uint.MinValue ? uint.MinValue : value > uint.MaxValue ? uint.MaxValue : (uint)value;
        }

        public static uint ToUint(this float value)
        {
            return value < uint.MinValue ? uint.MinValue : value > uint.MaxValue ? uint.MaxValue : (uint)value;
        }

        public static uint ToUint(this double value)
        {
            return value < uint.MinValue ? uint.MinValue : value > uint.MaxValue ? uint.MaxValue : (uint)value;
        }

        public static uint ToUint(this decimal value)
        {
            return value < uint.MinValue ? uint.MinValue : value > uint.MaxValue ? uint.MaxValue : (uint)value;
        }

        public static uint ToUint(this string value)
        {
            return value.ToDouble().ToUint();
        }

        public static long ToLong(this bool value)
        {
            return value ? 1 : 0;
        }

        public static long ToLong(this byte value)
        {
            return value;
        }

        public static long ToLong(this sbyte value)
        {
            return value;
        }

        public static long ToLong(this short value)
        {
            return value;
        }

        public static long ToLong(this ushort value)
        {
            return value;
        }

        public static long ToLong(this int value)
        {
            return value;
        }

        public static long ToLong(this uint value)
        {
            return value;
        }

        public static long ToLong(this ulong value)
        {
            return value > long.MaxValue ? long.MaxValue : (long)value;
        }

        public static long ToLong(this float value)
        {
            return value < long.MinValue ? long.MinValue : value > long.MaxValue ? long.MaxValue : (long)value;
        }

        public static long ToLong(this double value)
        {
            return value < long.MinValue ? long.MinValue : value > long.MaxValue ? long.MaxValue : (long)value;
        }

        public static long ToLong(this decimal value)
        {
            return value < long.MinValue ? long.MinValue : value > long.MaxValue ? long.MaxValue : (long)value;
        }

        public static long ToLong(this string value)
        {
            return value.ToDouble().ToLong();
        }

        public static ulong ToUlong(this bool value)
        {
            return (ulong)(value ? 1 : 0);
        }

        public static ulong ToUlong(this byte value)
        {
            return value;
        }

        public static ulong ToUlong(this sbyte value)
        {
            return value < 0 ? ulong.MinValue : (ulong)value;
        }

        public static ulong ToUlong(this short value)
        {
            return value < 0 ? ulong.MinValue : (ulong)value;
        }

        public static ulong ToUlong(this ushort value)
        {
            return value;
        }

        public static ulong ToUlong(this int value)
        {
            return value < 0 ? ulong.MinValue : (ulong)value;
        }

        public static ulong ToUlong(this uint value)
        {
            return value;
        }

        public static ulong ToUlong(this long value)
        {
            return value < 0 ? ulong.MinValue : (ulong)value;
        }

        public static ulong ToUlong(this float value)
        {
            return value < ulong.MinValue ? ulong.MinValue : value > ulong.MaxValue ? ulong.MaxValue : (ulong)value;
        }

        public static ulong ToUlong(this double value)
        {
            return value < ulong.MinValue ? ulong.MinValue : value > ulong.MaxValue ? ulong.MaxValue : (ulong)value;
        }

        public static ulong ToUlong(this decimal value)
        {
            return value < ulong.MinValue ? ulong.MinValue : value > ulong.MaxValue ? ulong.MaxValue : (ulong)value;
        }

        public static ulong ToUlong(this string value)
        {
            return value.ToDouble().ToUlong();
        }

        public static float ToFloat(this bool value)
        {
            return value ? 1 : 0;
        }

        public static float ToFloat(this byte value)
        {
            return value;
        }

        public static float ToFloat(this sbyte value)
        {
            return value;
        }

        public static float ToFloat(this short value)
        {
            return value;
        }

        public static float ToFloat(this ushort value)
        {
            return value;
        }

        public static float ToFloat(this int value)
        {
            return value;
        }

        public static float ToFloat(this uint value)
        {
            return value;
        }

        public static float ToFloat(this long value)
        {
            return value;
        }

        public static float ToFloat(this ulong value)
        {
            return value;
        }

        public static float ToFloat(this double value)
        {
            return value < float.MinValue ? float.MinValue : value > float.MaxValue ? float.MaxValue : (float)value;
        }

        public static float ToFloat(this decimal value)
        {
            return (float)value;
        }

        public static float ToFloat(this string value)
        {
            return value.ToDouble().ToFloat();
        }

        public static double ToDouble(this bool value)
        {
            return value ? 1 : 0;
        }

        public static double ToDouble(this byte value)
        {
            return value;
        }

        public static double ToDouble(this sbyte value)
        {
            return value;
        }

        public static double ToDouble(this short value)
        {
            return value;
        }

        public static double ToDouble(this ushort value)
        {
            return value;
        }

        public static double ToDouble(this int value)
        {
            return value;
        }

        public static double ToDouble(this uint value)
        {
            return value;
        }

        public static double ToDouble(this long value)
        {
            return value;
        }

        public static double ToDouble(this ulong value)
        {
            return value;
        }

        public static double ToDouble(this float value)
        {
            return value;
        }

        public static double ToDouble(this decimal value)
        {
            return (double)value;
        }

        public static double ToDouble(this string value)
        {
            if (value.StartsWith("-"))
            {
                try
                {
                    return double.Parse(value.ConvertStringCulture());
                }
                catch (Exception) { return double.MinValue; }
            }
            else
            {
                try
                {
                    return double.Parse(value.ConvertStringCulture());
                }
                catch (Exception) { return double.MaxValue; }
            }
        }

        public static decimal ToDecimal(this bool value)
        {
            return value ? 1 : 0;
        }

        public static decimal ToDecimal(this byte value)
        {
            return value;
        }

        public static decimal ToDecimal(this sbyte value)
        {
            return value;
        }

        public static decimal ToDecimal(this short value)
        {
            return value;
        }

        public static decimal ToDecimal(this ushort value)
        {
            return value;
        }

        public static decimal ToDecimal(this int value)
        {
            return value;
        }

        public static decimal ToDecimal(this uint value)
        {
            return value;
        }

        public static decimal ToDecimal(this long value)
        {
            return value;
        }

        public static decimal ToDecimal(this ulong value)
        {
            return value;
        }

        public static decimal ToDecimal(this float value)
        {
            return value < (float)decimal.MinValue ? decimal.MinValue : value > (float)decimal.MaxValue ? decimal.MaxValue : (decimal)value;
        }

        public static decimal ToDecimal(this double value)
        {
            return value < (double)decimal.MinValue ? decimal.MinValue : value > (double)decimal.MaxValue ? decimal.MaxValue : (decimal)value;
        }

        public static decimal ToDecimal(this string value)
        {
            return value.ToDouble().ToDecimal();
        }

        public static int Random(this int minValue, int maxValue)
        {
            return new Random().Next(minValue, maxValue);
        }

        public static int RollOver(this int value, int minValue, int maxValue)
        {
            int Diff = maxValue - minValue + 1;
            int NewValue = value;

            if (value > maxValue)
            {
                while (NewValue > maxValue)
                    NewValue -= Diff;
            }
            else if (value < minValue)
            {
                while (NewValue < maxValue)
                    NewValue += Diff;
            }

            return NewValue;
        }

        public static long RollOver(this long value, long minValue, long maxValue)
        {
            long Diff = maxValue - minValue + 1;
            long NewValue = value;

            if (value > maxValue)
            {
                while (NewValue > maxValue)
                    NewValue -= Diff;
            }
            else if (value < minValue)
            {
                while (NewValue < maxValue)
                    NewValue += Diff;
            }

            return NewValue;
        }
    }
}