namespace XunitMessages.Generation;

using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

[Generator]
public class MessageExtensionGenerator : ISourceGenerator
{
	private static readonly DiagnosticDescriptor InvalidXmlWarning = new DiagnosticDescriptor(
		id: "XUNITMGEN001",
		title: "Exception on generation",
		messageFormat: "Exception '{0}' {1}.",
		category: "MessageExtensionGenerator",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true);


	private static readonly DiagnosticDescriptor LogInfo = new DiagnosticDescriptor(
		id: "XUNITMGENLOG",
		title: "Log",
		messageFormat: "{0}",
		category: "MessageExtensionGenerator",
		DiagnosticSeverity.Warning,
		isEnabledByDefault: true);

	public void Execute(GeneratorExecutionContext context)
	{
		try
		{
			INamedTypeSymbol assertMType =
				context.Compilation.Assembly.GetTypeByMetadataName("XunitMessages.AssertM")!;

			StringBuilder sourceBuilder = new();
			sourceBuilder.Append(@"
namespace XunitMessages;
using System.Collections;
using System.ComponentModel;
using System.Text.RegularExpressions;

public static partial class AssertM
{");

			sourceBuilder.AppendLine();

			var xUnitRef =
				context.Compilation.SourceModule.ReferencedAssemblySymbols.First(r => r.Name == "xunit.assert");

			INamespaceSymbol xUnitNamespace =
				xUnitRef.GlobalNamespace.GetNamespaceMembers().Single(s => s.Name == "Xunit");

			INamedTypeSymbol assertType = xUnitNamespace.GetTypeMembers().Single(m => m.Name == "Assert");

			foreach (var member in assertType.GetMembers()
				         .OfType<IMethodSymbol>()
				         .Where(m =>
					         m.CanBeReferencedByName &&
					         m.IsDefinition &&
					         m.DeclaredAccessibility == Accessibility.Public &&
					         m.Name != "Equals" &&
					         m.Name != "ReferenceEquals" &&
					         m.GetAttributes()
						         .All(a => a.AttributeClass?.Name.Contains("ObsoleteAttribute") != true)))
			{
				this.BuildOverload(context, sourceBuilder, member);
			}

			sourceBuilder.Append("}");
			SourceText source = SourceText.From(sourceBuilder.ToString(), Encoding.UTF8);
			context.AddSource("AssertMgenerated.cs", source);
		}
		catch (Exception e)
		{
			context.ReportDiagnostic(Diagnostic.Create(MessageExtensionGenerator.InvalidXmlWarning, Location.None,
				e.Message, e.StackTrace));
		}
	}

	public void Initialize(GeneratorInitializationContext context)
	{
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

	private void BuildOverload(GeneratorExecutionContext context, StringBuilder sourceBuilder,
		IMethodSymbol member)
	{
		StringBuilder methodSignatureBuilder = new();
		methodSignatureBuilder.Append(
			$"\tpublic static {(this.IsTask(member.ReturnType) ? "async" : "")} {member.ReturnType.ToDisplayString()} {member.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}(");
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

			methodSignatureBuilder.Append(
				parameter.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));
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

			methodSignatureBuilder.Append("string? userMessage = null");
		}

		methodSignatureBuilder.Append(")");

		if (member.TypeParameters.Any(t => t.ConstraintTypes.Any()) ||
		    member.TypeParameters.Any(t => t.HasNotNullConstraint))
		{
			methodSignatureBuilder.Append(" where ");
		}


		foreach (ITypeParameterSymbol? typeParameter in member.TypeParameters)
		{
			if (typeParameter.ConstraintTypes.Any() || typeParameter.HasNotNullConstraint)
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
			}
		}

		this.Log(context, methodSignatureBuilder.ToString());
		sourceBuilder.Append(methodSignatureBuilder);
		sourceBuilder.AppendLine();
		sourceBuilder.Append("\t{");
		this.BuildCall(sourceBuilder, member);
		sourceBuilder.AppendLine();
		sourceBuilder.Append("\t}");
		sourceBuilder.AppendLine();
		sourceBuilder.AppendLine();
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
		context.ReportDiagnostic(Diagnostic.Create(MessageExtensionGenerator.LogInfo, Location.None, log));
#endif
	}
}