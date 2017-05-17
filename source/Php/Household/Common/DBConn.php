<?php
class DBConn {
	private $hostname = "";
	private $username = "";
	private $password = "";
	private $dbname = "";
	public function get() {
		return new mysqli ( $this->hostname, $this->username, $this->password, $this->dbname );
	}
}
?>