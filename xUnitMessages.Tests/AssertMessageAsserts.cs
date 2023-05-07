namespace XunitMessages.Tests;

using System.Collections;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Sdk;
using XunitAssertMessages;

public class AssertMessageAsserts
{
	private static void AssertWrapsCorrectly(Action t)
	{
		var exception = Assert.ThrowsAny<XunitException>(t.Invoke);

		Assert.StartsWith("Message", exception.UserMessage);
	}

	private static async Task AssertWrapsCorrectly(Func<Task> t)
	{
		var exception = await Assert.ThrowsAnyAsync<XunitException>(t.Invoke);

		Assert.StartsWith("Message", exception.UserMessage);
	}

	private class UnchangingObject : INotifyPropertyChanged
	{
#pragma warning disable CS0067
		public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore CS0067
	}

	[Fact]
	public void All2UsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.All(new[] { "T" }, _ => throw new Exception(), "Message"));
	}

	[Fact]
	public void AllUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.All(new[] { "T" }, (_, _) => throw new Exception(), "Message"));
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
			AssertM.Contains(12, (IDictionary<int, string>)new Dictionary<int, string> { { 1, "A" } }, "Message"));
	}

	[Fact]
	public void ContainsUsesMessage3()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Contains(12, (IReadOnlyDictionary<int, string>)new Dictionary<int, string> { { 1, "A" } },
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
			AssertM.Distinct(new[] { "A", "A" }, StringComparer.InvariantCultureIgnoreCase, "Message"));
	}

	[Fact]
	public void DistinctUsesMessage2()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() => AssertM.Distinct(new[] { "A", "A" }, "Message"));
	}

	[Fact]
	public void DoesNotContainUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() => AssertM.DoesNotContain(new[] { "B" }, _ => true, "Message"));
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
		AssertMessageAsserts.AssertWrapsCorrectly(() => AssertM.DoesNotContain("A",
			(IReadOnlyDictionary<string, string>)new Dictionary<string, string> { { "A", "Z" } }, "Message"));
	}

	[Fact]
	public void DoesNotMatchUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() => AssertM.DoesNotMatch("A", "A", "Message"));
	}

	[Fact]
	public void DoesNotMatchUsesMessage2()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() => AssertM.DoesNotMatch(new Regex("A"), "A", "Message"));
	}

	[Fact]
	public void EmptyUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() => AssertM.Empty(new[] { "A" }, "Message"));
	}

	[Fact]
	public void EndsWithUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() => AssertM.EndsWith("A", "B", "Message"));
	}

	[Fact]
	public void EndsWithUsesMessage2()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.EndsWith("A", "B", StringComparison.OrdinalIgnoreCase, "Message"));
	}

	[Fact]
	public void EqualUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Equal((IEnumerable<string>)new List<string>(), new List<string> { "A" }, "Message"));
	}

	[Fact]
	public void EqualUsesMessage10()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Equal(new object(), new object(), "Message"));
	}

	[Fact]
	public void EqualUsesMessage11()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Equal(new List<string>(), new List<string> { "A" }, StringComparer.OrdinalIgnoreCase, "Message"));
	}

	[Fact]
	public void EqualUsesMessage12()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Equal(0.1, 2.2, 1, "Message"));
	}

	[Fact]
	public void EqualUsesMessage2()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Equal(0.1, 2.2, 1, MidpointRounding.AwayFromZero, "Message"));
	}

	[Fact]
	public void EqualUsesMessage3()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Equal(0.1, 2.2, 0.1, "Message"));
	}

	[Fact]
	public void EqualUsesMessage4()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Equal(0.1f, 2.2f, 0.1f, "Message"));
	}

	[Fact]
	public void EqualUsesMessage5()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Equal(0.1m, 2.2m, 1, "Message"));
	}

	[Fact]
	public void EqualUsesMessage6()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Equal(DateTime.MinValue, DateTime.MaxValue, new TimeSpan(10), "Message"));
	}

	[Fact]
	public void EqualUsesMessage7()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Equal("A", "B", "Message"));
	}

	[Fact]
	public void EqualUsesMessage8()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Equal("A", "B", true, userMessage: "Message"));
	}

	[Fact]
	public void EqualUsesMessage9()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Equal("A", "B", StringComparer.InvariantCultureIgnoreCase, "Message"));
	}

	[Fact]
	public void EquivalentUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Equivalent("A", "B", false, "Message"));
	}

	[Fact]
	public void InRangeUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.InRange(10, 0, 1, "Message"));
	}

	[Fact]
	public void InRangeUsesMessage2()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.InRange("A", "B", "C", StringComparer.OrdinalIgnoreCase, "Message"));
	}

	[Fact]
	public void IsAssignableFromUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.IsAssignableFrom(typeof(int), "A", "Message"));
	}

	[Fact]
	public void IsAssignableFromUsesMessage2()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.IsAssignableFrom<int>("A", "Message"));
	}

	[Fact]
	public void IsNotTypeUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.IsNotType(typeof(string), "A", "Message"));
	}

	[Fact]
	public void IsNotTypeUsesMessage2()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.IsNotType<string>("A", "Message"));
	}

	[Fact]
	public void IsTypeUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.IsType(typeof(int), "A", "Message"));
	}

	[Fact]
	public void IsTypeUsesMessage2()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.IsType<int>("A", "Message"));
	}

	[Fact]
	public void MatchesUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Matches(new Regex("A"), "B", "Message"));
	}

	[Fact]
	public void MatchesUsesMessage2()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Matches("A", "B", "Message"));
	}

	[Fact]
	public void MultipleUsesMessage()

	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Multiple(new Action[] { () => Assert.False(true) }, "Message"));
	}

	[Fact]
	public void NotEmptyUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.NotEmpty(new string[] { }, "Message"));
	}

	[Fact]
	public void NotEqualUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.NotEqual((IEnumerable<string>)new List<string>(), new List<string>(), "Message"));
	}

	[Fact]
	public void NotEqualUsesMessage2()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.NotEqual(new string[] { }, new string[] { }, StringComparer.OrdinalIgnoreCase, "Message"));
	}

	[Fact]
	public void NotEqualUsesMessage3()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.NotEqual("S", "S", "Message"));
	}

	[Fact]
	public void NotEqualUsesMessage4()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.NotEqual("S", "S", StringComparer.OrdinalIgnoreCase, "Message"));
	}

	[Fact]
	public void NotEqualUsesMessage5()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.NotEqual(1.0, 1.0, 1, "Message"));
	}

	[Fact]
	public void NotEqualUsesMessage6()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.NotEqual(1.0m, 1.0m, 1, "Message"));
	}

	[Fact]
	public void NotInRangeUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.NotInRange("B", "A", "C", "Message"));
	}

	[Fact]
	public void NotInRangeUsesMessage2()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.NotInRange("B", "A", "C", StringComparer.OrdinalIgnoreCase, "Message"));
	}

	[Fact]
	public void NotNullUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.NotNull(null, "Message"));
	}

	[Fact]
	public void NotSameUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.NotSame("A", "A", "Message"));
	}

	[Fact]
	public void NotStrictEqualUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.NotStrictEqual("A", "A", "Message"));
	}

	[Fact]
	public void NullUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Null("A", "Message"));
	}

	[Fact]
	public void ProperSubsetUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.ProperSubset(new HashSet<string> { "A" }, new HashSet<string> { "B" }, "Message"));
	}

	[Fact]
	public void ProperSupersetUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.ProperSuperset(new HashSet<string> { "A" }, new HashSet<string> { "B" }, "Message"));
	}

	[Fact]
	public async Task PropertyChangedAsyncUsesMessage()
	{
		await AssertMessageAsserts.AssertWrapsCorrectly(async () =>
			await AssertM.PropertyChangedAsync(new UnchangingObject(), "A", () => Task.CompletedTask, "Message"));
	}

	[Fact]
	public void PropertyChangedUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.PropertyChanged(new UnchangingObject(), "A", () => { }, "Message"));
	}

	[Fact]
	public async Task RaisesAnyAsyncUsesMessage()
	{
		await AssertMessageAsserts.AssertWrapsCorrectly(async () =>
			await AssertM.RaisesAnyAsync<object>(_ => { }, _ => { }, () => Task.CompletedTask, "Message"));
	}

	[Fact]
	public void RaisesAnyUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.RaisesAny<object>(_ => { }, _ => { }, () => { }, "Message"));
	}

	[Fact]
	public async Task RaisesAsyncUsesMessage()
	{
		await AssertMessageAsserts.AssertWrapsCorrectly(async () =>
			await AssertM.RaisesAsync<object>(_ => { }, _ => { }, () => Task.CompletedTask, "Message"));
	}

	[Fact]
	public void RaisesUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Raises<object>(_ => { }, _ => { }, () => { }, "Message"));
	}

	[Fact]
	public void SameUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Same("A", "B", "Message"));
	}

	[Fact]
	public void SingleUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Single("A", 'c', "Message"));
	}

	[Fact]
	public void SingleUsesMessage2()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Single((IEnumerable)"AB", "Message"));
	}

	[Fact]
	public void SingleUsesMessage3()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Single("A", _ => false, "Message"));
	}

	[Fact]
	public void SingleUsesMessage4()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Single("AB", "Message"));
	}

	[Fact]
	public void StartsWithUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.StartsWith("A", "B", "Message"));
	}

	[Fact]
	public void StartsWithUsesMessage2()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.StartsWith("A", "B", StringComparison.OrdinalIgnoreCase, "Message"));
	}

	[Fact]
	public void StrictEqualUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.StrictEqual("A", "B", "Message"));
	}

	[Fact]
	public void SubsetUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Subset(new HashSet<string> { "A" }, new HashSet<string> { "B" }, "Message"));
	}

	[Fact]
	public void SupersetUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Superset(new HashSet<string> { "A" }, new HashSet<string> { "B" }, "Message"));
	}

	[Fact]
	public async Task ThrowsAnyAsyncUsesMessage()
	{
		await AssertMessageAsserts.AssertWrapsCorrectly(async () =>
			await AssertM.ThrowsAnyAsync<Exception>(() => Task.CompletedTask, "Message"));
	}

	[Fact]
	public void ThrowsAnyUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.ThrowsAny<Exception>(() => { }, "Message"));
	}

	[Fact]
	public void ThrowsAnyUsesMessage2()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.ThrowsAny<Exception>(() => "A", "Message"));
	}

	[Fact]
	public async Task ThrowsAsyncUsesMessage()
	{
		await AssertMessageAsserts.AssertWrapsCorrectly(async () =>
			await AssertM.ThrowsAsync<Exception>(() => Task.CompletedTask, "Message"));
	}

	[Fact]
	public async Task ThrowsAsyncUsesMessage2()
	{
		await AssertMessageAsserts.AssertWrapsCorrectly(async () =>
			await AssertM.ThrowsAsync(typeof(Exception), () => Task.CompletedTask, "Message"));
	}

	[Fact]
	public async Task ThrowsAsyncUsesMessage3()
	{
		await AssertMessageAsserts.AssertWrapsCorrectly(async () =>
			await AssertM.ThrowsAsync<ArgumentException>("A", () => Task.CompletedTask, "Message"));
	}

	[Fact]
	public void ThrowsUsesMessage2()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Throws<Exception>(() => { }, "Message"));
	}

	[Fact]
	public void ThrowsUsesMessage3()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Throws<Exception>(() => "A", "Message"));
	}

	[Fact]
	public void ThrowsUsesMessage4()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Throws(typeof(Exception), () => { }, "Message"));
	}

	[Fact]
	public void ThrowsUsesMessage5()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Throws(typeof(Exception), () => "A", "Message"));
	}

	[Fact]
	public void ThrowsUsesMessage6()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Throws<ArgumentException>("A", () => { }, "Message"));
	}

	[Fact]
	public void ThrowsUsesMessage7()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Throws<ArgumentException>("A", () => "A", "Message"));
	}
}