using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace WorkServer
{
    partial class String2
    {
        public void WriteStream(Stream stream)
        {
            WriteStream(stream, 0, Length);
        }
        public void WriteStream(Stream stream, int index, int length)
        {
            String2.WriteStream(this, stream, index, length);
        }
        public static void WriteStream(String2 data, Stream stream, int index, int length)
        {
            if (index + length > data.Length)
            {
                throw new IndexOutOfRangeException("WriteStream");
            }
            stream.Write(data.data, 0, length);
        }
        public String2 ReadStream(Stream stream, int length)
        {
            data = (byte[])String2.ReadStream(stream, Encode, length);
            return this;
        }
        public static String2 ReadStream(Stream stream, Encoding encode, int length)
        {
            String2 data = new String2(encode);
            String2 buffer = new String2(length, encode);
            int revlength = 0;
            int totallength = 0;
            while ((revlength = stream.Read((byte[])buffer, 0, length)) > 0)
            {
                data += buffer.SubString(0, revlength);
                totallength += revlength;
                if (totallength >= length)
                {
                    break;
                }
            }
            data.Encode = encode;
            return data;
        }
        public String2 ReadStream(Stream stream, String2 endFlag)
        {
            data = (byte[])ReadStream(stream, endFlag, String2.BUFFER_SIZE);
            return this;
        }
        public String2 ReadStream(Stream stream, String2 endflag, int buffersize)
        {
            data = (byte[])String2.ReadStream(stream, Encode, endflag, buffersize);
            return this;
        }
        public static String2 ReadStream(Stream stream, Encoding encode, String2 endFlag)
        {
            return String2.ReadStream(stream, encode, endFlag, String2.BUFFER_SIZE);
        }
        public static String2 ReadStream(Stream stream, Encoding encode, String2 endFlag, int buffersize)
        {
            String2 data = new String2(encode);
            String2 buffer = new String2(buffersize, encode);
            int revlength = 0;
            while ((revlength = stream.Read((byte[])buffer, 0, buffersize)) > 0)
            {
                data += buffer.SubString(0, revlength);
                Console.WriteLine(data);
                if (data.Length >= endFlag.Length && String2.CheckByte((byte[])data, data.Length - endFlag.Length, (byte[])endFlag, 0, endFlag.Length))
                {
                    break;
                }
            }
            data.Encode = encode;
            return data;
        }
    }
}
