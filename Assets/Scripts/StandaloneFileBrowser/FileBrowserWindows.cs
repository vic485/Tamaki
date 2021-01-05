using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Ookii.Dialogs;

#if UNITY_STANDALONE_WIN

namespace StandaloneFileBrowser
{
    public class FileBrowserWindows : IFileBrowser
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

        public string[] OpenFilePanel(string title, string directory, ExtensionFilter[] extensions, bool multiSelect)
        {
            var fd = new VistaOpenFileDialog {Title = title};
            if (extensions != null)
            {
                fd.Filter = GetFilterFromExtensions(extensions);
                fd.FilterIndex = 1;
            }
            else
            {
                fd.Filter = string.Empty;
            }

            fd.Multiselect = multiSelect;
            if (!string.IsNullOrWhiteSpace(directory))
            {
                fd.FileName = GetDirectoryPath(directory);
            }

            var res = fd.ShowDialog(new WindowWrapper(GetActiveWindow()));
            var fileNames = res == DialogResult.OK ? fd.FileNames : new string[0];
            fd.Dispose();
            return fileNames;
        }

        public string[] OpenFolderPanel(string title, string directory, bool multiSelect)
        {
            var fd = new VistaFolderBrowserDialog();
            fd.Description = title;
            if (!string.IsNullOrWhiteSpace(directory))
                fd.SelectedPath = GetDirectoryPath(directory);

            var res = fd.ShowDialog(new WindowWrapper(GetActiveWindow()));
            var fileNames = res == DialogResult.OK ? new[] {fd.SelectedPath} : new string[0];
            fd.Dispose();
            return fileNames;
        }

        public string SaveFilePanel(string title, string directory, string defaultName, ExtensionFilter[] extensions)
        {
            var fd = new VistaSaveFileDialog();
            fd.Title = title;
            var finalFileName = string.Empty;

            if (!string.IsNullOrWhiteSpace(directory))
                finalFileName = GetDirectoryPath(directory);

            if (!string.IsNullOrWhiteSpace(defaultName))
                finalFileName += defaultName;

            fd.FileName = finalFileName;
            if (extensions != null)
            {
                fd.Filter = GetFilterFromExtensions(extensions);
                fd.FilterIndex = 1;
                fd.DefaultExt = extensions[0].Extensions[0];
                fd.AddExtension = true;
            }
            else
            {
                fd.DefaultExt = string.Empty;
                fd.Filter = string.Empty;
                fd.AddExtension = false;
            }

            var res = fd.ShowDialog(new WindowWrapper(GetActiveWindow()));
            var fileName = res == DialogResult.OK ? fd.FileName : string.Empty;
            fd.Dispose();
            return fileName;
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

        // .NET Framework FileDialog Filter format
        // https://msdn.microsoft.com/en-us/library/microsoft.win32.filedialog.filter
        private static string GetFilterFromExtensions(IEnumerable<ExtensionFilter> extensions)
        {
            var filters = string.Empty;
            foreach (var filter in extensions)
            {
                filters += filter.Name + "(";

                filters = filter.Extensions.Aggregate(filters, (current, extension) => current + $"*.{extension},");

                filters = filters.Remove(filters.Length - 1);
                filters += ") |";

                filters = filter.Extensions.Aggregate(filters, (current, extension) => current + $"*.{extension}; ");

                filters += "|";
            }

            filters = filters.Remove(filters.Length - 1);
            return filters;
        }

        private static string GetDirectoryPath(string directory)
        {
            var directoryPath = Path.GetFullPath(directory);
            if (!directoryPath.EndsWith("\\"))
            {
                directory += "\\";
            }

            if (Path.GetPathRoot(directoryPath) == directoryPath)
                return directory;

            return Path.GetDirectoryName(directoryPath) + Path.DirectorySeparatorChar;
        }
    }
}

#endif
