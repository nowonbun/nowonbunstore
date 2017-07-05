<?php
class DBConn {
	private $hostname = "";
	private $username = "admin";
	private $password = "";
	private $dbname = "travel";
	public function get() {
		return new mysqli ( $this->hostname, $this->username, $this->password, $this->dbname ); // or die(mysqli_error());
			                                                                                  // return new PDO("mysql:host=".$this->hostname.";dbname=".$this->dbname, $this->username, $this->password);
	}
}
?>