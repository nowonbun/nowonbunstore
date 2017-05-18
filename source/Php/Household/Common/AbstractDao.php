<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Common/DBConn.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Log4j/Logger.php';
abstract class AbstractDao {
	// http://php.net/manual/kr/mysqli-stmt.bind-param.php
	// http://php.net/manual/en/mysqli.prepare.php
	// http://php.net/manual/en/pdostatement.execute.php
	private $db;
	private $mysqli;
	
	private $logger;
	
	public function AbstractDao(){
		Logger::configure ( $_SERVER ['DOCUMENT_ROOT'] . '/Household/Log4j/config.xml' );
		$this->logger = Logger::getLogger ( get_class ( $this ) );
	}
	protected function getStmt($qy) {
		$this->db = new DBConn ();
		$this->mysqli = $this->db->get ();
		$stmt = $this->mysqli->prepare ( $qy );
		return $stmt;
	}
	protected function close() {
		$this->mysqli->close ();
	}
	
	protected function setDebug($message) {
		$this->logger->debug ( $message );
	}
	protected function setInfoLog($message) {
		$this->logger->info ( $message );
	}
	protected function setErrorLog($message) {
		$this->logger->error ( $message );
	}
}
?>