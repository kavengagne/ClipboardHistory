using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ClipboardHistory.Classes;
using System.Reflection;
using System.Windows.Forms;

namespace ClipboardHistoryTests.Classes
{
	internal class TestUtils
	{
		public static void ClipboardUpdateEventHandler(object sender, EventArgs e) { /* Dummy EventHandler */ }
	}


	[TestFixture]
	public class ClipboardUpdateNotifier_Constructors_Tests
	{
		[Test]
		public void When_Creating_Object_With_Null_EventHandler_Object_Should_Be_Valid_Instance()
		{
			// Prepare
			EventHandler handler = null;
			
			// Act
			ClipboardUpdateNotifier notifier = new ClipboardUpdateNotifier(handler);

			// Assert
			Assert.IsNotNull(notifier);
			Assert.IsInstanceOfType(typeof(ClipboardUpdateNotifier), notifier);
		}

		[Test]
		public void When_Creating_Object_With_Valid_EventHandler_ClipboardUpdate_Field_Should_Be_This_EventHandler()
		{
			// Prepare
			EventHandler handler = new EventHandler(TestUtils.ClipboardUpdateEventHandler);

			// Act
			ClipboardUpdateNotifier notifier = new ClipboardUpdateNotifier(handler);

			// Assert
			FieldInfo info = typeof(ClipboardUpdateNotifier).GetField("ClipboardUpdate", BindingFlags.Instance | BindingFlags.NonPublic);
			EventHandler instanceHandler = (EventHandler)info.GetValue(notifier);
			Assert.AreEqual(handler, instanceHandler);
		}

		[Test]
		public void When_Creating_Object_Form_Field_Should_Be_Not_Null_And_Subclass_Of_Form()
		{
			// Prepare
			EventHandler handler = new EventHandler(TestUtils.ClipboardUpdateEventHandler);

			// Act
			ClipboardUpdateNotifier notifier = new ClipboardUpdateNotifier(handler);

			// Assert
			FieldInfo info = typeof(ClipboardUpdateNotifier).GetField("_form", BindingFlags.Instance | BindingFlags.NonPublic);
			Assert.IsNotNull(info.GetValue(notifier));
			Assert.IsTrue(info.GetValue(notifier).GetType().IsSubclassOf(typeof(Form)));
		}
	}
}
