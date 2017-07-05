using AisProjectCore.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisProjectCore.Domain.Abstract
{
    /// <summary>
    /// 会議室予約機能のインタフェースです。
    /// </summary>
    public interface IReserveProc
    {
        /// <summary>
        /// 会議室予約します。
        /// </summary>
        void ReserveAdd(ReserveModel ReserveEntity);

        /// <summary>
        /// 会議室予約を更新します。    
        /// </summary>
        void ReserveUpdate(ReserveModel ReserveEntity);

        /// <summary>
        /// 
        /// </summary>
        void ReserveDelete(ReserveModel ReserveEntity);

        /// <summary>
        /// 会議室予約状態を読み込みます。
        /// </summary>
        //IList<ReserveModel> ReserveSelect(string table, ReserveSelectModel ReserveSelectEntity);
    }
}
