using System.Runtime.InteropServices;

namespace BoilerplateAvaloniaApp.Presenter;

public partial class RuntimeImplementation : Runtime {
    private (Lazy<string> UserDirName, Lazy<string> CommonDirName) resourcesDirNames;

    private static string GetApplicationDataDirectory(bool perUser) {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
            return Environment.GetFolderPath(perUser ? Environment.SpecialFolder.LocalApplicationData : Environment.SpecialFolder.CommonApplicationData);
        } else {
            /* If the LocalApplicationData doesn't exist, Environment.SpecialFolder.LocalApplicationData returns an empty string.
             * GetFolderPath receives an option argument which can return the default path, even if it doesn't exist.
             * The creation of such directory in such case is handled by SS in GetResourcesDirectory().
             */
            var destinationFolder = perUser ? Environment.SpecialFolder.LocalApplicationData : Environment.SpecialFolder.ApplicationData;
            return Path.Combine(Environment.GetFolderPath(destinationFolder, Environment.SpecialFolderOption.DoNotVerify));
        }
    }

    public static string GetDefaultResourcesDirectory(bool perUser = true) {
        return "";
    }

    public string GetResourcesDirectory(bool perUser = true) {
        static string GetPathFreeOfSymbolicLinks(string dir, bool perUser) {
            // This condition applies for both file and directory symbolic links, hard links and directory junctions.
            if ((File.GetAttributes(dir) & FileAttributes.ReparsePoint) == 0) {
                var files = Directory.GetFiles(dir, "*", SearchOption.AllDirectories);
                if (files.All(f => (File.GetAttributes(f) & FileAttributes.ReparsePoint) == 0)) {
                    return dir;
                }
            }

            return "";
        }

        const string dir = "";

        if (!Directory.Exists(dir)) {
            Directory.CreateDirectory(dir);
            return dir;
        }

        if (perUser) {
            resourcesDirNames.UserDirName ??= new Lazy<string>(() => GetPathFreeOfSymbolicLinks(dir, perUser));
            return resourcesDirNames.UserDirName.Value;
        }

        resourcesDirNames.CommonDirName ??= new Lazy<string>(() => GetPathFreeOfSymbolicLinks(dir, perUser));

        return resourcesDirNames.CommonDirName.Value;
    }
}
