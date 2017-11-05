using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticsSystem.App_Code;
using LogisticsSystem.Models;
using System.Text;

namespace LogisticsSystem.Dao
{
    public class UserInfoDao : AbstractDao<UserInfo>
    {
        /// <summary>
        /// 인증
        /// </summary>
        /// <returns></returns>
        public UserInfo SelectUserInfo(String userid, String password)
        {
            base.ParameterInit();
            base.ParameterAdd("@id", userid);
            base.ParameterAdd("@pw", password);
            base.ParameterAdd("@state", Define.STATE_NORMAL);

            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT * ");
            sb.Append(" From tbl_UserInfo WHERE userid=@id and password=@pw and state=@state ");
            IList<UserInfo> ret = Select(sb.ToString(), GetParameter());
            if (ret.Count < 1)
            {
                return null;
            }
            return ret[0];

        }
        /// <summary>
        /// 유저정보 수정
        /// Database - CompanyCode Binding OK!
        /// </summary>
        public UserInfo ModifyUserInfo(UserInfo entity, LanguageType? lType, String comp)
        {
            ParameterInit();
            ParameterAdd("idx", entity.Idx);
            ParameterAdd("companycode", comp);
            StringBuilder query = new StringBuilder();
            query.Append(" UPDATE ");
            query.Append(" tbl_UserInfo ");
            query.Append(" set state = 1 ");
            query.Append(" where companycode = @companycode and idx=@idx");

            base.Delete(query.ToString(), GetParameter());

            Insert(entity, "tbl_UserInfo");

            ParameterInit();
            ParameterAdd("idx", ScopeIndentity("tbl_UserInfo"));
            ParameterAdd("companycode", comp);
            query.Append(" SELECT ");
            query.Append(" * ");
            query.Append(" FROM tbl_UserInfo where idx=@idx and companycode = @companycode ");
            UserInfo ret = Select(query.ToString(), GetParameter())[0];
            ret.NumberJoin();
            return ret;
        }
    }
}