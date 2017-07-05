package org.tempuri;

import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;

public class Test {
	public static void main(String[] args) {
		try {
			IMember member = new IMemberProxy();
			String key = member.setID("test@id.com", "123123", "太郎");
			System.out.println(getDataByKey(key));
			
			IMeetingRoom room = new IMeetingRoomProxy();
			key = room.getMaster();
			System.out.println(getDataByKey(key));
			
			key = room.getMeetingRoomList("2017-04-01");
			System.out.println(getDataByKey(key));
			
			key = room.getMeetingRoom("2017-04-01",1);
			System.out.println(getDataByKey(key));
			
			key = room.setMeetingRoom("2017-04-01",1,"17:00:00","18:00:00","123",0);
			System.out.println(getDataByKey(key));
			
			key = room.updateMeetingRoom(1,"17:00:00","18:00:00","1",0);
			System.out.println(getDataByKey(key));
			
			key = room.deleteMeetingRoom(1,"123");
			System.out.println(getDataByKey(key));
			
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
	protected static String getDataByKey(String key) {
		try {
			byte[] data = key.getBytes("UTF-8");
			byte[] length = reverse(BitConverter.getBytes(data.length));
			try (Socket sock = new Socket("192.168.1.200", 15000)) {
				OutputStream out = sock.getOutputStream();
				InputStream in = sock.getInputStream();
				out.write(length);
				out.write(data);
				in.read(length);
				int len = BitConverter.toInt32(reverse(length), 0);
				data = new byte[len];
				in.read(data, 0, len);
				return new String(data);
			}
		} catch (Throwable e) {
			throw new RuntimeException(e);
		}
	}
	public static byte[] reverse(byte[] data) {
		byte[] ret = new byte[data.length];
		for (int i = 0; i < ret.length; i++) {
			ret[i] = data[data.length - i - 1];
		}
		return ret;
	}
}
