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
            MaintainHistoryCollectionCapacity(Configuration.HistoryCollectionCapacity);
            base.InsertItem(0, item);
        }

        private void MaintainHistoryCollectionCapacity(int capacity)
        {
            int count = base.Count;
            while (count >= capacity)
            {
                base.RemoveAt(count - 1);
                count = base.Count;
            }
        } 
        #endregion
	}
}
