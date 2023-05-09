namespace XunitAssertMessages;

using System.ComponentModel;
using System.Diagnostics;
using Xunit;
using Xunit.Sdk;

/// <summary>
///     The static class containing overloads with a message for all <see cref="Assert" /> methods.
/// </summary>
public static partial class AssertM
{
	/// <inheritdoc cref="Assert.Equal{T}(System.Collections.Generic.IEnumerable{T},System.Collections.Generic.IEnumerable{T})"/>
	[Obsolete("This is an override of Object.Equals(). Call Assert.Equal() instead.", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public new static bool Equals(object a, object b)
	{
		throw new InvalidOperationException("Assert.Equals should not be used");
	}

	/// <inheritdoc cref="Assert.ReferenceEquals"/>
	[Obsolete("This is an override of Object.ReferenceEquals(). Call Assert.Same() instead.", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public new static bool ReferenceEquals(object a, object b)
	{
		throw new InvalidOperationException("Assert.ReferenceEquals should not be used");
	}

	/// <inheritdoc cref="Xunit.Assert.Equal(System.String,System.String)"/>
	public static void Equal(string? expected, string? actual)
	{
		// We add a non-message overload to avoid having ambiguous methods on an Equal("a","a") call.
		WithMessage(null, () => Xunit.Assert.Equal(expected, actual));

	}

	[DebuggerHidden]
	private static void WithMessage(string? message, Action action)
	{
		try
		{
			action.Invoke();
		}
		catch (XunitException xException)
		{
			if (message == null)
			{
				message = xException.Message;
			}
			else
			{
				message = message.Replace("{xMsg}", Environment.NewLine+ xException.Message);
			}

			throw new XunitException(message, xException.InnerException);
		}
	}

	[DebuggerHidden]
	private static async Task WithMessageAsync(string? message, Func<Task> action)
	{
		try
		{
			await action.Invoke();
		}
		catch (XunitException xException)
		{
			if (message == null)
			{
				message = xException.Message;
			}
			else
			{
				message = message.Replace("{xMsg}", Environment.NewLine+ xException.Message);
			}

			throw new XunitException(message, xException.InnerException);
		}
	}

	[DebuggerHidden]
	private static T WithMessage<T>(string? message, Func<T> action)
	{
		try
		{
			return action.Invoke();
		}
		catch (XunitException xException)
		{
			if (message == null)
			{
				message = xException.Message;
			}
			else
			{
				message = message.Replace("{xMsg}", Environment.NewLine+ xException.Message);
			}

			throw new XunitException(message, xException.InnerException);
		}
	}

	[DebuggerHidden]
	private static async Task<T> WithMessageAsync<T>(string? message, Func<Task<T>> action)
	{
		try
		{
			return await action.Invoke();
		}
		catch (XunitException xException)
		{
			if (message == null)
			{
				message = xException.Message;
			}
			else
			{
				message = message.Replace("{xMsg}", Environment.NewLine+ xException.Message);
			}

			throw new XunitException(message, xException.InnerException);
		}
	}
}