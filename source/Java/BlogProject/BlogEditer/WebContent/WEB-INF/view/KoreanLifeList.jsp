<%@page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8"%>
<!DOCTYPE html>
<html>
<head>
<jsp:include page="./share/pagetop.jsp"></jsp:include>
</head>
<body>
	<div class="container wow fadeIn animated" style="visibility: visible; animation-name: fadeIn;">
		<jsp:include page="./share/header.jsp"></jsp:include>
		<main class="mt-5">
			<div class="container">
				<div class="list-title">
	            	<h1>한국 생활 리스트</h1>
	            </div>
	            <section>
	            	<div class="wow fadeIn" style="text-align:right;">
	                    <button type="button" class="btn btn-md btn-primary">Write Post</button>
	                </div>
	                <hr class="mb-3 mt-3">
	                <!--Grid row-->
	                <div class="row wow fadeIn">
	                    <!--Grid column-->
	                    <div class="col-12">
	                        <h3 class="mb-3 font-weight-bold dark-grey-text">
	                            <strong>MDB Quick Start</strong>
	                        </h3>
	                        <p class="grey-text">Get started with MDBootstrap, the world's most popular Material Design framework for building responsive,mobile-first sites.</p>
	                        <a href="https://www.youtube.com/watch?v=cXTThxoywNQ" target="_blank" class="btn btn-default btn-md">Go view<i class="fa fa-play ml-2"></i>
	                        </a>
	                    </div>
	                    <!--Grid column-->
	                </div>
	                <!--Grid row-->
	                <hr class="mb-3 mt-3">
	                <!--Grid row-->
	                <div class="row mt-3 wow fadeIn">
	                    <div class="col-lg-5 col-xl-4 mb-4">
	                        <div class="view overlay rounded z-depth-1">
	                            <img src="https://mdbootstrap.com/wp-content/uploads/2017/11/brandflow-tutorial-fb.jpg" class="img-fluid" alt="">
	                            <a href="https://mdbootstrap.com/automated-app-start/" target="_blank">
	                                <div class="mask rgba-white-slight"></div>
	                            </a>
	                        </div>
	                    </div>
	                    <div class="col-lg-7 col-xl-7 ml-xl-4 mb-4">
	                        <h3 class="mb-3 font-weight-bold dark-grey-text">
	                            <strong>Bootstrap Automation</strong>
	                        </h3>
	                        <p class="grey-text">Learn how to create a smart website which learns your user and reacts properly to his behavior.</p>
	                        <a href="https://mdbootstrap.com/automated-app-start/" target="_blank" class="btn btn-primary btn-md">Start tutorial
	                            <i class="fa fa-play ml-2"></i>
	                        </a>
	                    </div>
	                </div>
	                <hr class="mb-3 mt-3">
	                
	                <nav class="d-flex justify-content-center wow fadeIn">
	                    <ul class="pagination pg-blue">
	                        <li class="page-item disabled">
	                            <a class="page-link" href="#" aria-label="Previous">
	                                <span aria-hidden="true">&laquo;</span>
	                                <span class="sr-only">Previous</span>
	                            </a>
	                        </li>
	                        <li class="page-item active">
	                            <a class="page-link" href="#">1
	                                <span class="sr-only">(current)</span>
	                            </a>
	                        </li>
	                        <li class="page-item">
	                            <a class="page-link" href="#">2</a>
	                        </li>
	                        <li class="page-item">
	                            <a class="page-link" href="#">3</a>
	                        </li>
	                        <li class="page-item">
	                            <a class="page-link" href="#">4</a>
	                        </li>
	                        <li class="page-item">
	                            <a class="page-link" href="#">5</a>
	                        </li>
	                        <li class="page-item">
	                            <a class="page-link" href="#" aria-label="Next">
	                                <span aria-hidden="true">&raquo;</span>
	                                <span class="sr-only">Next</span>
	                            </a>
	                        </li>
	                    </ul>
	                </nav>
	            </section>
	        </div>
		</main>
		<jsp:include page="./share/footer.jsp"></jsp:include>
		<jsp:include page="./share/pagebottom.jsp"></jsp:include>
	</div>
</body>
</html>