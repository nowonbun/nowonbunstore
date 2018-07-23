#coding: utf-8
from socket import *
from select import *
import sys
import threading,time,os

class WebServer:
    def __init__(self):
        self.HOST = ''
        self.PORT = 80
        self.BUFSIZE = 4096
        self.ADDR = (self.HOST,self.PORT)
        self.isAccept = False 
        self.sock = socket(AF_INET, SOCK_STREAM)
        self.sock.bind(self.ADDR)
        self.sock.listen(100)

    def run(self):
        self.isAccept = True
        self.isListen = True
        while True:
            c,info = self.sock.accept()
            if self.isListen == False:
                break
	    t = threading.Thread(target=self.recv,args=(c,))
            t.start()
            
    def recv(self,client):
        buffer = client.recv(self.BUFSIZE)
        print(buffer)
        self.send(client)

    def send(self,client):
        size = self.filesize("index.html")
        data = self.getFile("index.html")
        msg = "HTTP/1.1 200 OK\r\n"
        msg += "Keep-Alive: timeout=15, max=93\r\n"
        msg += "Connection: Keep-Alive\r\n"
        msg += "content-length: "+str(size)+"\r\n"
        msg += "\r\n"
        msg += data
        client.send(msg)

    def filesize(self,fname):
        info = os.stat(fname)
        return info.st_size

    def getFile(self,fname):
        try:
            f = open(fname, 'r')
            return f.read()
        except Exception as e:
            print(e)
        finally:
            f.close()


    def exit(self):
        self.isListen = False;
        try:
            c = socket(AF_INET, SOCK_STREAM)
            c.connect(self.ADDR)
            c.close()
        except Exception as e:
            print(e)
        finally:
            del c

    def __del__(self):
        if self.isAccept == True:
            try:
                self.sock.close()
                del self.sock
            except Exception as e:
                print(e)


obj = WebServer()
obj.run()
