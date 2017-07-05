<?php
session_cache_expire(60);
session_start();

abstract class SessionClass {
	protected function setUserSession($user) {
		$_SESSION ["USER"] = $user;
	}
	protected function getUserSession() {
		return $_SESSION ["USER"];
	}
	protected function setUserSessionSerialize($user) {
		$_SESSION ["USER"] = serialize($user);
	}
	protected function getUserSessionUnserialize() {
		return unserialize($_SESSION ["USER"]);
	}
	protected function setAdminUserSession($admin) {
		$_SESSION ["ADMINUSER"] = $admin;
	}
	protected function getAdminUserSession() {
		return $_SESSION ["ADMINUSER"];
	}
	protected function setAdminUserSessionSerialize($admin) {
		$_SESSION ["ADMINUSER"] = serialize($admin);
	}
	protected function getAdminUserSessionUnserialize() {
		return unserialize($_SESSION ["ADMINUSER"]);
	}
	protected function setBufferSession($obj){
		$_SESSION["BUFFER"] = $obj;
	}
	protected function getBufferSession(){
		return $_SESSION["BUFFER"];
	}
	protected function setBufferSessionSerialize($obj){
		$_SESSION["BUFFER"] = serialize($obj);
	}
	protected function getBufferSessionUnserialize(){
		return unserialize($_SESSION["BUFFER"]);
	}
	protected function setSession($name,$obj){
		$_SESSION[$name]= $obj;
	}
	protected function getSession($name){
		return $_SESSION[$name];
	}
	protected function setSessionSerialize($name,$obj){
		$_SESSION[$name] = serialize($obj);
	}
	protected function getSessionUnserialize($name){
		return unserialize($_SESSION[$name]);
	}
}
?>