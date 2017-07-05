<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/AbstractAction.php';
class LogoutAction extends AbstractAction {
	protected function initialize() {
		return true;
	}
	protected function main() {
		parent::setUserSession ( null );
		parent::redirect ( "/index.php" );
	}
	protected function error() {
	}
}
$obj = new LogoutAction ();
$obj->run ();
?>