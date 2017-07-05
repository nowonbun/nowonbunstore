using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AisProjectCore.Domain.Abstract
{
    /// <summary>
    /// 合理性チェック処理のインタフェースです。
    /// </summary>
    public interface IValidationProc
    {
        // TODO: ここに合理性チェックインタフェースを定義してください。
        bool Check(String chflag);

    }
}
