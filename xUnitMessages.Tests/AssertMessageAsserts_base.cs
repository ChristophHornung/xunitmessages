namespace XunitMessages.Tests;

using System.ComponentModel;
using Xunit;
using Xunit.Sdk;

public partial class AssertMessageAsserts
{
	internal static void AssertWrapsCorrectly(Action t)
	{
		var exception = Assert.ThrowsAny<XunitException>(t.Invoke);
		Assert.StartsWith("Message", exception.Message);
	}

	internal static async Task AssertWrapsCorrectly(Func<Task> t)
	{
		var exception = await Assert.ThrowsAnyAsync<XunitException>(t.Invoke);

		Assert.StartsWith("Message", exception.Message);
	}

	internal class UnchangingObject : INotifyPropertyChanged
	{
#pragma warning disable CS0067
		public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore CS0067
	}

	internal class TestRaiser
	{
		public event Action? Event;
		public event EventHandler<EventArgs> EventHA;
		public event Action<object>? EventO;
		public event Action<EventArgs>? EventA;

		public void Raise()
		{
			this.Event?.Invoke();
			this.EventO?.Invoke(new object());
			this.EventHA?.Invoke(this, EventArgs.Empty);
			this.EventA?.Invoke(EventArgs.Empty);
		}
	}
}