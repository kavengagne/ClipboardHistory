using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ClipboardHistoryApp.Classes
{
    public class ClipboardFormatter
    {
        #region Constants
        public const char Tab = '\t';
        public const char Space = '\u0020';
        public const char DefaultCharacter = Space;
        public const int DefaultNumber = 4; 
        #endregion


        #region Fields
        private char _indentationCharacter = DefaultCharacter;
        private int _indentationNumber = DefaultNumber;
        #endregion


        #region Properties
        public string Text { get; set; }

        public char IndentationCharacter
        {
            get { return _indentationCharacter; }
            set { _indentationCharacter = value; }
        }
        
        public int IndentationNumber
        {
            get { return _indentationNumber; }
            set { _indentationNumber = (value > 0) ? value : DefaultNumber; }
        }
        #endregion


        #region Constructors
        public ClipboardFormatter(string text) : this(text, DefaultCharacter, DefaultNumber)
        {
        }
        
        public ClipboardFormatter(string text, char indentationCharacter, int indentationNumber)
        {
            this.Text = text;
            this.IndentationCharacter = indentationCharacter;
            this.IndentationNumber = indentationNumber;
        } 
        #endregion


        #region ToString Overrides
        public override string ToString()
        {
            Dictionary<int, int> indentationLevels = GetIndentationLevels(this.Text);
            indentationLevels = NormalizeIndentationLevels(indentationLevels);
            string unindentedText = RemoveIndentation(this.Text);
            return ApplyIndentation(unindentedText, indentationLevels);
        }
        #endregion


        #region Methods
        private string ApplyIndentation(string text, Dictionary<int, int> levels)
        {
            string result = string.Empty;
            string[] lines = ClipboardDataItem.GetArrayOfLines(text);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                int padSize = (levels.ContainsKey(i)) ? line.Length + (levels[i] * this.IndentationNumber) : 0;
                result += line.PadLeft(padSize, this.IndentationCharacter);
                if ((lines.Length - 1) != i)
                {
                    result += Environment.NewLine;
                }
            }
            return result;
        }

        private static string RemoveIndentation(string text)
        {
            var trimmedLines = ClipboardDataItem.GetArrayOfLines(text).Select(line => line.TrimStart());
            return string.Join(Environment.NewLine, trimmedLines);
        }

        private static Dictionary<int, int> NormalizeIndentationLevels(Dictionary<int, int> levels)
        {
            int lowestLevel = FindLowestIndentationLevel(levels);
            var newLevels = new Dictionary<int, int>();
            foreach (var level in levels)
            {
                int diff = level.Value - lowestLevel;
                newLevels.Add(level.Key, (diff >= 0) ? diff : 0);
            }
            return newLevels;
        }

        private static Dictionary<int, int> GetIndentationLevels(string text)
        {
            var levels = new Dictionary<int, int>();
            string[] lines = ClipboardDataItem.GetArrayOfLines(text);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                // Matches everything from start of line to first non-whitespace char or end of line.
                Match match = Regex.Match(line, @"^(?<indent>([\s]{1,}))([\S]{1,}|$)");
                Group group = match.Groups["indent"];
                // Matches 1 tabulation or 4 spaces.
                MatchCollection matches = Regex.Matches(group.Value, @"([\t]{1}|[\u0020]{4})");
                if (line.Length > group.Value.Length)
                {
                    levels.Add(i, matches.Count);
                }
            }
            return levels;
        }

        private static int FindLowestIndentationLevel(Dictionary<int, int> levels)
        {
            var indentLevels = (levels.ContainsKey(0) && levels[0] == 0) ? levels.Skip(1) : levels;
            int lowestLevel = indentLevels.Aggregate(int.MaxValue, (current, level) => (level.Value < current) ? level.Value : current);
            return (lowestLevel == int.MaxValue) ? 0 : lowestLevel;
        } 
        #endregion
    }
}
