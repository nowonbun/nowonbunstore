using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using LogisticsSystem.App_Code;
using LogisticsSystem.Validate;

namespace LogisticsSystem.Models
{
    public class FormRequestOrderTableList
    {
        private IList<OrderTableSub> m_pList;
        public IList<OrderTableSub> List
        {
            get { return m_pList; }
        }
        public FormRequestOrderTableList()
        {
            m_pList = new List<OrderTableSub>();
        }
        private void InitList(int count)
        {
            for (int i = m_pList.Count; i < count; i++)
            {
                m_pList.Add(new OrderTableSub());
            }
        }
        /// <summary>
        /// 상품키변환
        /// </summary>
        /// 이게 순서대로 넣지 않으면 이상해 지는 버그가 있다.
        public ICollection<Int64> productIndex
        {
            get { return null; }
            set
            {
                List<Int64> pBuffer = (List<Int64>)value;
                for (int i = 0; i < value.Count; i++)
                {
                    if (pBuffer[i] != 0)
                    {
                        InitList(i + 1);
                        m_pList[i].ProductIndex = pBuffer[i];
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 상품타입변환
        /// </summary>
        public ICollection<String> productType
        {
            get { return null; }
            set {
                List<String> pBuffer = (List<String>)value;
                for (int i = 0; i < value.Count; i++)
                {
                    if (pBuffer[i] != "")
                    {
                        InitList(i+1);
                        m_pList[i].ProductType = pBuffer[i];
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 상품규격변환
        /// </summary>
        public ICollection<String> productspec
        {
            get { return null; }
            set
            {
                List<String> pBuffer = (List<String>)value;
                for (int i = 0; i < value.Count; i++)
                {
                    if (pBuffer[i] != "")
                    {
                        InitList(i + 1);
                        m_pList[i].ProductSpec = pBuffer[i];
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 상품양변환
        /// </summary>
        public ICollection<Decimal> productAmount
        {
            get { return null; }
            set
            {
                List<Decimal> pBuffer = (List<Decimal>)value;
                for (int i = 0; i < value.Count; i++)
                {
                    if (pBuffer[i] != 0)
                    {
                        InitList(i + 1);
                        m_pList[i].ProductAmount = pBuffer[i];
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 상품가격변환
        /// </summary>
        public ICollection<Decimal> productprice
        {
            get { return null; }
            set
            {
                List<Decimal> pBuffer = (List<Decimal>)value;
                for (int i = 0; i < value.Count; i++)
                {
                    if (pBuffer[i] != 0)
                    {
                        InitList(i + 1);
                        m_pList[i].ProductPrice = pBuffer[i];
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 상품총값변환
        /// </summary>
        public ICollection<Decimal> producttotal
        {
            get { return null; }
            set
            {
                List<Decimal> pBuffer = (List<Decimal>)value;
                for (int i = 0; i < value.Count; i++)
                {
                    if (pBuffer[i] != 0)
                    {
                        InitList(i + 1);
                        m_pList[i].ProductMoney = pBuffer[i];
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 상품총값변환
        /// </summary>
        public ICollection<String> productspec_disp
        {
            get { return null; }
            set
            {
                List<String> pBuffer = (List<String>)value;
                for (int i = 0; i < value.Count; i++)
                {
                    if (pBuffer[i] != "")
                    {
                        InitList(i + 1);
                        m_pList[i].ProductSpecDisp = pBuffer[i];
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// validate
        /// </summary>
        public List<String> Validate(LanguageType? lType)
        {
            List<String> Errmsg = new List<string>();
            if (this.m_pList.Count <= 0)
            {
                if (lType == LanguageType.Korea) Errmsg.Add("상품명이 입력되지 않았습니다.");
                else Errmsg.Add("商品名が入力されてありません。");
            }
            int i = 1;
            foreach (OrderTableSub pdata in this.m_pList)
            {
                List<String> pBuffer = pdata.Validate(lType,i);
                foreach (String pBuffer2 in pBuffer)
                {
                    Errmsg.Add(pBuffer2);
                }
                i++;
            }
            return Errmsg;
        }
    }
}