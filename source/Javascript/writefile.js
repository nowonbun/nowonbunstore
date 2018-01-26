var fs = require('fs');
var readline = require('readline');
var path = require('path');

var parameter = [];

var rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout
});

process.argv.forEach(function (val, index, array) {
    if(index > 1){
        parameter.push(val);
    }
});

(function(){
    if(parameter.length != 1){
        console.log('The parameter is wrong..');
        return;
    }
    var filepath = parameter[0];
    if (fs.existsSync( filepath ))
    {
        //saved = fs.writeFileSync(file, content, 'utf8');
        console.log('Exists file...');
        return;
    }
    fs.writeFileSync(filepath,'hello world','utf-8');
    /*var buffer = [];
    
    while(true){
        var exit = false;
        rl.question('WriteMode(if exit, put in "exit")> ', (answer) => {
            if('exit' === answer){
                exit = false;
                return;
            }
            buffer.push(answer);
            rl.close();
        });
        if(exit){
            break;
        }
    }*/
})();
process.exit();