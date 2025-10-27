using System;
using System.IO;
using Money_Box.IService;

namespace Money_Box.Platforms
{
    public class DatabasePathProvider : IDatabasePathProvider
    {
        public string GetLocalFilePath(string filename)
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(folderPath, filename);
        }
    }
}
