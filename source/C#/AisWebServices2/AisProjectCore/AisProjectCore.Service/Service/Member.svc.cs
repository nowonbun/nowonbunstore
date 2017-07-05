using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using AisProjectCore.Dinject.Concrete;

namespace AisProjectCore.Service.Service
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コード、svc、および config ファイルで同時にクラス名 "Member" を変更できます。
    // 注意: このサービスをテストするために WCF テスト クライアントを起動するには、ソリューション エクスプローラーで Member.svc または Member.svc.cs を選択し、デバッグを開始してください。
    public class Member : IMember
    {
        public String setID(String email, String token, String name)
        {
            JsonStructure json = new JsonStructure();
            json.Result = 0;
            return FactoryCore.getList().Push(json.build());
        }
    }
}
