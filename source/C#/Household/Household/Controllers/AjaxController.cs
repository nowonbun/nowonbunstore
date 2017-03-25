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
    [Household.Filters.ActionFilter]
    public partial class AjaxController : AbstractController
    {
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
        private bool Validate(ApplyBean bean, bool insert = false)
        {
            if (!insert)
            {
                if (String.IsNullOrEmpty(bean.HouseholdIdx))
                {
                    return false;
                }
            }
            if (String.IsNullOrEmpty(bean.HouseholdYear))
            {
                return false;
            }
            if (String.IsNullOrEmpty(bean.HouseholdMonth))
            {
                return false;
            }
            if (String.IsNullOrEmpty(bean.HouseholdDay))
            {
                return false;
            }
            if (String.IsNullOrEmpty(bean.HouseholdCategory))
            {
                return false;
            }
            if (String.IsNullOrEmpty(bean.HouseholdType))
            {
                return false;
            }
            if (String.IsNullOrEmpty(bean.HouseholdContent))
            {
                return false;
            }
            if (String.IsNullOrEmpty(bean.Householdprice))
            {
                return false;
            }
            return true;
        }
    }
}
