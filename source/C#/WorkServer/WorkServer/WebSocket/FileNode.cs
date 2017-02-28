using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WorkServer
{
    public class FileNode
    {
        public Stream StreamBuffer { get; private set; }
        public int Peek { get; set; }
        public int Length { get; set; }
        public bool Open { get; set; }
        public FileNode()
        {
            Init();
        }
        public void Init()
        {
            if (StreamBuffer != null)
            {
                StreamBuffer.Close();
            }
            StreamBuffer = null;
            Peek = 0;
            Length = 0;
            Open = false;
        }
        public void Complete()
        {
            if (StreamBuffer != null)
            {
                StreamBuffer.Flush();
            }
            Init();
        }
        public void SetStream(FileStream stream, int length)
        {
            this.StreamBuffer = stream;
            this.Peek = 0;
            this.Length = length;
            this.Open = true;
        }
        public static FileNode GetFileNode()
        {
            return new FileNode();
        }
    }
}
