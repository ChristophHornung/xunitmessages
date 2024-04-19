namespace XunitMessages.Tests;

using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Sdk;
using XunitAssertMessages;

public class AssertMessageAsserts
{
	private static void AssertWrapsCorrectly(Action t)
	{
		var exception = Assert.ThrowsAny<XunitException>(t.Invoke);
		Assert.StartsWith("Message", exception.Message);
	}

	private static async Task AssertWrapsCorrectly(Func<Task> t)
	{
		var exception = await Assert.ThrowsAnyAsync<XunitException>(t.Invoke);

		Assert.StartsWith("Message", exception.Message);
	}

	private class UnchangingObject : INotifyPropertyChanged
	{
#pragma warning disable CS0067
		public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore CS0067
	}

	[Fact]
	public async Task AllAsyncUsesMessage()
	{
		await AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.AllAsync(new[] { "T" }, _ => Task.FromException(new Exception()), "Message"));
	}

	[Fact]
	public async Task AllAsyncUsesMessage2()
	{
		await AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.AllAsync(new[] { "T" }, (_, _) => Task.FromException(new Exception()), "Message"));
	}

	[Fact]
	public void AllUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.All(new[] { "T" }, (_, _) => throw new Exception(), "Message"));
	}

	[Fact]
	public void AllUsesMessage2()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.All(new[] { "T" }, _ => throw new Exception(), "Message"));
	}

	[Fact]
	public void AssertEmptyUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() => AssertM.Empty(new[] { "T" }, "Message"));
	}

	[Fact]
	public async Task CollectionAsyncUsesMessage()
	{
		await AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.CollectionAsync(new[] { "T" }, [_ => Task.FromException(new Exception())], "Message"));
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
	public void ContainsUsesMessage10()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Contains("C",
				new Dictionary<string, string>([new KeyValuePair<string, string>("A", "A")]).AsReadOnly(), "Message"));
	}

	[Fact]
	public void ContainsUsesMessage11()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Contains("C", new HashSet<string>(["A"]), "Message"));
	}

	[Fact]
	public void ContainsUsesMessage12()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Contains("C", (ISet<string>)new HashSet<string>(["A"]), "Message"));
	}

	[Fact]
	public void ContainsUsesMessage13()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Contains("C", new SortedSet<string>(["A"]), "Message"));
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
	public void ContainsUsesMessage8()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Contains("C",
				new ConcurrentDictionary<string, string>([new KeyValuePair<string, string>("A", "A")]), "Message"));
	}

	[Fact]
	public void ContainsUsesMessage9()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Contains("C",
				new Dictionary<string, string>([new KeyValuePair<string, string>("A", "A")]), "Message"));
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
	public void DoesNotContain4()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.DoesNotContain("A",
				new Dictionary<string, string>([new KeyValuePair<string, string>("A", "A")]).AsReadOnly(), "Message"));
	}

	[Fact]
	public void DoesNotContain5()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.DoesNotContain("A",
				new Dictionary<string, string>([new KeyValuePair<string, string>("A", "A")]), "Message"));
	}

	[Fact]
	public void DoesNotContain6()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.DoesNotContain("A",
				new ConcurrentDictionary<string, string>([new KeyValuePair<string, string>("A", "A")]), "Message"));
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
	public void DoestNotContainUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.DoesNotContain("A", new SortedSet<string>(["A"]), "Message"));
	}

	[Fact]
	public void DoestNotContainUsesMessage2()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.DoesNotContain("A", (ISet<string>)new HashSet<string>(["A"]), "Message"));
	}

	[Fact]
	public void DoestNotContainUsesMessage3()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.DoesNotContain("A", new HashSet<string>(["A"]), "Message"));
	}

	[Fact]
	public void EmptyUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() => AssertM.Empty(new[] { "A" }, "Message"));
	}

	[Fact]
	public void EmptyUsesMessage2()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() => AssertM.Empty("A", "Message"));
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
			AssertM.Equal(new List<string>() { "A" }, new List<string> { "A" },
				new Func<string, string, bool>((_, _) => false), "Message"));
	}

	[Fact]
	public void EqualUsesMessage13()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Equal(new object(), new object(), new Func<object, object, bool>((_, _) => false), "Message"));
	}

	[Fact]
	public void EqualUsesMessage14()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Equal(0.1, 2.2, 1, "Message"));
	}

	[Fact]
	public void EqualUsesMessage15()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Equal(0.1f, 2.2f, 1, "Message"));
	}

	[Fact]
	public void EqualUsesMessage16()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Equal(0.1f, 2.2f, 1, MidpointRounding.AwayFromZero, "Message"));
	}

	[Fact]
	public void EqualUsesMessage17()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Equal(DateTime.Now, DateTime.Now.AddHours(1), "Message"));
	}

	[Fact]
	public void EqualUsesMessage18()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Equal(DateTimeOffset.Now, DateTimeOffset.Now.AddHours(1), "Message"));
	}

	[Fact]
	public void EqualUsesMessage19()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Equal(DateTimeOffset.Now, DateTimeOffset.Now.AddHours(1), TimeSpan.FromMinutes(59), "Message"));
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
	public void IsNotAssignableFromUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.IsNotAssignableFrom(typeof(string), "A", "Message"));
	}

	[Fact]
	public void IsNotAssignableFromUsesMessage2()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.IsNotAssignableFrom<string>("A", "Message"));
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
	public void NotEqualUsesMessage10()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.NotEqual(1.95f, 2.0f, 1, "Message"));
	}

	[Fact]
	public void NotEqualUsesMessage11()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.NotEqual(1.95f, 2.0f, 1, MidpointRounding.AwayFromZero, "Message"));
	}

	[Fact]
	public void NotEqualUsesMessage12()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.NotEqual(1.0f, 2.0f, 1.0f, "Message"));
	}

	[Fact]
	public void NotEqualUsesMessage13()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.NotEqual(1.0f, 2.0f, (_, _) => true, "Message"));
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
	public void NotEqualUsesMessage7()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.NotEqual(new List<string>(["A"]), new List<string>(["A"]), (string _, string _) => true,
				"Message"));
	}

	[Fact]
	public void NotEqualUsesMessage8()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.NotEqual(1.95, 2.0, 1, MidpointRounding.AwayFromZero, "Message"));
	}

	[Fact]
	public void NotEqualUsesMessage9()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.NotEqual(1.0, 2.0, 1.0, "Message"));
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
	public void NotNullUsesMessage2()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.NotNull((int?)null, "Message"));
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
	public void NullMessageDoesNotAppend()
	{
		static void AssertWrapsCorrectlyWithoutMessage(Action unwrapped, Action wrapped)
		{
			var exception = Assert.ThrowsAny<XunitException>(wrapped.Invoke);
			var originalException = Assert.ThrowsAny<XunitException>(unwrapped.Invoke);
			Assert.Equal(exception.Message, originalException.Message);
		}

		AssertWrapsCorrectlyWithoutMessage(
			() => Assert.NotNull(null),
			() => AssertM.NotNull(null, null));
	}

	[Fact]
	public void NullUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Null("A", "Message"));
	}

	[Fact]
	public void NullUsesMessage2()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Null((int?)3, "Message"));
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
			await AssertM.RaisesAnyAsync<object>(new Action<Action<object>>(_ => { }), _ => { },
				() => Task.CompletedTask, "Message"));
	}

	[Fact]
	public async Task RaisesAnyEventHandlerAsyncUsesMessage()
	{
		await AssertMessageAsserts.AssertWrapsCorrectly(async () =>
			await AssertM.RaisesAnyAsync<object>(new Action<EventHandler<object>>(_ => { }), _ => { },
				() => Task.CompletedTask, "Message"));
	}

	[Fact]
	public void RaisesAnyEventHandlerUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.RaisesAny<object>(new Action<EventHandler<object>>(_ => { }), _ => { }, () => { }, "Message"));
	}

	[Fact]
	public void RaisesAnyUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.RaisesAny(_ => { }, _ => { }, () => { }, "Message"));
	}

	[Fact]
	public async Task RaisesAnyAsyncUsesMessage2()
	{
		await AssertMessageAsserts.AssertWrapsCorrectly(async () =>
			await AssertM.RaisesAnyAsync(_ => { }, _ => { },
				() => Task.CompletedTask, "Message"));
	}

	[Fact]
	public void RaisesAnyUsesMessage2()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.RaisesAny<object>(new Action<Action<object>>(_ => { }), _ => { }, () => { }, "Message"));
	}

	[Fact]
	public void RaisesAnyUsesMessage3()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.RaisesAny<object>(new Action<Action<object>>(_ => { }), _ => { }, () => { }, "Message"));
	}

	[Fact]
	public async Task RaisesAsyncUsesMessage()
	{
		await AssertMessageAsserts.AssertWrapsCorrectly(async () =>
			await AssertM.RaisesAsync<object>(new Action<Action<object>>(_ => { }), _ => { }, () => Task.CompletedTask,
				"Message"));
	}

	[Fact]
	public async Task RaisesAsyncUsesMessage2()
	{
		await AssertMessageAsserts.AssertWrapsCorrectly(async () =>
			await AssertM.RaisesAsync(_ => { }, _ => { }, () => Task.CompletedTask,
				"Message"));
	}

	[Fact]
	public async Task RaisesAsyncUsesMessage3()
	{
		await AssertMessageAsserts.AssertWrapsCorrectly(async () =>
			await AssertM.RaisesAsync<object>(new Action<EventHandler<object>>(_ => { }), _ => { },
				() => Task.CompletedTask, "Message"));
	}

	[Fact]
	public void RaisesEventHandlerUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Raises<object>(new Action<EventHandler<object>>(_ => { }), _ => { }, () => { }, "Message"));
	}

	[Fact]
	public void RaisesUsesMessage()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Raises(_ => { }, _ => { }, () => { }, "Message"));
	}

	[Fact]
	public void RaisesUsesMessage2()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Raises<object>(new Action<Action<object>>(_ => { }), _ => { }, () => { }, "Message"));
	}

	[Fact]
	public void RaisesUsesMessage3()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Raises<string>(() => (Assert.RaisedEvent<string>?)null, () => { }, () => { }, () => { },
				"Message"));
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
		List<char> chars = "A".ToList();
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Single(chars, _ => false, "Message"));
	}

	[Fact]
	public void SingleUsesMessage4()
	{
		AssertMessageAsserts.AssertWrapsCorrectly(() =>
			AssertM.Single("ABCDEFG", "Message"));
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

	[Fact]
	public void WarnAboutBaseEquals()
	{
		Assert.NotEmpty(
			typeof(AssertM).GetMethod(nameof(AssertM.Equals),
					BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public)!
				.GetCustomAttributes(true).OfType<ObsoleteAttribute>()
				.Where(a => a.IsError));
		Assert.NotEmpty(
			typeof(AssertM).GetMethod(nameof(AssertM.ReferenceEquals),
					BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public)!
				.GetCustomAttributes(true).OfType<ObsoleteAttribute>()
				.Where(a => a.IsError));
	}
}