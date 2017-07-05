<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/AbstractAction.php';
class AppltCompleteAction extends AbstractAction {
	protected function initialize() {
		return true;
	}
	protected function main() {
	}
	protected function error() {
	}
}
$obj= new AppltCompleteAction();
$obj->run ();
?>
<!DOCTYPE html>
<html>
<head>
<meta content='text/html; charset=UTF-8' http-equiv='Content-Type' />
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<meta name="viewport"
	content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
<script
	src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
<link href="../css/common.css" rel="stylesheet">
<link href="../css/applyComplete.css" rel="stylesheet">
<script type="text/javascript" src="../js/adminProductAddComplete.js"></script>
<title><?=$obj->getTitle()?></title>
</head>
<body>
	<header><?=$obj->getHeader()?></header>
	<div class=main>
		<div>旅行商品追加完了。</div>
		<div><input type="button" value="メニューへ戻る"></div>
	</div>
	<footer><?=$obj->getFooter()?></footer>
</body>
</html>