<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Common/AbstractDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Entity/HshldRelation.php';
class HshldRelationDao extends AbstractDao {
	public function findListById($id) {
		$stmt = null;
		try {
			$qy = " select NDX,ID,RID from hshld_relation ";
			$qy .= " where id=? ";
			
			$stmt = parent::getStmt ( $qy );
			
			$stmt->bind_param ( "s", $id );
			$stmt->execute ();
			$stmt->bind_result ( $rndx, $rid, $rrid );
			
			$rslt = array ();
			while ( $stmt->fetch () ) {
				$item = new HshldRelation ();
				$item->setNdx ( $rndx );
				$item->setId ( $rid );
				$item->setRid ( $rrid );
				array_push ( $rslt, $item );
			}
			return $rslt;
		} catch ( Exception $e ) {
			echo $e;
			die ( $e );
		} finally {
			$stmt->close ();
			parent::close ();
		}
	}
	public function findListByRid($id) {
		$stmt = null;
		try {
			$qy = " select NDX,ID,RID from hshld_relation ";
			$qy .= " where rid=? ";
			
			$stmt = parent::getStmt ( $qy );
			
			$stmt->bind_param ( "s", $id );
			$stmt->execute ();
			$stmt->bind_result ( $rndx, $rid, $rrid );
			
			$rslt = array ();
			while ( $stmt->fetch () ) {
				$item = new HshldRelation ();
				$item->setNdx ( $rndx );
				$item->setId ( $rid );
				$item->setRid ( $rrid );
				array_push ( $rslt, $item );
			}
			return $rslt;
		} catch ( Exception $e ) {
			echo $e;
			die ( $e );
		} finally {
			$stmt->close ();
			parent::close ();
		}
	}
	public function find($id, $vid) {
		$stmt = null;
		try {
			$qy = " select NDX,ID,RID from hshld_relation ";
			$qy .= " where id=? and rid=?";
			
			$stmt = parent::getStmt ( $qy );
			
			$stmt->bind_param ( "ss", $id, $vid );
			$stmt->execute ();
			$stmt->bind_result ( $rndx, $rid, $rrid );
			
			$stmt->fetch ();
			$item = new HshldRelation ();
			$item->setNdx ( $rndx );
			$item->setId ( $rid );
			$item->setRid ( $rrid );
			return $item;
		} catch ( Exception $e ) {
			echo $e;
			die ( $e );
		} finally {
			$stmt->close ();
			parent::close ();
		}
	}
}
?>