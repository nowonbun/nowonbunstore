using System;
using System.Collections.Generic;
using LogisticsSystem.App_Code;
using LogisticsSystem.Models;
using System.Text;

namespace LogisticsSystem.Dao
{
    public class BoardDao : AbstractDao<Board>
    {
        /// <summary>
        /// 게시판 검색수
        /// Database - CompanyCode Binding NG!
        /// </summary>
        public int GetBoardCount()
        {
            ParameterInit();
            ParameterAdd("state", App_Code.Define.STATE_NORMAL);

            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" count(*) as count ");
            sb.Append(" FROM tbl_Board ");
            sb.Append(" WHERE state = @state ");
            return SelectCount(sb.ToString(), GetParameter());
        }
        /// <summary>
        /// 게시판 검색
        /// </summary>
        /// <param name="pageLimit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public IList<Board> SelectBoard(int pageLimit, int page)
        {
            ParameterInit();
            ParameterAdd("state", App_Code.Define.STATE_NORMAL);

            StringBuilder sb = new StringBuilder();
            sb.Append(" SELECT ");
            sb.Append(" TOP " + pageLimit.ToString() + " * ");
            sb.Append(" FROM tbl_Board ");
            sb.Append(" WHERE state = @state ");
            sb.Append(" AND idx not in ");
            sb.Append(" (SELECT TOP " + (pageLimit * (page - 1)).ToString() + " idx FROM tbl_Board WHERE state = @state order by num desc) ");
            sb.Append(" order by num desc");
            return Select(sb.ToString(), GetParameter());
        }

        public Int64 GetBoardMaxNum()
        {
            ParameterInit();
            ParameterAdd("state", Define.STATE_NORMAL);
            StringBuilder query = new StringBuilder();
            query.Append(" SELECT MAX(num) as num FROM tbl_board where state = @state ");
            return SelectCount(query.ToString(), GetParameter());
        }
        /// <summary>
        /// 게시판 입력 쿼리
        /// </summary>
        public int InsertBoard(Board entity)
        {
            return base.Insert(entity, "tbl_Board");
        }
        public Board SelectByIdx(Int64 idx)
        {
            StringBuilder query = new StringBuilder();
            ParameterInit();
            ParameterAdd("idx", idx);
            query.Append("SELECT * FROM tbl_board where idx = @idx order by idx desc");
            IList<Board> ret = base.Select(query.ToString(), GetParameter());
            if(ret.Count < 1)
            {
                return null;
            }
            return ret[0];
        }
        public int DeleteByIdx(Int64 idx)
        {
            StringBuilder query = new StringBuilder();
            ParameterInit();
            ParameterAdd("idx", idx);
            ParameterAdd("state", Define.STATE_DELETE);
            query.Append("UPDATE tbl_board set state = @state where idx = @idx");
            return base.Delete(query.ToString(), GetParameter());
        }
        public Int64 Modify(Board entity)
        {
            Int64 ret = 0;
            StringBuilder query = new StringBuilder();
            ParameterInit();
            ParameterAdd("idx", entity.Idx);
            ParameterAdd("state", Define.STATE_DELETE);
            query.Append("UPDATE tbl_board set state = @state where idx = @idx");
            if (base.Delete(query.ToString(), GetParameter()) > 0)
            {
                InsertBoard(entity);
                
                Comment comment = new Comment();
                ret = ScopeIndentity("tbl_board");
                CommentDao dao = FactoryDao.Instance().GetCommentDao();
                dao.Modify(entity.Idx,ret);
            }
            return ret;
        }
    }
}