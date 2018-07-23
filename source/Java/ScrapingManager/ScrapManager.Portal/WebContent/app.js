var util = {
	parseInt : function(value) {
		var ret = Number(value);
		if (Number.isNaN(ret)) {
			return -1;
		}
		return ret;
	},
	isFunction : function(fn) {
		if (fn !== null && fn !== undefined && typeof fn === "function") {
			return true;
		}
		return false;
	}
}
var app = angular.module('app', [ "ngRoute" ]);

app.service('_service', [ function() {
	var host = location.origin.replace("http", "ws") + location.pathname;
	var ws = new WebSocket(host + "index");
	var delegate = {
		open : null,
		close : null,
		error : null,
		message : null
	};
	ws.onopen = function(msg) {
		if (delegate.open != null) {
			delegate.open.call(this, msg);
		}
	};
	ws.onclose = function(msg) {
		if (delegate.close != null) {
			delegate.close.call(this, msg);
		}
	};
	ws.onerror = function(msg) {
		if (delegate.error != null) {
			delegate.error.call(this, msg);
		}
	};
	ws.onmessage = function(msg) {
		if (delegate.message != null) {
			delegate.message.call(this, JSON.parse(msg.data));
		}
	};
	sendNode = function(node) {
		if (ws.readyState === 1) {
			ws.send(node);
		} else {
			setTimeout(function() {
				sendNode(node);
			}, 1000);
		}
	}
	function define(key, func) {
		if(util.isFunction(func)) {
			delegate[key] = func;
			return;
		}
		console.error("It's not defined because not function method.");
	}
	this.open = function(func, $scope) {
		define("open", func);
	};
	this.close = function(func, $scope) {
		define("close", func);
	};
	this.error = function(func, $scope) {
		define("error", func);
	}
	this.message = function(func, $scope) {
		define("message", func);
	};
	this.send = function(key, value, func) {
		if (util.isFunction(func)) {
			define("message", function(msg){
				if(msg.key === key){
					func.call(this, msg);
				}
			});
		}
		sendNode(JSON.stringify({
			key : key,
			value : value
		}));
	}
} ]);

app.service('_loading', [ '$rootScope', function($rootScope) {
	this.show = function() {
		$rootScope.loading = true;
	}
	this.hide = function() {
		$rootScope.loading = false;
	}
} ]);

app.factory('_tabledata',[ function(){
	return {
		show: function(obj,page,op) {
			var option = {
					responsive: true,
					select: true
				};
			obj.css("width","");
			return obj.DataTable(option);
		},
		ajaxShow: function(op){
			function convertColumn(val) {
				if (val == null) {
					return null;
				}
				var ret = [ {
					data : null
				} ];
				for ( var i in val) {
					ret.push({
						data : val[i]
					});
				}
				return ret;
			}
			return op.object.DataTable({
				ajax : {
					url : op.url,
					type : "POST",
					complete : function() {
						if(util.isFunction(op.completecb)) {
							op.completecb.call(this);
						}						
					},
					error : function(xhr, error, thrown) { }
				},
				select: {
					style : 'single'
				},
				columnDefs : [ {
					className : 'control',
					targets : 0,
					data : null,
					defaultContent : ''
				} ],
				responsive : {
					details : {
						type : 'column',
						target : 0
					}
				},
				columns : convertColumn(op.columns)
			});
		}
	};
}]);

app.run([ '$rootScope', '_loading', function($rootScope, _loading) {
	_loading.show();
} ]);

app.config(function($routeProvider) {
	$routeProvider.when("/", {
		controller : "main",
		templateUrl : "./partical/main.tpl.html"
	});
	$routeProvider.when("/brokerlist", {
		controller : "brokerlist",
		templateUrl : "./partical/brokerlist.tpl.html"
	});
	$routeProvider.when("/manual", {
		controller : "manual",
		templateUrl : "./partical/manual.tpl.html"
	});
	$routeProvider.when("/pingponglist", {
		controller : "pingponglist",
		templateUrl : "./partical/pingponglist.tpl.html"
	});
	$routeProvider.when("/commondatalist", {
		controller : "commondatalist",
		templateUrl : "./partical/commondatalist.tpl.html"
	});
	$routeProvider.when("/packagedatalist", {
		controller : "packagedatalist",
		templateUrl : "./partical/packagedatalist.tpl.html"
	});
	$routeProvider.when("/requestdatalist", {
		controller : "requestdatalist",
		templateUrl : "./partical/requestdatalist.tpl.html"
	});
	$routeProvider.when("/resultdatalist", {
		controller : "resultdatalist",
		templateUrl : "./partical/resultdatalist.tpl.html"
	});
	$routeProvider.when("/createkey", {
		controller : "createkey",
		templateUrl : "./partical/createkey.tpl.html"
	});
	$routeProvider.when("/apikeylist", {
		controller : "apikeylist",
		templateUrl : "./partical/apikeylist.tpl.html"
	});
	$routeProvider.otherwise({
		redirectTo : "/"
	});
});

