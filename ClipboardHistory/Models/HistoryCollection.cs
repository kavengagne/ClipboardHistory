using System.Collections.ObjectModel;
using System.Linq;
using ClipboardHistoryApp.Classes;

namespace ClipboardHistoryApp.Models
{
    public class HistoryCollection : ObservableCollection<ClipboardDataItem>
    {
        #region Public Methods
        public void AddItem(ClipboardDataItem item)
        {
            bool canInsertItem = true;
            if (Configuration.PreventDuplicateItems)
            {
                canInsertItem = !Items.Take(1).Any(i => i.Data.Equals(item.Data));
            }
            if (canInsertItem)
            {
                InsertItem(0, item);
                MaintainHistoryCollectionCapacity(Configuration.CollectionCapacity);
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
            foreach (var item in Items)
            {
                var date = item.DateAndTime;
                item.Data = item.Data;
                item.DateAndTime = date;
            }
            MaintainHistoryCollectionCapacity(Configuration.CollectionCapacity);
        }
        #endregion
    }
}
