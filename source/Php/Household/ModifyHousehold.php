<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Common/AbstractController.php';
class SumHousehold extends AbstractController {
	protected function initialize(){
		
	}
	protected function main(){
		
	}
	protected function validate(){
		
	}
	protected function error($e){
		parent::setHeaderError(406,"");
	}
}
$obj = new SumHousehold();
$obj->run();
?>