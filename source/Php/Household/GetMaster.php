<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Common/AbstractController.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Dao/CtgryDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Dao/SysDtDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Dao/TpDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Entity/Ctgry.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Entity/SysDt.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Entity/Tp.php';
class GetMaster extends AbstractController {
	private $ctgryDao;
	private $sysDtDao;
	private $tpDao;
	protected function initialize() {
		$this->ctgryDao = new CtgryDao ();
		$this->sysDtDao = new SysDtDao ();
		$this->tpDao = new TpDao ();
		if ("test" != parent::getParam ( "gid" )) {
			throw new Exception ();
		}
	}
	protected function main() {
		$rslt = array ();
		$rslt ["CATEGORY"] = array ();
		$rslt ["TP"] = array ();
		$rslt ["SYSTEMDATA"] = array ();
		foreach ( $this->ctgryDao->findAll () as $key => $value ) {
			array_push ( $rslt ["CATEGORY"], $value->toString () );
		}
		foreach ( $this->tpDao->findAll () as $key => $value ) {
			array_push ( $rslt ["TP"], $value->toString () );
		}
		foreach ( $this->sysDtDao->findAll () as $key => $value ) {
			array_push ( $rslt ["SYSTEMDATA"], $value->toString () );
		}
		return $rslt;
	}
	protected function error() {
		http_response_code ( 406 );
	}
}
$obj = new GetMaster ();
$obj->run ();
?>