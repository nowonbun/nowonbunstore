using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Household.Common;
using Household.Models.Bean;
using System.Web.Mvc;

namespace Household.Controllers
{
    public class SearchFlow : AbstractFlow
    {
        private SearchBean model;

        public SearchFlow(HttpRequestBase request, HttpResponseBase response, HttpContextBase context, SearchBean model)
            : base(request, response, context)
        {
            this.model = model;
        }
        public override bool Validate()
        {
            if (UserSession == null)
            {
                ResultBean.Result = Define.LOGIN_ERROR;
                return false;
            }
            if (String.IsNullOrEmpty(model.Year) ||
            String.IsNullOrEmpty(model.Month))
            {
                ResultBean.Result = Define.RESULT_NG;
                ResultBean.Error = Message.DATA_EROR;
                return false;
            }
            return true;
        }

        public override ActionResult Run()
        {
            ResultBean.Result = Define.RESULT_OK;
            return Json(ResultBean, JsonRequestBehavior.AllowGet);
        }

        public override ActionResult Error()
        {
            return null;
        }



        private Decimal SumCreditTotal(IList<SearchResultBean.Node> list)
        {
            if (list.Count < 1)
            {
                return Decimal.Zero;
            }
            return list.Sum((node) =>
            {
                return node.PriceNum;
            });
        }

        private Decimal IncomeTotal(IList<SearchResultBean.Node> list)
        {
            if (list.Count < 1)
            {
                return Decimal.Zero;
            }
            return list.Where(node => String.Equals(node.Cd, "000") && String.Equals(node.Tp, "001")).Sum((node) =>
            {
                return node.PriceNum;
            });
        }

        private Decimal ExpendTotal(IList<SearchResultBean.Node> list)
        {
            if (list.Count < 1)
            {
                return Decimal.Zero;
            }
            return list.Where(node => String.Equals(node.Cd, "000") && !String.Equals(node.Tp, "001")).Sum((node) =>
            {
                return node.PriceNum;
            });
        }

        private Decimal SumTotal(IList<SearchResultBean.Node> list)
        {
            if (list.Count < 1)
            {
                return Decimal.Zero;
            }
            return list.Where((node) =>
            {
                return !String.Equals(node.Cd, "020");
            }).Sum((node) =>
            {
                return node.PriceNum;
            });
        }

    }
}