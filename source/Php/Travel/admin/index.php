<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/AbstractAction.php';
class IndexAdminAction extends AbstractAction {
	protected function initialize() {
		return true;
	}
	protected function main() {
		parent::redirect ( "/admin/adminLogin.php" );
	}
	protected function error() {
	}
}
$obj= new IndexAdminAction ();
$obj->run ();
?>