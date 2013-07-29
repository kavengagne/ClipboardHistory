using System;
using System.Collections.Generic;
using System.Text;
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
            base.InsertItem(0, item);
            MaintainHistoryCollectionCapacity(Configuration.HistoryCollectionCapacity);
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
                item.CopyDataFull = item.CopyDataFull;
            }
            MaintainHistoryCollectionCapacity(Configuration.HistoryCollectionCapacity);
        }
        #endregion
	}
}
