<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/AbstractAction.php';
class IndexAction extends AbstractAction {
	protected function initialize() {
		return true;
	}
	protected function main() {
	}
	protected function error() {
	}
}
$obj= new IndexAction ();
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
<link href="css/common.css" rel="stylesheet">
<link href="css/error.css" rel="stylesheet">
<script type="text/javascript" src="js/error.js"></script>
<title><?=$obj->getTitle()?></title>
</head>
<body>
	<header><?=$obj->getHeader()?></header>
	<div class=main>
		<div class="error">
			エラーが発生しました。<br>
			ログインページに移動します。<br />
			<span></span>
		</div>
	</div>
	<footer><?=$obj->getFooter()?></footer>
</body>
</html>
