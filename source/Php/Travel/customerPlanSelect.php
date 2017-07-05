<?php
/**
 * 商品詳細画面
 */
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/AbstractAction.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Dao/ApplicationDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Entity/Application.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Entity/Member.php';
class customerPlanSelectAction extends AbstractAction {
	private $application;
	private $member;
	private $memo = null;
	protected function initialize() {
		parent::checkAuthUserToRedirect ();
		return true;
	}
	protected function main() {
		$this->member = parent::getUserSessionUnserialize ();
	}
	protected function error() {
	}
	public function getMember() {
		return $this->member;
	}
}
$obj = new customerPlanSelectAction ();
$obj->run ();
?>

<!DOCTYPE HTML>
<html>
<head>
<meta content='text/html; charset=UTF-8' http-equiv='Content-Type' />
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<meta name="viewport"
	content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
<script
	src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
<script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
<link rel="stylesheet"
	href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
<link href="../css/common.css" rel="stylesheet">
<link href="../css/customerPlanSelect.css?ver=3" rel="stylesheet">
<script type="text/javascript" src="../js/customerPlanSelect.js?ver=2"></script>
<title><?=$obj->getTitle()?></title>
<style>
</style>
</head>
<body>
	<header><?=$obj->getHeader()?>
	<div class="logout">
			<a href="/logout.php">Logout</a>
		</div>
	</header>
	<form method="post" action="../customerPlanConfirm.php">
		<div class="main">
			<div class="inline">
				<table class="information">
					<thead>
						<tr>
							<th colspan="2">詳細情報登録</th>
						</tr>
					</thead>
					<tbody>
						<tr>
							<td>商品名</td>
							<td><?=$_GET['productName'] ?></td>
						</tr>
						<tr>
							<td>人数</td>
							<td><select id="peopleNumber" name="peopleNumber">
									<option value="1">1</option>
									<option value="2">2</option>
									<option value="3">3</option>
									<option value="4">4</option>
									<option value="5">5</option>
							</select></td>
						</tr>
						<tr>
							<td colspan="2" class="member">
								<div class="member">
									<table >
										<thead>
											<tr>
												<th>姓名</th>
												<th>生年月日</th>
												<th>メモ</th>
											</tr>
										</thead>
										<tbody>
											<tr>
												<td><?=$obj->getMember()->getName()?></td>
												<td><?=$obj->getMember()->getBirth()?></td>
												<td><input type="text" name="pmemo"></td>
											</tr>
										</tbody>
									</table>
								</div>
							</td>
						</tr>
						<tr>
							<td colspan="2" class="errorLine"><span id="errorMsg"
								class="errorMsg"></span></td>
						</tr>
					</tbody>
					<tfoot>
						<tr>
							<td colspan="2"><input type="submit" value="予約確認"><input
								type="button" value="キャンセル"
								onclick="location.href='../customerPlanList.php'"></td>
						</tr>
					</tfoot>
				</table>
				<input type="hidden" name="productCode"
					value="<?=$_GET['productCode'] ?>">
			</div>
		</div>
		<div class="template">
			<table>
				<tbody id="addMemberTemplate">
					<tr>
						<td><input type='text' name="name" class="inputcheck" alt="姓名"></td>
						<td><input type='text' name="birth" class="inputcheck birth" alt="生年月日"></td>
						<td><input type='text' name="memo"></td>						
					</tr>
				</tbody>
			</table>
		</div>
	</form>
	<footer><?=$obj->getFooter()?></footer>
</body>
</html>