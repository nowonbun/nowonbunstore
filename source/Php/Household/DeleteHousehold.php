<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Common/AbstractController.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Dao/HshldDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Entity/Hshld.php';
class DeleteHousehold extends AbstractController {
	private $hshldDao;
	private $idx;
	private $gid;
	protected function initialize() {
		$this->hshldDao = new HshldDao ();
	}
	protected function validate() {
		$this->idx = parent::getParam ( "IDX" );
		$this->gid = parent::getParam ( "GID" );
		return true;
	}
	protected function main() {
		$item = $this->hshldDao->find ( $this->idx, $this->gid );
		$this->hshldDao->delete ( $this->idx, $this->gid );
		return $item->toArray ();
	}
	protected function error($e) {
		parent::setHeaderError ( 406, "" );
	}
}
$obj = new DeleteHousehold ();
$obj->run ();
?>