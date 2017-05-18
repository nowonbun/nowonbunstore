<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Common/AbstractController.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Dao/UsrNfDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Entity/UsrNf.php';
class CheckUser extends AbstractController {
	private $usrNfDao;
	
	private $gid;
	
	protected function initialize() {
		$this->usrNfDao = new UsrNfDao ();
	}
	protected function validate() {
		$this->gid = parent::getParam("GID");
		return true;
	}
	protected function main() {
		$item = $this->usrNfDao->find($this->gid);
		if($item == NULL){
			return false;
		}
		return $item->toArray();
	}
	
	protected function error($e){
		parent::setHeaderError(406,"");
	}
}
$obj = new CheckUser ();
$obj->run ();
?>