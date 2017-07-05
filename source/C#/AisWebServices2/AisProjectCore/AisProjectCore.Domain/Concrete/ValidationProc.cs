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
    public class ValidationProc : Injector, IValidationProc
    {
        /// <summary>
        /// データベースアクセスモデルです。
        /// </summary>
        private IRepository repo { get; set; }
        
        // 登録及び更新が可能な場合はTRUEを、不可の場合はFALSEを返します。
        public bool Check(string chflag)
        {
            // 受け渡された値により登録か更新かを判断
            if (chflag.Equals("Insert"))
            {
                return InsertChecked();
            }
            else if (chflag.Equals("Update"))
            {
                return UpdateChecked();
            }

            throw new NotImplementedException();
        }

        public bool InsertChecked()
        {
            // 新たに登録するデータの格納
            DateTime AddDay = new DateTime();
            DateTime AddStartTime = new DateTime();
            DateTime AddEndTime = new DateTime();
            String AddMeetingRoom = "";

            // すでに登録済みのデータの格納
            DateTime OldDay = new DateTime();
            DateTime OldStartTime = new DateTime();
            DateTime OldEndTime = new DateTime();
            String OldMeetingRoom = "";



            return logic(OldDay, AddDay,OldStartTime, AddStartTime, 
                OldEndTime,AddEndTime, OldMeetingRoom, AddMeetingRoom);
        }

        public bool UpdateChecked()
        {
            // 新たに更新を行うデータの格納
            DateTime UpdDay = new DateTime();
            DateTime UpdStartTime = new DateTime();
            DateTime UpdEndTime = new DateTime();
            String UpdMeetingRoom = "";

            // 登録済みデータの格納
            DateTime OldDay = new DateTime();
            DateTime OldStartTime = new DateTime();
            DateTime OldEndTime = new DateTime();
            String OldMeetingRoom = "";


            return logic(OldDay, UpdDay, OldStartTime, UpdStartTime,
                OldEndTime, UpdEndTime, OldMeetingRoom, UpdMeetingRoom);
        }

        public bool logic(DateTime OldDay, DateTime NewDay, DateTime OldStTime, 
            DateTime NewStTime, DateTime OldEndTime, DateTime NewEndTime, String OldRoom, String NewRoom)
        {
            // デフォルト値としてTRUEを設定
            bool checkfg = true;

            // 登録及び更新の日付が違う時点でチェックを行う必要がないため、同日かのみを確認
            if (OldDay == NewDay)
            {
                // 同日かつ登録及び更新を行いたい部屋が同じもののみチェックを行う
                if (OldRoom.Equals(NewRoom))
                {
                // 以下ひとつでも該当する場合は使用時間の重複とみなす
                    if (OldStTime == NewStTime || OldEndTime == NewEndTime)
                    {
                        checkfg = false;
                    }
                    if (OldStTime >= NewStTime && OldEndTime >= NewEndTime)
                    {
                        checkfg = false;
                    }
                    if (OldStTime <= NewStTime && OldEndTime >= NewEndTime)
                    {
                        checkfg = false;
                    }
                    if (OldStTime >= NewStTime && OldEndTime >= NewEndTime)
                    {
                        checkfg = false;
                    }
                    if (OldStTime < NewStTime && OldEndTime < NewEndTime)
                    {
                        if (NewStTime <= OldEndTime)
                        {
                            checkfg = false;
                        }
                    }
                    if (OldStTime > NewStTime && OldEndTime > NewEndTime)
                    {
                        if (OldStTime <= NewEndTime)
                        {
                            checkfg = false;
                        }
                    }
                }
            }

            // 上記チェックに一つも該当しなかった場合にTRUEとして値が返される
            return checkfg;
        }

    }
}
