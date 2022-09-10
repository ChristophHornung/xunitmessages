namespace xUnitMessages.Example;

using Xunit;

// Alias the assert to quickly migrate existing code to use AssertM.
using Assert = XunitAssertMessages.AssertM;

public class Test
{
	[Fact]
	public void AssertsSomething()
	{
		// Prior to the alias above this call worked using the Xunit.Assert class
		Assert.Equal(10, 9);
		// Now with the alias we can add a message
		Assert.Equal(10, 9, "Message");

		// If for some reason you do not want to use the alias, the AssertM class can be used directly.
		XunitAssertMessages.AssertM.Equal(10, 9, "Message");
	}
}