using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HouseholdORM;
using Household.Common;
using Household.Models.Bean;
using System.Web.Mvc;
using Household.Models.Master;

namespace Household.Controllers
{
    public class SearchFlow : AbstractFlow
    {
        [ResourceDao]
        protected IHshldDao hshldDao;

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

            DateTime dt = new DateTime(Convert.ToInt32(model.Year), Convert.ToInt32(model.Month), 1);
            IList<Hshld> hshldList = hshldDao.SelectToInfoByDate(UserSession.Grpd, dt);
            IList<Hshld> hshldCreditList = hshldDao.SelectToCreditByDate(UserSession.Grpd, dt.AddMonths(-1));
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
            dataList.AccountAmountNum = hshldDao.SelectSumAccountTotal(UserSession.Grpd, dt.AddMonths(1));


            dataList.IncomeAmountNum = IncomeTotal(dataList.TotalList);
            dataList.ExpendAmountNum = ExpendTotal(dataList.TotalList);

            //nomalList create
            if (!String.IsNullOrEmpty(model.Day))
            {
                dataList.TotalList = dataList.TotalList.Where(node => String.Equals(model.Day, node.Day)).ToList();
            }
            if (!String.IsNullOrEmpty(model.Type))
            {
                dataList.TotalList = dataList.TotalList.Where(node => String.Equals(model.Type, node.Tp)).ToList();
            }
            dataList.TotalList = dataList.TotalList.OrderBy((node) => { return node.Date; }).ToList();
            dataList.TotalAmountNum = SumTotal(dataList.TotalList);

            ResultBean.Add("DATA", dataList);

            ResultBean.Result = Define.RESULT_OK;
            return Json(ResultBean, JsonRequestBehavior.AllowGet);
        }

        public override ActionResult Error()
        {
            return null;
        }

        private SearchResultBean.Node CreateNode(Hshld entity)
        {
            SearchResultBean.Node node = new SearchResultBean.Node();
            node.Idx = entity.Ndx;
            node.Tp = entity.Tp;
            node.Cd = entity.Cd;
            node.Date = entity.Dt;
            node.Day = entity.Dt.Day.ToString();
            node.Type = FactoryMaster.Instance().GetTypeMaster().GetTypeNameByCode(entity.Tp);
            node.Category = FactoryMaster.Instance().GetCategoryMaster().GetCategoryNameByCode(entity.Cd);
            node.Content = entity.Cntxt;
            node.PriceNum = Util.GetPlusMinus(entity.Tp) ? entity.Prc : entity.Prc * -1;
            node.Pdt = entity.Pdt.ToString(Define.PDT_FORMAT);
            return node;
        }
        private SearchResultBean.Node CreateSumCreditNode(Decimal price)
        {
            SearchResultBean.Node node = new SearchResultBean.Node();
            node.Day = "--";
            node.Type = FactoryMaster.Instance().GetTypeMaster().GetTypeNameByCode("002");
            node.Category = FactoryMaster.Instance().GetCategoryMaster().GetCategoryNameByCode("020");
            node.Content = FactoryMaster.Instance().GetCategoryMaster().GetCategoryNameByCode("020");
            node.PriceNum = price;
            return node;
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