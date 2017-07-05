<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/AbstractAction.php';
class LogoutAdminAction extends AbstractAction {
	protected function initialize() {
		return true;
	}
	protected function main() {
		parent::setAdminUserSession ( null );
		parent::redirect ( "/admin/" );
	}
	protected function error() {
	}
}
$obj = new LogoutAdminAction ();
$obj->run ();
?>