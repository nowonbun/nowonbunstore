<?php
/**
 * 会員加入
 */
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/AbstractAction.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Entity/Member.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Dao/MemberDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/DefineCommon.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/DefineMessage.php';
class NewApplyConfirmMember extends AbstractAction {
	private $member;
	protected function initialize() {
		$this->member = parent::getBufferSessionUnserialize ();
		if ($this->member == null) {
			return false;
		}
		return true;
	}
	protected function main() {
		if (parent::isPostBack ()) {
			parent::setBufferSession(null);
			$dao = new MemberDao();
			if($dao->insertMember($this->member) == DefineCommon::$LOGIN_APPLY_OK){
				parent::redirect ( "/applyComplete.php" );
			}
			parent::redirect ( "/error.php" );
		}
	}
	protected function error() {
		parent::redirect ( "/error.php" );
	}
	public function getMember() {
		return $this->member;
	}
}
$obj = new NewApplyConfirmMember ();
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
<link href="css/applyConfirm.css" rel="stylesheet">
<script type="text/javascript" src="js/applyConfirm.js"></script>
<title><?=$obj->getTitle()?></title>
</head>
<body>
	<header><?=$obj->getHeader()?></header>
	<div class=main>
		<div class="apply">

			<table class="applyBox">
				<thead>
					<tr>
						<th colspan="2">会員加入</th>
					</tr>
				</thead>
				<tbody>
					<tr>
						<td id="Lpid" class="label">ID</td>
						<td><?=$obj->getMember()->getId()?></td>
					</tr>
					<tr>
						<td id="Lpwd" class="label">PASSWORD</td>
						<td>********************</td>
					</tr>
					<tr>
						<td id="Lname" class="label">お名前</td>
						<td><?=$obj->getMember()->getName()?></td>
					</tr>
					<tr>
						<td id="Lbirth" class="label">誕生日</td>
						<td><?=$obj->getMember()->getBirth()?></td>
					</tr>
					<tr>
						<td colspan="2" class="info">こちらの情報で新規加入を行います。</td>
					</tr>
				</tbody>
				<tfoot>
					<tr>
						<td colspan="2">
							<form method="post">
								<input type="submit" value="確認" /> <input type="button"
									id="cancel" value="キャンセル" />
							</form>
						</td>
					</tr>
				</tfoot>
			</table>
		</div>
	</div>
	<footer><?=$obj->getFooter()?></footer>
</body>
</html>