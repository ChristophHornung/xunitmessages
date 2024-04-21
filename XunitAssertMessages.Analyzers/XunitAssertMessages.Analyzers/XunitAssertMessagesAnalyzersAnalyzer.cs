namespace XunitAssertMessages.Analyzers;

using System.Collections.Immutable;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class XunitAssertMessagesAnalyzersAnalyzer : DiagnosticAnalyzer
{
	public const string DiagnosticId = "XunitAssertMessagesAnalyzers";
	private const string Category = "Naming";

	// You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
	// See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
	private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle),
		Resources.ResourceManager, typeof(Resources));

	private static readonly LocalizableString MessageFormat =
		new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager,
			typeof(Resources));

	private static readonly LocalizableString Description =
		new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager,
			typeof(Resources));

	private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
		XunitAssertMessagesAnalyzersAnalyzer.DiagnosticId, XunitAssertMessagesAnalyzersAnalyzer.Title,
		XunitAssertMessagesAnalyzersAnalyzer.MessageFormat, XunitAssertMessagesAnalyzersAnalyzer.Category,
		DiagnosticSeverity.Warning, isEnabledByDefault: true,
		description: XunitAssertMessagesAnalyzersAnalyzer.Description);

	private Assembly? xunitAssembly;
	private string? test;
	private IList<Type> types = [];
	private IList<object> instances = [];

	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
	{
		get { return [XunitAssertMessagesAnalyzersAnalyzer.Rule]; }
	}

	private static void AnalyzeSymbol(SymbolAnalysisContext context)
	{
		// TODO: Replace the following code with your own analysis, generating Diagnostic objects for any issues you find
		var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

		// Find just those named type symbols with names containing lowercase letters.
		if (namedTypeSymbol.Name.ToCharArray().Any(char.IsLower))
		{
			// For all such symbols, produce a diagnostic.
			var diagnostic = Diagnostic.Create(XunitAssertMessagesAnalyzersAnalyzer.Rule, namedTypeSymbol.Locations[0],
				namedTypeSymbol.Name);

			context.ReportDiagnostic(diagnostic);
		}
	}

	public override void Initialize(AnalysisContext context)
	{
		context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
		context.EnableConcurrentExecution();

		// Get all loaded assemblies
		this.xunitAssembly = AppDomain.CurrentDomain.GetAssemblies()
			.FirstOrDefault(a => a.FullName.Contains("xunit.analyzers"));
		this.test = string.Join(",", AppDomain.CurrentDomain.GetAssemblies()
			.Select(a => a.GetName().Name));

		// From the xunit.analyzers assembly, get all classes that inherit from Xunit.Analyzers.AssertUsageAnalyzerBase
		// First we get the type itself
		Type assertUsageAnalyzerType = this.xunitAssembly.GetType("Xunit.Analyzers.AssertUsageAnalyzerBase");
		if (assertUsageAnalyzerType != null)
		{
			this.types = this.xunitAssembly.GetTypes()
				.Where(t => t.IsSubclassOf(assertUsageAnalyzerType)).ToList();

			this.instances = [..this.types.Select(Activator.CreateInstance)];
		}
		else
		{
			this.test = "No base";
		}

		context.RegisterCompilationStartAction(context => { this.AnalyzeCompilation(context); });

		foreach (object instance in this.instances)
		{
			context.RegisterCompilationStartAction(context =>
			{
				// This is the original code that we are trying to replicate
				// var xunitContext = CreateXunitContext(context.Compilation);
				// if (ShouldAnalyze(xunitContext))
				//	AnalyzeCompilation(context, xunitContext);

				// We call the CreateXunitContext method to get the XunitContext object via reflection on the instance
				// of the AssertUsageAnalyzerBase class
				var createXunitContext = instance.GetType().GetMethod("CreateXunitContext");
				if (createXunitContext == null)
				{
					this.test = "CreateXunitContext method not found";
					return;
				}

				// We invoke the method to get the XunitContext object
				object xunitContext = createXunitContext.Invoke(instance, new object[] { context.Compilation });

				// We check if we should analyze the compilation
				var shouldAnalyzeMethod = instance.GetType().GetMethod("ShouldAnalyze");
				if (shouldAnalyzeMethod == null)
				{
					this.test = "ShouldAnalyze method not found";
					return;
				}

				bool shouldAnalyze = (bool)shouldAnalyzeMethod.Invoke(instance, new object[] { xunitContext });
				if (shouldAnalyze)
				{
					// Now we replicate the AnalyzeCompilation method code
					// The original code is
					//var assertType = TypeSymbolFactory.Assert(context.Compilation);
					//if (assertType is null)
					//	return;

					//context.RegisterOperationAction(context =>
					//{
					//	if (context.Operation is IInvocationOperation invocationOperation)
					//	{
					//		var methodSymbol = invocationOperation.TargetMethod;
					//		if (methodSymbol.MethodKind != MethodKind.Ordinary || !SymbolEqualityComparer.Default.Equals(methodSymbol.ContainingType, assertType) || !targetMethods.Contains(methodSymbol.Name))
					//			return;

					//		AnalyzeInvocation(context, xunitContext, invocationOperation, methodSymbol);
					//	}
					//}, OperationKind.Invocation);

					// We check for our own assert type
					INamedTypeSymbol? assertMType = context.Compilation.GetTypeByMetadataName("XunitAssertMessages.AssertM");
					if (assertMType == null)
					{
						return;
					}

					// We get the target methods by getting the private field from the AssertUsageAnalyzerBase class called targetMethods
					FieldInfo? targetMethodsField = instance.GetType().GetField("targetMethods", BindingFlags.NonPublic | BindingFlags.Instance);
					if (targetMethodsField == null)
					{
						this.test = "targetMethods field not found";
						return;
					}

					// We get the value of the field
					string[] targetMethods = (string[])targetMethodsField.GetValue(instance);

					context.RegisterOperationAction(context =>
					{
						if (context.Operation is IInvocationOperation invocationOperation)
						{
							var methodSymbol = invocationOperation.TargetMethod;
							if (methodSymbol.MethodKind != MethodKind.Ordinary || !SymbolEqualityComparer.Default.Equals(methodSymbol.ContainingType, assertMType) || !targetMethods.Contains(methodSymbol.Name))
								return;

							AnalyzeInvocation(instance, context, xunitContext, invocationOperation, methodSymbol);
						}
					}, OperationKind.Invocation);
				}
			});
		}
	}

	private void AnalyzeInvocation(object instance, OperationAnalysisContext context, object xunitContext, IInvocationOperation invocationOperation, IMethodSymbol methodSymbol)
	{
		throw new NotImplementedException();
	}

	private void AnalyzeCompilation(CompilationStartAnalysisContext context)
	{
		context.RegisterOperationAction(this.AnalyzeOperation, OperationKind.Invocation);
	}

	private void AnalyzeOperation(OperationAnalysisContext context)
	{
		if (this.xunitAssembly == null)
		{
			context.ReportDiagnostic(Diagnostic.Create(XunitAssertMessagesAnalyzersAnalyzer.Rule,
				context.Operation.Syntax.GetLocation(), "Xunit.Analyzers assembly not found"));
		}

		if (this.test != null)
		{
			context.ReportDiagnostic(Diagnostic.Create(XunitAssertMessagesAnalyzersAnalyzer.Rule,
				context.Operation.Syntax.GetLocation(), this.test));
		}
	}
}