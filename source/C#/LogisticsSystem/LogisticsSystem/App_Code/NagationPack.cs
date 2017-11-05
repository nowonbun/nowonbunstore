using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace LogisticsSystem.App_Code
{
    public class NaviNode
    {
        public String URL { get; set; }
        public String Name { get; set; }
    }
    public class NavigationPack : List<NaviNode>
    {
        public NavigationPack(String ServerPath, String Controller, String Action, LanguageType? pType)
        {
            if (Controller == null || Action == null)
            {
                return;
            }
            String sParentkey = null;
            String controllkey = Controller;
            String actionkey = Action;
            do
            {
                controllkey = controllkey.ToLower();
                actionkey = actionkey.ToLower();
                sParentkey = Search(ServerPath, controllkey, actionkey, pType);
                if (sParentkey != null)
                {
                    String[] pData = sParentkey.Split('/');
                    if (pData.Length == 3)
                    {
                        controllkey = pData[1];
                        actionkey = pData[2];
                    }
                    else if (pData.Length == 2)
                    {
                        controllkey = pData[0];
                        actionkey = pData[1];
                    }
                    else
                    {
                        LogWriter.Instance().LogWrite("NavigationPack is Parentkey Error - " + sParentkey);
                        sParentkey = null;
                    }
                }
            } while (sParentkey != null);
        }
        protected String Search(String ServerPath, String controllkey, String actionkey, LanguageType? pType)
        {
            bool pNaviOpen = false;
            String sParentkey = null;
            String Step = "";
            NaviNode pNode = null;

            String sPath = String.Format("{0}/Navigation.xml", ServerPath);
            if (!File.Exists(sPath))
            {
                LogWriter.Instance().LogWrite("NavigationPack File is nothing");
                return null;
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
                            if ("navi".Equals(xtr.Name))
                            {
                                if (controllkey.Equals(xtr.GetAttribute("controllkey").ToLower())
                                    && actionkey.Equals(xtr.GetAttribute("actionkey").ToLower()))
                                {
                                    pNaviOpen = true;
                                    sParentkey = xtr.GetAttribute("parentKey");
                                    pNode = new NaviNode();
                                }
                            }
                            else if (pNaviOpen)
                            {
                                Step = xtr.Name;
                            }
                            break;
                        case XmlNodeType.Text:
                            if (pNaviOpen)
                            {
                                if ("url".Equals(Step))
                                {
                                    pNode.URL = xtr.Value;
                                }
                                else if (pType == LanguageType.Korea && "korea".Equals(Step))
                                {
                                    pNode.Name = xtr.Value;
                                }
                                else if (pType != LanguageType.Korea && "japan".Equals(Step))
                                {
                                    pNode.Name = xtr.Value;
                                }
                            }
                            break;
                        case XmlNodeType.EndElement:
                            if ("navi".Equals(xtr.Name) && pNaviOpen)
                            {
                                Add(pNode);
                                xtr.Close();
                                return sParentkey;
                            }
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                LogWriter.Instance().LogWrite("NavigationPack Error - " + e.ToString());
            }
            finally
            {
                if (xtr != null)
                {
                    xtr.Close();
                }
            }
            xtr.Close();
            return sParentkey;
        }
        public override string ToString()
        {
            String pRet = "";
            for (int i = Count - 1; i >= 0; i--)
            {
                //if (this[i].URL != null && this[i].URL.Length > 0)
                //{
                //    pRet += "<a href='" + this[i].URL + "'>";
                //    pRet += this[i].Name;
                //    pRet += "</a>&nbsp;&nbsp;>&nbsp;&nbsp;";
                //}
                //else
                //{
                    pRet += this[i].Name + "&nbsp;&nbsp;>&nbsp;&nbsp;";
                //}
            }
            return pRet;
        }
    }
}