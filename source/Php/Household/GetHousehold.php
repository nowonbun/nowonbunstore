<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Common/AbstractController.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Dao/HshldDao.php';
class GetHouseholdList extends AbstractController {
	private $hshldDao;
	
	protected function initialize() {
		$tihs->hshldDao = new HshldDao ();
	}
	protected function validate() {
	}
	protected function main() {
	}
	protected function error($e) {
		parent::setHeaderError ( 406, "" );
	}
}
$obj = new GetHouseholdList ();
$obj->run ();
?>