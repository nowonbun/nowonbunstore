<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Common/AbstractController.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Dao/HshldDao.php';
class GetHouseholdList extends AbstractController {
	private $hshldDao;
	private $gid;
	private $year;
	private $month;
	protected function initialize() {
		$this->hshldDao = new HshldDao ();
	}
	protected function validate() {
		$this->gid = parent::getParam ( "GID" );
		$this->year = parent::getParam ( "YEAR" );
		$this->month = parent::getParam ( "MONTH" );
		
		return true;
	}
	protected function main() {
		$rslt = array ();
		foreach ( $this->hshldDao->findList ( $this->gid, $this->getThisMonth (), $this->getNextMonth () ) as $value ) {
			array_push ( $rslt, $value->toArray () );
		}
		parent::setDebug("SELECT");
		return $rslt;
	}
	protected function error($e) {
		parent::setHeaderError ( 406, "" );
	}
	private function getThisMonth() {
		$date = date_create ( $this->year . "-" . $this->month . "-01" );
		return date_format ( $date, "Y-m-d" );
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
$obj = new GetHouseholdList ();
$obj->run ();
?>