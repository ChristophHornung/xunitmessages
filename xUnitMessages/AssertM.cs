namespace XunitMessages;

using System.ComponentModel;
using Xunit;
using Xunit.Sdk;

/// <summary>
/// The static class containing overloads with a message for all <see cref="Assert"/> methods.
/// </summary>
public static partial class AssertM
{
	/// <summary>Do not call this method.</summary>
	[Obsolete("This is an override of Object.Equals(). Call Assert.Equal() instead.", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public new static bool Equals(object a, object b)
	{
		throw new InvalidOperationException("Assert.Equals should not be used");
	}

	/// <summary>Do not call this method.</summary>
	[Obsolete("This is an override of Object.ReferenceEquals(). Call Assert.Same() instead.", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public new static bool ReferenceEquals(object a, object b)
	{
		throw new InvalidOperationException("Assert.ReferenceEquals should not be used");
	}

	private static void WithMessage(string? message, Action action)
	{
		try
		{
			action.Invoke();
		}
		catch (XunitException xunitException)
		{
			throw new XunitException(message ?? xunitException.Message, xunitException.InnerException);
		}
	}

	private static T WithMessage<T>(string? message, Func<T> action)
	{
		try
		{
			return action.Invoke();
		}
		catch (XunitException xunitException)
		{
			throw new XunitException(message ?? xunitException.Message, xunitException.InnerException);
		}
	}
}