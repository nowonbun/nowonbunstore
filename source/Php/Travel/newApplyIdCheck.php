<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/AbstractAction.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Dao/MemberDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/DefineCommon.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/DefineMessage.php';
class NewApplyIdCheck extends AbstractAction {
	private $pid;
	private $result;
	protected function initialize() {
		if (parent::isPostBack ()) {
			if ($_POST ["pid"] != null) {
				$this->pid = $_POST ["pid"];
				return true;
			}
		}
		return false;
	}
	protected function main() {
		$dao = new MemberDao();
		$rslt = $dao->checkId($this->pid);
		if($rslt == DefineCommon::$LOGIN_CHECK_OK){
			$json = array (
					"result" => "ok"
			);
			$this->result = json_encode ( $json );
			return;
		}
		$this->error();
	}
	protected function error() {
		$json = array (
				"result" => "ng" 
		);
		$this->result = json_encode ( $json );
	}
	public function getResult() {
		return $this->result;
	}
}
$obj = new NewApplyIdCheck ();
$obj->run ();
?>
<?=$obj->getResult()?>