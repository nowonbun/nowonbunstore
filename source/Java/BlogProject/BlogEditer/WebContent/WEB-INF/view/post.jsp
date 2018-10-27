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
			<div style="text-align:right">
				<a class="btn btn-primary btn-sm" role="button">작성하기</a>
			</div>
			<div class="row">
			  <div class="col-12 d-flex align-items-stretch">
				<div class="card my-blog-post">
				  <div class="card-body">
				  	<form>
						<h1 class="card-title">
							<input class="form-control form-control-lg" type="text" placeholder="제목">
						</h1>
						<div class="card-text">
							<textarea id="summernote" name="editordata">
							</textarea>
						</div>
						<div class="mt-2 row">
							<div class="col-3">
								<input class="form-control form-control-sm" type="text" placeholder="urlkey">
							</div>
							<div class="col-2">
								<input class="form-control form-control-sm" type="number" placeholder="CHANGEFREG">
							</div>
							<div class="col-2">
								<input class="form-control form-control-sm" type="number" placeholder="PRIORITY">	
							</div>
							<div class="col-5">
								<div class="row">
									<div class="col-8">
										<input class="form-control form-control-sm" type="text" placeholder="IMAGE">
									</div>
									<div class="file-field col-4">
										<div class="btn btn-primary btn-sm float-left" style="margin:0px;">
											<span>Choose</span> <input type="file" id="img_file" accept="image/*">
										</div>
									</div>
								</div>
							</div>
						</div>
					</form>
				  </div>
				</div>
			  </div>
			</div>
			<fieldset class="box-shadow-0 px-3 py-3 blog-radius mb-3 mt-3 my-style-custom" >
				<legend class="box-shadow-0 blog-legend px-3 blog-radius">
					<label>최신글</label>
				</legend>
				<div style="text-align:center;">
					<img src="./img/image_fix.png" style="height:40px">
				</div>
			</fieldset>
			<fieldset class="box-shadow-0 px-3 py-3 blog-radius mb-3 my-style-custom">
				<legend class="box-shadow-0 blog-legend px-3 blog-radius">
					<label>댓글 달기</label>
				</legend>
				<div style="text-align:center;">
					<img src="./img/image_fix.png" style="height:40px">
				</div>
			</fieldset>
		</main>
		<jsp:include page="./share/footer.jsp"></jsp:include>
	</div>
	<jsp:include page="./share/pagebottom.jsp"></jsp:include>
	<script>
		$(function(){
			$('#summernote').summernote({
				height: 500
			});
			$(".note-popover.popover").each(function(){
				$(this).hide();
			});
		});
	</script>
</body>
</html>