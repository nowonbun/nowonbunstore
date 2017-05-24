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
		$this->year = parent::getParam ( "YEAR" );
		$this->month = parent::getParam ( "MONTH" );
		$this->cd = parent::getParam ( "CD" );
		$this->tp = parent::getParam ( "TP" );
		return true;
	}
	protected function main() {
		return array (
				value => $this->hshldDao->sum ( $this->gid, $this->cd, $this->tp, $this->getNextMonth () ) 
		);
	}
	protected function error($e) {
		parent::setHeaderError ( 406, "" );
	}
	private function getNextMonth() {
		$yearBuffer = intval ( $this->year );
		$monthBuffer = intval ( $this->month ) + 1;
		if ($monthBuffer > 12) {
			$monthBuffer = 1;
			$yearBuffer ++;
		}
		$date = date_create ( $yearBuffer . "-" . $monthBuffer . "-01" );
		return date_format ( $date, "Y-m-d" );
	}
}
$obj = new SumHousehold ();
$obj->run ();
?>