using System.Collections.ObjectModel;
using System.Linq;

namespace ClipboardHistoryApp.Classes
{
    public class HistoryCollection : ObservableCollection<ClipboardDataItem>
    {
        #region Public Methods
        public void AddItem(ClipboardDataItem item)
        {
            bool canInsertItem = true;
            if (Configuration.PreventDuplicateItems)
            {
                canInsertItem = !Items.Take(1).Any(i => i.CopyDataFull.Equals(item.CopyDataFull));
            }
            if (canInsertItem)
            {
                base.InsertItem(0, item);
                MaintainHistoryCollectionCapacity(Configuration.HistoryCollectionCapacity);
            }
        }

        public void MaintainHistoryCollectionCapacity(int capacity)
        {
            int count = Count;
            while (count > capacity)
            {
                RemoveAt(count - 1);
                count = Count;
            }
        } 

        public void Refresh()
        {
            foreach (ClipboardDataItem item in Items)
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
