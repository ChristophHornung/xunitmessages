namespace xUnitMessages.Example;

using Xunit;

// Alias the assert to quickly migrate existing code to use AssertM.
using Assert = XunitAssertMessages.AssertM;

public class Test
{
	[Fact]
	public void AssertsSomethingMigration()
	{
		// Prior to the alias above this call worked using the Xunit.Assert class
		Assert.Equal(10, 9);
		// Now with the alias we can add a message
		Assert.Equal(10, 9, "This is so wrong.");

		// If for some reason you do not want to use the alias, the AssertM class can be used directly.
		XunitAssertMessages.AssertM.Equal(10, 9, "This is so wrong.");
	}

	[Fact]
	public void AssertsSomethingKeepOriginalMessage()
	{
		// We can include the original message in our own.
		Assert.Equal(10, 9, "This is so wrong. {xMsg}");

		// Without a message the message will be constructed by xUnit.
		Assert.Equal(10, 9);

		// For interpolated strings use a double curly.
		Assert.Equal(10, 9, $"{9} is so wrong. {{xMsg}}");
	}
}