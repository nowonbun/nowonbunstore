using System;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace LogisticsSystem.App_Code
{
    public class LogWriter
    {
        private const String LogDirectoryPath = "C:\\LOG";
        private static LogWriter oLogInstance = null;
        public LogWriter()
        {
            DirectoryInfo DI = new DirectoryInfo(LogDirectoryPath);
            if (!DI.Exists)
            {
                DI.Create();
            }
        }
        public void LogWrite(String sLogString)
        {
            try
            {
                DateTime DT = DateTime.Now;
                FileInfo FI = new FileInfo(String.Format("{0}\\{1:0000}{2:00}{3:00}.txt",LogDirectoryPath, DT.Year, DT.Month, DT.Day));
                String cTime = String.Format("{0:00}:{1:00}:{2:00}:{3:000}", DT.Hour, DT.Minute, DT.Second, DT.Millisecond);
                using (FileStream FS = new FileStream(FI.FullName, FileMode.Append))
                {
                    byte[] pData = Encoding.Default.GetBytes(cTime + "\t" + sLogString + "\r\n");
                    FS.Write(pData, 0, pData.Length);
                    FS.Close();
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine("LogWriter Error - " + e.ToString());
            }
        }
        public void LogWrite(String ID,String sLogString)
        {
            try
            {
                DateTime DT = DateTime.Now;
                DirectoryInfo DI = new DirectoryInfo(String.Format("{0}\\{1:0000}{2:00}{3:00}", LogDirectoryPath,DT.Year, DT.Month, DT.Day));
                if (!DI.Exists)
                {
                    DI.Create();
                }
                FileInfo FI = new FileInfo(String.Format("{0}\\{1}.txt",DI.FullName, ID));
                String cTime = String.Format("{0:00}:{1:00}:{2:00}:{3:000}", DT.Hour, DT.Minute, DT.Second, DT.Millisecond);
                using(FileStream FS = new FileStream(FI.FullName, FileMode.Append))
                { 
                    byte[] pData = Encoding.Default.GetBytes(cTime + "\t" + sLogString + "\r\n");
                    FS.Write(pData, 0, pData.Length);
                    FS.Close();
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine("LogWriter Error - " + e.ToString());
            }
        }
        public void FunctionStart()
        {
            LogWrite(String.Format("▼▼▼▼▼{0}▼▼▼▼▼", new StackTrace(true).GetFrame(1).GetMethod().Name));
        }
        public void FunctionEnd()
        {
            LogWrite(String.Format("▲▲▲▲▲{0}▲▲▲▲▲", new StackTrace(true).GetFrame(1).GetMethod().Name));
        }
        public void FunctionLog()
        {
            LogWrite(String.Format("☆☆☆☆☆{0}☆☆☆☆☆", new StackTrace(true).GetFrame(1).GetMethod().Name));
        }
        public void LineLog()
        {
            LogWrite(String.Format("-------{0}-----", new StackTrace(true).GetFrame(1).GetFileLineNumber()));
        }
        public void FileLog()
        {
            LogWrite(String.Format("*****{0}*****", new StackTrace(true).GetFrame(1).GetFileName()));
        }
        public void MessageLog(String msg, String result)
        {
            LogWrite(String.Format("&&&&& MESSAGE : {0} ,DATA : {1}&&&&&", msg, result));
        }
        public void HandleLog(IntPtr data)
        {
            LogWrite(String.Format("+++++ HANDLE : {0} +++++", data));
        }
        public static LogWriter Instance()
        {
            if (oLogInstance == null)
            {
                oLogInstance = new LogWriter();
            }
            return oLogInstance;
        }
    }
}