using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AisProjectCore.Service.Service
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "IMember" を変更できます。
    [ServiceContract]
    public interface IMember
    {
        [OperationContract]
        String setID(String email, String token, String name);
    }
}
