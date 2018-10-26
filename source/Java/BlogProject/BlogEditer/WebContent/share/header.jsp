<%@page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8"%>
<header class="mt-5">
	<section class="card my-blog-header" >
		<div class="background"></div>
		<div class="card-body text-center py-3 px-3 my-3">
			<h1 class="mb-4" style="margin:0px!important;">
				<strong>명월의 개발 일기</strong>
			</h1>
		</div>
		<!-- Navbar -->
		<nav class="navbar navbar-expand-lg navbar-light white menu-nav-tool-blog">
			<!-- Collapse button -->
			<button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
				<span class="navbar-toggler-icon"></span>
				
			</button>
			<!-- Collapsible content -->
			<div class="collapse navbar-collapse" id="navbarSupportedContent">
				<ul class="navbar-nav mr-auto">
					<jsp:include page="menu.jsp"></jsp:include>
				</ul>
			</div>
		</nav>
		<!-- Navbar -->
	</section>
</header>