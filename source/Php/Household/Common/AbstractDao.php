<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Common/DBConn.php';
abstract class AbstractDao {
	// http://php.net/manual/kr/mysqli-stmt.bind-param.php
	// http://php.net/manual/en/mysqli.prepare.php
	// http://php.net/manual/en/pdostatement.execute.php
	private $db;
	private $mysqli;
	protected function getStmt($qy) {
		$this->db = new DBConn ();
		$this->mysqli = $this->db->get ();
		$stmt = $this->mysqli->prepare ( $qy );
		return $stmt;
	}
	protected function close() {
		$this->mysqli->close ();
	}
}
?>