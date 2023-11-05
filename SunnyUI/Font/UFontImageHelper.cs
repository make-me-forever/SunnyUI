/******************************************************************************
 * SunnyUI ��Դ�ؼ��⡢������⡢��չ��⡢��ҳ�濪����ܡ�
 * CopyRight (C) 2012-2023 ShenYongHua(������).
 * QQȺ��56829229 QQ��17612584 EMail��SunnyUI@QQ.Com
 *
 * Blog:   https://www.cnblogs.com/yhuse
 * Gitee:  https://gitee.com/yhuse/SunnyUI
 * GitHub: https://github.com/yhuse/SunnyUI
 *
 * SunnyUI.dll can be used for free under the GPL-3.0 license.
 * If you use this code, please keep this note.
 * �����ʹ�ô˴��룬�뱣����˵����
 ******************************************************************************
 * �ļ�����: UFontImage.cs
 * �ļ�˵��: ����ͼƬ������
 * ��ǰ�汾: V3.1
 * ��������: 2020-01-01
 *
 * 2020-01-01: V2.2.0 �����ļ�˵��
 * 2020-05-21: V2.2.5 ��������Դ�ļ��м������壬�������Ϊ�ļ���
 *                    ��л����Ǳ� https://gitee.com/maikebing
 * 2021-06-15: V3.0.4 ����FontAwesomeV5������ͼ�꣬�ع�����
 * 2021-06-15: V3.3.5 ����FontAwesomeV6������ͼ�꣬�ع�����
 * 2023-05-16: V3.3.6 �ع�DrawFontImage����
 * 2022-05-17: V3.3.7 �޸���һ���������Ա༭��ͼ����ʾ��ȫ������
 * 2023-10-25: V3.5.1 MaterialIcons����ͼ�����3������
******************************************************************************/

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;

namespace Sunny.UI
{
    /// <summary>
    /// ����ͼƬ������
    /// </summary>
    public static class FontImageHelper
    {
        public static readonly Dictionary<UISymbolType, FontImages> Fonts = new Dictionary<UISymbolType, FontImages>();

        /// <summary>
        /// ���캯��
        /// </summary>
        static FontImageHelper()
        {
            Fonts.Add(UISymbolType.FontAwesomeV4, new FontImages(UISymbolType.FontAwesomeV4, ReadFontFileFromResource("Sunny.UI.Font.FontAwesome.ttf")));
            Fonts.Add(UISymbolType.ElegantIcons, new FontImages(UISymbolType.ElegantIcons, ReadFontFileFromResource("Sunny.UI.Font.ElegantIcons.ttf")));
            Fonts.Add(UISymbolType.FontAwesomeV6Brands, new FontImages(UISymbolType.FontAwesomeV6Brands, ReadFontFileFromResource("Sunny.UI.Font.fa-brands-400.ttf")));
            Fonts.Add(UISymbolType.FontAwesomeV6Regular, new FontImages(UISymbolType.FontAwesomeV6Regular, ReadFontFileFromResource("Sunny.UI.Font.fa-regular-400.ttf")));
            Fonts.Add(UISymbolType.FontAwesomeV6Solid, new FontImages(UISymbolType.FontAwesomeV6Solid, ReadFontFileFromResource("Sunny.UI.Font.fa-solid-900.ttf")));
            Fonts.Add(UISymbolType.MaterialIcons, new FontImages(UISymbolType.MaterialIcons, ReadFontFileFromResource("Sunny.UI.Font.MaterialIcons-Regular.ttf")));
        }

        private static byte[] ReadFontFileFromResource(string name)
        {
            byte[] buffer = null;
            Stream fontStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
            if (fontStream != null)
            {
                buffer = new byte[fontStream.Length];
                fontStream.Read(buffer, 0, (int)fontStream.Length);
                fontStream.Close();
            }

            return buffer;
        }

        /// <summary>
        /// ��ȡ�����С
        /// </summary>
        /// <param name="graphics">GDI��ͼ</param>
        /// <param name="symbol">�ַ�</param>
        /// <param name="symbolSize">��С</param>
        /// <returns>�����С</returns>
        internal static SizeF GetFontImageSize(this Graphics graphics, int symbol, int symbolSize)
        {
            Font font = GetFont(symbol, symbolSize);
            if (font == null)
            {
                return new SizeF(0, 0);
            }

            return graphics.MeasureString(char.ConvertFromUtf32(symbol), font);
        }

        private static UISymbolType GetSymbolType(int symbol)
        {
            return (UISymbolType)symbol.Div(100000);
        }

        private static int GetSymbolValue(int symbol)
        {
            return symbol.Mod(100000);
        }

