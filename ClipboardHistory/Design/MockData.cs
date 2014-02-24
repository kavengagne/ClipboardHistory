using System;
using System.Linq;
using ClipboardHistoryApp.Classes;

namespace ClipboardHistoryApp.Design
{
    public class MockData
    {
        private readonly HistoryCollection _historyCollection;

        public HistoryCollection HistoryCollection
        {
            get { return this._historyCollection; }
        }

        public string CopyDataShort
        {
            get { return null; }
        }

        public string IsErrorMessage
        {
            get { return null; }
        }

        public string DateAndTime
        {
            get { return null; }
        }

        public string NumberOfLines
        {
            get { return null; }
        }

        public string CopyDataSize
        {
            get { return null; }
        }

        public string CopyDataFull
        {
            get { return null; }
        }

        public MockData()
        {
            _historyCollection = new HistoryCollection();
            _historyCollection.AddItem(new ClipboardDataItem(
                "    Sound and Signal Flow" + Environment.NewLine +
                "    (collapsed, click to expand) " + Environment.NewLine +
                "    Lesson 2: The DAW" + Environment.NewLine +
                "    (collapsed, click to expand)" + Environment.NewLine +
                "    Lesson 3: The Mixer" + Environment.NewLine +
                "    (collapsed, click to expand)" + Environment.NewLine +
                "    Lesson 4: Dynamic Effects"));
            _historyCollection.AddItem(new ClipboardDataItem(
                "    public HistoryCollection HistoryCollection" + Environment.NewLine +
                "    {" + Environment.NewLine +
                "        get { return this._historyCollection; }" + Environment.NewLine +
                "    }"));
            _historyCollection.AddItem(new ClipboardDataItem(
                "    bool canInsertItem = true;" + Environment.NewLine +
                "    if (Configuration.PreventDuplicateItems)" + Environment.NewLine +
                "    {" + Environment.NewLine +
                "        canInsertItem = !Items.Take(1).Any(i => i.CopyDataFull.Equals(item.CopyDataFull));" + Environment.NewLine +
                "    }" + Environment.NewLine +
                "    if (canInsertItem)" + Environment.NewLine +
                "    {" + Environment.NewLine +
                "        base.InsertItem(0, item);" + Environment.NewLine +
                "        MaintainHistoryCollectionCapacity(Configuration.HistoryCollectionCapacity);" + Environment.NewLine +
                "    }"));
            _historyCollection.AddItem(new ClipboardDataItem(
                "dwdw"));
            _historyCollection.AddItem(new ClipboardDataItem(
                "222"));
        }
    }
}
