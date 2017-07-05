/**
 * TestLocator.java
 *
 * このファイルはWSDLから自動生成されました / [en]-(This file was auto-generated from WSDL)
 * Apache Axis 1.4 Apr 22, 2006 (06:55:48 PDT) WSDL2Java生成器によって / [en]-(by the Apache Axis 1.4 Apr 22, 2006 (06:55:48 PDT) WSDL2Java emitter.)
 */

package jp.co.aisdata;

public class TestLocator extends org.apache.axis.client.Service implements jp.co.aisdata.Test {

    public TestLocator() {
    }


    public TestLocator(org.apache.axis.EngineConfiguration config) {
        super(config);
    }

    public TestLocator(java.lang.String wsdlLoc, javax.xml.namespace.QName sName) throws javax.xml.rpc.ServiceException {
        super(wsdlLoc, sName);
    }

    // BasicHttpBinding_ITestのプロキシクラスの取得に使用します / [en]-(Use to get a proxy class for BasicHttpBinding_ITest)
    private java.lang.String BasicHttpBinding_ITest_address = "http://192.168.1.200:10000/Test.svc";

    public java.lang.String getBasicHttpBinding_ITestAddress() {
        return BasicHttpBinding_ITest_address;
    }

    // WSDDサービス名のデフォルトはポート名です / [en]-(The WSDD service name defaults to the port name.)
    private java.lang.String BasicHttpBinding_ITestWSDDServiceName = "BasicHttpBinding_ITest";

    public java.lang.String getBasicHttpBinding_ITestWSDDServiceName() {
        return BasicHttpBinding_ITestWSDDServiceName;
    }

    public void setBasicHttpBinding_ITestWSDDServiceName(java.lang.String name) {
        BasicHttpBinding_ITestWSDDServiceName = name;
    }

    public jp.co.aisdata.ITest getBasicHttpBinding_ITest() throws javax.xml.rpc.ServiceException {
       java.net.URL endpoint;
        try {
            endpoint = new java.net.URL(BasicHttpBinding_ITest_address);
        }
        catch (java.net.MalformedURLException e) {
            throw new javax.xml.rpc.ServiceException(e);
        }
        return getBasicHttpBinding_ITest(endpoint);
    }

    public jp.co.aisdata.ITest getBasicHttpBinding_ITest(java.net.URL portAddress) throws javax.xml.rpc.ServiceException {
        try {
            jp.co.aisdata.BasicHttpBinding_ITestStub _stub = new jp.co.aisdata.BasicHttpBinding_ITestStub(portAddress, this);
            _stub.setPortName(getBasicHttpBinding_ITestWSDDServiceName());
            return _stub;
        }
        catch (org.apache.axis.AxisFault e) {
            return null;
        }
    }

    public void setBasicHttpBinding_ITestEndpointAddress(java.lang.String address) {
        BasicHttpBinding_ITest_address = address;
    }

    /**
     * 与えられたインターフェースに対して、スタブの実装を取得します。 / [en]-(For the given interface, get the stub implementation.)
     * このサービスが与えられたインターフェースに対してポートを持たない場合、 / [en]-(If this service has no port for the given interface,)
     * ServiceExceptionが投げられます。 / [en]-(then ServiceException is thrown.)
     */
    public java.rmi.Remote getPort(Class serviceEndpointInterface) throws javax.xml.rpc.ServiceException {
        try {
            if (jp.co.aisdata.ITest.class.isAssignableFrom(serviceEndpointInterface)) {
                jp.co.aisdata.BasicHttpBinding_ITestStub _stub = new jp.co.aisdata.BasicHttpBinding_ITestStub(new java.net.URL(BasicHttpBinding_ITest_address), this);
                _stub.setPortName(getBasicHttpBinding_ITestWSDDServiceName());
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
        if ("BasicHttpBinding_ITest".equals(inputPortName)) {
            return getBasicHttpBinding_ITest();
        }
        else  {
            java.rmi.Remote _stub = getPort(serviceEndpointInterface);
            ((org.apache.axis.client.Stub) _stub).setPortName(portName);
            return _stub;
        }
    }

    public javax.xml.namespace.QName getServiceName() {
        return new javax.xml.namespace.QName("http://tempuri.org/", "Test");
    }

    private java.util.HashSet ports = null;

    public java.util.Iterator getPorts() {
        if (ports == null) {
            ports = new java.util.HashSet();
            ports.add(new javax.xml.namespace.QName("http://tempuri.org/", "BasicHttpBinding_ITest"));
        }
        return ports.iterator();
    }

    /**
    * 指定したポート名に対するエンドポイントのアドレスをセットします / [en]-(Set the endpoint address for the specified port name.)
    */
    public void setEndpointAddress(java.lang.String portName, java.lang.String address) throws javax.xml.rpc.ServiceException {
        
if ("BasicHttpBinding_ITest".equals(portName)) {
            setBasicHttpBinding_ITestEndpointAddress(address);
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