        /// <summary>
        /// ��������ͼƬ
        /// </summary>
        /// <param name="graphics">GDI��ͼ</param>
        /// <param name="symbol">�ַ�</param>
        /// <param name="symbolSize">��С</param>
        /// <param name="color">��ɫ</param>
        /// <param name="rect">����</param>
        /// <param name="xOffset">����ƫ��</param>
        /// <param name="yOffSet">����ƫ��</param>
        public static void DrawFontImage(this Graphics graphics, int symbol, int symbolSize, Color color,
            RectangleF rect, int xOffset = 0, int yOffSet = 0, int angle = 0)
        {
            SizeF sf = graphics.GetFontImageSize(symbol, symbolSize);
            float left = rect.Left + ((rect.Width - sf.Width) / 2.0f).RoundEx();
            float top = rect.Top + ((rect.Height - sf.Height) / 2.0f).RoundEx();
            //graphics.DrawFontImage(symbol, symbolSize, color, left, top + 1, xOffset, yOffSet);

            // �ѻ����ԭ��(Ĭ�������Ͻ�)��λ�Ƶ���������
            graphics.TranslateTransform(left + sf.Width / 2, top + sf.Height / 2);
            // ��ת����
            graphics.RotateTransform(angle);
            // ���˻���x,y���ƶ����ľ���
            graphics.TranslateTransform(-(left + sf.Width / 2), -(top + sf.Height / 2));

            graphics.DrawFontImage(symbol, symbolSize, color, left, top, xOffset, yOffSet);

            graphics.TranslateTransform(left + sf.Width / 2, top + sf.Height / 2);
            graphics.RotateTransform(-angle);
            graphics.TranslateTransform(-(left + sf.Width / 2), -(top + sf.Height / 2));
        }

        /// <summary>
        /// ��������ͼƬ
        /// </summary>
        /// <param name="graphics">GDI��ͼ</param>
        /// <param name="symbol">�ַ�</param>
        /// <param name="symbolSize">��С</param>
        /// <param name="color">��ɫ</param>
        /// <param name="left">��</param>
        /// <param name="top">��</param>
        /// <param name="xOffset">����ƫ��</param>
        /// <param name="yOffSet">����ƫ��</param>
        private static void DrawFontImage(this Graphics graphics, int symbol, int symbolSize, Color color,
            float left, float top, int xOffset = 0, int yOffSet = 0)
        {
            Font font = GetFont(symbol, symbolSize);
            if (font == null) return;

            var symbolValue = GetSymbolValue(symbol);
            string text = char.ConvertFromUtf32(symbolValue);
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            graphics.DrawString(text, font, color, left + xOffset, top + yOffSet);
            graphics.TextRenderingHint = TextRenderingHint.SystemDefault;
            graphics.InterpolationMode = InterpolationMode.Default;
        }

        /// <summary>
        /// ����ͼƬ
        /// </summary>
        /// <param name="symbol">�ַ�</param>
        /// <param name="size">��С</param>
        /// <param name="color">��ɫ</param>
        /// <returns>ͼƬ</returns>
        public static Image CreateImage(int symbol, int size, Color color)
        {
            Bitmap image = new Bitmap(size, size);
            using Graphics g = image.Graphics();
            SizeF sf = g.GetFontImageSize(symbol, size);
            g.DrawFontImage(symbol, size, color, (image.Width - sf.Width) / 2.0f, (image.Height - sf.Height) / 2.0f);
            return image;
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="symbol">�ַ�</param>
        /// <param name="imageSize">��С</param>
        /// <returns>����</returns>
        public static Font GetFont(int symbol, int imageSize)
        {
            var symbolType = GetSymbolType(symbol);
            var symbolValue = GetSymbolValue(symbol);
            switch (symbolType)
            {
                case UISymbolType.FontAwesomeV4:
                    if (symbol > 0xF000)
                        return Fonts[UISymbolType.FontAwesomeV4].GetFont(symbolValue, imageSize);
                    else
                        return Fonts[UISymbolType.ElegantIcons].GetFont(symbolValue, imageSize);
                case UISymbolType.FontAwesomeV6Brands:
                    return Fonts[UISymbolType.FontAwesomeV6Brands].GetFont(symbolValue, imageSize);
                case UISymbolType.FontAwesomeV6Regular:
                    return Fonts[UISymbolType.FontAwesomeV6Regular].GetFont(symbolValue, imageSize);
                case UISymbolType.FontAwesomeV6Solid:
                    return Fonts[UISymbolType.FontAwesomeV6Solid].GetFont(symbolValue, imageSize);
                case UISymbolType.MaterialIcons:
                    return Fonts[UISymbolType.MaterialIcons].GetFont(symbolValue, imageSize, 3);
                default:
                    return null;
            }
        }
    }
}