namespace XunitMessages.Tests;

using Xunit;
using Xunit.Sdk;

public class AssertMessageAsserts
{
	private static void AssertWrapsCorrectly(Action t)
	{
		var exception = Assert.ThrowsAny<XunitException>(t.Invoke);

		Assert.StartsWith("Message", exception.UserMessage);
	}

	[Fact]
	public void All2UsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.All(new[] { "T" }, t => throw new Exception(), "Message"));
	}

	[Fact]
	public void AllUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.All(new[] { "T" }, t => throw new Exception(), "Message"));
	}

	[Fact]
	public void AssertEmptyUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() => AssertM.Empty(new[] { "T" }, "Message"));
	}

	[Fact]
	public void CollectionUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Collection(new[] { "T" }, Array.Empty<Action<string>>(), "Message"));
	}

	[Fact]
	public void ContainsUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Contains(new[] { "T" }, _ => false, "Message"));
	}

	[Fact]
	public void ContainsUsesMessage2()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Contains(12, (IDictionary<int, string>)new Dictionary<int, string>() { { 1, "A" } }, "Message"));
	}

	[Fact]
	public void ContainsUsesMessage3()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Contains(12, (IReadOnlyDictionary<int, string>)new Dictionary<int, string>() { { 1, "A" } },
				"Message"));
	}

	[Fact]
	public void ContainsUsesMessage4()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() => AssertM.Contains("A", "B", "Message"));
	}

	[Fact]
	public void ContainsUsesMessage5()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Contains(12, new[] { 1 }, "Message"));
	}

	[Fact]
	public void ContainsUsesMessage6()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Contains("A", new[] { "B" }, StringComparer.InvariantCultureIgnoreCase, "Message"));
	}

	[Fact]
	public void ContainsUsesMessage7()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Contains("A", "B", StringComparison.InvariantCultureIgnoreCase, "Message"));
	}

	[Fact]
	public void DistinctUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Distinct(new[] { "A", "B" }, StringComparer.InvariantCultureIgnoreCase, "Message"));
	}

	[Fact]
	public void DistinctUsesMessage2()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() => AssertM.Distinct(new[] { "A", "B" }, "Message"));
	}

	[Fact]
	public void DoesNotContainUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() => AssertM.DoesNotContain(new[] { "B" }, (a) => false, "Message"));
	}

	[Fact]
	public void DoesNotContainUsesMessage2()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() => AssertM.DoesNotContain("B", new[] { "B" }, "Message"));
	}

	[Fact]
	public void DoesNotContainUsesMessage3()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.DoesNotContain("B", new[] { "B" }, StringComparer.InvariantCultureIgnoreCase, "Message"));
	}

	[Fact]
	public void DoesNotContainUsesMessage4()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() => AssertM.DoesNotContain("B",
			(IDictionary<string, string>)new Dictionary<string, string> { { "B", "Z" } }, "Message"));
	}

	[Fact]
	public void DoesNotContainUsesMessage5()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() => AssertM.DoesNotContain("B", "B", "Message"));
	}

	[Fact]
	public void DoesNotContainUsesMessage6()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.DoesNotContain("B", "B", StringComparison.InvariantCultureIgnoreCase, "Message"));
	}

	[Fact]
	public void DoesNotContainUsesMessage7()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() => AssertM.DoesNotContain("B",
			(IReadOnlyDictionary<string, string>)new Dictionary<string, string> { { "A", "Z" } }, "Message"));
	}

	[Fact]
	public void DoesNotMatchUsesMessage()
	{
	}

	[Fact]
	public void DoesNotMatchUsesMessage2()
	{
	}

	[Fact]
	public void EmptyUsesMessage()
	{
	}

	[Fact]
	public void EndsWithUsesMessage()
	{
	}

	[Fact]
	public void EndsWithUsesMessage2()
	{
	}

	[Fact]
	public void EqualUsesMessage()
	{
	}

	[Fact]
	public void EqualUsesMessage10()
	{
	}

	[Fact]
	public void EqualUsesMessage11()
	{
	}

	[Fact]
	public void EqualUsesMessage12()
	{
	}

	[Fact]
	public void EqualUsesMessage2()
	{
	}

	[Fact]
	public void EqualUsesMessage3()
	{
	}

	[Fact]
	public void EqualUsesMessage4()
	{
	}

	[Fact]
	public void EqualUsesMessage5()
	{
	}

	[Fact]
	public void EqualUsesMessage6()
	{
	}

	[Fact]
	public void EqualUsesMessage7()
	{
	}

	[Fact]
	public void EqualUsesMessage8()
	{
	}

	[Fact]
	public void EqualUsesMessage9()
	{
	}

	[Fact]
	public void EquivalentUsesMessage()
	{
	}

	[Fact]
	public void InRangeUsesMessage()
	{
	}

	[Fact]
	public void InRangeUsesMessage2()
	{
	}

	[Fact]
	public void IsAssignableFromUsesMessage()
	{
	}

	[Fact]
	public void IsAssignableFromUsesMessage2()
	{
	}

	[Fact]
	public void IsNotTypeUsesMessage()
	{
	}

	[Fact]
	public void IsNotTypeUsesMessage2()
	{
	}

	[Fact]
	public void IsTypeUsesMessage()
	{
	}

	[Fact]
	public void IsTypeUsesMessage2()
	{
	}

	[Fact]
	public void MatchesUsesMessage()
	{
	}

	[Fact]
	public void MatchesUsesMessage2()
	{
	}

	[Fact]
	public void MultipleUsesMessage()
	{
	}

	[Fact]
	public void NotEmptyUsesMessage()
	{
	}

	[Fact]
	public void NotEqualUsesMessage()
	{
	}

	[Fact]
	public void NotEqualUsesMessage2()
	{
	}

	[Fact]
	public void NotEqualUsesMessage3()
	{
	}

	[Fact]
	public void NotEqualUsesMessage4()
	{
	}

	[Fact]
	public void NotEqualUsesMessage5()
	{
	}

	[Fact]
	public void NotEqualUsesMessage6()
	{
	}

	[Fact]
	public void NotInRangeUsesMessage()
	{
	}

	[Fact]
	public void NotInRangeUsesMessage2()
	{
	}

	[Fact]
	public void NotNullUsesMessage()
	{
	}

	[Fact]
	public void NotSameUsesMessage()
	{
	}

	[Fact]
	public void NotStrictEqualUsesMessage()
	{
	}

	[Fact]
	public void NullUsesMessage()
	{
	}

	[Fact]
	public void ProperSubsetUsesMessage()
	{
	}

	[Fact]
	public void ProperSupersetUsesMessage()
	{
	}

	[Fact]
	public async Task PropertyChangedAsyncUsesMessage()
	{
	}

	[Fact]
	public void PropertyChangedUsesMessage()
	{
	}

	[Fact]
	public async Task RaisesAnyAsyncUsesMessage()
	{
	}

	[Fact]
	public void RaisesAnyUsesMessage()
	{
	}

	[Fact]
	public async Task RaisesAsyncUsesMessage()
	{
	}

	[Fact]
	public void RaisesUsesMessage()
	{
	}

	[Fact]
	public void SameUsesMessage()
	{
	}

	[Fact]
	public void SingleUsesMessage()
	{
	}

	[Fact]
	public void SingleUsesMessage2()
	{
	}

	[Fact]
	public void SingleUsesMessage3()
	{
	}

	[Fact]
	public void SingleUsesMessage4()
	{
	}

	[Fact]
	public void StartsWithUsesMessage()
	{
	}

	[Fact]
	public void StartsWithUsesMessage2()
	{
	}

	[Fact]
	public void StrictEqualUsesMessage()
	{
	}

	[Fact]
	public void SubsetUsesMessage()
	{
	}

	[Fact]
	public void SupersetUsesMessage()
	{
	}

	[Fact]
	public async Task ThrowsAnyAsyncUsesMessage()
	{
	}

	[Fact]
	public void ThrowsAnyUsesMessage()
	{
	}

	[Fact]
	public void ThrowsAnyUsesMessage2()
	{
	}

	[Fact]
	public async Task ThrowsAsyncUsesMessage()
	{
	}

	[Fact]
	public async Task ThrowsAsyncUsesMessage2()
	{
	}

	[Fact]
	public async Task ThrowsAsyncUsesMessage3()
	{
	}

	[Fact]
	public void ThrowsUsesMessage2()
	{
	}

	[Fact]
	public void ThrowsUsesMessage3()
	{
	}

	[Fact]
	public void ThrowsUsesMessage4()
	{
	}

	[Fact]
	public void ThrowsUsesMessage5()
	{
	}

	[Fact]
	public void ThrowsUsesMessage6()
	{
	}

	[Fact]
	public void ThrowsUsesMessage7()
	{
	}
}