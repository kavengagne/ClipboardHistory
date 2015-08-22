using ClipboardHistoryApp.Classes;
using GalaSoft.MvvmLight;

namespace ClipboardHistoryApp.Models
{
    public class HistoryConfiguration : ObservableObject
    {
        #region Fields
        private readonly HistoryCollection _historyCollection;

        private int _collectionCapacity = Configuration.CollectionCapacity;
        private int _snippetNumLines = Configuration.SnippetNumLines;
        private int _toolTipHoverDelay = Configuration.ToolTipHoverDelay;
        private bool _visualStudioClipboardOnly = Configuration.VisualStudioClipboardOnly;
        private bool _preventDuplicateItems = Configuration.PreventDuplicateItems;
        private bool _displayTimestamp = Configuration.DisplayTimestamp;
        #endregion Fields

        
        #region Properties
        public int CollectionCapacity
        {
            get { return _collectionCapacity; }
            set
            {
                if (_collectionCapacity != value && Configuration.SaveProperty(() => Configuration.CollectionCapacity, value))
                {
                    _collectionCapacity = value;
                    RaisePropertyChanged(() => CollectionCapacity);
                    _historyCollection.Refresh();
                }
            }
        }

        public int SnippetNumLines
        {
            get { return _snippetNumLines; }
            set
            {
                if (_snippetNumLines != value && Configuration.SaveProperty(() => Configuration.SnippetNumLines, value))
                {
                    _snippetNumLines = value;
                    RaisePropertyChanged(() => SnippetNumLines);
                    _historyCollection.Refresh();
                }
            }
        }

        public int ToolTipHoverDelay
        {
            get { return _toolTipHoverDelay; }
            set
            {
                if (_toolTipHoverDelay != value && Configuration.SaveProperty(() => Configuration.ToolTipHoverDelay, value))
                {
                    _toolTipHoverDelay = value;
                    RaisePropertyChanged(() => ToolTipHoverDelay);
                }
            }
        }

        public bool VisualStudioClipboardOnly
        {
            get { return _visualStudioClipboardOnly; }
            set
            {
                if (_visualStudioClipboardOnly != value
                    && Configuration.SaveProperty(() => Configuration.VisualStudioClipboardOnly, value))
                {
                    _visualStudioClipboardOnly = value;
                    RaisePropertyChanged(() => VisualStudioClipboardOnly);
                }
            }
        }

        public bool PreventDuplicateItems
        {
            get { return _preventDuplicateItems; }
            set
            {
                if (_preventDuplicateItems != value && Configuration.SaveProperty(() => Configuration.PreventDuplicateItems, value))
                {
                    _preventDuplicateItems = value;
                    RaisePropertyChanged(() => PreventDuplicateItems);
                }
            }
        }

        public bool DisplayTimestamp
        {
            get { return _displayTimestamp; }
            set
            {
                if (_displayTimestamp != value && Configuration.SaveProperty(() => Configuration.DisplayTimestamp, value))
                {
                    _displayTimestamp = value;
                    RaisePropertyChanged(() => DisplayTimestamp);
                }
            }
        }
        #endregion Properties


        #region Constructors
        public HistoryConfiguration(HistoryCollection historyCollection)
        {
            _historyCollection = historyCollection;
        }
        #endregion Constructors
    }
}