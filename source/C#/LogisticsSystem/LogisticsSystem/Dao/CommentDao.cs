using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticsSystem.App_Code;
using LogisticsSystem.Models;
using System.Text;

namespace LogisticsSystem.Dao 
{
    public class CommentDao : AbstractDao<Comment>
    {
        /// <summary>
        /// 댓글 검색수
        /// Database - CompanyCode Binding NG!
        /// </summary>
        public int GetCommentCount(Int64 boardidx)
        {
            ParameterInit();
            ParameterAdd("state", App_Code.Define.STATE_NORMAL);
            ParameterAdd("boardidx", boardidx);

            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" count(*) as count ");
            sb.Append(" FROM tbl_BoardComment ");
            sb.Append(" WHERE state = @state ");
            sb.Append(" AND boardidx = @boardidx ");
            return SelectCount(sb.ToString(), GetParameter());
        }
        /// <summary>
        /// 댓글 검색
        /// </summary>
        /// <param name="pageLimit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public IList<Comment> SelectComment(Int64 boardidx)
        {
            ParameterInit();
            ParameterAdd("state", App_Code.Define.STATE_NORMAL);
            ParameterAdd("boardidx", boardidx);

            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT * ");
            sb.Append(" FROM tbl_BoardComment ");
            sb.Append(" WHERE state = @state ");
            sb.Append(" AND boardidx = @boardidx ");
            sb.Append(" order by idx asc");
            return Select(sb.ToString(), GetParameter());
        }
        /// <summary>
        /// 댓글 입력 쿼리
        /// </summary>
        public void InsertComment(Comment entity)
        {
            base.Insert(entity, "tbl_BoardComment");
        }
        public int DeleteByIdx(Int64 idx)
        {
            StringBuilder query = new StringBuilder();
            ParameterInit();
            ParameterAdd("idx", idx);
            ParameterAdd("state", Define.STATE_DELETE);
            query.Append("UPDATE tbl_BoardComment set state = @state where idx = @idx");
            return base.Delete(query.ToString(), GetParameter());
        }
        public int Modify(Int64 preIdx, Int64 afterIdx)
        {
            StringBuilder query = new StringBuilder();
            ParameterInit();
            ParameterAdd("preIdx", preIdx);
            ParameterAdd("afterIdx", afterIdx);
            ParameterAdd("state", Define.STATE_DELETE);
            query.Append("UPDATE tbl_BoardComment set boardidx = @afterIdx where boardidx = @preIdx");
            return base.Update(query.ToString(), GetParameter());
        }
    }
}