<%@page import="common.FactoryJspServlet"%>
<%@page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8"%>
<%FactoryJspServlet.action("index.jsp", request, response);%>
<!DOCTYPE html>
<html>
<head>
	<jsp:include page="./share/pagetop.jsp"></jsp:include>
</head>
<body>
	<div class="container wow fadeIn animated" style="visibility: visible; animation-name: fadeIn;">
		<jsp:include page="./share/header.jsp"></jsp:include>
		<main class="mt-5">
			<div class="row">
			  <div class="col-12 d-flex align-items-stretch">
				<div class="card my-blog-post">
				  <div class="card-body">
					<h1 class="card-title">Card title</h1>
					<p class="card-text">At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis
					  praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint
					  occaecati cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id
					  est laborum et dolorum fuga. Et harum quidem rerum facilis est et expedita distinctio. Nam libero
					  tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat
					  facere possimus, omnis voluptas assumenda est, omnis dolor repellendus.</p>
				  </div>
				</div>
			  </div>
			</div>
			<div class="row mt-3 mb-3">
			  <div class="col-12 d-flex align-items-stretch">
				<div class="card my-style-custom">
				  <div class="card-body my-pre-post-nav">
					<div class="row">
						<div class="col-12 mb-1">pre</div>
						<div class="col-12 my-blog-line"></div>
						<div class="col-12 mt-1">post</div>
					</div>
				  </div>
				</div>
			  </div>
			</div>
			<fieldset class="box-shadow-0 px-3 py-3 blog-radius mb-3 my-style-custom" >
				<legend class="box-shadow-0 blog-legend px-3 blog-radius">최신글</legend>
				<div style="text-align:center;">
					<img src="./img/image_fix.png" style="height:40px">
				</div>
			</fieldset>
			<fieldset class="box-shadow-0 px-3 py-3 blog-radius mb-3 my-style-custom">
				<legend class="box-shadow-0 blog-legend px-3 blog-radius">댓글 달기</legend>
				<div style="text-align:center;">
					<img src="./img/image_fix.png" style="height:40px">
				</div>
			</fieldset>
		</main>
		<jsp:include page="./share/footer.jsp"></jsp:include>
	</div>
	<jsp:include page="./share/pagebottom.jsp"></jsp:include>
</body>
</html>