<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Common/AbstractController.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Dao/HshldDao.php';
class SumHousehold extends AbstractController {
	private $hshldDao;
	private $gid;
	private $cd;
	private $tp;
	protected function initialize() {
		$this->hshldDao = new HshldDao ();
	}
	protected function validate() {
		$this->gid = parent::getParam ( "GID" );
		// $this->year = parent::getParam ( "YEAR" );
		// $this->month = parent::getParam ( "MONTH" );
		$this->cd = parent::getParam ( "CD" );
		$this->tp = parent::getParam ( "TP" );
		return true;
	}
	protected function main() {
		return array (
				value => $this->hshldDao->sum ( $this->gid, $this->cd, $this->tp ) 
		);
	}
	protected function error($e) {
		parent::setHeaderError ( 406, "" );
	}
}
$obj = new SumHousehold ();
$obj->run ();
?>