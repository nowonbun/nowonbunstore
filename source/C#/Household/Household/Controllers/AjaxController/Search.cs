using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Household.Common;
using Household.Models.Bean;
using Household.Models.Master;
using System.Web.Security;
using Household.Filters;
using HouseholdORM;
using log4net;

namespace Household.Controllers
{
    public partial class AjaxController : AbstractController
    {
        [ResourceDao]
        protected IHshldDao hshldDao4;

        [HttpPost]
        public ActionResult Search(SearchBean model)
        {
            return WorkAjax(model, (item, ret) =>
            {
                SearchBean bean = item as SearchBean;
                if (UserSession == null)
                {
                    ret.Result = Define.LOGIN_ERROR;
                    return ret;
                }
                if (String.IsNullOrEmpty(bean.Year) ||
                String.IsNullOrEmpty(bean.Month))
                {
                    ret.Result = Define.RESULT_NG;
                    ret.Error = Message.DATA_EROR;
                    return ret;
                }
                DateTime dt = new DateTime(Convert.ToInt32(bean.Year), Convert.ToInt32(bean.Month), 1);
                IList<Hshld> hshldList = hshldDao4.SelectToInfoByDate(UserSession.Grpd, dt);
                IList<Hshld> hshldCreditList = hshldDao4.SelectToCreditByDate(UserSession.Grpd, dt.AddMonths(-1));
                SearchResultBean dataList = new SearchResultBean();

                //nomalList create
                foreach (Hshld hshld in hshldList)
                {
                    SearchResultBean.Node node = CreateNode(hshld);
                    if (String.Equals(hshld.Cd, "010"))
                    {
                        dataList.AddAccount(node);
                        node = (SearchResultBean.Node)node.Clone();
                        node.PriceNum = node.PriceNum * -1;

                    }
                    dataList.AddTotal(node);
                }

                //CreditList create
                foreach (Hshld hshldCredit in hshldCreditList)
                {
                    SearchResultBean.Node node = CreateNode(hshldCredit);
                    dataList.AddCredit(node);
                }

                //CreditList create
                dataList.CreditList = dataList.CreditList.OrderBy((node) => { return node.Date; }).ToList();
                dataList.CreditAmountNum = SumCreditTotal(dataList.CreditList);

                //Next coding is put the total of credit into the datalist.
                if (!Decimal.Zero.Equals(dataList.CreditAmountNum))
                {
                    dataList.AddTotal(CreateSumCreditNode(dataList.CreditAmountNum));
                }

                //AccountList create
                dataList.AccountList = dataList.AccountList.OrderBy((node) => { return node.Date; }).ToList();
                dataList.AccountAmountNum = hshldDao4.SelectSumAccountTotal(UserSession.Grpd, dt.AddMonths(1));


                dataList.IncomeAmountNum = IncomeTotal(dataList.TotalList);
                dataList.ExpendAmountNum = ExpendTotal(dataList.TotalList);

                //nomalList create
                if (!String.IsNullOrEmpty(bean.Day))
                {
                    dataList.TotalList = dataList.TotalList.Where(node => String.Equals(bean.Day, node.Day)).ToList();
                }
                if (!String.IsNullOrEmpty(bean.Type))
                {
                    dataList.TotalList = dataList.TotalList.Where(node => String.Equals(bean.Type, node.Tp)).ToList();
                }
                dataList.TotalList = dataList.TotalList.OrderBy((node) => { return node.Date; }).ToList();
                dataList.TotalAmountNum = SumTotal(dataList.TotalList);

                ret.Add("DATA", dataList);

                ret.Result = Define.RESULT_OK;
                return ret;
            });
        }
    }
}