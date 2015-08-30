using System;
using System.Text.RegularExpressions;
using ClipboardHistoryApp.Classes;
using GalaSoft.MvvmLight;

namespace ClipboardHistoryApp.Models
{
    public class ClipboardDataItem : ObservableObject
    {
        #region Fields
        private string _dateAndTime;
        private string _numberOfLines;
        private string _dataSize;
        private string _data;
        private string _snippet;
        private bool _isErrorMessage;
        #endregion Fields


        #region Properties
        public string DateAndTime
        {
            get { return _dateAndTime; }
            set
            {
                if (_dateAndTime != value)
                {
                    _dateAndTime = value;
                    RaisePropertyChanged(() => DateAndTime);
                }
            }
        }

        public string NumberOfLines
        {
            get { return _numberOfLines; }
            set
            {
                if (_numberOfLines != value)
                {
                    _numberOfLines = value;
                    RaisePropertyChanged(() => NumberOfLines);
                }
            }
        }

        public string DataSize
        {
            get { return _dataSize; }
            set
            {
                if (_dataSize != value)
                {
                    _dataSize = value;
                    RaisePropertyChanged(() => DataSize);
                }
            }
        }

        public string Data
        {
            get { return _data; }
            set
            {
                _data = value;
                RaisePropertyChanged(() => Data);
                Snippet = ApplyClipboardFormat(StripToNumberOfLines(value, Configuration.SnippetNumLines));
                DataSize = GetDataSizeString(value);
                NumberOfLines = GetNumberOfLinesString(GetArrayOfLines(value).Length);
                DateAndTime = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss");
            }
        }

        public string Snippet
        {
            get { return _snippet; }
            set
            {
                if (_snippet != value)
                {
                    _snippet = value;
                    RaisePropertyChanged(() => Snippet);
                }
            }
        }

        public bool IsErrorMessage
        {
            get { return _isErrorMessage; }
            set
            {
                if (_isErrorMessage != value)
                {
                    _isErrorMessage = value;
                    RaisePropertyChanged(() => IsErrorMessage);
                }
            }
        }
        #endregion Properties


        #region Constructors
        public ClipboardDataItem(string data, bool isErrorMessage)
        {
            _data = data;
            _isErrorMessage = isErrorMessage;

            _snippet = ApplyClipboardFormat(StripToNumberOfLines(data, Configuration.SnippetNumLines));
            _dataSize = GetDataSizeString(data);
            _numberOfLines = GetNumberOfLinesString(GetArrayOfLines(data).Length);
            _dateAndTime = DateTime.Now.ToString("yyyy-MM-dd H:mm:ss");
        }
        public ClipboardDataItem(string data) : this(data, false) { }
        #endregion Constructors


        #region Public Methods
        public static string[] GetArrayOfLines(string text)
        {
            return Regex.Split(text, "\r\n|\r|\n");
        }
        #endregion Public Methods


        #region Private Methods
        private static string StripToNumberOfLines(string text, int numberOfLines)
        {
            string result = string.Empty;
            string[] lines = GetArrayOfLines(text);

            if (numberOfLines <= 0) { return ""; }

            if (lines.Length <= numberOfLines)
            {
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

        private static string ApplyClipboardFormat(string text)
        {
            var formatter = new ClipboardFormatter(text);
            return formatter.ToString();
        }

        private static string GetNumberOfLinesString(int numberOfLines)
        {
            return numberOfLines + " line" + ((numberOfLines != 1) ? "s" : "");
        }

        private static string GetDataSizeString(string text)
        {
            var size = (text.Length * sizeof(Char)) / 1024f;
            return size.ToString("0.000") + " kb";
        }
        #endregion Private Methods
    }
}
