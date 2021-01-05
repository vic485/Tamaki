#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEditor;

namespace StandaloneFileBrowser
{
    public class FileBrowserEditor : IFileBrowser
    {
        public string[] OpenFilePanel(string title, string directory, ExtensionFilter[] extensions, bool multiSelect)
        {
            var path = extensions == null
                ? EditorUtility.OpenFilePanel(title, directory, "")
                : EditorUtility.OpenFilePanelWithFilters(title, directory, GetFilterFromExtensions(extensions));

            return string.IsNullOrWhiteSpace(path) ? new string[0] : new[] {path};
        }

        public string[] OpenFolderPanel(string title, string directory, bool multiSelect)
        {
            var path = EditorUtility.OpenFolderPanel(title, directory, string.Empty);
            return string.IsNullOrWhiteSpace(path) ? new string[0] : new[] {path};
        }

        public string SaveFilePanel(string title, string directory, string defaultName, ExtensionFilter[] extensions)
        {
            var ext = extensions != null ? extensions[0].Extensions[0] : "";
            var name = string.IsNullOrWhiteSpace(ext) ? defaultName : $"{defaultName}.{ext}";
            return EditorUtility.SaveFilePanel(title, directory, name, ext);
        }

        public void OpenFilePanelAsync(string title, string directory, ExtensionFilter[] extensions, bool multiSelect,
            Action<string[]> callback)
        {
            callback.Invoke(OpenFilePanel(title, directory, extensions, multiSelect));
        }

        public void OpenFolderPanelAsync(string title, string directory, bool multiSelect, Action<string[]> callback)
        {
            callback.Invoke(OpenFolderPanel(title, directory, multiSelect));
        }

        public void SaveFilePanelAsync(string title, string directory, string defaultName, ExtensionFilter[] extensions,
            Action<string> callback)
        {
            callback.Invoke(SaveFilePanel(title, directory, defaultName, extensions));
        }

        private static string[] GetFilterFromExtensions(IList<ExtensionFilter> extensions)
        {
            var filters = new string[extensions.Count * 2];
            for (var i = 0; i < extensions.Count; i++)
            {
                filters[i * 2] = extensions[i].Name;
                filters[i * 2 + 1] = string.Join(",", extensions[i].Extensions);
            }

            return filters;
        }
    }
}

#endif
