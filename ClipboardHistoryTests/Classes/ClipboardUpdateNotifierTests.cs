using System;
using System.Collections.Generic;
using System.Text;
using ClipboardHistoryApp.Classes;
using NUnit.Framework;
using System.Reflection;
using System.Windows.Forms;


namespace ClipboardHistoryTests.Classes
{
	#region Constructors
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
			Assert.IsInstanceOf(typeof(ClipboardUpdateNotifier), notifier);
		}
	} 
	#endregion


	#region Fields
	public class ClipboardUpdateNotifier_Fields_Tests
	{
		private void ClipboardUpdateEventHandler(object sender, EventArgs e) { /* Dummy EventHandler */ }

		[Test]
		public void When_Creating_Object_With_Valid_EventHandler_ClipboardUpdate_Field_Should_Be_This_EventHandler()
		{
			// Prepare
			EventHandler handler = new EventHandler(ClipboardUpdateEventHandler);

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
			EventHandler handler = new EventHandler(ClipboardUpdateEventHandler);

			// Act
			ClipboardUpdateNotifier notifier = new ClipboardUpdateNotifier(handler);

			// Assert
			FieldInfo info = typeof(ClipboardUpdateNotifier).GetField("_form", BindingFlags.Instance | BindingFlags.NonPublic);
			Assert.IsNotNull(info.GetValue(notifier));
			Assert.IsTrue(info.GetValue(notifier).GetType().IsSubclassOf(typeof(Form)));
		}
	} 
	#endregion


	#region OnClipboardUpdate Event Handler
	[TestFixture]
	public class ClipboardUpdateNotifier_OnClipboardUpdate_Tests
	{
		private bool _eventHandlerCalled = false;

		private void ClipboardUpdateEventHandler(object sender, EventArgs e)
		{
			this._eventHandlerCalled = true;
		}

		[Test]
		public void When_Called_With_Valid_EventHandler_Should_Call_EventHandler_Delegate()
		{
			// Prepare
			EventHandler handler = new EventHandler(ClipboardUpdateEventHandler);
			ClipboardUpdateNotifier notifier = new ClipboardUpdateNotifier(handler);
			this._eventHandlerCalled = false;
			
			// Act
			UnitTestHelper.RunInstanceMethod(typeof(ClipboardUpdateNotifier), "OnClipboardUpdate",
											 notifier, new object[1] { new ClipboardEventArgs() });

			// Assert
			Assert.IsTrue(this._eventHandlerCalled);
		}
	} 
	#endregion
}
