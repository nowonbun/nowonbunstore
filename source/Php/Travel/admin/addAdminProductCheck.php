<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/AbstractAction.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Dao/ProductDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/DefineCommon.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/DefineMessage.php';
class AddAdminProductCheck extends AbstractAction {
	private $code;
	private $result;
	protected function initialize() {
		if (parent::isPostBack ()) {
			if ($_POST ["code"] != null) {
				$this->code = $_POST ["code"];
				return true;
			}
		}
		return false;
	}
	protected function main() {
		$dao = new ProductDao();
		$rslt = $dao->checkCode($this->code);
		if($rslt == DefineCommon::$CODE_CHECK_OK){
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
$obj = new AddAdminProductCheck();
$obj->run ();
?>
<?=$obj->getResult()?>