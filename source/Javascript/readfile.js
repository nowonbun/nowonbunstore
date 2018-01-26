var fs = require('fs');
var path = require('path');

var parameter = [];

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
    if(!fs.existsSync( filepath )){
        console.log('Not exists file.');
        return;
    }
    /*
    if(!fs.lstatSync(filepath).isFile()){
        console.log('Not exists file.');
        return;
    }*/
    var content = fs.readFileSync(filepath,'utf-8');
    console.log(content);
})();
process.exit();