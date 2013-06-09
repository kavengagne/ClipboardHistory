using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ClipboardHistory.Classes;
using System.Reflection;

namespace ClipboardHistoryTests.Classes
{	
	#region Constructors
	[TestFixture]
	public class HistoryCollection_Constructors_Tests
	{
		[Test, ExpectedException(exceptionType: typeof(ArgumentException))]
		public void When_Creating_Object_With_Negative_Capacity_Object_Should_Throw_ArgumentException()
		{
			// Prepare
			int inputCapacity = -1;

			// Act
			HistoryCollection historyCollection = new HistoryCollection(inputCapacity);
		}

		[Test]
		public void When_Creating_Object_With_Negative_Capacity_Object_Should_Be_Null()
		{
			// Prepare
			int inputCapacity = -1;

			// Act
			HistoryCollection historyCollection = null;
			try {
				historyCollection = new HistoryCollection(inputCapacity);
			}
			catch (Exception) { }

			// Assert
			Assert.IsNull(historyCollection);
		}

		[Test]
		public void When_Creating_Object_With_Valid_Capacity_Capacity_Field_Should_Be_Set_To_Input_Capacity()
		{
			// Prepare
			int inputCapacity = 5;

			// Act
			HistoryCollection historyCollection = new HistoryCollection(inputCapacity);

			// Assert
			FieldInfo info = typeof(HistoryCollection).GetField("_capacity", BindingFlags.Instance | BindingFlags.NonPublic);
			Assert.IsNotNull(info);
			Assert.AreEqual(inputCapacity, (int)info.GetValue(historyCollection));
		}
	}
	#endregion


	#region MaintainHistoryCollectionCapacity Method
	[TestFixture]
	public class HistoryCollection_MaintainHistoryCollectionCapacity_Tests
	{
		[Test]
		public void When_Adding_Element_To_HistoryCollection_Count_Should_Never_Be_Higher_Than_Capacity()
		{
			// Prepare
			int capacity = 5;
			HistoryCollection historyCollection = new HistoryCollection(capacity);

			// Act
			historyCollection.AddItem(new ClipboardDataItem("Line 7"));
			historyCollection.AddItem(new ClipboardDataItem("Line 6"));
			historyCollection.AddItem(new ClipboardDataItem("Line 5"));
			historyCollection.AddItem(new ClipboardDataItem("Line 4"));
			historyCollection.AddItem(new ClipboardDataItem("Line 3"));
			historyCollection.AddItem(new ClipboardDataItem("Line 2"));
			historyCollection.AddItem(new ClipboardDataItem("Line 1"));

			// Assert
			Assert.AreEqual(capacity, historyCollection.Count);
		}

		[Test]
		public void When_Adding_Element_To_A_Full_HistoryCollection_Older_Element_Should_Be_Removed()
		{
			// Prepare
			int capacity = 5;
			HistoryCollection historyCollection = new HistoryCollection(capacity);

			// Act
			historyCollection.AddItem(new ClipboardDataItem("Line 7"));
			historyCollection.AddItem(new ClipboardDataItem("Line 6"));
			historyCollection.AddItem(new ClipboardDataItem("Line 5"));
			historyCollection.AddItem(new ClipboardDataItem("Line 4"));
			historyCollection.AddItem(new ClipboardDataItem("Line 3"));
			historyCollection.AddItem(new ClipboardDataItem("Line 2"));
			historyCollection.AddItem(new ClipboardDataItem("Line 1"));

			// Assert
			int count = historyCollection.Count;
			ClipboardDataItem lastItem = historyCollection[count - 1];
			Assert.AreEqual("Line 5", lastItem.CopyDataFull);
		}
	}
	#endregion
}
