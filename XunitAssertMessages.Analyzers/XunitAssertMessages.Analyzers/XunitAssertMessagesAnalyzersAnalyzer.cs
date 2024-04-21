namespace XunitAssertMessages.Analyzers;

using System.Collections.Immutable;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class XunitAssertMessagesAnalyzersAnalyzer : DiagnosticAnalyzer
{
#if DEBUG
	private static readonly DiagnosticDescriptor DebugOnlyRule = new(
		"debug001", "Debug Only output",
		"{0}", "Debug",
		DiagnosticSeverity.Warning, isEnabledByDefault: true, customTags: [WellKnownDiagnosticTags.CompilationEnd]);
#endif

	private static readonly DiagnosticDescriptor AssertM001MustReferenceXunitAssertRule = new(
		"assertM001",
		"When referencing XunitAssertMessages.Analyzers the Xunit.Analyzers nuget needs to be referenced as well",
		"Reference the Xunit.Analyzers for XunitAssertMessages.Analyzers to work", "AssertM",
		DiagnosticSeverity.Warning, isEnabledByDefault: true, customTags: [WellKnownDiagnosticTags.CompilationEnd]);

	private Assembly? xunitAssembly;
	private string? test;
	private IList<Type> types = [];
	private IList<object> instances = [];
	private readonly IList<DiagnosticDescriptor> inheritedSupportedDiagnostics = [];

	public XunitAssertMessagesAnalyzersAnalyzer()
	{
		// When initializing the analyzer we get all the DiagnosticDescriptors of xunit.analyzers
		this.xunitAssembly = AppDomain.CurrentDomain.GetAssemblies()
			.FirstOrDefault(a => a.FullName.Contains("xunit.analyzers"));
		// We get the static Xunit.Analyzers.Descriptors class
		Type? descriptorsType = this.xunitAssembly?.GetType("Xunit.Analyzers.Descriptors");
		if (descriptorsType != null)
		{
			// We get all the static fields of the Descriptors class
			PropertyInfo[] properties = descriptorsType.GetProperties(BindingFlags.Public | BindingFlags.Static);
			foreach (PropertyInfo property in properties)
			{
				// We get the value of the field
				object? value = property.GetValue(null);
				if (value is DiagnosticDescriptor descriptor)
				{
					// We add the descriptor to the SupportedDiagnostics
					this.inheritedSupportedDiagnostics.Add(descriptor);
				}
			}
		}
		else
		{
			this.test = "No Descriptors";
		}
	}

#if DEBUG
	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
	[
		XunitAssertMessagesAnalyzersAnalyzer.DebugOnlyRule, ..this.inheritedSupportedDiagnostics
	];
#else
	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [..this.inheritedSupportedDiagnostics];
#endif

	public override void Initialize(AnalysisContext context)
	{
		context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
		context.EnableConcurrentExecution();

		// Get all loaded assemblies
		this.xunitAssembly = AppDomain.CurrentDomain.GetAssemblies()
			.FirstOrDefault(a => a.FullName.Contains("xunit.analyzers"));

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

		context.RegisterCompilationStartAction(this.AnalyzeCompilation);

		foreach (object instance in this.instances)
		{
			context.RegisterCompilationStartAction(ctx =>
			{
				// This is the original code that we are trying to replicate
				// var xunitContext = CreateXunitContext(context.Compilation);
				// if (ShouldAnalyze(xunitContext))
				//	AnalyzeCompilation(context, xunitContext);

				// We call the CreateXunitContext method to get the XunitContext object via reflection on the instance
				// of the AssertUsageAnalyzerBase class, the method is protected, so we have to use reflection
				var createXunitContext = instance.GetType()
					.GetMethod("CreateXunitContext", BindingFlags.NonPublic | BindingFlags.Instance);
				if (createXunitContext == null)
				{
					this.test = "CreateXunitContext method not found";
					return;
				}

				// We invoke the method to get the XunitContext object
				object xunitContext = createXunitContext.Invoke(instance, [ctx.Compilation]);

				// We check if we should analyze the compilation
				MethodInfo? shouldAnalyzeMethod = instance.GetType()
					.GetMethod("ShouldAnalyze", BindingFlags.NonPublic | BindingFlags.Instance);
				if (shouldAnalyzeMethod == null)
				{
					this.test = "ShouldAnalyze method not found";
					return;
				}

				bool shouldAnalyze = (bool)shouldAnalyzeMethod.Invoke(instance, [xunitContext]);
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
					INamedTypeSymbol? assertMType =
						ctx.Compilation.GetTypeByMetadataName("XunitAssertMessages.AssertM");
					if (assertMType == null)
					{
						return;
					}

					// We get the target methods by getting the static private field from the AssertUsageAnalyzerBase class called targetMethods
					FieldInfo? targetMethodsField = assertUsageAnalyzerType.GetField("targetMethods",
						BindingFlags.NonPublic | BindingFlags.Instance);
					if (targetMethodsField == null)
					{
						this.test = $"{instance.GetType().Name}: targetMethods field not found";
						return;
					}

					// We get the value of the field
					HashSet<string> targetMethods = (HashSet<string>)targetMethodsField.GetValue(instance);

					ctx.RegisterOperationAction(context =>
					{
						if (context.Operation is IInvocationOperation invocationOperation)
						{
							var methodSymbol = invocationOperation.TargetMethod;
							if (methodSymbol.MethodKind != MethodKind.Ordinary ||
							    !SymbolEqualityComparer.Default.Equals(methodSymbol.ContainingType, assertMType) ||
							    !targetMethods.Contains(methodSymbol.Name))
							{
								return;
							}

							this.AnalyzeInvocation(instance, context, xunitContext, invocationOperation, methodSymbol);
						}
					}, OperationKind.Invocation);
				}
			});
		}
	}

	private void AnalyzeInvocation(object instance, OperationAnalysisContext context, object xunitContext,
		IInvocationOperation invocationOperation, IMethodSymbol methodSymbol)
	{
		// instance is the AssertUsageAnalyzerBase instance
		// we replicate the AnalyzeInvocation method code but need to change the context to drop the last parameter
		// We get the protected AnalyzeInvocation method from the AssertUsageAnalyzerBase class
		MethodInfo? analyzeInvocationMethod = instance.GetType()
			.GetMethod("AnalyzeInvocation", BindingFlags.NonPublic | BindingFlags.Instance);
		if (analyzeInvocationMethod == null)
		{
			this.test = "AnalyzeInvocation method not found";
			return;
		}

		// We tweak the context to drop the last parameter

		//This is what we want to call, but the constructor is internal, so we again have to use reflection
		//IInvocationOperation tweakedInvocationOperation = new InvocationOperation(invocationOperation.Syntax,
		//	invocationOperation.TargetMethod, invocationOperation.Instance,
		//	invocationOperation.Arguments.RemoveAt(invocationOperation.Arguments.Length - 1),
		//	invocationOperation.IsImplicit, invocationOperation.Type, invocationOperation.ConstantValue,
		//	invocationOperation.Language);

		// The type Microsoft.CodeAnalysis.Operations.InvocationOperation itself is internal, so we have to use reflection to get it
		// Its signature is: internal InvocationOperation(IMethodSymbol targetMethod, ITypeSymbol? constrainedToType, IOperation? instance,
		// bool isVirtual, ImmutableArray<IArgumentOperation> arguments, SemanticModel? semanticModel, SyntaxNode syntax, ITypeSymbol? type, bool isImplicit)
		Assembly operationsAssembly = typeof(IInvocationOperation).Assembly;
		Type invocationOperationType =
			operationsAssembly.GetType("Microsoft.CodeAnalysis.Operations.InvocationOperation");

		// We get the constructor of the InvocationOperation class
		ConstructorInfo? invocationOperationConstructor = invocationOperationType.GetConstructor(
			BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[]
			{
				typeof(IMethodSymbol), typeof(ITypeSymbol), typeof(IOperation), typeof(bool),
				typeof(ImmutableArray<IArgumentOperation>), typeof(SemanticModel), typeof(SyntaxNode),
				typeof(ITypeSymbol), typeof(bool)
			}, null);

		if (invocationOperationConstructor == null)
		{
			this.test = "InvocationOperation constructor not found";
			return;
		}

		// We create the tweakedInvocationOperation object
		object tweakedInvocationOperation = invocationOperationConstructor.Invoke([
			invocationOperation.TargetMethod,
			invocationOperation.ConstrainedToType,
			invocationOperation.Instance,
			invocationOperation.IsVirtual,
			invocationOperation.Arguments.RemoveAt(invocationOperation.Arguments.Length - 1),
			invocationOperation.SemanticModel,
			invocationOperation.Syntax,
			invocationOperation.Type,
			invocationOperation.IsImplicit
		]);

		// We invoke the method
		try
		{
			analyzeInvocationMethod.Invoke(instance, [context, xunitContext, tweakedInvocationOperation, methodSymbol]);
		}
		catch (Exception e)
		{
			this.test = e.Message;
			this.test += e.InnerException?.Message;
		}
	}

	private void AnalyzeCompilation(CompilationStartAnalysisContext context)
	{
		context.RegisterCompilationEndAction(this.AnalyzeCompilationEnd);
	}

	private void AnalyzeCompilationEnd(CompilationAnalysisContext context)
	{
		if (this.xunitAssembly == null)
		{
			context.ReportDiagnostic(
				Diagnostic.Create(XunitAssertMessagesAnalyzersAnalyzer.AssertM001MustReferenceXunitAssertRule, null));
		}
#if DEBUG
		if (this.test != null)
		{
			context.ReportDiagnostic(Diagnostic.Create(XunitAssertMessagesAnalyzersAnalyzer.DebugOnlyRule,
				null, this.test));
		}
#endif
	}
}