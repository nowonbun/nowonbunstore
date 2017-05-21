<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Common/AbstractController.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Dao/HshldDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Entity/Hshld.php';
class ModifyHousehold extends AbstractController {
	private $hshldDao;
	private $idx;
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
		$this->idx = parent::getParam ( "IDX" );
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
		$item->setNdx ( $this->idx );
		$item->setId ( $this->gid );
		$item->setCd ( $this->cd );
		$item->setTp ( $this->tp );
		$item->setDt ( $this->dt );
		$item->setCntxt ( $this->cntxt );
		$item->setPrc ( $this->prc );
		$this->hshldDao->update ( $item );
		return $item->toArray ();
	}
	protected function error($e) {
		parent::setHeaderError ( 406, "" );
	}
}
$obj = new ModifyHousehold ();
$obj->run ();
?>