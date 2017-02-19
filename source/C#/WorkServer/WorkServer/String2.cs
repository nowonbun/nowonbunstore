using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace WorkServer
{
    public sealed class String2 : IComparable, ICloneable
    {
        private byte[] data;
        private Encoding encode;
        private static int BUFFER_SIZE = 4096;
        public static String2 LF = new byte[] { 0x0A };
        public static String2 CRLF = new byte[] { 0x0D, 0x0A };
        public static String2 SPACE = new byte[] { 0x20 };
        public static String2 VACUUM = new byte[] { 0x00 };
        public String2(byte[] data) : this(data, Encoding.Default) { }
        public String2(Encoding encode) : this(new byte[0], encode) { }
        public String2(String data, Encoding encode) : this(encode.GetBytes(data), encode) { }
        public String2(String data) : this(data, Encoding.Default) { }
        public String2(int length) : this(length, Encoding.Default) { }
        public String2(int length, Encoding encode) : this(new byte[length], encode) { }
        public String2(byte[] data, Encoding encode)
        {
            this.data = data;
            this.encode = encode;
        }
        public static implicit operator String2(byte[] data)
        {
            return new String2(data);
        }
        public static implicit operator String2(string data)
        {
            return new String2(data);
        }
        public static explicit operator string(String2 data)
        {
            return data.ToString();
        }
        public static explicit operator byte[](String2 data)
        {
            return data.ToBytes();
        }
        public byte this[int index]
        {
            get
            {
                if (index >= Length)
                {
                    throw new IndexOutOfRangeException("String2 - Indexer");
                }
                return this.data[index];
            }
        }
        public Encoding Encode
        {
            get { return this.encode; }
            set { this.encode = value; }
        }
        public int Length
        {
            get { return this.data.Length; }
        }
        private class String2LinkNode
        {
            private String2 data = null;
            private String2LinkNode next = null;
            public String2 Data
            {
                get { return this.data; }
            }
            public String2LinkNode Next
            {
                get { return next; }
            }
            public String2LinkNode Add(String2 data)
            {
                this.data = data;
                this.next = new String2LinkNode();
                return this.next;
            }
            public bool IsNull()
            {
                object data = (object)this.data;
                return data == null;
            }
        }
        public static String2 operator +(String2 val1, byte[] val2)
        {
            return val1 + new String2(val2, val1.Encode);
        }
        public static String2 operator +(byte[] val1, String2 val2)
        {
            return new String2(val1, val2.Encode) + val2;
        }
        public static String2 operator +(String2 val1, string val2)
        {
            return val1 + new String2(val2, val1.Encode);
        }
        public static String2 operator +(string val1, String2 val2)
        {
            return new String2(val1, val2.Encode) + val2;
        }
        public static String2 operator +(String2 val1, String2 val2)
        {
            if (!object.Equals(val1.Encode, val2.Encode))
            {
                throw new FormatException("Two data is not accordance.");
            }
            byte[] buffer = new byte[val1.Length + val2.Length];
            Array.Copy(val1.ToBytes(), buffer, val1.Length);
            Array.Copy(val2.ToBytes(), 0, buffer, val1.Length, val2.Length);
            return new String2(buffer, val1.Encode);
        }
        public static bool operator ==(String2 val1, String2 val2)
        {
            object buffer = (object)val1;
            if (buffer == null)
            {
                return false;
            }
            return val1.Equals(val2);
        }
        public static bool operator !=(String2 val1, String2 val2)
        {
            object buffer = (object)val1;
            if (buffer == null)
            {
                return false;
            }
            return !val1.Equals(val2);
        }
        public bool Equals(string obj)
        {
            return Equals(new String2(obj, Encode));
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (!(obj is String2))
            {
                return false;
            }
            String2 temp = obj as String2;
            if (!object.Equals(Encode, temp.Encode))
            {
                return false;
            }
            if (Length != temp.Length)
            {
                return false;
            }
            int count = (Length / 2) + (Length % 2);
            for (int i = 0; i < count; i++)
            {
                if (this[i] != temp[i])
                {
                    return false;
                }
                if (this[Length - 1 - i] != temp[Length - 1 - i])
                {
                    return false;
                }
            }
            return true;
        }
        private static bool CheckByte(byte[] v1, int i1, byte[] v2, int i2, int len)
        {
            int count = (len / 2) + (len % 2);
            for (int i = 0; i < count; i++)
            {
                if (v1[i1 + i] != v2[i2 + i])
                {
                    return false;
                }
                if (v1[len - 1 - i] != v2[len - 1 - i])
                {
                    return false;
                }
            }
            return true;
        }
        public String2 SubString(int index, int length)
        {
            byte[] ret = new byte[length];
            Array.Copy(data, index, ret, 0, length);
            return new String2(ret, Encode);
        }
        public String2 Trim()
        {
            int spos = 0;
            int epos = 0;
            int count = (Length / 2) + (Length % 2);
            for (int i = 0; i < count; i++)
            {
                if (this[i] != 0x00 && this[i] != 0x20)
                {
                    spos = i;
                }
                if (this[Length - 1 - i] != 0x20 && this[Length - 1 - i] != 0x00)
                {
                    epos = Length - 1 - i;
                }
            }
            return SubString(spos, epos - spos);
        }
        public int IndexOf(string source)
        {
            return IndexOf(source, 0);
        }
        public int IndexOf(string source, int index)
        {
            return IndexOf(new String2(source, Encode), index);
        }
        public int IndexOf(String2 source)
        {
            return IndexOf(source, 0);
        }
        public int IndexOf(String2 source, int index)
        {
            if (!object.Equals(Encode, source.Encode))
            {
                throw new FormatException("Two data is not accordance.");
            }
            if (source.Length < 1)
            {
                return -1;
            }
            if (Length < source.Length + index)
            {
                return -1;
            }
            for (int i = index; i < Length - source.Length; i++)
            {
                if (String2.CheckByte(data, i, source.ToBytes(), 0, source.Length))
                {
                    return i;
                }
            }
            return -1;
        }
        public int IndexLastOf(String2 source)
        {
            if (!object.Equals(Encode, source.Encode))
            {
                throw new FormatException("Two data is not accordance.");
            }
            if (source.Length < 1)
            {
                return -1;
            }
            if (Length < source.Length)
            {
                return -1;
            }
            for (int i = Length - source.Length - 1; i >= 0; i--)
            {
                if (String2.CheckByte(data, i, source.ToBytes(), 0, source.Length))
                {
                    return i;
                }
            }
            return -1;
        }
        public String2 Replace(byte[] oldVal, byte[] newVal)
        {
            return Replace(new String2(oldVal, Encode), new String2(newVal, Encode));
        }
        public String2 Replace(string oldVal, string newVal)
        {
            return Replace(new String2(oldVal, Encode), new String2(newVal, Encode));
        }
        public String2 Replace(String2 oldVal, String2 newVal)
        {
            if (!object.Equals(Encode, oldVal.Encode) || !object.Equals(Encode, newVal.Encode))
            {
                throw new FormatException("Two data is not accordance.");
            }
            if (oldVal.Length < 1)
            {
                throw new ArgumentNullException("Replace");
            }
            if (Length < oldVal.Length)
            {
                return Copy();
            }
            int pre = 0;
            int peek = 0;
            String2LinkNode fin = new String2LinkNode();
            String2LinkNode node = fin;
            while ((peek = IndexOf(oldVal, pre + 1)) > -1)
            {
                node = node.Add(SubString(pre, peek - pre));
                node = node.Add(newVal);
                pre = peek;
            }
            node = node.Add(SubString(pre, peek - pre));
            String2 ret = fin.Data;
            while (fin.Next != null)
            {
                fin = fin.Next;
                if (fin.IsNull()) break;
                ret += fin.Data;
            }
            return ret;
        }
        public String2[] Split(byte[] source)
        {
            return Split(new String2(source, Encode));
        }
        public String2[] Split(string source)
        {
            return Split(new String2(source, Encode));
        }
        public String2[] Split(String2 source)
        {
            if (!object.Equals(Encode, source.Encode))
            {
                throw new FormatException("Two data is not accordance.");
            }
            if (source.Length < 1)
            {
                throw new ArgumentNullException("Split");
            }
            if (Length < source.Length)
            {
                return new String2[] { Copy() };
            }
            int count = 0;
            int pre = 0;
            int peek = 0;
            String2LinkNode fin = new String2LinkNode();
            String2LinkNode node = fin;
            while ((peek = IndexOf(source, pre)) > -1)
            {
                node = node.Add(SubString(pre, peek + source.Length - pre));
                pre = peek + source.Length;
                count++;
            }
            node = node.Add(SubString(pre, Length - pre));
            String2[] ret = new String2[count + 1];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = fin.Data;
                fin = fin.Next;
            }
            return ret;
        }
        public String2 Reverse()
        {
            byte[] ret = new byte[Length];
            Array.Copy(data, ret, Length);
            Array.Reverse(ret);
            return new String2(ret, Encode);
        }
        public String2 Clear()
        {
            this.data = new byte[0];
            return this;
        }
        public void RemoveVacuum()
        {
            this.data = (byte[])Replace(VACUUM, "");
        }
        public int CompareTo(object obj)
        {
            return Equals(obj) ? 0 : -1;
        }
        public String2 Copy()
        {
            return (String2)Clone();
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public String2 ToBinary()
        {
            String2 ret = new String2(new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 }, Encode);
            for (int i = 0; i < 0x10; i++)
            {
                ret += String.Format(" 0x{0:X2}", i);
            }
            ret += CRLF;
            for (int i = 1, j = 0x00; i < Length; i++, j += 0x10)
            {
                --i;
                bool header = true;
                ret += CRLF;
                ret += String.Format("0x{0:X2}    ", j);
                for (; i < Length && (i % 0x10 == 0 && !header); i++)
                {
                    ret += String.Format("0x{0:X2}", this[i]);
                    header = false;
                }
            }
            ret += CRLF;
            return ret;
        }
        public byte[] ToBytes()
        {
            return data;
        }
        public override string ToString()
        {
            return Encode.GetString(data);
        }

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
