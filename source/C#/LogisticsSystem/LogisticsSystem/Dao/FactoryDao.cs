using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogisticsSystem.App_Code;

namespace LogisticsSystem.Dao
{
    public class FactoryDao
    {
        private Dictionary<Type, IDao> flyweight = new Dictionary<Type, IDao>();
        private static FactoryDao instance = null;
        private FactoryDao()
        {

        }
        public static FactoryDao Instance()
        {
            if(instance == null)
            {
                instance = new FactoryDao();
            }
            return instance;
        }
        public UserInfoDao GetUserInfoDao()
        {
            
            if (!flyweight.ContainsKey(typeof(UserInfoDao)))
            {
                flyweight.Add(typeof(UserInfoDao), new UserInfoDao());
            }
            return flyweight[typeof(UserInfoDao)] as UserInfoDao;
        }
        public CompanyInfoDao GetCompanyInfoDao()
        {
            if (!flyweight.ContainsKey(typeof(CompanyInfoDao)))
            {
                flyweight.Add(typeof(CompanyInfoDao), new CompanyInfoDao());
            }
            return flyweight[typeof(CompanyInfoDao)] as CompanyInfoDao;
        }
        public ConnectDao GetConnectDao()
        {
            if (!flyweight.ContainsKey(typeof(ConnectDao)))
            {
                flyweight.Add(typeof(ConnectDao), new ConnectDao());
            }
            return flyweight[typeof(ConnectDao)] as ConnectDao;
        }
        public BoardDao GetBoardDao()
        {
            if (!flyweight.ContainsKey(typeof(BoardDao)))
            {
                flyweight.Add(typeof(BoardDao), new BoardDao());
            }
            return flyweight[typeof(BoardDao)] as BoardDao;
        }
        public CommentDao GetCommentDao()
        {
            if (!flyweight.ContainsKey(typeof(CommentDao)))
            {
                flyweight.Add(typeof(CommentDao), new CommentDao());
            }
            return flyweight[typeof(CommentDao)] as CommentDao;
        }
        public CodeMasterDao GetCodeMasterDao()
        {
            if (!flyweight.ContainsKey(typeof(CodeMasterDao)))
            {
                flyweight.Add(typeof(CodeMasterDao), new CodeMasterDao());
            }
            return flyweight[typeof(CodeMasterDao)] as CodeMasterDao;
        }
        public CustomerInfoDao GetCustomerInfoDao()
        {
            if (!flyweight.ContainsKey(typeof(CustomerInfoDao)))
            {
                flyweight.Add(typeof(CustomerInfoDao), new CustomerInfoDao());
            }
            return flyweight[typeof(CustomerInfoDao)] as CustomerInfoDao;
        }
        public DocumentDao GetDocumentDao()
        {
            if (!flyweight.ContainsKey(typeof(DocumentDao)))
            {
                flyweight.Add(typeof(DocumentDao), new DocumentDao());
            }
            return flyweight[typeof(DocumentDao)] as DocumentDao;
        }
        public ProductInfoDao GetProductInfoDao()
        {
            if (!flyweight.ContainsKey(typeof(ProductInfoDao)))
            {
                flyweight.Add(typeof(ProductInfoDao), new ProductInfoDao());
            }
            return flyweight[typeof(ProductInfoDao)] as ProductInfoDao;
        }
        public OrderTableDao GetOrderTableDao()
        {
            if (!flyweight.ContainsKey(typeof(OrderTableDao)))
            {
                flyweight.Add(typeof(OrderTableDao), new OrderTableDao());
            }
            return flyweight[typeof(OrderTableDao)] as OrderTableDao;
        }
        public OrderTableSubDao GetOrderTableSubDao()
        {
            if (!flyweight.ContainsKey(typeof(OrderTableDao)))
            {
                flyweight.Add(typeof(OrderTableSubDao), new OrderTableSubDao());
            }
            return flyweight[typeof(OrderTableSubDao)] as OrderTableSubDao;
        }
        public DeliveryTableDao GetDeliveryTableDao()
        {
            if (!flyweight.ContainsKey(typeof(DeliveryTableDao)))
            {
                flyweight.Add(typeof(DeliveryTableDao), new DeliveryTableDao());
            }
            return flyweight[typeof(DeliveryTableDao)] as DeliveryTableDao;
        }
        public ProductFlowDao GetProductFlowDao()
        {
            if (!flyweight.ContainsKey(typeof(ProductFlowDao)))
            {
                flyweight.Add(typeof(ProductFlowDao), new ProductFlowDao());
            }
            return flyweight[typeof(ProductFlowDao)] as ProductFlowDao;
        }
        public DeliveryTableSubDao GetDeliveryTableSubDao()
        {
            if (!flyweight.ContainsKey(typeof(DeliveryTableSubDao)))
            {
                flyweight.Add(typeof(DeliveryTableSubDao), new DeliveryTableSubDao());
            }
            return flyweight[typeof(DeliveryTableSubDao)] as DeliveryTableSubDao;
        }

        public BillDao GetBillDao()
        {
            if (!flyweight.ContainsKey(typeof(BillDao)))
            {
                flyweight.Add(typeof(BillDao), new BillDao());
            }
            return flyweight[typeof(BillDao)] as BillDao;
        }
        public CargoDao GetCargoDao()
        {
            if (!flyweight.ContainsKey(typeof(CargoDao)))
            {
                flyweight.Add(typeof(CargoDao), new CargoDao());
            }
            return flyweight[typeof(CargoDao)] as CargoDao;
        }
    }
}