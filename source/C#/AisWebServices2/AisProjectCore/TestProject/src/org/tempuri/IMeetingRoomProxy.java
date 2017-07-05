package org.tempuri;

public class IMeetingRoomProxy implements org.tempuri.IMeetingRoom {
  private String _endpoint = null;
  private org.tempuri.IMeetingRoom iMeetingRoom = null;
  
  public IMeetingRoomProxy() {
    _initIMeetingRoomProxy();
  }
  
  public IMeetingRoomProxy(String endpoint) {
    _endpoint = endpoint;
    _initIMeetingRoomProxy();
  }
  
  private void _initIMeetingRoomProxy() {
    try {
      iMeetingRoom = (new org.tempuri.MeetingRoomLocator()).getBasicHttpBinding_IMeetingRoom();
      if (iMeetingRoom != null) {
        if (_endpoint != null)
          ((javax.xml.rpc.Stub)iMeetingRoom)._setProperty("javax.xml.rpc.service.endpoint.address", _endpoint);
        else
          _endpoint = (String)((javax.xml.rpc.Stub)iMeetingRoom)._getProperty("javax.xml.rpc.service.endpoint.address");
      }
      
    }
    catch (javax.xml.rpc.ServiceException serviceException) {}
  }
  
  public String getEndpoint() {
    return _endpoint;
  }
  
  public void setEndpoint(String endpoint) {
    _endpoint = endpoint;
    if (iMeetingRoom != null)
      ((javax.xml.rpc.Stub)iMeetingRoom)._setProperty("javax.xml.rpc.service.endpoint.address", _endpoint);
    
  }
  
  public org.tempuri.IMeetingRoom getIMeetingRoom() {
    if (iMeetingRoom == null)
      _initIMeetingRoomProxy();
    return iMeetingRoom;
  }
  
  public java.lang.String getMaster() throws java.rmi.RemoteException{
    if (iMeetingRoom == null)
      _initIMeetingRoomProxy();
    return iMeetingRoom.getMaster();
  }
  
  public java.lang.String getMeetingRoomList(java.lang.String date) throws java.rmi.RemoteException{
    if (iMeetingRoom == null)
      _initIMeetingRoomProxy();
    return iMeetingRoom.getMeetingRoomList(date);
  }
  
  public java.lang.String getMeetingRoom(java.lang.String date, java.lang.Integer meetingRoomCode) throws java.rmi.RemoteException{
    if (iMeetingRoom == null)
      _initIMeetingRoomProxy();
    return iMeetingRoom.getMeetingRoom(date, meetingRoomCode);
  }
  
  public java.lang.String setMeetingRoom(java.lang.String date, java.lang.Integer meetingRoomCode, java.lang.String startTime, java.lang.String endTime, java.lang.String token, java.lang.Integer purposeCode) throws java.rmi.RemoteException{
    if (iMeetingRoom == null)
      _initIMeetingRoomProxy();
    return iMeetingRoom.setMeetingRoom(date, meetingRoomCode, startTime, endTime, token, purposeCode);
  }
  
  public java.lang.String updateMeetingRoom(java.lang.Integer code, java.lang.String startTime, java.lang.String endTime, java.lang.String token, java.lang.Integer purposeCode) throws java.rmi.RemoteException{
    if (iMeetingRoom == null)
      _initIMeetingRoomProxy();
    return iMeetingRoom.updateMeetingRoom(code, startTime, endTime, token, purposeCode);
  }
  
  public java.lang.String deleteMeetingRoom(java.lang.Integer code, java.lang.String token) throws java.rmi.RemoteException{
    if (iMeetingRoom == null)
      _initIMeetingRoomProxy();
    return iMeetingRoom.deleteMeetingRoom(code, token);
  }
  
  
}