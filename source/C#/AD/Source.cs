using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace ADListSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            String domain = "";
            String ou = "";
            String id = "";
            String pw = "";
            String groupname = "";
            PrincipalContext oPrincipalContext = new PrincipalContext(ContextType.Domain,
                                                                      domain,
                                                                      ou,
                                                                      ContextOptions.SimpleBind,
                                                                      id,
                                                                      pw);
            List<UserPrincipal> members = GetGroupMembers(oPrincipalContext, groupname);
            Output(members);
            Console.WriteLine("Please Any Key...");
            Console.ReadKey();
        }
        static List<UserPrincipal> GetGroupMembers(PrincipalContext oPrincipalContext, String group)
        {
            GroupPrincipal oGroupPrincipal = GroupPrincipal.FindByIdentity(oPrincipalContext, group);
            List<UserPrincipal> ret = new List<UserPrincipal>();
            if (oGroupPrincipal == null)
            {
                return ret;
            }
            PrincipalCollection coll = oGroupPrincipal.Members;
            IEnumerator<Principal> principal = coll.GetEnumerator();
            while (principal.MoveNext())
            {
                try
                {
                    if ("group".Equals(principal.Current.StructuralObjectClass.ToLower()))
                    {
                        ret.AddRange(GetGroupMembers(oPrincipalContext, principal.Current.SamAccountName));
                    }
                    else
                    {
                        UserPrincipal user = UserPrincipal.FindByIdentity(oPrincipalContext, principal.Current.SamAccountName);
                        if (user == null)
                        {
                            continue;
                        }
                        ret.Add(user);
                    }
                }
                catch (MultipleMatchesException)
                {
                    continue;
                }
                catch (PrincipalOperationException)
                {
                    continue;
                }
            }
            return ret;
        }
        static void Output(List<UserPrincipal> members)
        {

            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("Result");
            int rc = 0;
            int cc = 0;
            IRow row = null;
            //TITLE            
            row = sheet.CreateRow(rc++);
            row.CreateCell(cc++).SetCellValue("EmployeeId");
            row.CreateCell(cc++).SetCellValue("SamAccountName");
            row.CreateCell(cc++).SetCellValue("DisplayName");
            row.CreateCell(cc++).SetCellValue("GivenName");
            row.CreateCell(cc++).SetCellValue("MiddleName");
            row.CreateCell(cc++).SetCellValue("Surname");
            row.CreateCell(cc++).SetCellValue("EmailAddress");
            row.CreateCell(cc++).SetCellValue("VoiceTelephoneNumber");
            row.CreateCell(cc++).SetCellValue("AccountExpirationDate");
            row.CreateCell(cc++).SetCellValue("Description");
            row.CreateCell(cc++).SetCellValue("Enabled");
            //data;
            foreach (var m in members)
            {
                cc = 0;
                row = sheet.CreateRow(rc++);
                row.CreateCell(cc++).SetCellValue(m.EmployeeId);
                row.CreateCell(cc++).SetCellValue(m.SamAccountName);
                row.CreateCell(cc++).SetCellValue(m.DisplayName);
                row.CreateCell(cc++).SetCellValue(m.GivenName);
                row.CreateCell(cc++).SetCellValue(m.MiddleName);
                row.CreateCell(cc++).SetCellValue(m.Surname);
                row.CreateCell(cc++).SetCellValue(m.EmailAddress);
                row.CreateCell(cc++).SetCellValue(m.VoiceTelephoneNumber);
                row.CreateCell(cc++).SetCellValue(m.AccountExpirationDate ?? new DateTime(1));
                row.CreateCell(cc++).SetCellValue(m.Description);
                row.CreateCell(cc++).SetCellValue(m.Enabled==true);
            }
            using (FileStream fs = new FileStream("c:\\temp\\test.xls", FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }
        }
    }
}
