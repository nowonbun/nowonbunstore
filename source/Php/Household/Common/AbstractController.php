<?php
error_reporting ( E_ALL ^ E_NOTICE || E_WARNING );
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Common/DefineMessage.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Common/SessionClass.php';
abstract class AbstractController extends SessionClass {
	private $param;
	public function run() {
		try {
			if (! $this->isPostBack ()) {
				throw new Exception ();
			}
			if ($_POST ["p"] == null) {
				throw new Exception ();
			}
			$this->param = json_decode ( base64_decode ( substr ( $_POST ["p"], 2 ) ) );
			
			$this->initialize ();
			echo "AA" . base64_encode ( json_encode ( $this->main () ) );
		} catch ( Exception $e ) {
			$this->error ();
		}
	}
	protected function isPostBack() {
		return strtoupper ( $_SERVER ['REQUEST_METHOD'] ) == 'POST';
	}
	protected function getParam($key) {
		return $this->param->{$key};
	}
	protected function redirect($path) {
		header ( "Location: " . $path );
		die ();
	}
	protected abstract function initialize();
	protected abstract function main();
	protected abstract function error();
}
?>