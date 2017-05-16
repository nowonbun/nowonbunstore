<?php
class DBConn {
	private $hostname = "192.168.0.2";
	private $username = "nowonbun";
	private $password = "ghkdtnsduq1";
	private $dbname = "household_dev";
	public function get() {
		return new mysqli ( $this->hostname, $this->username, $this->password, $this->dbname );
	}
}
?>