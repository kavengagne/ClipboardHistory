using System.Linq;
using ClipboardHistoryApp.Classes;
using NUnit.Framework;

// Resharper disable All
namespace ClipboardHistoryTests.Classes
{	
    #region MaintainHistoryCollectionCapacity Method
    [TestFixture]
    public class HistoryCollection_MaintainHistoryCollectionCapacity_Tests
    {
        [Test]
        public void When_Adding_Element_To_HistoryCollection_Count_Should_Never_Be_Higher_Than_Capacity()
        {
            // Prepare
            int savedCapacity = Configuration.HistoryCollectionCapacity;
            Configuration.SaveHistoryCollectionCapacity(5);
            var historyCollection = new HistoryCollection();

            // Act
            historyCollection.AddItem(new ClipboardDataItem("Line 7"));
            historyCollection.AddItem(new ClipboardDataItem("Line 6"));
            historyCollection.AddItem(new ClipboardDataItem("Line 5"));
            historyCollection.AddItem(new ClipboardDataItem("Line 4"));
            historyCollection.AddItem(new ClipboardDataItem("Line 3"));
            historyCollection.AddItem(new ClipboardDataItem("Line 2"));
            historyCollection.AddItem(new ClipboardDataItem("Line 1"));

            // Assert
            Assert.AreEqual(Configuration.HistoryCollectionCapacity, historyCollection.Count);
            Configuration.SaveHistoryCollectionCapacity(savedCapacity);
        }

        [Test]
        public void When_Adding_Element_To_A_Full_HistoryCollection_Older_Element_Should_Be_Removed()
        {
            // Prepare
            int savedCapacity = Configuration.HistoryCollectionCapacity;
            Configuration.SaveHistoryCollectionCapacity(5);
            var historyCollection = new HistoryCollection();

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
            Assert.AreEqual("Line 5", lastItem.Data);
            Configuration.SaveHistoryCollectionCapacity(savedCapacity);
        }
    }
    #endregion
}
