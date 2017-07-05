using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisProjectCore.Domain.Entity
{
    public interface IRepository : IDisposable
    {
        /// <summary>
        /// データベースに接続する。
        /// </summary>
        void Connect();

        /// <summary>
        /// データベース接続を切端する。
        /// </summary>
        void Close();

        /// <summary>
        /// Insertを要求する。
        /// </summary>
        /// <typeparam name="T">InsertするデータのテーブルEntity</typeparam>
        /// <param name="entity">Insertするデータ</param>
        void Insert<T>(T entity);

        /// <summary>
        /// Updateを要求する。
        /// </summary>
        /// <typeparam name="T">UpdateするデータのデータEntity</typeparam>
        /// <param name="entity">Updateするデータ</param>
        void Update<T>(T entity);

        /// <summary>
        /// Deleteを要求する。
        /// </summary>
        /// <typeparam name="P">Delete条件のModel</typeparam>
        /// <param name="where">Delete条件データ</param>
        void Delete<P>(P where);

        /// <summary>
        /// Selectを要求する。
        /// </summary>
        /// <typeparam name="T">テーブルEntity</typeparam>
        /// <typeparam name="P">Select条件のModel</typeparam>
        /// <param name="where">Select条件データ</param>
        /// <returns>Selectの結果T型の集合</returns>
        IList<T> Select<T, P>(P where);

        /// <summary>
        /// テーブルの全データをSelectする。
        /// </summary>
        /// <typeparam name="T">テーブルEntity</typeparam>
        /// <param name="table">テーブル名</param>
        /// <returns>Selectの結果T型の集合</returns>
        IList<T> SelectAll<T>(string table);
    }
}
