using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Utilities.Memcached.Tests.TestData
{
    public static class MemcacheProcessHelper
    {
        public const string MEMCACHE_PROCESS_ENTRYPOINT = "memcached.exe";

        public static string MemcachePath
        {
            get { return Path.GetFullPath(Path.Combine("TestData", MEMCACHE_PROCESS_ENTRYPOINT)); }
        }
    }
}
