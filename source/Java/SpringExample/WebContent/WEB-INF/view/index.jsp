<%@ page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core"%>
<!DOCTYPE html>
<html>
<head>
<meta charset="EUC-KR">
<title>Insert title here</title>
</head>
<body>
<c:if test="${test.check eq true}">
    ${test.data} 
</c:if>

<br />
<c:forEach items="${test.iterdata}" var="item">
    <label >${item}</label>
</c:forEach>
<script src="https://code.jquery.com/jquery-3.3.1.min.js"></script>
<script>
	$(function(){
		$.ajax({
			//url:"ajax",
			url:"ajax.html",
			type: "GET",
			//dataType: "json",
			//data: JSON.stringify({data:"test"}),
			success: function (data, textStatus, jqXHR) {
			    console.log(data);
			},
			error: function (jqXHR, textStatus, errorThrown) {
			    
			},
			complete: function (jqXHR, textStatus) {
			    
			}
		});
	});
</script>
</body>
</html>