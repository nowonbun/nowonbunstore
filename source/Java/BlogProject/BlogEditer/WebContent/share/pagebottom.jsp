<%@page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8"%>
<script type="text/javascript" src="js/jquery-3.3.1.min.js"></script>
<script type="text/javascript" src="js/popper.min.js"></script>
<!-- script type="text/javascript" src="js/bootstrap.min.js"></script-->
<script type="text/javascript" src="js/compiled-4.5.9.min.js"></script>
<script src="http://cdnjs.cloudflare.com/ajax/libs/summernote/0.8.9/summernote.js"></script>
<script>
	/*var side = (function(obj) {
		$(obj.eventAttach);
		$(obj.onLoad);
		return obj;
	})({
		onLoad : function() {
			new WOW().init();
			$(".button-collapse").sideNav();
			var el = document.querySelector('.custom-scrollbar');
			Ps.initialize(el);
		},
		eventAttach : function() {
			$(document).off("click", "#createCategoryBtn").on("click", "#createCategoryBtn", function() {
				$.ajax({
					url : "GetCategoryList",
					type : "GET",
					success : function(data, textStatus, jqXHR) {
						$("#blogSelectArea").html("");
						$("#blogSelectArea").append($(data).prop("id", "parentCategory"));
						$("#blogSelectArea").append($("<label></label>").html("Parent Category"));
						$('#parentCategory').material_select();
						$("#createCategoryModal").modal("show");
					}
				});
			});
			$("#createCategoryModal").on('hidden.bs.modal', function() {
				$("#categoryNm").val("");
				$('#parentCategory').material_select("destroy");
				$("#blogSelectArea").html("");
			});
			$("#createCategory").on("click", function() {
				$.ajax({
					url : "SetCategory",
					type : "POST",
					dataType : "json",
					data : JSON.stringify({
						categoryName : $("#categoryNm").val(),
						parent : $("#parentCategory").val()
					}),
					success : function(data, textStatus, jqXHR) {
						if (data.data === "ok") {
							$("#createCategoryModal").modal("hide");
							toastr.info("The category was updated.");
							return;
						}
						toastr.error("The category was exists.");
					}
				});
			});
		}
	});*/
</script>