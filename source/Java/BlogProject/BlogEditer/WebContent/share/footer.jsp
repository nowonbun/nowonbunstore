<%@page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8"%>
<footer class="page-footer text-center font-small mdb-color darken-2 mt-4 wow fadeIn">
	<div class="footer-copyright py-3">
		2018 Copyright: <a href="https://mdbootstrap.com/bootstrap-tutorial/" target="_blank">www.nowonbun.com </a>
	</div>
</footer>

<div class="modal fade" id="createCategoryModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
	<div class="modal-dialog modal-notify modal-success" role="document">
		<div class="modal-content">
			<div class="modal-header">
				<p class="heading lead">Create Category</p>
				<button type="button" class="close" data-dismiss="modal" aria-label="Close">
					<span aria-hidden="true" class="white-text">&times;</span>
				</button>
			</div>
			<div class="modal-body">
				<div class="text-center">
					<div class="md-form">
						<input type="text" id="categoryNm" class="form-control" style="margin-bottom: 2.5rem;"> <label for="categoryNm">Category Name</label>
					</div>
					<div class="md-form blog-select" id="blogSelectArea"></div>
				</div>
			</div>
			<div class="modal-footer">
				<button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
				<button type="button" class="btn btn-primary" id="createCategory">Create</button>
			</div>
		</div>
	</div>
</div>