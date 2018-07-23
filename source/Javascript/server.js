const net = require('net');

net.createServer(function(c) { //'connection' listener
    console.log('server connected');
    c.on('end', function() {
        console.log('server disconnected');
    });
    
    c.on('error',function(err){
        console.log(err);
    });
    c.on('close',function(err){
        console.log(err);
    });
    c.write('hello\r\n');
    //c.pipe(c);
    //c.close();
}).listen(9999);