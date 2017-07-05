<?php
/**
 * 会員加入
 */
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/AbstractAction.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Entity/Member.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/DefineCommon.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/DefineMessage.php';
class NewApplyMember extends AbstractAction {
	private $errorMsg = null;
	private $pid;
	private $pwd;
	private $name;
	private $birth;
	protected function initialize() {
		$member = parent::getBufferSessionUnserialize();
		if($member != null){
			parent::setBufferSession(null);
			$this->pid = $member->getId();
			$this->name = $member->getName();
			$this->birth = $member->getBirth();
			return false;
		}
		if (parent::isPostBack ()) {
			if ($_POST ["pid"] == null) {
				$this->errorMsg = DefineMessage::$APPLY_ERROR1;
				return false;
			}
			$this->pid = $_POST ["pid"];
			if ($_POST ["idchecked"] == null || $_POST ["idchecked"] == "0") {
				$this->errorMsg = DefineMessage::$APPLY_ERROR2;
				return false;
			}
			if ($_POST ["pwd"] == null) {
				$this->errorMsg = DefineMessage::$APPLY_ERROR3;
				return false;
			}
			if ($_POST ["pwd"] != $_POST ["rpwd"]) {
				$this->errorMsg = DefineMessage::$APPLY_ERROR4;
				return false;
			}
			if ($_POST ["name"] == null) {
				$this->errorMsg = DefineMessage::$APPLY_ERROR5;
				return false;
			}
			$this->name = $_POST ["name"];
			if ($_POST ["birth"] == null) {
				$this->errorMsg = DefineMessage::$APPLY_ERROR6;
				return false;
			}
			$this->birth = $_POST ["birth"];
			$this->pwd = $_POST ["pwd"];
			return true;
		}
		return false;
	}
	protected function main() {
		$member = new Member ();
		$member->setId ( $this->pid );
		$member->setPassword ( $this->pwd );
		$member->setName ( $this->name );
		$member->setBirth ( $this->birth );
		parent::setBufferSessionSerialize( $member );
		parent::redirect ( "/applyConfirm.php" );
	}
	protected function error() {
	}
	public function getErrorMsg() {
		return $this->errorMsg;
	}
	public function getPid() {
		return $this->pid;
	}
	public function getName() {
		return $this->name;
	}
	public function getBirth() {
		return $this->birth;
	}
}
$obj = new NewApplyMember ();
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
<script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
<link rel="stylesheet"
	href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
<link href="css/common.css" rel="stylesheet">
<link href="css/newApply.css" rel="stylesheet">
<script type="text/javascript" src="js/newApply.js"></script>
<title><?=$obj->getTitle()?></title>
</head>
<body>
	<header><?=$obj->getHeader()?></header>
	<div class=main>
		<div class="apply">
			<form method="post">
				<table class="applyBox">
					<thead>
						<tr>
							<th colspan="2">会員加入</th>
						</tr>
					</thead>
					<tbody>
						<tr>
							<td id="Lpid" class="label">ID</td>
							<td><input type="text" id="pid" name="pid" required autofocus
								autocomplete="off" value="<?=$obj->getPid()?>" /> <input
								type="button" id="checkerID" value="IDチェック" /> <input
								type="hidden" id="idchecked" name="idchecked" value="0" /></td>
						</tr>
						<tr>
							<td id="Lpwd" class="label">PASSWORD</td>
							<td><input type="password" name="pwd" id="pwd" required autofocus
								autocomplete="off" /></td>
						</tr>
						<tr>
							<td id="Lrpwd" class="label">PASSWORD再入力</td>
							<td><input type="password" name="rpwd" id="rpwd" required
								autofocus autocomplete="off" /></td>
						</tr>
						<tr>
							<td id="Lname" class="label">お名前</td>
							<td><input type="text" name="name" id="name" required autofocus
								autocomplete="off" value="<?=$obj->getName()?>" /></td>
						</tr>
						<tr>
							<td id="Lbirth" class="label">誕生日</td>
							<td><input type="text" name="birth" id="birth" required autofocus
								autocomplete="off" readonly="readonly"
								value="<?=$obj->getBirth()?>" /></td>
						</tr>
						<tr>
							<td colspan="2" class="errorLine"><span id="errorMsg"
								class="errorMsg"><?=$obj->getErrorMsg()?></span></td>
						</tr>
					</tbody>
					<tfoot>
						<tr>
							<td colspan="2"><input type="submit" value="新規加入" /> <input
								type="button" id="cancel" value="キャンセル" /></td>
						</tr>
					</tfoot>
				</table>
			</form>
		</div>
	</div>
	<footer><?=$obj->getFooter()?></footer>
</body>
</html>