<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Common/AbstractDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Entity/UsrNf.php';
class UsrNfDao extends AbstractDao {
	public function find($id) {
		$stmt = null;
		try {
			$qy = " select ID,NAME,CREATEDATE from usr_nf ";
			$qy .= " where ID = ? ";
			$stmt = parent::getStmt ( $qy );
			$stmt->bind_param ( "s", $id );
			$stmt->execute ();
			$stmt->bind_result ( $rid, $rname, $rcreatedate );
			
			if ($stmt->fetch ()) {
				$item = new UsrNf ();
				$item->setId ( $rid );
				$item->setName ( $rname );
				$item->setEmail ( $remail );
				$item->setCreatedate ( $rcreatedate );
				return $item;
			}
			return NULL;
		} catch ( Exception $e ) {
			throw $e;
		} finally {
			$stmt->close ();
			parent::close ();
		}
	}
	public function insert($item) {
		$stmt = null;
		try {
			$qy = "insert into usr_nf(ID,NAME,CREATEDATE)values";
			$qy .= "(?,?,now())";
			$stmt = parent::getStmt ( $qy );
			$stmt->bind_param ( "ss", $pid, $pname );
			$pid = $item->getId ();
			$pname = $item->getName ();
			if ($stmt->execute ()) {
				return true;
			}
			return false;
		} catch ( Exception $e ) {
			throw $e;
		} finally {
			$stmt->close ();
			parent::close ();
		}
	}
	public function update($item) {
		$stmt = null;
		try {
			$qy = "update usr_nf set NAME=? , CREATEDATE=now()";
			$qy .= "where id=?";
			$stmt = parent::getStmt ( $qy );
			$stmt->bind_param ( "ss", $pname, $pid );
			$pid = $item->getId ();
			$pname = $item->getName ();
			if ($stmt->execute ()) {
				return true;
			}
			return false;
		} catch ( Exception $e ) {
			throw $e;
		} finally {
			$stmt->close ();
			parent::close ();
		}
	}
}
?>