using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace ClipboardHistoryApp.Classes
{
    public class ClipboardDataItem : DependencyObject
    {
        #region Properties
        public string DateAndTime
        {
            get { return (string)GetValue(DateAndTimeProperty); }
            set { SetValue(DateAndTimeProperty, value); }
        }
        public static readonly DependencyProperty DateAndTimeProperty =
            DependencyProperty.Register("DateAndTime", typeof(string), typeof(ClipboardDataItem), new UIPropertyMetadata(""));

        public string NumberOfLines
        {
            get { return (string)GetValue(NumberOfLinesProperty); }
            private set
            {
                SetValue(NumberOfLinesProperty, value);
            }
        }
        public static readonly DependencyProperty NumberOfLinesProperty =
            DependencyProperty.Register("NumberOfLines", typeof(string), typeof(ClipboardDataItem), new UIPropertyMetadata(""));

        public string CopyDataSize
        {
            get { return (string)GetValue(CopyDataSizeProperty); }
            private set
            {
                SetValue(CopyDataSizeProperty, value);
            }
        }
        public static readonly DependencyProperty CopyDataSizeProperty =
            DependencyProperty.Register("CopyDataSize", typeof(string), typeof(ClipboardDataItem), new UIPropertyMetadata(""));

        public string CopyDataFull
        {
            get { return (string)GetValue(CopyDataFullProperty); }
            set
            {
                SetValue(CopyDataFullProperty, value);
                this.CopyDataShort = ApplyClipboardFormat(StripToNumberOfLines(value, Configuration.CopyDataShortNumLines));
                this.CopyDataSize = GetCopyDataSizeString(value);
                this.NumberOfLines = GetNumberOfLinesString(GetArrayOfLines(value).Length);
                this.DateAndTime = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss");
            }
        }

        public static readonly DependencyProperty CopyDataFullProperty =
            DependencyProperty.Register("CopyDataFull", typeof(string), typeof(ClipboardDataItem), new UIPropertyMetadata(""));

        public string CopyDataShort
        {
            get { return (string)GetValue(CopyDataShortProperty); }
            private set { SetValue(CopyDataShortProperty, value); }
        }
        public static readonly DependencyProperty CopyDataShortProperty =
            DependencyProperty.Register("CopyDataShort", typeof(string), typeof(ClipboardDataItem), new UIPropertyMetadata(""));

        public bool IsErrorMessage
        {
            get { return (bool)GetValue(IsErrorMessageProperty); }
            private set { SetValue(IsErrorMessageProperty, value); }
        }
        public static readonly DependencyProperty IsErrorMessageProperty =
            DependencyProperty.Register("IsErrorMessage", typeof(bool), typeof(ClipboardDataItem), new UIPropertyMetadata(false));
        #endregion


        #region Constructors
        public ClipboardDataItem(string copyData, bool isErrorMessage)
        {
            this.CopyDataFull = copyData;
            this.IsErrorMessage = isErrorMessage;
        }
        public ClipboardDataItem(string copyData) : this(copyData, false) { }
        #endregion


        #region Methods
        private static string StripToNumberOfLines(string text, int numberOfLines)
        {
            string result = string.Empty;
            string[] lines = GetArrayOfLines(text);

            if (numberOfLines <= 0) { return ""; }

            if (lines.Length <= numberOfLines) {
                numberOfLines = lines.Length;
            }

            for (int i = 0; i < numberOfLines; i++)
            {
                result += lines[i];
                if ((numberOfLines - 1) != i)
                {
                    result += Environment.NewLine;
                }			
            }
            return result;
        }

        public static string[] GetArrayOfLines(string text)
        {
            return Regex.Split(text, "\r\n|\r|\n");
        }

        private static string ApplyClipboardFormat(string text)
        {
            var formatter = new ClipboardFormatter(text);
            return formatter.ToString();
        }

        private static string GetNumberOfLinesString(int numberOfLines)
        {
            return numberOfLines + " line" + ((numberOfLines != 1) ? "s" : "");
        }

        private static string GetCopyDataSizeString(string text)
        {
            var size = (text.Length * sizeof(Char)) / 1024f;
            return size.ToString("0.000") + " kb";
        }
        #endregion
    }
}
