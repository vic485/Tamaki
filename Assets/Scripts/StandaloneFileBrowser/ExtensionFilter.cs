namespace StandaloneFileBrowser
{
    public struct ExtensionFilter
    {
        public string Name;
        public string[] Extensions;

        public ExtensionFilter(string name, params string[] extensions)
        {
            Name = name;
            Extensions = extensions;
        }
    }
}
