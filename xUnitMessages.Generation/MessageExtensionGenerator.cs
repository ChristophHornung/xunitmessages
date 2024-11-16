namespace XunitMessages.Generation;

using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

#pragma warning disable RS1035 // Do not use banned APIs for analyzers

[Generator]
public class MessageExtensionGenerator : ISourceGenerator
{
	private static readonly DiagnosticDescriptor generationWarning = new(
		id: "XUNITMGEN001",
		title: "Exception on generation",
		messageFormat: "Exception '{0}' {1}",
		category: "MessageExtensionGenerator",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true);

#if DEBUG
	private static readonly DiagnosticDescriptor logInfo = new(
		id: "XUNITMGENLOG",
		title: "Log",
		messageFormat: "{0}",
		category: "MessageExtensionGenerator",
		DiagnosticSeverity.Warning,
		isEnabledByDefault: true);
#endif

	public void Execute(GeneratorExecutionContext context)
	{
		try
		{
			StringBuilder sourceBuilder = new();
			sourceBuilder.Append(@"
namespace XunitAssertMessages;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;

public static partial class AssertM
{");

			sourceBuilder.AppendLine();

			IAssemblySymbol xUnitRef =
				context.Compilation.SourceModule.ReferencedAssemblySymbols.First(r => r.Name == "xunit.assert");

			INamespaceSymbol xUnitNamespace =
				xUnitRef.GlobalNamespace.GetNamespaceMembers().Single(s => s.Name == "Xunit");

			INamedTypeSymbol assertType = xUnitNamespace.GetTypeMembers().Single(m => m.Name == "Assert");

			foreach (IMethodSymbol? member in assertType.GetMembers()
				         .OfType<IMethodSymbol>()
				         .Where(MessageExtensionGenerator.IsMethodToExtend))
			{
				this.BuildOverload(context, sourceBuilder, member);
			}

			sourceBuilder.Append("}");
			SourceText source = SourceText.From(sourceBuilder.ToString(), Encoding.UTF8);
			context.AddSource("AssertMgenerated.cs", source);
		}
		catch (Exception e)
		{
			context.ReportDiagnostic(Diagnostic.Create(MessageExtensionGenerator.generationWarning, Location.None,
				e.Message, e.StackTrace));
		}
	}

	public void Initialize(GeneratorInitializationContext context)
	{
	}

	private static bool IsMethodToExtend(IMethodSymbol m)
	{
		return m.CanBeReferencedByName &&
		       m.IsDefinition &&
		       m.DeclaredAccessibility == Accessibility.Public &&
		       m.Name != "Equals" &&
		       m.Name != "ReferenceEquals" &&
		       m.GetAttributes()
			       .All(a => a.AttributeClass?.Name.Contains("ObsoleteAttribute") != true);
	}

	private static bool IsUnoverloadedStringEqual(IMethodSymbol methodSymbol)
	{
		// The Equal(string string) has an overload with optional parameters. That would make our own optional parameter overload ambiguous.
		if (methodSymbol is { Name: "Equal", Parameters.Length: 2 })
		{
			return methodSymbol.Parameters[0].Type.ToDisplayString() == "string?" &&
			       methodSymbol.Parameters[1].Type.ToDisplayString() == "string?";
		}

		return false;
	}

	private static bool HasNoMessage(IMethodSymbol member)
	{
		return member.Name != "False" && member.Name != "True" && member.Name != "Fail";
	}

	private static bool IsReturnable(IMethodSymbol member)
	{
		return !member.ReturnsVoid && member.ReturnType.ToDisplayString() != "System.Threading.Tasks.Task";
	}

	private static string EscapeParameterName(string parameterName)
	{
		if (parameterName == "object")
		{
			return "@object";
		}

		return parameterName;
	}

	/// <summary>
	/// Builds the documentation for the overload.
	/// </summary>
	/// <param name="context">The context.</param>
	/// <param name="sourceBuilder">The source builder to append to.</param>
	/// <param name="member">The member to build the documentation for.</param>
	private void BuildOverloadDocumentation(GeneratorExecutionContext context, StringBuilder sourceBuilder,
		IMethodSymbol member)
	{
		StringBuilder methodDocBuilder = new();

		methodDocBuilder.AppendLine(
			$"\t/// <inheritdoc cref=\"{this.ToInheritdocComment(member.GetDocumentationCommentId()!)}\"/>");

		sourceBuilder.Append(methodDocBuilder);
	}

	private string ToInheritdocComment(string documentationCommentId)
	{
		return documentationCommentId
			.Replace("M:", "")
			.Replace("``1(", "{T}(")
			.Replace("``2(", "{T,TValue}(")
			.Replace("``0", "T")
			.Replace("``1", "TValue");
	}

	private void BuildOverload(GeneratorExecutionContext context, StringBuilder sourceBuilder,
		IMethodSymbol member)
	{
		StringBuilder methodSignatureBuilder = new();
		this.BuildOverloadDocumentation(context, methodSignatureBuilder, member);
		methodSignatureBuilder.Append(
			$"\tpublic static{(this.IsTask(member.ReturnType) ? " async " : " ")}{member.ReturnType.ToDisplayString()} {member.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}(");
		bool first = true;
		foreach (IParameterSymbol parameter in member.Parameters)
		{
			if (first)
			{
				first = false;
			}
			else
			{
				methodSignatureBuilder.Append(", ");
			}

			// Copy the attributes from the parameter to the method signature
			foreach (AttributeData attribute in parameter.GetAttributes().Where(this.CanCopyAttribute))
			{
				methodSignatureBuilder.Append("[");
				methodSignatureBuilder.Append(attribute.AttributeClass!.ToDisplayString());

				// Also copy the arguments of the attribute
				if (attribute.ConstructorArguments.Length > 0)
				{
					methodSignatureBuilder.Append("(");
					bool firstArgument = true;
					foreach (TypedConstant argument in attribute.ConstructorArguments)
					{
						if (firstArgument)
						{
							firstArgument = false;
						}
						else
						{
							methodSignatureBuilder.Append(", ");
						}

						// Copy the argument as a named parameter
						methodSignatureBuilder.Append(argument.ToCSharpString());
					}

					methodSignatureBuilder.Append(")");
				}

				methodSignatureBuilder.Append("]");
			}

			methodSignatureBuilder.Append(
				parameter.Type.ToDisplayString());
			methodSignatureBuilder.Append(" ");
			methodSignatureBuilder.Append(MessageExtensionGenerator.EscapeParameterName(parameter.Name));
			if (parameter.HasExplicitDefaultValue)
			{
				methodSignatureBuilder.Append("=");
				methodSignatureBuilder.Append(this.GetDefaultValue(parameter));
			}
		}

		if (MessageExtensionGenerator.HasNoMessage(member))
		{
			if (!first)
			{
				methodSignatureBuilder.Append(", ");
			}

			if (MessageExtensionGenerator.IsUnoverloadedStringEqual(member))
			{
				// We can't make this optional here. It would make the overload ambiguous.
				methodSignatureBuilder.Append("string? userMessage");
			}
			else
			{
				methodSignatureBuilder.Append("string? userMessage = null");
			}
		}

		methodSignatureBuilder.Append(")");

		if (member.TypeParameters.Any(t => t.ConstraintTypes.Any()) ||
		    member.TypeParameters.Any(t => t.HasValueTypeConstraint) ||
		    member.TypeParameters.Any(t => t.HasNotNullConstraint))
		{
			methodSignatureBuilder.Append(" where ");
		}


		foreach (ITypeParameterSymbol? typeParameter in member.TypeParameters)
		{
			if (typeParameter.ConstraintTypes.Any() || 
			    typeParameter.HasValueTypeConstraint ||
			    typeParameter.HasNotNullConstraint)
			{
				first = true;
				methodSignatureBuilder.Append(typeParameter.ToDisplayString());
				methodSignatureBuilder.Append(": ");
				foreach (var constraint in typeParameter.ConstraintTypes)
				{
					if (first)
					{
						first = false;
					}
					else
					{
						methodSignatureBuilder.Append(", ");
					}

					methodSignatureBuilder.Append(constraint.ToDisplayString());
				}

				if (typeParameter.HasNotNullConstraint)
				{
					if (first)
					{
						first = false;
					}
					else
					{
						methodSignatureBuilder.Append(", ");
					}

					methodSignatureBuilder.Append("notnull");
				}

				if (typeParameter.HasValueTypeConstraint)
				{
					if (first)
					{
						first = false;
					}
					else
					{
						methodSignatureBuilder.Append(", ");
					}

					methodSignatureBuilder.Append("struct");
				}
			}
		}

		// this.Log(context, methodSignatureBuilder.ToString());
		sourceBuilder.AppendLine("// Disable the 'must have a non-null value when exiting as the call guarantees it.");
		sourceBuilder.AppendLine("#pragma warning disable CS8777");
		sourceBuilder.Append(methodSignatureBuilder);
		sourceBuilder.AppendLine();
		sourceBuilder.Append("\t{");
		this.BuildCall(sourceBuilder, member);
		sourceBuilder.AppendLine();
		sourceBuilder.Append("\t}");
		sourceBuilder.AppendLine();
		sourceBuilder.AppendLine("#pragma warning restore CS8777");
		sourceBuilder.AppendLine();
	}

	private bool CanCopyAttribute(AttributeData arg)
	{
		return arg.AttributeClass!.ToDisplayString() != "System.Runtime.CompilerServices.NullableAttribute";
	}

	private void BuildCall(StringBuilder sourceBuilder, IMethodSymbol member)
	{
		StringBuilder callBuilder = new();
		callBuilder.AppendLine();


		callBuilder.Append("\t");
		callBuilder.Append("\t");
		if (MessageExtensionGenerator.IsReturnable(member))
		{
			callBuilder.Append("return ");
		}

		if (this.IsTask(member.ReturnType))
		{
			callBuilder.Append("await ");
		}

		if (MessageExtensionGenerator.HasNoMessage(member))
		{
			if (this.IsTask(member.ReturnType))
			{
				callBuilder.Append("WithMessageAsync(userMessage, async () => await ");
			}
			else
			{
				callBuilder.Append("WithMessage(userMessage, () => ");
			}
		}

		callBuilder.Append($"Xunit.Assert.{member.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}(");
		bool first = true;
		foreach (IParameterSymbol parameter in member.Parameters)
		{
			if (first)
			{
				first = false;
			}
			else
			{
				callBuilder.Append(", ");
			}

			callBuilder.Append(MessageExtensionGenerator.EscapeParameterName(parameter.Name));
		}

		if (MessageExtensionGenerator.HasNoMessage(member))
		{
			callBuilder.Append(")");
		}

		callBuilder.Append(");");
		callBuilder.AppendLine();

		//this.Log(context, callBuilder.ToString());
		sourceBuilder.Append(callBuilder);
	}

	private bool IsTask(ITypeSymbol memberReturnType)
	{
		return memberReturnType.Name == "Task";
	}

	private string? GetDefaultValue(IParameterSymbol symbol)
	{
		if (symbol.HasExplicitDefaultValue == false)
		{
			return null;
		}

		if (symbol.ExplicitDefaultValue == null)
		{
			return "null";
		}

		if (symbol.ExplicitDefaultValue is string stringValue)
		{
			return $"\"{stringValue.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"";
		}

		if (symbol.ExplicitDefaultValue is bool value)
		{
			return value ? "true" : "false";
		}

		return symbol.ExplicitDefaultValue.ToString();
	}

	private void Log(GeneratorExecutionContext context, string log)
	{
#if DEBUG
		context.ReportDiagnostic(Diagnostic.Create(MessageExtensionGenerator.logInfo, Location.None, log));
#endif
	}
}