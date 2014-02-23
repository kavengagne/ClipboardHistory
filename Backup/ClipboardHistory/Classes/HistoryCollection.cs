using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ClipboardHistory.Classes
{
	public class HistoryCollection : ObservableCollection<ClipboardDataItem>
	{
        #region Fields
        #endregion


        #region Constructor
        public HistoryCollection() : base()
        {
        }
        #endregion


        #region Methods
        public void AddItem(ClipboardDataItem item)
        {
            bool canInsertItem = true;
            if (Configuration.PreventDuplicateItems)
            {
                canInsertItem = !base.Items.Take(1).Any(i => i.CopyDataFull.Equals(item.CopyDataFull));
            }
            if (canInsertItem)
            {
                base.InsertItem(0, item);
                MaintainHistoryCollectionCapacity(Configuration.HistoryCollectionCapacity);
            }
        }

        public void MaintainHistoryCollectionCapacity(int capacity)
        {
            int count = base.Count;
            while (count > capacity)
            {
                base.RemoveAt(count - 1);
                count = base.Count;
            }
        } 

        public void Refresh()
        {
            foreach (ClipboardDataItem item in base.Items)
            {
                var date = item.DateAndTime;
                item.CopyDataFull = item.CopyDataFull;
                item.DateAndTime = date;
            }
            MaintainHistoryCollectionCapacity(Configuration.HistoryCollectionCapacity);
        }
        #endregion
	}
}
