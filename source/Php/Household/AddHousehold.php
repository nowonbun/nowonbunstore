<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Common/AbstractController.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Dao/HshldDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Entity/Hshld.php';
class AddHousehold extends AbstractController {
	private $hshldDao;
	private $gid;
	private $cd;
	private $tp;
	private $dt;
	private $cntxt;
	private $prc;
	protected function initialize() {
		$this->hshldDao = new HshldDao ();
	}
	protected function validate() {
		$this->gid = parent::getParam ( "GID" );
		$this->cd = parent::getParam ( "CD" );
		$this->tp = parent::getParam ( "TP" );
		$this->dt = parent::getParam ( "DT" );
		$this->cntxt = parent::getParam ( "CNTXT" );
		$this->prc = parent::getParam ( "PRC" );
		return true;
	}
	protected function main() {
		$item = new Hshld ();
		$item->setId ( $this->gid );
		$item->setCd ( $this->cd );
		$item->setTp ( $this->tp );
		$item->setDt ( $this->dt );
		$item->setCntxt ( $this->cntxt );
		$item->setPrc ( $this->prc );
		parent::setDebug("INSERT");
		$this->hshldDao->insert ( $item );
		return $item->toArray ();
	}
	protected function error($e) {
		parent::setHeaderError ( 406, "" );
	}
}
$obj = new AddHousehold ();
$obj->run ();
?>