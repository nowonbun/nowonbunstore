using System;
using System.Collections.Generic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.IO;
using System.Text;

namespace LogisticsSystem.App_Code
{
    public class PDFCreate:IDisposable
    {
        private Document document;
        private PdfWriter writer;
        private HTMLWorker worker;
        private MemoryStream ms;

        public PDFCreate()
        {
            //document = new Document(PageSize.A4.Rotate());
            document = new Document(PageSize.A4);
            ms = new MemoryStream();
            writer = PdfWriter.GetInstance(document, ms);//new FileStream("d:\\abc.pdf",FileMode.Create,FileAccess.Write));
            worker = new HTMLWorker(document);
            Dictionary<string, object> dicProvider = new Dictionary<string, object>();
            dicProvider.Add(HTMLWorker.FONT_PROVIDER, new NewFontProvider());
            worker.SetProviders(dicProvider);
        }
        public void Dispose()
        {
            worker.Dispose();
            writer.Dispose();
            ms.Dispose();
            document.Dispose();
        }
        public void Open()
        {
            document.Open();
            worker.StartDocument();
            document.NewPage();
        }
        public void Close()
        {
            worker.EndDocument();
            worker.Close();
            document.Close();
        }
        protected byte[] SetParsing(byte[] pData,Dictionary<String, Object> parameter)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Encoding.UTF8.GetString(pData));
            foreach (String key in parameter.Keys)
            {
                sb.Replace("##"+key+"##", parameter[key].ToString());
            }

            return Encoding.UTF8.GetBytes(sb.ToString());

        }
        protected String SetParsing2(byte[] pData, Dictionary<String, Object> parameter)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Encoding.UTF8.GetString(pData));
            foreach (String key in parameter.Keys)
            {
                sb.Replace(key, parameter[key].ToString());
            }

            return sb.ToString();

        }
        public Stream PdfCreater(String samplePath,Dictionary<String,Object> parameter)
        {
            using (FileStream fs = new FileStream(samplePath, FileMode.Open, FileAccess.Read))
            {
                byte[] pBuffer = new byte[fs.Length];
                fs.Read(pBuffer, 0, pBuffer.Length);
                using (MemoryStream buffer = new MemoryStream(SetParsing(pBuffer, parameter)))
                {
                    using (StreamReader reader = new StreamReader(buffer))
                    {
                        worker.Parse(reader);
                        reader.Close();
                    }
                }
                fs.Close();
            }
            return ms;
        }
        public class NewFontProvider : FontFactoryImp
        {
            public override Font GetFont(string fontname, string encoding, bool embedded, float size, int style, BaseColor color, bool cached)
            {
                if (string.IsNullOrEmpty(fontname))
                {
                    fontname = "c:\\windows\\fonts\\msmincho.ttc,0";
                    //fontname = "c:\\NotoSansCJK.ttc,0";
                    encoding = BaseFont.IDENTITY_H;
                    embedded = BaseFont.EMBEDDED;
                }
                else if ("korea".Equals(fontname))
                {
                    fontname = "c:\\windows\\fonts\\batang.ttc,0";
                    //fontname = "c:\\NotoSansCJK.ttc,0";
                    encoding = BaseFont.IDENTITY_H;
                    embedded = BaseFont.EMBEDDED;
                }
                else if ("japan".Equals(fontname))
                {
                    //fontname = "c:\\windows\\fonts\\msgothic.ttc,0";
                    fontname = "c:\\windows\\fonts\\msmincho.ttc,0";
                    encoding = BaseFont.IDENTITY_H;
                    embedded = BaseFont.EMBEDDED;
                }

                return base.GetFont(fontname, encoding, embedded, size, style, color, cached);
            }
        }
    }
}