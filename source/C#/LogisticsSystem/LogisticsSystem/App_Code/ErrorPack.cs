using System;
using System.Xml;

namespace LogisticsSystem.App_Code
{
    public class ErrorPack
    {
        public static String GetError(String ServerPath, String ErrorCode, LanguageType? pType)
        {
            String pStep = "";
            String pMsg = "";
            bool ErrerOpen = false;
            XmlTextReader xtr = null;
            try
            {
                xtr = new XmlTextReader(ServerPath);
                while (xtr.Read())
                {
                    switch (xtr.NodeType)
                    {
                        case XmlNodeType.Element:
                            pStep = xtr.Name;
                            if (pStep == "ErrorCode")
                            {
                                if (xtr.GetAttribute("key").Equals("ERR" + ErrorCode))
                                {
                                    ErrerOpen = true;
                                }
                            }
                            break;
                        case XmlNodeType.Text:
                            if (ErrerOpen)
                            {
                                if ((pType == LanguageType.Korea && "korea".Equals(pStep)) ||
                                    (pType != LanguageType.Korea && "japan".Equals(pStep)))
                                {
                                    pMsg = xtr.Value;
                                }
                            }
                            break;
                        case XmlNodeType.EndElement:
                            if (xtr.Name.Equals("ErrorCode") && ErrerOpen)
                            {
                                xtr.Close();
                                return pMsg;
                            }
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                LogWriter.Instance().LogWrite("LanguagePack Error - " + e.ToString());
            }
            finally
            {
                if (xtr != null)
                {
                    xtr.Close();
                }
            }
            return "";
        }
    }
}