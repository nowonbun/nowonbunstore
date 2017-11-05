using System;
using System.Data;
using ScrappingORMCore;

namespace ScrappingServer.Entity
{
    [Table("tbl_종합평가사이트")]
    class ScrappingTotalData : ScrappingORMCore.Entity
    {
        [Column("IDX", SqlDbType.BigInt, Key = true, Identity = true)]
        private Int64 _Idx;
        public Int64 Idx { get { return _Idx; } set { _Idx = value; } }

        [Column("신청번호", SqlDbType.BigInt)]
        private Int64 _ApplyNum;
        public Int64 ApplyNum { get { return _ApplyNum; } set { _ApplyNum = value; } }

        [Column("사이트코드", SqlDbType.Char)]
        private String _SiteCode;
        public String SiteCode { get { return _SiteCode; } set { _SiteCode = value; } }

        [Column("ID", SqlDbType.VarChar)]
        private String _Id;
        public String Id { get { return _Id; } set { _Id = value; } }

        [Column("정산금액1", SqlDbType.VarChar)]
        private String _SettlementFee1;
        public String SettlementFee1 { get { return _SettlementFee1; } set { _SettlementFee1 = value; } }

        [Column("정산금액2", SqlDbType.VarChar)]
        private String _SettlementFee2;
        public String SettlementFee2 { get { return _SettlementFee2; } set { _SettlementFee2 = value; } }

        [Column("정산금액3", SqlDbType.VarChar)]
        private String _SettlementFee3;
        public String SettlementFee3 { get { return _SettlementFee3; } set { _SettlementFee3 = value; } }

        [Column("정산금액합계", SqlDbType.VarChar)]
        private String _SettlementTotalFee;
        public String SettlementTotalFee { get { return _SettlementTotalFee; } set { _SettlementTotalFee = value; } }

        [Column("월평균정산금액", SqlDbType.VarChar)]
        private String _MonthAverageFee;
        public String MonthAverageFee { get { return _MonthAverageFee; } set { _MonthAverageFee = value; } }

        [Column("정산예정금액", SqlDbType.VarChar)]
        private String _ExpectFee;
        public String ExpectFee { get { return _ExpectFee; } set { _ExpectFee = value; } }

        [Column("추천승인요율", SqlDbType.VarChar)]
        private String _RecommendationApplyPercent;
        public String RecommendationApplyPercent { get { return _RecommendationApplyPercent; } set { _RecommendationApplyPercent = value; } }

        [Column("승인가능한도", SqlDbType.VarChar)]
        private String _LimitApplyFee;
        public String LimitApplyFee { get { return _LimitApplyFee; } set { _LimitApplyFee = value; } }

        [Column("승인요율", SqlDbType.VarChar)]
        private String _ApplyPercent;
        public String ApplyPercent { get { return _ApplyPercent; } set { _ApplyPercent = value; } }

        [Column("최종승인한도", SqlDbType.VarChar)]
        private String _ApplyFee;
        public String ApplyFee { get { return _ApplyFee; } set { _ApplyFee = value; } }


    }
}
