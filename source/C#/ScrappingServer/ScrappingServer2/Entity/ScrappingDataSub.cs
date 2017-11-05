using System;
using System.Data;
using ScrappingORMCore;

namespace ScrappingServer.Entity
{
    [Table("tbl_스크래핑Data_sub")]
    class ScrappingDataSub : ScrappingORMCore.Entity
    {
        [Column("IDX", SqlDbType.BigInt, Key = true, Identity = true)]
        private Int64 _Idx;
        public Int64 Idx { get { return _Idx; } set { _Idx = value; } }

        [Column("DataIdx", SqlDbType.BigInt)]
        private Int64 _DataIdx;
        public Int64 DataIdx { get { return _DataIdx; } set { _DataIdx = value; } }

        [Column("Data01", SqlDbType.VarChar)]
        private String _Data01;
        public String Data01 { get { return _Data01; } set { _Data01 = value; } }

        [Column("Data02", SqlDbType.VarChar)]
        private String _Data02;
        public String Data02 { get { return _Data02; } set { _Data02 = value; } }

        [Column("Data03", SqlDbType.VarChar)]
        private String _Data03;
        public String Data03 { get { return _Data03; } set { _Data03 = value; } }

        [Column("Data04", SqlDbType.VarChar)]
        private String _Data04;
        public String Data04 { get { return _Data04; } set { _Data04 = value; } }

        [Column("Data05", SqlDbType.VarChar)]
        private String _Data05;
        public String Data05 { get { return _Data05; } set { _Data05 = value; } }

        [Column("Data06", SqlDbType.VarChar)]
        private String _Data06;
        public String Data06 { get { return _Data06; } set { _Data06 = value; } }

        [Column("Data07", SqlDbType.VarChar)]
        private String _Data07;
        public String Data07 { get { return _Data07; } set { _Data07 = value; } }

        [Column("Data08", SqlDbType.VarChar)]
        private String _Data08;
        public String Data08 { get { return _Data08; } set { _Data08 = value; } }

        [Column("Data09", SqlDbType.VarChar)]
        private String _Data09;
        public String Data09 { get { return _Data09; } set { _Data09 = value; } }

        [Column("Data10", SqlDbType.VarChar)]
        private String _Data10;
        public String Data10 { get { return _Data10; } set { _Data10 = value; } }

        [Column("Data11", SqlDbType.VarChar)]
        private String _Data11;
        public String Data11 { get { return _Data11; } set { _Data11 = value; } }

        [Column("Data12", SqlDbType.VarChar)]
        private String _Data12;
        public String Data12 { get { return _Data12; } set { _Data12 = value; } }

        [Column("Data13", SqlDbType.VarChar)]
        private String _Data13;
        public String Data13 { get { return _Data13; } set { _Data13 = value; } }

        [Column("Data14", SqlDbType.VarChar)]
        private String _Data14;
        public String Data14 { get { return _Data14; } set { _Data14 = value; } }

        [Column("Data15", SqlDbType.VarChar)]
        private String _Data15;
        public String Data15 { get { return _Data15; } set { _Data15 = value; } }

        [Column("Data16", SqlDbType.VarChar)]
        private String _Data16;
        public String Data16 { get { return _Data16; } set { _Data16 = value; } }

        [Column("Data17", SqlDbType.VarChar)]
        private String _Data17;
        public String Data17 { get { return _Data17; } set { _Data17 = value; } }

        [Column("Data18", SqlDbType.VarChar)]
        private String _Data18;
        public String Data18 { get { return _Data18; } set { _Data18 = value; } }

        [Column("Data19", SqlDbType.VarChar)]
        private String _Data19;
        public String Data19 { get { return _Data19; } set { _Data19 = value; } }

        [Column("Data20", SqlDbType.VarChar)]
        private String _Data20;
        public String Data20 { get { return _Data20; } set { _Data20 = value; } }

        [Column("Data21", SqlDbType.VarChar)]
        private String _Data21;
        public String Data21 { get { return _Data21; } set { _Data21 = value; } }

        [Column("Data22", SqlDbType.VarChar)]
        private String _Data22;
        public String Data22 { get { return _Data22; } set { _Data22 = value; } }

        [Column("Data23", SqlDbType.VarChar)]
        private String _Data23;
        public String Data23 { get { return _Data23; } set { _Data23 = value; } }

        [Column("Data24", SqlDbType.VarChar)]
        private String _Data24;
        public String Data24 { get { return _Data24; } set { _Data24 = value; } }

        [Column("Data25", SqlDbType.VarChar)]
        private String _Data25;
        public String Data25 { get { return _Data25; } set { _Data25 = value; } }

        [Column("Data26", SqlDbType.VarChar)]
        private String _Data26;
        public String Data26 { get { return _Data26; } set { _Data26 = value; } }

        [Column("Data27", SqlDbType.VarChar)]
        private String _Data27;
        public String Data27 { get { return _Data27; } set { _Data27 = value; } }

        [Column("Data28", SqlDbType.VarChar)]
        private String _Data28;
        public String Data28 { get { return _Data28; } set { _Data28 = value; } }

        [Column("Data29", SqlDbType.VarChar)]
        private String _Data29;
        public String Data29 { get { return _Data29; } set { _Data29 = value; } }
        
    }
}
