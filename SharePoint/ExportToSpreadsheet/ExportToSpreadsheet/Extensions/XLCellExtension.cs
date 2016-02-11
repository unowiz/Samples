using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ExportToSpreadsheet {

    public static class XLCellExtension {

        public static IXLCell SetHtmlValue(this IXLCell target, string html) {
            var xmlRoot = XElement.Parse(html);
            target.SetValue(xmlRoot.Value);
            var rootText = Regex.Replace(xmlRoot.ToString(), "\\r\\n", "");
            var spanIndex = 0;
            foreach (var item in xmlRoot.Descendants("span")) {
                var itemText = item.ToString();
                var rawStart = rootText.IndexOf(itemText, spanIndex);
                var rawLength = itemText.Length;
                var trimStart = Regex.Replace(rootText.Substring(0, rawStart), "<.+?>", "").Length - 1;
                var trimLength = item.Value.Length;
                var richText = target.RichText.Substring(trimStart, trimLength);
                richText.SetCssStyle(item);
                spanIndex = rawStart + rawLength;
            }
            var strongIndex = 0;
            foreach (var item in xmlRoot.Descendants("strong")) {
                var itemText = item.ToString();
                var rawStart = rootText.IndexOf(itemText, strongIndex);
                var rawLength = itemText.Length;
                var trimStart = Regex.Replace(rootText.Substring(0, rawStart), "<.+?>", "").Length - 1;
                var trimLength = item.Value.Length;
                var richText = target.RichText.Substring(trimStart, trimLength);
                richText.SetBold();
                richText.SetCssStyle(item);
                strongIndex = rawStart + rawLength;
            }
            return target;
        }

        private static void SetCssStyle<T>(this IXLFormattedText<T> richText, XElement element) {
            var xmlStyle = element.Attribute("style");
            if (xmlStyle != null) {
                var cssStyles = xmlStyle.Value.Split(';').Select(str => {
                    var pair = str.Split(':');
                    pair[0] = pair[0].Trim();
                    pair[1] = pair[1].Trim();
                    return Tuple.Create(pair[0], pair[1]);
                });
                var cssColor = cssStyles.FirstOrDefault(pair => pair.Item1 == "color");
                if (cssColor != null) {
                    richText.SetFontColor(XLColor.FromHtml(cssColor.Item2));
                }
            }
        }

    }

}
