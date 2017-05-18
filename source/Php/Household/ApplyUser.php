<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Common/AbstractController.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Dao/UsrNfDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Entity/UsrNf.php';
class ApplyUser extends AbstractController {
	private $usrNfDao;
	private $gid;
	private $name;
	protected function initialize() {
		$this->usrNfDao = new UsrNfDao ();
	}
	protected function validate() {
		$this->gid = parent::getParam ( "GID" );
		$this->name = parent::getParam ( "NAME" );
		return true;
	}
	protected function main() {
		$hasUser = false;
		$item = $this->usrNfDao->find ( $this->gid );
		if ($item != null) {
			$hasUser = true;
		} else {
			$item = new UsrNf ();
		}
		$item->setId ( $this->gid );
		$item->setName ( $this->name );
		if ($hasUser) {
			$this->usrNfDao->update ( $item );
		} else {
			$this->usrNfDao->insert ( $item );
		}
		return $item->toArray ();
	}
	protected function error($e) {
		parent::setHeaderError ( 406, "" );
	}
}
$obj = new ApplyUser ();
$obj->run ();
?>