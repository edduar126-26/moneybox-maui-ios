using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Money_Box.IService
{
    public interface IDatabasePathProvider
    {
        string GetLocalFilePath(string filename);
    }
}
