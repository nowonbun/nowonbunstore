using AisProjectCore.Dinject.Inheritance;
using AisProjectCore.Domain.Abstract;
using AisProjectCore.Domain.Entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisProjectCore.Domain.Concrete
{
    /// <summary>
    /// 会議室予約機能の具現隊です。
    /// </summary>
    public sealed class ReserveProc : Injector, IReserveProc
    {
        /// <summary>
        /// データベースアクセスモデルです。
        /// </summary>
        private IRepository repo { get; set; }
        private IValidationProc validate { get; set; }

        public void ReserveAdd(ReserveModel entity)
        {
            repo.Connect();

            repo.Insert(entity);

            repo.Close();
        }
        public void ReserveUpdate(ReserveModel entity)
        {
            repo.Connect();

            repo.Update(entity);

            repo.Close();
        }
        public void ReserveDelete(ReserveModel entity)
        {
            repo.Connect();

            repo.Delete(entity);

            repo.Close();
        }
        //public IList<ReserveModel> ReserveSelect(string table, ReserveSelectModel entity)
        //{
        //    repo.Connect();

        //    var result = repo.Select<ReserveModel, ReserveSelectModel>(table, entity);

        //    repo.Close();

        //    return result;
        //}
    }
}
