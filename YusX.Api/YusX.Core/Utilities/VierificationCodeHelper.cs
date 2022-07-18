using SkiaSharp;
using System;
using System.IO;
using System.Linq;

namespace YusX.Core.Utilities
{
    /// <summary>
    /// 验证码帮助类
    /// </summary>
    public static class VierificationCodeHelper
    {
        /// <summary>
        /// 验证码字体集合
        /// </summary>
        private static readonly string[] _fonts = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };

        /// <summary>
        /// 验证码颜色集合
        /// </summary>
        private static readonly SKColor[] _colors = { SKColors.Black, SKColors.Green, SKColors.Brown };

        /// <summary>
        /// 验证码内容集合
        /// </summary>
        private static readonly string[] _chars = new string[] {
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        /// <summary>
        /// 创建验证码图片的 BASE64 字符串
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string CreateBase64Image(string code)
        {
            var randomizer = new Random(Guid.NewGuid().GetHashCode());
            var imgInfo = new SKImageInfo(code.Length * 18, 32);
            using var bitmap = new SKBitmap(imgInfo);
            using var canvas = new SKCanvas(bitmap);

            // 设置画布颜色
            canvas.Clear(SKColors.White);

            // 配置画笔
            using var pen = new SKPaint
            {
                FakeBoldText = true,
                Style = SKPaintStyle.Fill,
                TextSize = 20,
            };

            // 绘制随机字符
            for (int i = 0; i < code.Length; i++)
            {
                // 配置随机颜色
                pen.Color = randomizer.GetRandom(_colors);
                // 配置随机字体
                pen.Typeface = SKTypeface.FromFamilyName(randomizer.GetRandom(_fonts), 700, 20, SKFontStyleSlant.Italic);
                var point = new SKPoint()
                {
                    X = i * 16,
                    Y = 22

                };
                canvas.DrawText(code.Substring(i, 1), point, pen);
            }

            // 绘制噪点
            var points = Enumerable.Range(0, 100)
                .Select(_ => new SKPoint(randomizer.Next(bitmap.Width), randomizer.Next(bitmap.Height)))
                .ToArray();
            canvas.DrawPoints(SKPointMode.Points, points, pen);

            // 绘制贝塞尔线条
            for (int i = 0; i < 2; i++)
            {
                var p1 = new SKPoint(0, 0);
                var p2 = new SKPoint(0, 0);
                var p3 = new SKPoint(0, 0);
                var p4 = new SKPoint(0, 0);

                var touchPoints = new SKPoint[] { p1, p2, p3, p4 };

                using var bPen = new SKPaint();
                bPen.Color = randomizer.GetRandom(_colors);
                bPen.Style = SKPaintStyle.Stroke;

                using var path = new SKPath();
                path.MoveTo(touchPoints[0]);
                path.CubicTo(touchPoints[1], touchPoints[2], touchPoints[3]);
                canvas.DrawPath(path, bPen);
            }

            return bitmap.ToBase64String(SKEncodedImageFormat.Png);
        }

        /// <summary>
        /// SKBitmap 转 Base64String
        /// </summary>
        /// <param name="bitmap">要转换的位图</param>
        /// <param name="format">图片文件格式</param>
        /// <returns></returns>
        public static string ToBase64String(this SKBitmap bitmap, SKEncodedImageFormat format)
        {
            using var memStream = new MemoryStream();
            using var wstream = new SKManagedWStream(memStream);
            bitmap.Encode(wstream, format, 32);
            memStream.TryGetBuffer(out ArraySegment<byte> buffer);
            return Convert.ToBase64String(buffer.Array, 0, (int)memStream.Length);
        }

        /// <summary>
        /// 获取随机文本，相邻字符不会相同
        /// </summary>
        /// <param name="textLength">文本长度</param>
        /// <returns></returns>
        public static string RandomText(int textLength = 4)
        {
            var code = string.Empty;
            var lastRandomIndex = -1;
            for (int i = 1; i <= textLength; i++)
            {
                int randomIndex = new Random(Guid.NewGuid().GetHashCode()).Next(_chars.Length);
                // 相邻字符相同，递归重新获取
                if (lastRandomIndex == randomIndex)
                {
                    return RandomText();
                }
                lastRandomIndex = randomIndex;
                code += _chars[randomIndex];
            }
            return code;
        }

        /// <summary>
        /// 随机挑选数组中的一个元素
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="randomizer">随机器</param>
        /// <param name="items">元素数组</param>
        /// <returns></returns>
        public static T GetRandom<T>(this Random randomizer, T[] items)
            => items[(randomizer ?? new Random()).Next(items.Length)];
    }
}