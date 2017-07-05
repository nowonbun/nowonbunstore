/**
 * IMeetingRoom.java
 *
 * このファイルはWSDLから自動生成されました / [en]-(This file was auto-generated from WSDL)
 * Apache Axis 1.4 Apr 22, 2006 (06:55:48 PDT) WSDL2Java生成器によって / [en]-(by the Apache Axis 1.4 Apr 22, 2006 (06:55:48 PDT) WSDL2Java emitter.)
 */

package org.tempuri;

public interface IMeetingRoom extends java.rmi.Remote {
    public java.lang.String getMaster() throws java.rmi.RemoteException;
    public java.lang.String getMeetingRoomList(java.lang.String date) throws java.rmi.RemoteException;
    public java.lang.String getMeetingRoom(java.lang.String date, java.lang.Integer meetingRoomCode) throws java.rmi.RemoteException;
    public java.lang.String setMeetingRoom(java.lang.String date, java.lang.Integer meetingRoomCode, java.lang.String startTime, java.lang.String endTime, java.lang.String token, java.lang.Integer purposeCode) throws java.rmi.RemoteException;
    public java.lang.String updateMeetingRoom(java.lang.Integer code, java.lang.String startTime, java.lang.String endTime, java.lang.String token, java.lang.Integer purposeCode) throws java.rmi.RemoteException;
    public java.lang.String deleteMeetingRoom(java.lang.Integer code, java.lang.String token) throws java.rmi.RemoteException;
}
