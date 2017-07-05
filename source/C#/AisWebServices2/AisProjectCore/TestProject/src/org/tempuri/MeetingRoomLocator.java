/**
 * MeetingRoomLocator.java
 *
 * このファイルはWSDLから自動生成されました / [en]-(This file was auto-generated from WSDL)
 * Apache Axis 1.4 Apr 22, 2006 (06:55:48 PDT) WSDL2Java生成器によって / [en]-(by the Apache Axis 1.4 Apr 22, 2006 (06:55:48 PDT) WSDL2Java emitter.)
 */

package org.tempuri;

public class MeetingRoomLocator extends org.apache.axis.client.Service implements org.tempuri.MeetingRoom {

    public MeetingRoomLocator() {
    }


    public MeetingRoomLocator(org.apache.axis.EngineConfiguration config) {
        super(config);
    }

    public MeetingRoomLocator(java.lang.String wsdlLoc, javax.xml.namespace.QName sName) throws javax.xml.rpc.ServiceException {
        super(wsdlLoc, sName);
    }

    // BasicHttpBinding_IMeetingRoomのプロキシクラスの取得に使用します / [en]-(Use to get a proxy class for BasicHttpBinding_IMeetingRoom)
    private java.lang.String BasicHttpBinding_IMeetingRoom_address = "http://192.168.1.200:10000/Service/MeetingRoom.svc";

    public java.lang.String getBasicHttpBinding_IMeetingRoomAddress() {
        return BasicHttpBinding_IMeetingRoom_address;
    }

    // WSDDサービス名のデフォルトはポート名です / [en]-(The WSDD service name defaults to the port name.)
    private java.lang.String BasicHttpBinding_IMeetingRoomWSDDServiceName = "BasicHttpBinding_IMeetingRoom";

    public java.lang.String getBasicHttpBinding_IMeetingRoomWSDDServiceName() {
        return BasicHttpBinding_IMeetingRoomWSDDServiceName;
    }

    public void setBasicHttpBinding_IMeetingRoomWSDDServiceName(java.lang.String name) {
        BasicHttpBinding_IMeetingRoomWSDDServiceName = name;
    }

    public org.tempuri.IMeetingRoom getBasicHttpBinding_IMeetingRoom() throws javax.xml.rpc.ServiceException {
       java.net.URL endpoint;
        try {
            endpoint = new java.net.URL(BasicHttpBinding_IMeetingRoom_address);
        }
        catch (java.net.MalformedURLException e) {
            throw new javax.xml.rpc.ServiceException(e);
        }
        return getBasicHttpBinding_IMeetingRoom(endpoint);
    }

    public org.tempuri.IMeetingRoom getBasicHttpBinding_IMeetingRoom(java.net.URL portAddress) throws javax.xml.rpc.ServiceException {
        try {
            org.tempuri.BasicHttpBinding_IMeetingRoomStub _stub = new org.tempuri.BasicHttpBinding_IMeetingRoomStub(portAddress, this);
            _stub.setPortName(getBasicHttpBinding_IMeetingRoomWSDDServiceName());
            return _stub;
        }
        catch (org.apache.axis.AxisFault e) {
            return null;
        }
    }

    public void setBasicHttpBinding_IMeetingRoomEndpointAddress(java.lang.String address) {
        BasicHttpBinding_IMeetingRoom_address = address;
    }

    /**
     * 与えられたインターフェースに対して、スタブの実装を取得します。 / [en]-(For the given interface, get the stub implementation.)
     * このサービスが与えられたインターフェースに対してポートを持たない場合、 / [en]-(If this service has no port for the given interface,)
     * ServiceExceptionが投げられます。 / [en]-(then ServiceException is thrown.)
     */
    public java.rmi.Remote getPort(Class serviceEndpointInterface) throws javax.xml.rpc.ServiceException {
        try {
            if (org.tempuri.IMeetingRoom.class.isAssignableFrom(serviceEndpointInterface)) {
                org.tempuri.BasicHttpBinding_IMeetingRoomStub _stub = new org.tempuri.BasicHttpBinding_IMeetingRoomStub(new java.net.URL(BasicHttpBinding_IMeetingRoom_address), this);
                _stub.setPortName(getBasicHttpBinding_IMeetingRoomWSDDServiceName());
                return _stub;
            }
        }
        catch (java.lang.Throwable t) {
            throw new javax.xml.rpc.ServiceException(t);
        }
        throw new javax.xml.rpc.ServiceException("インターフェースに対するスタブの実装がありません: / [en]-(There is no stub implementation for the interface:)  " + (serviceEndpointInterface == null ? "null" : serviceEndpointInterface.getName()));
    }

    /**
     * 与えられたインターフェースに対して、スタブの実装を取得します。 / [en]-(For the given interface, get the stub implementation.)
     * このサービスが与えられたインターフェースに対してポートを持たない場合、 / [en]-(If this service has no port for the given interface,)
     * ServiceExceptionが投げられます。 / [en]-(then ServiceException is thrown.)
     */
    public java.rmi.Remote getPort(javax.xml.namespace.QName portName, Class serviceEndpointInterface) throws javax.xml.rpc.ServiceException {
        if (portName == null) {
            return getPort(serviceEndpointInterface);
        }
        java.lang.String inputPortName = portName.getLocalPart();
        if ("BasicHttpBinding_IMeetingRoom".equals(inputPortName)) {
            return getBasicHttpBinding_IMeetingRoom();
        }
        else  {
            java.rmi.Remote _stub = getPort(serviceEndpointInterface);
            ((org.apache.axis.client.Stub) _stub).setPortName(portName);
            return _stub;
        }
    }

    public javax.xml.namespace.QName getServiceName() {
        return new javax.xml.namespace.QName("http://tempuri.org/", "MeetingRoom");
    }

    private java.util.HashSet ports = null;

    public java.util.Iterator getPorts() {
        if (ports == null) {
            ports = new java.util.HashSet();
            ports.add(new javax.xml.namespace.QName("http://tempuri.org/", "BasicHttpBinding_IMeetingRoom"));
        }
        return ports.iterator();
    }

    /**
    * 指定したポート名に対するエンドポイントのアドレスをセットします / [en]-(Set the endpoint address for the specified port name.)
    */
    public void setEndpointAddress(java.lang.String portName, java.lang.String address) throws javax.xml.rpc.ServiceException {
        
if ("BasicHttpBinding_IMeetingRoom".equals(portName)) {
            setBasicHttpBinding_IMeetingRoomEndpointAddress(address);
        }
        else 
{ // Unknown Port Name
            throw new javax.xml.rpc.ServiceException(" 未知のポートに対してはエンドポイントのアドレスをセットできません / [en]-(Cannot set Endpoint Address for Unknown Port)" + portName);
        }
    }

    /**
    * 指定したポート名に対するエンドポイントのアドレスをセットします / [en]-(Set the endpoint address for the specified port name.)
    */
    public void setEndpointAddress(javax.xml.namespace.QName portName, java.lang.String address) throws javax.xml.rpc.ServiceException {
        setEndpointAddress(portName.getLocalPart(), address);
    }

}
