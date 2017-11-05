using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace LogisticsSystem.App_Code
{
    public enum LanguageType
    {
        Korea,
        Japan,
    }
    public class LanguagePack : Dictionary<String, String>
    {
        public LanguagePack(String pPath, String Controller, String Action, LanguageType? pType)
        {
            if(Controller == null || Action == null)
            {
                return;
            }
            String pStep = "";
            String key = "";
            String sPath = String.Format("{0}/{1}/{2}.xml", pPath, Controller, Action);
            if (!File.Exists(sPath))
            {
                LogWriter.Instance().LogWrite("LanguagePack File is nothing");
                LogWriter.Instance().LogWrite("Path - " + sPath);
                return;
            }
            XmlTextReader xtr = null;
            try
            {
                xtr = new XmlTextReader(sPath);
                while (xtr.Read())
                {
                    switch (xtr.NodeType)
                    {
                        case XmlNodeType.Element:
                            pStep = xtr.Name;
                            if (pStep == "column")
                            {
                                key = xtr.GetAttribute("key");
                            }
                            break;
                        case XmlNodeType.Text:
                            if ((pType == LanguageType.Korea && "korea".Equals(pStep)) ||
                                (pType != LanguageType.Korea && "japan".Equals(pStep)))
                            {
                                Set(key, xtr.Value);
                            }
                            break;
                        case XmlNodeType.EndElement:
                            pStep = "";
                            if (pStep == "column")
                            {
                                key = "";
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
                if(xtr != null)
                {
                    xtr.Close();
                }
            }
        }
        private void Set(String key,String value)
        {
            if (ContainsKey(key))
            {
                this[key] = value;
            }
            else
            {
                Add(key, value);
            }
        }
    }
}