app.directive("loading", function() {
	return {
		restrict : "A",
		template : "<div class='loader'></div>"
	};
});

app.filter('dateFormat', function() {
	return function(x) {
		var date = new Date(x);
		return date.getFullYear() + "/" + (date.getMonth() + 1) + "/"
				+ date.getDate() + " " + date.getHours() + ":"
				+ date.getMinutes() + ":" + date.getSeconds();
	};
});

app.controller('main', [ '$scope', '_loading', function($scope, _loading) {
	_loading.show();
	$scope.menu = [ {
		href : "./#!/manual",
		color : "#8c3500e8",
		icon : "fa-child",
		font : "#fff",
		description : "Manual scrapping"
	}, {
		href : "./#!/brokerlist",
		color : "#008c0be8",
		icon : "fa-server",
		font : "#fff",
		description : "Broker list"
	}, {
		href : "./#!/pingponglist",
		color : "#4a50f7e8",
		icon : "fa-gear",
		font : "#fff",
		description : "Ping-pong history"
	}, {
		href : "./#!/commondatalist",
		color : "#8c8200e8",
		icon : "fa-file-text-o",
		font : "#fff",
		description : "Common data list"
	}, {
		href : "./#!/packagedatalist",
		color : "#00648ce8",
		icon : "fa-file-code-o",
		font : "#fff",
		description : "Package data list"
	}, {
		href : "./#!/requestdatalist",
		color : "#b56ce2e8",
		icon : "fa-magic",
		font : "#fff",
		description : "Request data list"
	},{
		href : "./#!/apikeylist",
		color : "#8e6969d4",
		icon : "fa-address-book",
		font : "#fff",
		description : "API Key list"
	}, {
		href : "./#!/resultdatalist",
		color : "#8d9287e8",
		icon : "fa-magnet",
		font : "#fff",
		description : "Result data list"
	} ];
	_loading.hide();
} ]);

app.controller('brokerlist', [ '$scope', '_loading', '_tabledata', function($scope, _loading, _tabledata) {
	_loading.hide();
	var table = _tabledata.ajaxShow({
		object:$("#table-grid"),
		url:"./GetBroker",
		columns:["key", "ip", "count", "lastpingupdated", "connected", "disconnected", "active"]
	});
} ]);

app.controller('pingponglist', [ '$scope', '_loading', '_tabledata', function($scope, _loading, _tabledata) {
	_loading.hide();
	var table = _tabledata.ajaxShow({
		object:$("#table-grid"),
		url:"./GetPingPong",
		columns:["key", "ip", "lastupdatedStr"]
	});
} ]);

app.controller('commondatalist', [ '$scope', '_loading', '_tabledata', function($scope, _loading, _tabledata) {
    _loading.hide();
    var table = _tabledata.ajaxShow({
        object:$("#table-grid"),
        url:"./GetCommonDataList",
        columns:["mallkey", "key", "idx", "data"]
    });
} ]);

app.controller('packagedatalist', [ '$scope', '_loading', '_tabledata', function($scope, _loading, _tabledata) {
    _loading.hide();
    var table = _tabledata.ajaxShow({
        object:$("#table-grid"),
        url:"./GetPackageDataList",
        columns:["mallkey", "key", "idx", "separation","data"]
    });
} ]);

