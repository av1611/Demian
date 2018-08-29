using System;
using System.IO;
using Pocket.Common;

namespace Demian
{
    public sealed class FileText : IText
    {
        private readonly Lazy<string> _content;

        public FileText(string path)
        {
            path.EnsureNotNull();
            path.Ensure(x => !string.IsNullOrEmpty(path));
            path.Ensure(File.Exists);
            
            _content = new Lazy<string>(() => File.ReadAllText(path));
        }

        public string Content => _content.Value;
    }
}