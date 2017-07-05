package org.tempuri;

public class IMemberProxy implements org.tempuri.IMember {
  private String _endpoint = null;
  private org.tempuri.IMember iMember = null;
  
  public IMemberProxy() {
    _initIMemberProxy();
  }
  
  public IMemberProxy(String endpoint) {
    _endpoint = endpoint;
    _initIMemberProxy();
  }
  
  private void _initIMemberProxy() {
    try {
      iMember = (new org.tempuri.MemberLocator()).getBasicHttpBinding_IMember();
      if (iMember != null) {
        if (_endpoint != null)
          ((javax.xml.rpc.Stub)iMember)._setProperty("javax.xml.rpc.service.endpoint.address", _endpoint);
        else
          _endpoint = (String)((javax.xml.rpc.Stub)iMember)._getProperty("javax.xml.rpc.service.endpoint.address");
      }
      
    }
    catch (javax.xml.rpc.ServiceException serviceException) {}
  }
  
  public String getEndpoint() {
    return _endpoint;
  }
  
  public void setEndpoint(String endpoint) {
    _endpoint = endpoint;
    if (iMember != null)
      ((javax.xml.rpc.Stub)iMember)._setProperty("javax.xml.rpc.service.endpoint.address", _endpoint);
    
  }
  
  public org.tempuri.IMember getIMember() {
    if (iMember == null)
      _initIMemberProxy();
    return iMember;
  }
  
  public java.lang.String setID(java.lang.String email, java.lang.String token, java.lang.String name) throws java.rmi.RemoteException{
    if (iMember == null)
      _initIMemberProxy();
    return iMember.setID(email, token, name);
  }
  
  
}