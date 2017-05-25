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
            base.Logger.Info("Search Flow Initialize");
            this.model = model;
        }
        public override bool Validate()
        {
            if (UserSession == null)
            {
                ResultBean.Result = Define.LOGIN_ERROR;
                base.Logger.Error("Session Null");
                return false;
            }
            if (String.IsNullOrEmpty(model.Year) ||
            String.IsNullOrEmpty(model.Month))
            {
                base.Logger.Error("year or month null");
                base.Logger.Error(" model Year - " + model.Year);
                base.Logger.Error(" model Month - " + model.Month);
                ResultBean.Result = Define.RESULT_NG;
                ResultBean.Error = Message.DATA_EROR;
                return false;
            }
            base.Logger.Info("Validate Ok!");
            return true;
        }

        public override ActionResult Run()
        {
            base.Logger.Info("Run Ok!");
            String json = HttpConnector.GetInstance().GetDataRequest("GetHouseholdList.php", new Dictionary<String, Object>()
            {
                {"GID",UserSession.Id},
                {"YEAR",model.Year},
                {"MONTH",model.Month}
            });
            base.Logger.Info("GetHouseholdList.php Ok!");
            IList<HouseHold> householdList = GetListByJson<HouseHold>(json);
            json = HttpConnector.GetInstance().GetDataRequest("GetHouseholdList2.php", new Dictionary<String, Object>()
            {
                {"GID",UserSession.Id},
                {"YEAR",model.Year},
                {"MONTH",model.Month},
                {"CATEGORY","020"}
            });
            base.Logger.Info("GetHouseholdList2.php Ok!");
            IList<HouseHold> creditList = GetListByJson<HouseHold>(json);

            json = HttpConnector.GetInstance().GetDataRequest("SumHousehold.php", new Dictionary<String, Object>()
            {
                {"GID",UserSession.Id},
                {"CD","010"},
                {"TP","011"},
                {"YEAR",model.Year},
                {"MONTH",model.Month}
            });
            base.Logger.Info("SumHousehold.php Ok!");
            HouseholdSum income = GetObjectByJson<HouseholdSum>(json);
            json = HttpConnector.GetInstance().GetDataRequest("SumHousehold.php", new Dictionary<String, Object>()
            {
                {"GID",UserSession.Id},
                {"CD","010"},
                {"TP","012"},
                {"YEAR",model.Year},
                {"MONTH",model.Month}
            });
            base.Logger.Info("SumHousehold.php Ok!");
            HouseholdSum expend = GetObjectByJson<HouseholdSum>(json);
            Decimal accountSum = income.Value - expend.Value;
            SearchResultBean result = new SearchResultBean();
            //nomalList create
            foreach (HouseHold hshld in householdList.OrderBy(node => node.Date))
            {
                SearchResultBean.Node node = CreateNode(hshld);
                if (String.Equals(hshld.Ctgry, "010"))
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
            base.Logger.Info("Run Ok!");

            return Json(ResultBean, JsonRequestBehavior.AllowGet);
        }

        public override ActionResult Error()
        {
            return null;
        }

        private IList<T> GetListByJson<T>(String json)
        {
            if (json == null)
            {
                return new List<T>();
            }
            else
            {
                return JsonConvert.DeserializeObject<List<T>>(json);
            }
        }
        private T GetObjectByJson<T>(String json)
        {
            if (json == null)
            {
                return (T)Activator.CreateInstance(typeof(T));
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        private SearchResultBean.Node CreateNode(HouseHold entity)
        {
            SearchResultBean.Node node = new SearchResultBean.Node();
            node.Idx = entity.Index;
            node.Tp = entity.Tp;
            node.Cd = entity.Ctgry;
            node.Date = entity.Date;
            node.Day = entity.Date.Day.ToString();
            node.Type = FactoryMaster.Instance().GetTypeMaster().Where(item => String.Equals(item.Tp, entity.Tp)).Single().Nm;
            node.Category = FactoryMaster.Instance().GetCategoryMaster().Where(item => String.Equals(item.Cd, entity.Ctgry)).Single().Nm;
            node.Content = entity.Context;
            node.PriceNum = Util.GetPlusMinus(entity.Tp) ? entity.Price : entity.Price * -1;
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