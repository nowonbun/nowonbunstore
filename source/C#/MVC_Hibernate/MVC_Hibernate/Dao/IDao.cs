using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;

namespace MVC_Hibernate.Dao
{
    public interface IDao
    {

    }

    public abstract class AbstractDao<T>
    {
        private ISession _session;
        public AbstractDao(ISession session)
        {
            this._session = session;
        }
        protected IList<T> Transaction(Func<ISession, IList<T>> action)
        {
            using (var transaction = this._session.BeginTransaction())
            {
                try
                {
                    var ret = action(_session);
                    _session.Transaction.Commit();
                    return ret;
                }
                catch (Exception e)
                {
                    _session.Transaction.Rollback();
                    throw e;
                }
            }
        }
        protected T Transaction(Func<ISession, T> action)
        {
            using (var transaction = this._session.BeginTransaction())
            {
                try
                {
                    var ret = action(_session);
                    _session.Transaction.Commit();
                    return ret;
                }
                catch (Exception e)
                {
                    _session.Transaction.Rollback();
                    throw e;
                }
            }
        }
        protected Object Transaction(Func<ISession, Object> action)
        {
            using (var transaction = this._session.BeginTransaction())
            {
                try
                {
                    var ret = action(_session);
                    _session.Transaction.Commit();
                    return ret;
                }
                catch (Exception e)
                {
                    _session.Transaction.Rollback();
                    throw e;
                }
            }
        }
        protected int Transaction(Func<ISession, int> action)
        {
            using (var transaction = this._session.BeginTransaction())
            {
                try
                {
                    var ret = action(_session);
                    _session.Transaction.Commit();
                    return ret;
                }
                catch (Exception e)
                {
                    _session.Transaction.Rollback();
                    throw e;
                }
            }
        }
        protected void Transaction(Action<ISession> action)
        {
            using (var transaction = this._session.BeginTransaction())
            {
                try
                {
                    action(_session);
                    _session.Transaction.Commit();
                }
                catch (Exception e)
                {
                    _session.Transaction.Rollback();
                    throw e;
                }
            }
        }
    }
}