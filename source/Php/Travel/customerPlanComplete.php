<?php
/**
 * 商品詳細画面
 */
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/AbstractAction.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Entity/Application.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Entity/Application_Member.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Dao/ApplicationDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Dao/ApplicationMemberDao.php';

class customerPlanCompleteAction extends AbstractAction {	
	private $getUser;
	private $applicationmemberlist;
	
	protected function initialize() {
		parent::checkAuthUserToRedirect ();
		return true;
	}
	protected function main() {
		$applicationmember = new Application_member();
		$application = new Application ();
		$applicationDao = new ApplicationDao ();
		$applicationMemberDao = new ApplicationMemberDao();
		
		$user = parent::getUserSessionUnserialize ();
		$this->getUser = $user->getId ();
		
		$application->setMemeberId ( $this->getUser);
		$application->setProductCode ( $_POST ['productCode'] );
		$application->setMemo ( $_POST ['memo'] );
		
		if ($applicationDao->insert ( $application ) == DefineCommon::$PRODUCT_APPLY_NG) {
			parent::redirect ( "/error.php" );
		}
		for($i=0;$i<$_POST['peopleNumber'];$i++){
			$applicationmember -> setApplicationIdx(2);
			$applicationmember -> setBirth($_POST['birth']);
			$applicationmember -> setMemo($_POST['memo']);
			$applicationmember -> setName($_POST['name']);
			
			if($applicationMemberDao->insert($applicationmember)==DefineCommon::$PRODUCT_APPLY_NG){
				parent::redirect("/error.php");
			}
		}
	}
	protected function error() {
	}
}
$obj = new customerPlanCompleteAction ();
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
<link href="../css/common.css" rel="stylesheet">
<link href="../css/customerPlanComplete.css" rel="stylesheet">
<title><?=$obj->getTitle()?></title>
<style></style>
</head>
<body>
	<header><?=$obj->getHeader()?>
	<div class="logout">
			<a href="/logout.php">Logout</a>
		</div>
	</header>
	<div class="main">
		<div class="inline">
			<table>
				<thead>
					<tr>
						<th colspan="2">ご予約ありがとうございました。</th>
					</tr>
				</thead>
				<tbody>
				</tbody>
			</table>
			<input type="button" value="商品リストへ戻る"
				onclick="location.href='/customerPlanList.php'">
		</div>
	</div>
	<footer><?=$obj->getFooter()?></footer>
</body>
</html>