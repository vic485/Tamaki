using System;

namespace StandaloneFileBrowser
{
    public interface IFileBrowser
    {
        string[] OpenFilePanel(string title, string directory, ExtensionFilter[] extensions, bool multiSelect);
        string[] OpenFolderPanel(string title, string directory, bool multiSelect);
        string SaveFilePanel(string title, string directory, string defaultName, ExtensionFilter[] extensions);

        void OpenFilePanelAsync(string title, string directory, ExtensionFilter[] extensions, bool multiSelect,
            Action<string[]> callback);

        void OpenFolderPanelAsync(string title, string directory, bool multiSelect, Action<string[]> callback);

        void SaveFilePanelAsync(string title, string directory, string defaultName, ExtensionFilter[] extensions,
            Action<string> callback);
    }
}
