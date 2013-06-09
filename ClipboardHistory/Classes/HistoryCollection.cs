using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ClipboardHistory.Classes
{
	public class HistoryCollection : ObservableCollection<ClipboardDataItem>
	{
		private int _capacity;

		public HistoryCollection(int capacity) : base()
		{
			if (capacity < 1)
			{
				throw new ArgumentException("Capacity must be greater than zero (0)", "capacity");
			}
			else
			{
				this._capacity = capacity;
			}
		}

		
		public void AddItem(ClipboardDataItem item)
		{
			MaintainHistoryCollectionCapacity(this._capacity);
			base.InsertItem(0, item);
		}

		private void MaintainHistoryCollectionCapacity(int capacity)
		{
			int count = base.Count;
			if (count == capacity)
			{
				base.RemoveAt(count - 1);
			}
		}
	}
}
