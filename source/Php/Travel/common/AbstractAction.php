<?php
error_reporting ( E_ALL ^ E_NOTICE || E_WARNING );
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/DefineMessage.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/SessionClass.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Entity/Administrator.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Entity/Member.php';
abstract class AbstractAction extends SessionClass {
	public function run() {
		if ($this->initialize ()) {
			$this->main ();
		} else {
			$this->error ();
		}
	}
	protected function isPostBack() {
		return strtoupper ( $_SERVER ['REQUEST_METHOD'] ) == 'POST';
	}
	protected function redirect($path) {
		header ( "Location: " . $path );
		die ();
	}
	public function getTitle() {
		return DefineMessage::$TITLE;
	}
	public function getHeader() {
		return DefineMessage::$HEADER;
	}
	public function getFooter() {
		return DefineMessage::$FOOTER;
	}
	public function checkAuthAdminUserToRedirect(){
		$temp = parent::getAdminUserSessionUnserialize();
		if($temp== null){
			$this->redirect("/error.php");
		}
		parent::setAdminUserSessionSerialize($temp);
	}
	public function checkAuthUserToRedirect(){
		$temp = parent::getUserSessionUnserialize();
		if($temp== null){
			$this->redirect("/error.php");
		}
		parent::setUserSessionSerialize($temp);
	}
	protected abstract function initialize();
	protected abstract function main();
	protected abstract function error();
}
?>