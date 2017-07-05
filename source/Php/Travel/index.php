<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/AbstractAction.php';
class IndexAction extends AbstractAction {
	protected function initialize() {
		return true;
	}
	protected function main() {
		parent::redirect ( "/login.php" );
	}
	protected function error() {
	}
}
$obj= new IndexAction ();
$obj->run ();
?>
