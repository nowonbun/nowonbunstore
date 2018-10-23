<%@page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8"%>
<header>
	<div id="slide-out" class="side-nav sn-bg-4 mdb-sidenav fixed" style="transform: translateX(-100%);">
		<ul class="custom-scrollbar list-unstyled ps ps--theme_default ps--active-y" style="max-height: 100vh;" data-ps-id="eb47bdf6-9d5d-addc-556e-6d5d60c0f0da">
			<li class="logo-sn d-block waves-effect">
				<div class="text-center">
					<a class="pl-0" href="https://mdbootstrap.com/"> <span class="main-title-nav"> <strong>개발 일지</strong>
					</span>
					</a>
				</div>
			</li>
			<li>
				<hr class="my-1">
			</li>
			<li>
				<jsp:include page="./sidemenu.jsp"></jsp:include>
			</li>
		</ul>
		<div class="sidenav-bg mask-strong"></div>
	</div>
	<nav class="navbar fixed-top navbar-expand-md navbar-light white double-nav scrolling-navbar">
		<div class="float-left">
			<a href="#" data-activates="slide-out" class="button-collapse"><i class="fa fa-bars"></i><span class="sr-only" aria-hidden="true">Toggle side navigation</span></a>
		</div>
		<div class="mr-auto">
			<span class="d-none d-md-inline-block"> <a id="navbar-category-gettingstarted-jquery" class="btn btn-info btn-sm my-0 ml-3 waves-effect waves-light" href="/getting-started/" role="button">
					<i class="fa fa-home"></i> <span class="d-none d-lg-inline-block mr-1">Home</span>
			</a> <span id="dynamicContentWrapper-mainNavbar2"></span>
			</span>
		</div>
		<span class="main-title-nav"> <strong>개발 일지</strong>
		</span>
		<ul class="nav navbar-nav nav-flex-icons ml-auto">
			<li class="nav-item"><a href="/contact" data-toggle="modal" data-target="#contactForm" class="nav-link waves-effect"> <i class="fa fa-envelope"></i> <span class="sr-only">Contact
						us</span>
			</a></li>
		</ul>
	</nav>
</header>