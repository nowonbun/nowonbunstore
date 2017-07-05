package jp.co.aisinfo.Util;

import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;

import jp.co.aisinfo.Common.Define;

public class StubJson {
	
	String jsonString;
	
	public String getMaster () {
		
		jsonString = "{" + "\"Result\":0," + "\"Error\": null," + "\"Data\":[" + "{" + "\"MeetingRoomMaster\":[" + "{"
				+ "\"Code\":1," + "\"Name\":\"âÔãcé∫1\"" + "}," + "{" + "\"Code\":2," + "\"Name\":\"âÔãcé∫2\"" + "}" + "]"
				+ "}," + "{" + "\"PurposeMaster\":[" + "{" + "\"Code\":1," + "\"Content\":\"Ç®ãqólÉ~Å[ÉeÉBÉìÉO\"" + "}," + "{"
				+ "\"Code\":2," + "\"Content\":\"ïîèêí∑âÔãc\"" + "}," + "{" + "\"Code\":3," + "\"Content\":\"âcã∆âÔãc\"" + "},"
				+ "{" + "\"Code\":4," + "\"Content\":\"é–ì‡ã≥àÁ\"" + "}," + "{" + "\"Code\":5," + "\"Content\":\"É`Å[ÉÄâÔãc\""
				+ "}" + "]" + "}" + "]" + "}";

		return jsonString;
	}

	public String getMeetingRoom1 () {
		
		jsonString = "{" + "\"Result\":0," + "\"Error\": null," + "\"Data\":[" + "{" + "\"RoomData\":[" + "{"
				+ "\"Code\":1," + "\"DateTime\":\"2017-04-13 00:00:00\"," + "\"MeetingRoomCode\":1,"
				+ "\"PurposeCode\":1," + "\"EmployeeCode\":1," + "\"Name\":\"çÇäÏü™\","
				+ "\"Email\":\"kohs@ais-info.co.jp\"," + "\"OAuth\":\"1234567890\","
				+ "\"ReserveDate\":\"2017-04-13 11:23:06\"," + "\"ReserveStart\":\"14:00:00\","
				+ "\"ReserveEnd\":\"14:59:59\"" + "}," + "{" + "\"Code\":2," + "\"DateTime\":\"2017-04-13 00:00:00\","
				+ "\"MeetingRoomCode\":1," + "\"PurposeCode\":2," + "\"EmployeeCode\":2," + "\"Name\":\"â©è~â‹\","
				+ "\"Email\":\"hwangsy@ais-info.co.jp\"," + "\"OAuth\":\"9876543210\","
				+ "\"ReserveDate\":\"2017-04-13 11:44:10\"," + "\"ReserveStart\":\"15:30:00\","
				+ "\"ReserveEnd\":\"15:59:59\"" + "}" + "]" + "}," + "{" + "\"RoomData\":[" + "{" + "\"Code\":3,"
				+ "\"DateTime\":\"2017-04-13 00:00:00\"," + "\"MeetingRoomCode\":2," + "\"PurposeCode\":3,"
				+ "\"EmployeeCode\":3," + "\"Name\":\"Ê‚åc˚a\"," + "\"Email\":\"chokm@ais-info.co.jp\","
				+ "\"OAuth\":\"285643210\"," + "\"ReserveDate\":\"2017-04-13 10:23:06\","
				+ "\"ReserveStart\":\"14:00:00\"," + "\"ReserveEnd\":\"14:59:59\"" + "}," + "{" + "\"Code\":4,"
				+ "\"DateTime\":\"2017-04-13 00:00:00\"," + "\"MeetingRoomCode\":2," + "\"PurposeCode\":2,"
				+ "\"EmployeeCode\":1," + "\"Name\":\"çÇäÏü™\"," + "\"Email\":\"kohs@ais-info.co.jp\","
				+ "\"OAuth\":\"1234567890\"," + "\"ReserveDate\":\"2017-04-13 13:20:10\","
				+ "\"ReserveStart\":\"15:00:00\"," + "\"ReserveEnd\":\"15:59:59\"" + "}" + "]" + "}" + "]" + "}";
		
		return jsonString;
	}
	
