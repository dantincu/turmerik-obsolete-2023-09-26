using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turmerik.RegexH;

namespace Turmerik.Text.MdH
{
    public interface ITabsToMdTableConverter
    {
        string[] LinesToMdTable(
            string[] linesArr,
            bool firstLineIsHeader = false);

        string LinesToMdTable(
            string linesStr,
            bool firstLineIsHeader = false);

        string LineToMdTable(string line);
    }

    public class TabsToMdTableConverter : ITabsToMdTableConverter
    {
        private ITextReplacerComponent textReplacer;

        public TabsToMdTableConverter(
            ITextReplacerComponent textReplacer)
        {
            this.textReplacer = textReplacer ?? throw new ArgumentNullException(nameof(textReplacer));
        }

        public string[] LinesToMdTable(
            string[] linesArr,
            bool firstLineIsHeader = false)
        {
            var linesList = linesArr.Select(LineToMdTable).ToList();

            if (firstLineIsHeader && linesArr.Any())
            {
                string[] headerCellsArr = linesArr.First().Split('\t');

                string[] headerDelimCells = headerCellsArr.Select(
                    cell => "--").ToArray();

                string headerDelimLine = string.Join(" | ", headerDelimCells);
                headerDelimLine = $" | {headerDelimLine} |  ";

                linesList.Insert(1, headerDelimLine);
            }

            return linesList.ToArray();
        }

        public string LinesToMdTable(
            string linesStr,
            bool firstLineIsHeader = false)
        {
            string[] linesArr = linesStr.Split('\n');

            linesArr = LinesToMdTable(
                linesArr,
                firstLineIsHeader);

            linesStr = string.Join("\n", linesArr);
            return linesStr;
        }

        public string LineToMdTable(string line)
        {
            string[] lineParts = line.TrimEnd('\r').Split('\t');
            line = string.Join(" | ", lineParts);

            line = $" | {line} |  ";
            return line;
        }
    }
}
