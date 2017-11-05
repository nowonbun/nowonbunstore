using System;
using System.Collections.Generic;

public class RazorApi
{
    private static RazorApi oLogInstance;
    public static RazorApi Instance()
    {
        if (oLogInstance == null)
        {
            oLogInstance = new RazorApi();
        }
        return oLogInstance;
    }
    public String DisplayView(dynamic ViewBag,String key)
    {
        Dictionary<String, String> pDic = ViewBag.Disp;
        if (pDic!= null && pDic.ContainsKey(key))
        {
            return pDic[key];
        }
        return "";
    }
    public String DisplayView(dynamic ViewBag, String key, String comment) 
    {
        return DisplayView(ViewBag, key);
    }
    public String MasterView(dynamic ViewBag, String key)
    {
        Dictionary<String, String> pDic = ViewBag.Master;
        if (pDic != null && pDic.ContainsKey(key))
        {
            return pDic[key];
        }
        return "";
    }
}