	public String getMeetingRoom2 () {
		
		jsonString = "{" + "\"Result\":0," + "\"Error\": null," + "\"Data\":[" + "{" + "\"Code\":1,"
				+ "\"DateTime\":\"2017-04-13 00:00:00\"," + "\"MeetingRoomCode\":1," + "\"PurposeCode\":1,"
				+ "\"EmployeeCode\":1," + "\"Name\":\"çÇäÏü™\"," + "\"Email\":\"kohs@ais-info.co.jp\","
				+ "\"OAuth\":\"1234567890\"," + "\"ReserveDate\":\"2017-04-13 11:23:06\","
				+ "\"ReserveStart\":\"14:00:00\"," + "\"ReserveEnd\":\"14:59:59\"" + "}," + "{" + "\"Code\":2,"
				+ "\"DateTime\":\"2017-04-13 00:00:00\"," + "\"MeetingRoomCode\":1," + "\"PurposeCode\":2,"
				+ "\"EmployeeCode\":2," + "\"Name\":\"â©è~â‹\"," + "\"Email\":\"hwangsy@ais-info.co.jp\","
				+ "\"OAuth\":\"9876543210\"," + "\"ReserveDate\":\"2017-04-13 11:44:10\","
				+ "\"ReserveStart\":\"15:30:00\"," + "\"ReserveEnd\":\"15:59:59\"" + "}" + "]" + "}";

		return jsonString;
	}
	
	public String setMeetingRoom () {
		
		jsonString = "{" + "\"Result\":0," + "\"Error\": null," + "\"Data\":[" + "{" + "\"Code\":1,"
				+ "\"DateTime\":\"2017-04-13 00:00:00\"," + "\"MeetingRoomCode\":1," + "\"PurposeCode\":1,"
				+ "\"EmployeeCode\":1," + "\"Name\":\"çÇäÏü™\"," + "\"Email\":\"kohs@ais-info.co.jp\","
				+ "\"OAuth\":\"1234567890\"," + "\"ReserveDate\":\"2017-04-13 11:23:06\","
				+ "\"ReserveStart\":\"14:00:00\"," + "\"ReserveEnd\":\"14:59:59\"" + "}," + "{" + "\"Code\":2,"
				+ "\"DateTime\":\"2017-04-13 00:00:00\"," + "\"MeetingRoomCode\":1," + "\"PurposeCode\":2,"
				+ "\"EmployeeCode\":2," + "\"Name\":\"â©è~â‹\"," + "\"Email\":\"hwangsy@ais-info.co.jp\","
				+ "\"OAuth\":\"9876543210\"," + "\"ReserveDate\":\"2017-04-13 11:44:10\","
				+ "\"ReserveStart\":\"15:30:00\"," + "\"ReserveEnd\":\"15:59:59\"" + "}," + "{" + "\"Code\":5,"
				+ "\"DateTime\":\"2017-04-13 00:00:00\"," + "\"MeetingRoomCode\":1," + "\"PurposeCode\":3,"
				+ "\"EmployeeCode\":2," + "\"Name\":\"â©è~â‹\"," + "\"Email\":\"hwangsy@ais-info.co.jp\","
				+ "\"OAuth\":\"9876543210\"," + "\"ReserveDate\":\"2017-04-13 11:44:10\","
				+ "\"ReserveStart\":\"16:30:00\"," + "\"ReserveEnd\":\"17:29:29\"" + "}" + "]" + "}";
		
		return jsonString;
	}
	
