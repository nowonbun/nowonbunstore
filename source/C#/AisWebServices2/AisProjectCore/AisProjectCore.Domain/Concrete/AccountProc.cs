using AisProjectCore.Dinject.Inheritance;
using AisProjectCore.Domain.Abstract;
using AisProjectCore.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisProjectCore.Domain.Concrete
{
    /// <summary>
    /// アカウント管理機能の具現隊です。
    /// </summary>
    public sealed class AccountProc : Injector, IAccountProc
    {
        /// <summary>
        /// データベースアクセスモデルです。
        /// </summary>
        private IRepository repo { get; set; }
        
    }
}
