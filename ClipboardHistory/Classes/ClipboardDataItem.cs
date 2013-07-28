using System.Text.RegularExpressions;
using System.Windows;
using System;

namespace ClipboardHistory.Classes
{
	public class ClipboardDataItem : DependencyObject
	{
 		#region Properties
        public string DateAndTime
        {
            get { return (string)GetValue(DateAndTimeProperty); }
            private set { SetValue(DateAndTimeProperty, value); }
        }
        public static readonly DependencyProperty DateAndTimeProperty =
            DependencyProperty.Register("DateAndTime", typeof(string), typeof(ClipboardDataItem), new UIPropertyMetadata(""));

        public string NumberOfLines
        {
            get { return (string)GetValue(NumberOfLinesProperty); }
            private set {
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
			set {
				SetValue(CopyDataFullProperty, value);
				this.CopyDataShort = GetStringStrippedToNumberOfLines(value, Configuration.CopyDataShortNumLines);
                this.CopyDataSize = GetCopyDataSizeString(value);
                this.NumberOfLines = GetNumberOfLinesString(GetStringLines(value).Length);
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
		private static string GetStringStrippedToNumberOfLines(string value, int numberOfLines)
		{
			string data = string.Empty;
			string[] lines = GetStringLines(value);

			if (numberOfLines <= 0) { return ""; }
			if (lines.Length <= numberOfLines) { return value; }

			for (int i = 0; i < numberOfLines; i++)
			{
				data += lines[i];
				if ((numberOfLines - 1) != i)
				{
					data += System.Environment.NewLine;
				}			
			}
			return data;
		}

		private static string[] GetStringLines(string value)
		{
			string[] lines = Regex.Split(value, "\r\n|\r|\n");
			return lines;
		}

        private string GetNumberOfLinesString(int numLines)
        {
            string result = string.Empty;
            result = numLines + " line" + ((numLines != 1) ? "s" : "");
            return result;
        }

        private string GetCopyDataSizeString(string value)
        {
            string result = string.Empty;
            var size = (value.Length * sizeof(Char)) / 1024f;
            result = size.ToString("0.000") + " kb";
            return result;
        }
		#endregion
	}
}