app.controller('manual', [ '$scope', '_loading', '$http',  function($scope, _loading, $http) {
	_loading.show();
	$("#mallcd").change(function() {
		var _this = $(this);
		$scope.$apply(function() {
			$scope.mallcd = _this.val();
		});
	});
	$("#scraptype").change(function() {
		var _this = $(this);
		$scope.$apply(function() {
			$scope.scraptype = _this.val();
		});
	});
	$("#exec").change(function() {
		var _this = $(this);
		$scope.$apply(function() {
			$scope.exec = _this.val();
		});
	});
	$('.mdb-select').material_select();
	_loading.hide();
	var validation = function() {
		if ($.trim($scope.apikey) === "") {
			return "apikey를 입력해주세요.";
		}
		if ($.trim($scope.mallcd) === "") {
			return "쇼핑몰을 선택해 주십시오.";
		}
		if ($.trim($scope.id1) === "") {
			return "아이디를 입력해 주십시오.";
		}
		if ($.trim($scope.pw1) === "") {
			return "패스워드를 입력해 주십시오.";
		}
		if ($.trim($scope.scraptype) === "") {
			return "스크래핑 코드를 입력해 주십시오.";
		}
		if ($.trim($scope.exec) === "") {
			return "스크래핑 종류를 입력해 주십시오.";
		}
		return true;
	}
	$scope.execute = function() {
		_loading.show();
		var valid = validation.call(true);
		if (valid !== true) {
			$scope.errorMsg = valid;
			_loading.hide();
			return;
		}
		$http({
			method : "GET",
			url : "ScrapStart",
			params : {
				ReqNo : $scope.reqno,
				MallCD : $scope.mallcd,
				Id1 : $scope.id1,
				Id2 : $scope.id2,
				Id3 : $scope.id3,
				Pw1 : $scope.pw1,
				Pw2 : $scope.pw2,
				Pw3 : $scope.pw3,
				Option1 : $scope.option1,
				Option2 : $scope.option2,
				Option3 : $scope.option3,
				ScrapType : $scope.scraptype,
				Sdate : $scope.sdate,
				Edate : $scope.edate,
				Exec : $scope.exec
			}
		}).then(function(res) {
			location.href = "./#!/";
		}, function(res) {
			console.error(res.statusText);
		});
	}
} ]);

app.controller('requestdatalist', [ '$scope', '_loading', '_tabledata', function($scope, _loading, _tabledata) {
	_loading.hide();
	var table = _tabledata.ajaxShow({
		object:$("#table-grid"),
		url:"./GetRequestDataList",
		columns:["mallkey","key","apikey","sdate","edate","scraptype","exec","id1","id2","id3","option1","option2","option3","createDateStr"]
	});
} ]);

app.controller('resultdatalist', [ '$scope', '_loading', '_tabledata', function($scope, _loading, _tabledata) {
    _loading.hide();
    var table = _tabledata.ajaxShow({
        object:$("#table-grid"),
        url:"./GetResultDataList",
        columns:["mallkey","key","resultcd","resultmsg","starttimeStr","endtimeStr"]
    });
} ]);

app.controller('apikeylist', [ '$scope', '_service', '_loading', '_tabledata', '$http', function($scope, _service, _loading, _tabledata, $http) {
	_loading.hide();
	var table = _tabledata.ajaxShow({
		object:$('#table-grid'),
		url:"./GetApiList",
		columns:["key","bizno","name","ip","callback"]
	});
	
	table.on('select', function(e, dt, type, indexes) {
		if (type === 'row') {
			$("#deletebtn").prop("disabled", false);
			$scope.selectid = table.rows(indexes).data()[0].key;
			$scope.$apply();
		}
	});
	table.on('deselect', function(e, dt, type, indexes) {
		if (type === 'row') {
			$("#deletebtn").prop("disabled", true);
			$scope.selectid = null;
			$scope.$apply();
		}
	});
	$scope.delete = function(apikey) {
		_service.send("apikeylist-del", $scope.selectid, function(msg){
			$scope.reload();
			$("#deletedlg").modal("hide");
		});					
	}
	$scope.reload = function(){
		table.ajax.reload();
	}

	$scope.apply = function(){
		var validation = function() {
			if ($.trim($scope.bizno) === "") {
				return "사업자번호를 입력해주세요.";
			}
			if ($.trim($scope.name) === "") {
				return "상호(명칭)를 입력해주세요.";
			}
			if ($.trim($scope.ip) === "") {
				return "아이피를 입력해주세요.";
			}
			return true;
		}
		var valid = validation.call(true);
		if (valid !== true) {
			$scope.errorMsg = valid;
			return;
		}
		$http({
			method : "POST",
			url : "./SetKeyData",
			data : {
				bizno : $scope.bizno,
				name : $scope.name,
				ip : $scope.ip,
				callback : $scope.callback
			}
		}).then(function(data,status,headers,config){
			$scope.reload();
			$("#createdlg").modal("hide");
		},function(data,status,headers,config){
			$scope.errorMsg = "System error";
		});
	}
} ]);