#if UNITY_STANDALONE_WIN

using System;
using System.Windows.Forms;

namespace StandaloneFileBrowser
{
    public class WindowWrapper : IWin32Window
    {
        public IntPtr Handle { get; }

        public WindowWrapper(IntPtr handle)
        {
            Handle = handle;
        }
    }
}

#endif
