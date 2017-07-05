package jp.co.aisdata;

public class ITestProxy implements jp.co.aisdata.ITest {
  private String _endpoint = null;
  private jp.co.aisdata.ITest iTest = null;
  
  public ITestProxy() {
    _initITestProxy();
  }
  
  public ITestProxy(String endpoint) {
    _endpoint = endpoint;
    _initITestProxy();
  }
  
  private void _initITestProxy() {
    try {
      iTest = (new jp.co.aisdata.TestLocator()).getBasicHttpBinding_ITest();
      if (iTest != null) {
        if (_endpoint != null)
          ((javax.xml.rpc.Stub)iTest)._setProperty("javax.xml.rpc.service.endpoint.address", _endpoint);
        else
          _endpoint = (String)((javax.xml.rpc.Stub)iTest)._getProperty("javax.xml.rpc.service.endpoint.address");
      }
      
    }
    catch (javax.xml.rpc.ServiceException serviceException) {}
  }
  
  public String getEndpoint() {
    return _endpoint;
  }
  
  public void setEndpoint(String endpoint) {
    _endpoint = endpoint;
    if (iTest != null)
      ((javax.xml.rpc.Stub)iTest)._setProperty("javax.xml.rpc.service.endpoint.address", _endpoint);
    
  }
  
  public jp.co.aisdata.ITest getITest() {
    if (iTest == null)
      _initITestProxy();
    return iTest;
  }
  
  public java.lang.String doWork() throws java.rmi.RemoteException{
    if (iTest == null)
      _initITestProxy();
    return iTest.doWork();
  }
  
  
}