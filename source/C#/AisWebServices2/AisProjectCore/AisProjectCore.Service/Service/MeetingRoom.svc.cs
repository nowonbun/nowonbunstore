using System;
using System.Collections.Generic;
using AisProjectCore.Dinject.Concrete;

namespace AisProjectCore.Service.Service
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コード、svc、および config ファイルで同時にクラス名 "MeetingRoom" を変更できます。
    // 注意: このサービスをテストするために WCF テスト クライアントを起動するには、ソリューション エクスプローラーで MeetingRoom.svc または MeetingRoom.svc.cs を選択し、デバッグを開始してください。
    public class MeetingRoom : IMeetingRoom
    {
        public string getMaster()
        {
            JsonStructure json = new JsonStructure();
            json.Result = 0;
            IDictionary<String, Object> example = new Dictionary<String, Object>();
            json.Data = example;
            IList<object> MeetingRoomMaster = new List<object>();
            MeetingRoomMaster.Add(new { Code = 1, Name = "会議室1" });
            MeetingRoomMaster.Add(new { Code = 2, Name = "会議室2" });
            IList<object> PurposeMaster = new List<object>();
            PurposeMaster.Add(new { Code = 1, Content = "お客様ミーティング" });
            PurposeMaster.Add(new { Code = 2, Content = "部署長会議" });
            PurposeMaster.Add(new { Code = 3, Content = "営業会議" });
            PurposeMaster.Add(new { Code = 4, Content = "社内教育" });
            PurposeMaster.Add(new { Code = 5, Content = "チーム会議" });
            example.Add("MeetingRoomMaster", MeetingRoomMaster);
            example.Add("PurposeMaster", PurposeMaster);
            return FactoryCore.getList().Push(json.build());
        }

        public string getMeetingRoomList(string date)
        {
            JsonStructure json = new JsonStructure();
            json.Result = 0;
            IDictionary<String, Object> example = new Dictionary<String, Object>();
            json.Data = example;
            IList<object> Room1 = new List<object>();
            Room1.Add(new
            {
                Code = 1,
                DateTime = "2017-04-13 00:00:00",
                MeetingRoomCode = 1,
                PurposeCode = 1,
                EmployeeCode = 1,
                Name = "高喜洙",
                Email = "kohs@ais-info.co.jp",
                OAuth = "1234567890",
                ReserveDate = "2017-04-13 11:23:06",
                ReserveStart = "14:00:00",
                ReserveEnd = "14:59:59"
            });
            Room1.Add(new
            {
                Code=2,
                DateTime="2017-04-13 00:00:00",
                MeetingRoomCode=1,
                PurposeCode=2,
                EmployeeCode=2,
                Name="黄淳嘩",
                Email="hwangsy@ais-info.co.jp",
                OAuth="9876543210",
                ReserveDate="2017-04-13 11:44:10",
                ReserveStart="15:30:00",
                ReserveEnd="15:59:59"
            });
            example.Add("1", Room1);
            IList<object> Room2 = new List<object>();
            Room2.Add(new
            {
                Code=3,
                DateTime="2017-04-13 00:00:00",
                MeetingRoomCode=2,
                PurposeCode=3,
                EmployeeCode=3,
                Name="趙慶珉",
                Email="chokm@ais-info.co.jp",
                OAuth="285643210",
                ReserveDate="2017-04-13 10:23:06",
                ReserveStart="14:00:00",
                ReserveEnd="14:59:59"
            });
            Room2.Add(new
            {
                Code=4,
                DateTime="2017-04-13 00:00:00",
                MeetingRoomCode=2,
                PurposeCode=2,
                EmployeeCode=1,
                Name="高喜洙",
                Email="kohs@ais-info.co.jp",
                OAuth="1234567890",
                ReserveDate="2017-04-13 13:20:10",
                ReserveStart="15:00:00",
                ReserveEnd="15:59:59"
            });
            example.Add("2", Room1);
            return FactoryCore.getList().Push(json.build());
        }

        public string getMeetingRoom(string date, int meetingRoomCode)
        {
            JsonStructure json = new JsonStructure();
            json.Result = 0;
            IList<object> Room1 = new List<object>();
            json.Data = Room1;
            Room1.Add(new
            {
                Code = 1,
                DateTime = "2017-04-13 00:00:00",
                MeetingRoomCode = 1,
                PurposeCode = 1,
                EmployeeCode = 1,
                Name = "高喜洙",
                Email = "kohs@ais-info.co.jp",
                OAuth = "1234567890",
                ReserveDate = "2017-04-13 11:23:06",
                ReserveStart = "14:00:00",
                ReserveEnd = "14:59:59"
            });
            Room1.Add(new
            {
                Code = 2,
                DateTime = "2017-04-13 00:00:00",
                MeetingRoomCode = 1,
                PurposeCode = 2,
                EmployeeCode = 2,
                Name = "黄淳嘩",
                Email = "hwangsy@ais-info.co.jp",
                OAuth = "9876543210",
                ReserveDate = "2017-04-13 11:44:10",
                ReserveStart = "15:30:00",
                ReserveEnd = "15:59:59"
            });       
            return FactoryCore.getList().Push(json.build());
        }

        public string setMeetingRoom(string date, int meetingRoomCode, string startTime, string endTime, string token, int purposeCode)
        {
            JsonStructure json = new JsonStructure();
            json.Result = 0;
            IList<object> Room1 = new List<object>();
            json.Data = Room1;
            Room1.Add(new
            {
                Code = 1,
                DateTime = "2017-04-13 00:00:00",
                MeetingRoomCode = 1,
                PurposeCode = 1,
                EmployeeCode = 1,
                Name = "高喜洙",
                Email = "kohs@ais-info.co.jp",
                OAuth = "1234567890",
                ReserveDate = "2017-04-13 11:23:06",
                ReserveStart = "14:00:00",
                ReserveEnd = "14:59:59"
            });
            Room1.Add(new
            {
                Code = 2,
                DateTime = "2017-04-13 00:00:00",
                MeetingRoomCode = 1,
                PurposeCode = 2,
                EmployeeCode = 2,
                Name = "黄淳嘩",
                Email = "hwangsy@ais-info.co.jp",
                OAuth = "9876543210",
                ReserveDate = "2017-04-13 11:44:10",
                ReserveStart = "15:30:00",
                ReserveEnd = "15:59:59"
            });
            return FactoryCore.getList().Push(json.build());
        }

        public string updateMeetingRoom(int code, string startTime, string endTime, string token, int purposeCode)
        {
            JsonStructure json = new JsonStructure();
            json.Result = 0;
            IList<object> Room1 = new List<object>();
            json.Data = Room1;
            Room1.Add(new
            {
                Code = 1,
                DateTime = "2017-04-13 00:00:00",
                MeetingRoomCode = 1,
                PurposeCode = 1,
                EmployeeCode = 1,
                Name = "高喜洙",
                Email = "kohs@ais-info.co.jp",
                OAuth = "1234567890",
                ReserveDate = "2017-04-13 11:23:06",
                ReserveStart = "14:00:00",
                ReserveEnd = "14:59:59"
            });
            Room1.Add(new
            {
                Code = 2,
                DateTime = "2017-04-13 00:00:00",
                MeetingRoomCode = 1,
                PurposeCode = 2,
                EmployeeCode = 2,
                Name = "黄淳嘩",
                Email = "hwangsy@ais-info.co.jp",
                OAuth = "9876543210",
                ReserveDate = "2017-04-13 11:44:10",
                ReserveStart = "15:30:00",
                ReserveEnd = "15:59:59"
            });
            return FactoryCore.getList().Push(json.build());
        }
        public string deleteMeetingRoom(int code, string token)
        {
            JsonStructure json = new JsonStructure();
            json.Result = 0;
            IList<object> Room1 = new List<object>();
            json.Data = Room1;
            Room1.Add(new
            {
                Code = 1,
                DateTime = "2017-04-13 00:00:00",
                MeetingRoomCode = 1,
                PurposeCode = 1,
                EmployeeCode = 1,
                Name = "高喜洙",
                Email = "kohs@ais-info.co.jp",
                OAuth = "1234567890",
                ReserveDate = "2017-04-13 11:23:06",
                ReserveStart = "14:00:00",
                ReserveEnd = "14:59:59"
            });
            Room1.Add(new
            {
                Code = 2,
                DateTime = "2017-04-13 00:00:00",
                MeetingRoomCode = 1,
                PurposeCode = 2,
                EmployeeCode = 2,
                Name = "黄淳嘩",
                Email = "hwangsy@ais-info.co.jp",
                OAuth = "9876543210",
                ReserveDate = "2017-04-13 11:44:10",
                ReserveStart = "15:30:00",
                ReserveEnd = "15:59:59"
            });
            return FactoryCore.getList().Push(json.build());
        }
    }
}
