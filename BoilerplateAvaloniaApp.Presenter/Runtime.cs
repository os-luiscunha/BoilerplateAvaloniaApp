using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace BoilerplateAvaloniaApp.Presenter;

public abstract class Runtime {
    protected static IEnumerable<string> AssembliesToLoad => new[] { "BoilerplateAvaloniaApp.Presenter" };
    private const string DebugAssembly = "BoilerplateAvaloniaApp.Debug";

    public bool DebugMode { get; }

    public bool HotfixMode { get; }

    public bool IsUnsupportedVersion { get; private set; }
    public bool RunningUnitTests { get; set; }

    // using a delegate instead of a private field to make it harder to bypass

    [DebuggerNonUserCode]
    public static Runtime Instance { get; private set; }

    public static void SetupCurrentThreadCulture() {
        //make sure exceptions in submit feedbacks are not translated
        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
    }

    public static IEnumerable<Assembly> LoadAssemblies(IEnumerable<string> assemblies) {
        return AppDomain.CurrentDomain.GetAssemblies().Where(assembly => assemblies.Contains(assembly.GetName().Name));
    }

    protected virtual void InitializeIsUnsupportedVersion() { }

    internal bool AllowToLoadAnyModelFeatureRegardlessOfStatus { get; }

    protected Runtime(
        string[] commandLineArguments,
        object protectedResourcesAccessService,
        bool debugModeRequested = false,
        bool hotfixModeRequested = false,
        Func<string[], bool> allowToLoadAnyModelFeatureRegardlessOfStatus = null) {
        SetupCurrentThreadCulture();

        if (Instance == null) {
            Instance = this;
        } else {
            throw new InvalidOperationException("Instance already set");
        }

        InitializeIsUnsupportedVersion();

        Initialize();

        var assembliesNames = AssembliesToLoad.ToArray();

        var start = DateTime.Now;
        if (debugModeRequested) {
            if (LoadAndCheckDebugAssembly(DebugAssembly, out _)) {
                assembliesNames = assembliesNames.Append(DebugAssembly).ToArray();
            } else {
                debugModeRequested = false;
            }
        }

        var assemblies = LoadAssemblies(assembliesNames).ToList();
        assemblies.Insert(0, typeof(Runtime).Assembly); // add current assembly as first
        //AutoRegistryType.Loader.Init(assemblies);
        //TypeLoader.DiscoverTypesInAssemblies(assemblies, typeof(V1::OutSystems.Model.IService), typeof(V1::OutSystems.Model.IPluginService));

        if (debugModeRequested) {
            DebugMode = true;
            Console.WriteLine("Types load took " + (DateTime.Now - start).TotalMilliseconds + "ms");
        } else {
            DebugMode = false;
        }

        HotfixMode = hotfixModeRequested;

        InitializeFeaturesProvider();

        AllowToLoadAnyModelFeatureRegardlessOfStatus = allowToLoadAnyModelFeatureRegardlessOfStatus?.Invoke(commandLineArguments) == true;
    }

    protected virtual void Initialize() { }

    protected virtual void InitializeFeaturesProvider() { }

    // this method is overridden in QueryGrabber's Runtime Implementation

    private static bool LoadAndCheckDebugAssembly(string debugAssembly, out bool assemblyExists) {
        assemblyExists = false;
        return false;
    }

    protected static readonly Dictionary<Guid, (Thread, CancellationTokenSource)> WorkItems = new Dictionary<Guid, (Thread, CancellationTokenSource)>();
}
