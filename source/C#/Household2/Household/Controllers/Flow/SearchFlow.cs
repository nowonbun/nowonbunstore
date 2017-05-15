using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Household.Common;
using Household.Models.Bean;
using System.Web.Mvc;
using Newtonsoft.Json;
using Household.Models.Entity;
using Household.Models.Master;

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
            String json = HttpConnector.GetInstance().GetDataRequest("GetHouseholdList", new Dictionary<String, String>()
            {
                {"GID",UserSession.Id},
                {"YEAR",model.Year},
                {"MONTH",model.Month}
            });
            List<HouseHold> householdList = JsonConvert.DeserializeObject<List<HouseHold>>(json);
            json = HttpConnector.GetInstance().GetDataRequest("GetHouseholdList2", new Dictionary<String, String>()
            {
                {"GID",UserSession.Id},
                {"YEAR",model.Year},
                {"MONTH",model.Month},
                {"CATEGORY","020"}
            });
            List<HouseHold> creditList = JsonConvert.DeserializeObject<List<HouseHold>>(json);
            Decimal accountSum = Decimal.Parse(HttpConnector.GetInstance().GetDataRequest("SumHousehold", new Dictionary<String, String>()
            {
                {"GID",UserSession.Id},
                {"CD","010"},
                {"TP","011"}
            })) - Decimal.Parse(HttpConnector.GetInstance().GetDataRequest("SumHousehold", new Dictionary<String, String>()
            {
                {"GID",UserSession.Id},
                {"CD","010"},
                {"TP","012"}
            }));
            SearchResultBean result = new SearchResultBean();
            //nomalList create
            foreach (HouseHold hshld in householdList.OrderBy(node => node.Date))
            {
                SearchResultBean.Node node = CreateNode(hshld);
                if (String.Equals(hshld.Ctgry.Cd, "010"))
                {
                    result.AddAccount(node);
                    node = (SearchResultBean.Node)node.Clone();
                    node.PriceNum = node.PriceNum * -1;
                }
                result.AddTotal(node);
            }
            //crediteList Create
            result.CreditList = creditList.OrderBy(node => node.Date).Select(node => CreateNode(node)).ToList();
            result.CreditAmountNum = SumCreditTotal(result.CreditList);

            //Next coding is put the total of credit into the datalist.
            if (!Decimal.Zero.Equals(result.CreditAmountNum))
            {
                result.AddTotal(CreateSumCreditNode(result.CreditAmountNum));
            }
            //AccountList create
            result.AccountList = result.AccountList.OrderBy((node) => { return node.Date; }).ToList();
            result.AccountAmountNum = accountSum;

            result.IncomeAmountNum = IncomeTotal(result.TotalList);
            result.ExpendAmountNum = ExpendTotal(result.TotalList);

            //nomalList create
            if (!String.IsNullOrEmpty(model.Day))
            {
                result.TotalList = result.TotalList.Where(node => String.Equals(model.Day, node.Day)).ToList();
            }
            if (!String.IsNullOrEmpty(model.Type))
            {
                result.TotalList = result.TotalList.Where(node => String.Equals(model.Type, node.Tp)).ToList();
            }
            result.TotalList = result.TotalList.OrderBy((node) => { return node.Date; }).ToList();
            result.TotalAmountNum = SumTotal(result.TotalList);

            ResultBean.Result = Define.RESULT_OK;
            ResultBean.Add("DATA", result);

            return Json(ResultBean, JsonRequestBehavior.AllowGet);
        }

        public override ActionResult Error()
        {
            return null;
        }

        private SearchResultBean.Node CreateNode(HouseHold entity)
        {
            SearchResultBean.Node node = new SearchResultBean.Node();
            node.Idx = entity.Index;
            node.Tp = entity.Tp.Tp;
            node.Cd = entity.Ctgry.Cd;
            node.Date = entity.Date;
            node.Day = entity.Date.Day.ToString();
            node.Type = FactoryMaster.Instance().GetTypeMaster().Where(item => String.Equals(item.Tp, entity.Tp.Tp)).Single().Nm;
            node.Category = FactoryMaster.Instance().GetCategoryMaster().Where(item => String.Equals(item.Cd, entity.Ctgry.Cd)).Single().Nm;
            node.Content = entity.Context;
            node.PriceNum = Util.GetPlusMinus(entity.Tp.Tp) ? entity.Price : entity.Price * -1;
            node.Pdt = entity.Createdate.ToString(Define.PDT_FORMAT);
            return node;
        }
        private SearchResultBean.Node CreateSumCreditNode(Decimal price)
        {
            SearchResultBean.Node node = new SearchResultBean.Node();
            node.Day = "--";
            node.Type = FactoryMaster.Instance().GetTypeMaster().Where(item => String.Equals(item.Tp, "002")).Single().Nm;
            node.Category = FactoryMaster.Instance().GetCategoryMaster().Where(item => String.Equals(item.Cd, "020")).Single().Nm;
            node.Content = node.Category;
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