using System;
using StandaloneFileBrowser;

namespace StandaloneFileBrowser
{
    public static class FileBrowser
    {
        private static readonly IFileBrowser PlatformWrapper;

        static FileBrowser()
        {
#if UNITY_STANDALONE_WIN
            PlatformWrapper = new FileBrowserWindows();
#elif UNITY_EDITOR
        _platformWrapper = new FileBrowserEditor();
#else
        throw new PlatformNotSupportedException("Attempted to use File Browser on unsupported system!");
#endif
        }

        public static string[] OpenFilePanel(string title, string directory, string extension, bool multiSelect)
        {
            var extensions = string.IsNullOrWhiteSpace(extension) ? null : new[] {new ExtensionFilter("", extension)};
            return OpenFilePanel(title, directory, extensions, multiSelect);
        }

        public static string[] OpenFilePanel(string title, string directory, ExtensionFilter[] extensions,
            bool multiSelect)
        {
            return PlatformWrapper.OpenFilePanel(title, directory, extensions, multiSelect);
        }
        
        // TODO: finish?
    }
}
