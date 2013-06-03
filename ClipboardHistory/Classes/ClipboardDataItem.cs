using System.Text.RegularExpressions;
using System.Windows;

namespace ClipboardHistory.Classes
{
	public class ClipboardDataItem : DependencyObject
	{
		// TODO: Migrate this to Visual Studio Options Panel
		private const int COPYDATA_SHORT_NUM_LINES = 5;


		#region Properties
		public string CopyDataFull
		{
			get { return (string)GetValue(CopyDataFullProperty); }
			set {
				SetValue(CopyDataFullProperty, value);
				CopyDataShort = GetStringStrippedToNumberOfLines(value, COPYDATA_SHORT_NUM_LINES);
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
		public ClipboardDataItem(string CopyData, bool isErrorMessage)
		{
			CopyDataFull = CopyData;
			IsErrorMessage = isErrorMessage;
		}
		public ClipboardDataItem(string CopyData) : this(CopyData, false) { }
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
			data += string.Format("{0}... [{1} lines] ...", System.Environment.NewLine, lines.Length);
			return data;
		}

		private static string[] GetStringLines(string value)
		{
			string[] lines = Regex.Split(value, "\r\n|\r|\n");
			return lines;
		}
		#endregion
	}
}
