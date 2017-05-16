<?php
session_cache_expire ( 60 );
session_start ();
abstract class SessionClass {
	protected function setSession($name, $obj) {
		$_SESSION [$name] = serialize ( $obj );
	}
	protected function getSession($name) {
		return unserialize ( $_SESSION [$name] );
	}
}
?>