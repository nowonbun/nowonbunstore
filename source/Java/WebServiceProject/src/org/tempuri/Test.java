/**
 * Test.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis 1.4 Apr 22, 2006 (06:55:48 PDT) WSDL2Java emitter.
 */

package org.tempuri;

public interface Test extends javax.xml.rpc.Service {
    public java.lang.String getBasicHttpBinding_ITestAddress();

    public org.tempuri.ITest getBasicHttpBinding_ITest() throws javax.xml.rpc.ServiceException;

    public org.tempuri.ITest getBasicHttpBinding_ITest(java.net.URL portAddress) throws javax.xml.rpc.ServiceException;
}
