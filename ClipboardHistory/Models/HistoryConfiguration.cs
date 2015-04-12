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
        #endregion Fields

        
        #region Properties
        public int CollectionCapacity
        {
            get { return _collectionCapacity; }
            set
            {
                if (_collectionCapacity != value && Configuration.SaveProperty("CollectionCapacity", value))
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
                if (_snippetNumLines != value && Configuration.SaveProperty("SnippetNumLines", value))
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
                if (_toolTipHoverDelay != value && Configuration.SaveProperty("ToolTipHoverDelay", value))
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
                if (_visualStudioClipboardOnly != value &&
                    Configuration.SaveProperty("VisualStudioClipboardOnly", value))
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
                if (_preventDuplicateItems != value && Configuration.SaveProperty("PreventDuplicateItems", value))
                {
                    _preventDuplicateItems = value;
                    RaisePropertyChanged(() => PreventDuplicateItems);
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