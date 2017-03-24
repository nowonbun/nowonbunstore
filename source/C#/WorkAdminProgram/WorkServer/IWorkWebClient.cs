using System;
using System.IO;

namespace WorkServer
{
    public interface IWorkWebClient
    {
        String Path { get; }

        String Parameter { get; }

        Exception LastException { get; }

        void Initialize(String2 header);

        void Run(ResponeCode code, FileInfo file, bool fileoption = false);
    }
}
