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

		public HistoryCollection(int capacity)
		{
			this._capacity = capacity;
		}

		protected override void InsertItem(int index, ClipboardDataItem item)
		{
			MaintainHistoryListCapacity(this._capacity);
			base.InsertItem(index, item);
		}

		private void MaintainHistoryListCapacity(int capacity)
		{
			int count = base.Count;
			if (count == capacity)
			{
				base.RemoveAt(count - 1);
			}
		}
	}
}
