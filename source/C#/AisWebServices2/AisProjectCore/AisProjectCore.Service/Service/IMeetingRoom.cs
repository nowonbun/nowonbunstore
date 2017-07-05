using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AisProjectCore.Service.Service
{
    // メモ: [リファクター] メニューの [名前の変更] コマンドを使用すると、コードと config ファイルの両方で同時にインターフェイス名 "IMeetingRoom" を変更できます。
    [ServiceContract]
    public interface IMeetingRoom
    {
        [OperationContract]
        String getMaster();

        [OperationContract]
        String getMeetingRoomList(String date);

        [OperationContract]
        String getMeetingRoom(String date, int meetingRoomCode);

        [OperationContract]
        String setMeetingRoom(String date, int meetingRoomCode, String startTime, String endTime, String token, int purposeCode);

        [OperationContract]
        String updateMeetingRoom(int code, String startTime, String endTime, String token, int purposeCode);

        [OperationContract]
        String deleteMeetingRoom(int code, String token);
    }
}
