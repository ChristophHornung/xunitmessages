// ReSharper disable once CheckNamespace 
// Polyfill namespace to enable Visual Studio syntax coloring support for pre-.NET 7

namespace System.Diagnostics.CodeAnalysis;

// Adapted from https://github.com/dotnet/runtime/blob/1e9c6a82aca4904828636b3638962c05a5f8d9c8/src/libraries/System.Private.CoreLib/src/System/Diagnostics/CodeAnalysis/StringSyntaxAttribute.cs
// to polyfill Visual Studio syntax coloring support for pre-.NET 7

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
/// <summary>Specifies the syntax used in a string.</summary>
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property)]
internal sealed class StringSyntaxAttribute : Attribute
{
	/// <summary>The syntax identifier for strings containing composite formats for string formatting.</summary>
	public const string CompositeFormat = nameof(StringSyntaxAttribute.CompositeFormat);

	/// <summary>The syntax identifier for strings containing date format specifiers.</summary>
	public const string DateOnlyFormat = nameof(StringSyntaxAttribute.DateOnlyFormat);

	/// <summary>The syntax identifier for strings containing date and time format specifiers.</summary>
	public const string DateTimeFormat = nameof(StringSyntaxAttribute.DateTimeFormat);

	/// <summary>The syntax identifier for strings containing <see cref="Enum"/> format specifiers.</summary>
	public const string EnumFormat = nameof(StringSyntaxAttribute.EnumFormat);

	/// <summary>The syntax identifier for strings containing <see cref="Guid"/> format specifiers.</summary>
	public const string GuidFormat = nameof(StringSyntaxAttribute.GuidFormat);

	/// <summary>The syntax identifier for strings containing JavaScript Object Notation (JSON).</summary>
	public const string Json = nameof(StringSyntaxAttribute.Json);

	/// <summary>The syntax identifier for strings containing numeric format specifiers.</summary>
	public const string NumericFormat = nameof(StringSyntaxAttribute.NumericFormat);

	/// <summary>The syntax identifier for strings containing regular expressions.</summary>
	public const string Regex = nameof(StringSyntaxAttribute.Regex);

	/// <summary>The syntax identifier for strings containing time format specifiers.</summary>
	public const string TimeOnlyFormat = nameof(StringSyntaxAttribute.TimeOnlyFormat);

	/// <summary>The syntax identifier for strings containing <see cref="TimeSpan"/> format specifiers.</summary>
	public const string TimeSpanFormat = nameof(StringSyntaxAttribute.TimeSpanFormat);

	/// <summary>The syntax identifier for strings containing URIs.</summary>
	public const string Uri = nameof(StringSyntaxAttribute.Uri);

	/// <summary>The syntax identifier for strings containing XML.</summary>
	public const string Xml = nameof(StringSyntaxAttribute.Xml);

	/// <summary>Initializes the <see cref="StringSyntaxAttribute"/> with the identifier of the syntax used.</summary>
	/// <param name="syntax">The syntax identifier.</param>
	public StringSyntaxAttribute(string syntax)
	{
		this.Syntax = syntax;
		this.Arguments = [];
	}

	/// <summary>Initializes the <see cref="StringSyntaxAttribute"/> with the identifier of the syntax used.</summary>
	/// <param name="syntax">The syntax identifier.</param>
	/// <param name="arguments">Optional arguments associated with the specific syntax employed.</param>
	public StringSyntaxAttribute(string syntax, params object?[] arguments)
	{
		this.Syntax = syntax;
		this.Arguments = arguments;
	}

	/// <summary>Gets the identifier of the syntax used.</summary>
	public string Syntax { get; }

	/// <summary>Optional arguments associated with the specific syntax employed.</summary>
	public object?[] Arguments { get; }
}