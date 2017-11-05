using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LogisticsSystem.Models;
using LogisticsSystem.App_Code;
using LogisticsSystem.Dao;


namespace LogisticsSystem.Controllers
{
    public class BoardController : AbstractController
    {
        private const int PAGELIMIT = 10000;
        private BoardDao boardDao = FactoryDao.Instance().GetBoardDao();
        private CommentDao commentDao = FactoryDao.Instance().GetCommentDao();

        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult List()
        {
            LogWriter.Instance().LogWrite("/Board/List 접속");
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                LogWriter.Instance().LogWrite("/Board/List 에서 세션 만료로 에러가 발생합니다.");
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Board/List 인증 에러");
                return base.Logout();
            }

            int count = Convert.ToInt32(Math.Ceiling((Double)((Double)boardDao.GetBoardCount() / (Double)PAGELIMIT)));
            IList<Board> list = boardDao.SelectBoard(PAGELIMIT, 1);
            ViewBag.listcount = count;
            list.AsParallel().ForAll((board) =>
            {
                board.Title += " (" + commentDao.GetCommentCount(board.Idx).ToString() + ")";
            });
            ViewBag.list = list;

            Session[Define.Session.CONTROLLER] = "Board";
            Session[Define.Session.ACTION] = "List";
            return View("~/Views/Board/Web/List.cshtml", Define.MASTER_VIEW);
        }
        [PipelineFilter]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult Create()
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                LogWriter.Instance().LogWrite("/Board/List 에서 세션 만료로 에러가 발생합니다.");
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Board/Create - Error");
                return base.Logout();
            }

            ViewBag.user = UserSession.UserId;
            Session[Define.Session.CONTROLLER] = "Board";
            Session[Define.Session.ACTION] = "List";
            return View("~/Views/Board/Web/Create.cshtml", Define.MASTER_VIEW);
        }
        [PipelineFilter]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult ModifyView(Int64 idx)
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                LogWriter.Instance().LogWrite("/Board/List 에서 세션 만료로 에러가 발생합니다.");
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Board/ModifyView - Error");
                return base.Logout();
            }

            Board board = boardDao.SelectByIdx(idx);

            ViewBag.board = board;
            ViewBag.user = UserSession.UserId;
            ViewBag.comment = commentDao.SelectComment(idx);
            Session[Define.Session.CONTROLLER] = "Board";
            Session[Define.Session.ACTION] = "List";
            return View("~/Views/Board/Web/Create.cshtml", Define.MASTER_VIEW);
        }
        [PipelineFilter]
        [AuthorizeFilter]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Insert(Board board)
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                LogWriter.Instance().LogWrite("/Board/List 에서 세션 만료로 에러가 발생합니다.");
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Board/Insert - Error");
                return base.Logout();
            }

            board.Creater = UserSession.UserId;
            board.CreateDate = DateTime.Now;
            board.State = Define.STATE_NORMAL.ToString();
            if (!String.IsNullOrEmpty(board.Title.Trim()))
            {
                board.Num = boardDao.GetBoardMaxNum();
                boardDao.InsertBoard(board);
            }
            return Redirect("/Board/List");

        }
        [PipelineFilter]
        [AuthorizeFilter]
        public ActionResult View(Int64 idx)
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                LogWriter.Instance().LogWrite("/Board/List 에서 세션 만료로 에러가 발생합니다.");
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Board/View - Error");
                return base.Logout();
            }

            Board board = boardDao.SelectByIdx(idx);
            ViewBag.board = board;
            Session[Define.Session.CONTROLLER] = "Board";
            Session[Define.Session.ACTION] = "List";
            ViewBag.user = UserSession.UserId;
            ViewBag.comment = commentDao.SelectComment(idx);
            ViewBag.modifyCheck = Object.Equals(board.Creater, UserSession.UserId);
            return View("~/Views/Board/Web/View.cshtml", Define.MASTER_VIEW);
        }

        [PipelineFilter]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult Delete(Int64 idx)
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                LogWriter.Instance().LogWrite("/Board/List 에서 세션 만료로 에러가 발생합니다.");
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Board/Delete - Error");
                return base.Logout();
            }
            boardDao.DeleteByIdx(idx);
            return Redirect("/Board/List");
        }
        [PipelineFilter]
        [AuthorizeFilter]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Modify(Board board)
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                LogWriter.Instance().LogWrite("/Board/List 에서 세션 만료로 에러가 발생합니다.");
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Board/Modify - Error");
                return base.Logout();
            }
            board.Creater = UserSession.UserId;
            board.CreateDate = DateTime.Now;
            board.State = Define.STATE_NORMAL.ToString();
            Int64 index = boardDao.Modify(board);
            return Redirect("/Board/View?idx=" + index);
        }

        [PipelineFilter]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult CommentApply(Comment comment)
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                LogWriter.Instance().LogWrite("/Board/List 에서 세션 만료로 에러가 발생합니다.");
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Board/CommentApply - Error");
                return base.Logout();
            }
            comment.Creater = UserSession.UserId;
            comment.CreateDate = DateTime.Now;
            comment.State = Define.STATE_NORMAL.ToString();
            if (!String.IsNullOrEmpty(comment.Context.Trim()))
            {
                commentDao.InsertComment(comment);
            }
            return Redirect("/Board/View?idx=" + comment.BoardIdx);

        }
        [PipelineFilter]
        [AuthorizeFilter]
        [HttpPost]
        public ActionResult CommentDelete(Int64 idx, int boardidx)
        {
            if (!SessionCheck(Define.Session.SESSION_CHECK))
            {
                LogWriter.Instance().LogWrite("/Board/List 에서 세션 만료로 에러가 발생합니다.");
                return ErrorPage("/Home/Error");
            }
            if (!CheckAuth())
            {
                LogWriter.Instance().LogWrite(UserSession.UserId, "/Board/CommentApply - Error");
                return base.Logout();
            }
            commentDao.DeleteByIdx(idx);
            return Redirect("/Board/View?idx=" + boardidx);
        }
    }
}