	public String updateMeetingRoom () {
		
		jsonString = "{" + "\"Result\":0," + "\"Error\": null," + "\"Data\":[" + "{" + "\"Code\":1,"
				+ "\"DateTime\":\"2017-04-13 00:00:00\"," + "\"MeetingRoomCode\":1," + "\"PurposeCode\":1,"
				+ "\"EmployeeCode\":1," + "\"Name\":\"çÇäÏü™\"," + "\"Email\":\"kohs@ais-info.co.jp\","
				+ "\"OAuth\":\"1234567890\"," + "\"ReserveDate\":\"2017-04-13 11:23:06\","
				+ "\"ReserveStart\":\"14:00:00\"," + "\"ReserveEnd\":\"14:59:59\"" + "}," + "{" + "\"Code\":2,"
				+ "\"DateTime\":\"2017-04-13 00:00:00\"," + "\"MeetingRoomCode\":1," + "\"PurposeCode\":2,"
				+ "\"EmployeeCode\":2," + "\"Name\":\"â©è~â‹\"," + "\"Email\":\"hwangsy@ais-info.co.jp\","
				+ "\"OAuth\":\"9876543210\"," + "\"ReserveDate\":\"2017-04-13 11:44:10\","
				+ "\"ReserveStart\":\"15:00:00\"," + "\"ReserveEnd\":\"15:59:59\"" + "}," + "{" + "\"Code\":5,"
				+ "\"DateTime\":\"2017-04-13 00:00:00\"," + "\"MeetingRoomCode\":1," + "\"PurposeCode\":3,"
				+ "\"EmployeeCode\":2," + "\"Name\":\"â©è~â‹\"," + "\"Email\":\"hwangsy@ais-info.co.jp\","
				+ "\"OAuth\":\"9876543210\"," + "\"ReserveDate\":\"2017-04-13 11:44:10\","
				+ "\"ReserveStart\":\"16:30:00\"," + "\"ReserveEnd\":\"17:29:29\"" + "}" + "]" + "}";

		return jsonString;
	}
	
	public String deleteMeetingRoom () {
		
		jsonString = "{" + "\"Result\":0," + "\"Error\": null," + "\"Data\":[" + "{" + "\"Code\":1,"
				+ "\"DateTime\":\"2017-04-13 00:00:00\"," + "\"MeetingRoomCode\":1," + "\"PurposeCode\":1,"
				+ "\"EmployeeCode\":1," + "\"Name\":\"çÇäÏü™\"," + "\"Email\":\"kohs@ais-info.co.jp\","
				+ "\"OAuth\":\"1234567890\"," + "\"ReserveDate\":\"2017-04-13 11:23:06\","
				+ "\"ReserveStart\":\"14:00:00\"," + "\"ReserveEnd\":\"14:59:59\"" + "}," + "{" + "\"Code\":2,"
				+ "\"DateTime\":\"2017-04-13 00:00:00\"," + "\"MeetingRoomCode\":1," + "\"PurposeCode\":2,"
				+ "\"EmployeeCode\":2," + "\"Name\":\"â©è~â‹\"," + "\"Email\":\"hwangsy@ais-info.co.jp\","
				+ "\"OAuth\":\"9876543210\"," + "\"ReserveDate\":\"2017-04-13 11:44:10\","
				+ "\"ReserveStart\":\"15:00:00\"," + "\"ReserveEnd\":\"15:59:59\"" + "}" + "]" + "}";
		
		return jsonString;
	}
	
	public String setID () {
		
		jsonString = "{\"Result\": 0,\"Error\": null, \"Data\": null}";
		
		return jsonString;
	}
	
	public String getDataByKey(String key, String method) {
		try {
			byte[] data = key.getBytes(Define.DEFAULT_ENCODING);
			byte[] length = ArrayUtil.reverse(BitConverter.getBytes(data.length));
			try (Socket sock = new Socket(Define.SOCKET_IP_ADDRESS, Define.SOCKET_PORT)) {
				OutputStream out = sock.getOutputStream();
				InputStream in = sock.getInputStream();
				out.write(length);
				out.write(data);
				in.read(length);
				int len = BitConverter.toInt32(ArrayUtil.reverse(length), 0);
				data = new byte[len];
				in.read(data, 0, len);
//				return new String(data);
				switch (method) {
				case "getMaster":
					return getMaster();
				case "getMeetingRoom1":
					return getMeetingRoom1();
				case "getMeetingRoom2":
					return getMeetingRoom2();
				case "setMeetingRoom":
					return setMeetingRoom();
				case "updateMeetingRoom":
					return updateMeetingRoom();
				case "deleteMeetingRoom":
					return deleteMeetingRoom();
				case "setID":
					return setID();
				}
				return null;
			}
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}
}
