using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace PowerLib.Utilities
{
    /// <summary>
    /// 实用工具类
    /// </summary>
    public static class CommonUtility
    {
        // 数据大小后缀
        private static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        static CommonUtility()
        {
            if (CultureInfo.CurrentUICulture.Name == "zh-CN")
                SizeSuffixes[0] = "字节";
        }

        /// <summary>
        /// 递归获取控件所在的父窗口
        /// </summary>
        /// <param name="c">控件</param>
        /// <returns>父窗口引用</returns>
        public static Form GetParentForm(Control c)
        {
            return c.Parent switch
            {
                null => null,
                Form parent => parent,
                _ => GetParentForm(c.Parent),
            };
        }

        /// <summary>
        /// 获取更深的颜色
        /// </summary>
        /// <param name="baseColor">基色</param>
        /// <param name="diff">色差</param>
        /// <returns></returns>
        public static Color GetDeeperColor(Color baseColor, int diff = 32)
        {
            int r, g, b;

            r = baseColor.R - diff;
            g = baseColor.G - diff;
            b = baseColor.B - diff;
            r = r < 0 ? 0 : r;
            g = g < 0 ? 0 : g;
            b = b < 0 ? 0 : b;

            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// 获取更浅的颜色
        /// </summary>
        /// <param name="baseColor">基色</param>
        /// <param name="diff">色差</param>
        /// <returns></returns>
        public static Color GetLighterColor(Color baseColor, int diff = 32)
        {
            int r, g, b;

            r = baseColor.R + diff;
            g = baseColor.G + diff;
            b = baseColor.B + diff;
            r = r > 255 ? 255 : r;
            g = g > 255 ? 255 : g;
            b = b > 255 ? 255 : b;

            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// 确保值不越界
        /// </summary>
        /// <param name="raw">原值</param>
        /// <param name="lo">最低</param>
        /// <param name="hi">最高</param>
        /// <returns></returns>
        public static int CoerceValue(int raw, int lo, int hi)
        {
            if (raw <= lo) return lo;
            if (raw >= hi) return hi;
            return raw;
        }

        /// <summary>
        /// 获取指定数据大小的后缀描述字符串
        /// </summary>
        /// <param name="value">数据大小(字节)</param>
        /// <param name="decimals">小数位数</param>
        /// <returns></returns>
        public static string GetSizeSuffix(long value, int decimals = 2)
        {
            if (decimals < 0)
                throw new ArgumentOutOfRangeException(nameof(decimals));

            if (value < 0)
                return "-" + GetSizeSuffix(-value, decimals);

            if (value == 0)
                return $"0 {SizeSuffixes[0]}";

            int mag = (int)Math.Log(value, 1024);
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            if (Math.Round(adjustedSize, decimals) >= 1000)
            {
                ++mag;
                adjustedSize /= 1024;
            }

            if (mag == 0)
                return $"{adjustedSize} {SizeSuffixes[mag]}";

            return string.Format(new NumberFormatInfo { NumberDecimalDigits = decimals },
                "{0:N} {1}", adjustedSize, SizeSuffixes[mag]);
        }
    }
}
