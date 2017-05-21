<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Common/AbstractDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Entity/Hshld.php';
class HshldDao extends AbstractDao {
	public function findList($id, $start, $end) {
		$stmt = null;
		try {
			$qy = " select NDX, ID, CD, TP, DT, CNTXT, PRC, PDT from hshld ";
			$qy .= " where (id = ? or id in (select id from hshld_relation where rid = ?)) ";
			$qy .= " and dt >= ? and dt < ? ";
			$stmt = parent::getStmt ( $qy );
			parent::setDebug ( $start );
			parent::setDebug ( $end );
			$stmt->bind_param ( "ssss", $id, $id, $start, $end );
			$stmt->execute ();
			$stmt->bind_result ( $r1, $r2, $r3, $r4, $r5, $r6, $r7, $r8 );
			
			$rslt = array ();
			while ( $stmt->fetch () ) {
				$item = new Hshld ();
				$item->setNdx ( $r1 );
				$item->setId ( $r2 );
				$item->setCd ( $r3 );
				$item->setTp ( $r4 );
				$item->setDt ( $r5 );
				$item->setCntxt ( $r6 );
				$item->setPrc ( $r7 );
				$item->setPdt ( $r8 );
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
	public function findListByCtgry($id, $start, $end, $ctgry) {
		$stmt = null;
		try {
			$qy = " select NDX, ID, CD, TP, DT, CNTXT, PRC, PDT from hshld ";
			$qy .= " where (id = ? or id in (select id from hshld_relation where rid = ?)) ";
			$qy .= " and dt >= ? and dt < ? ";
			$qy .= " and cd = ? ";
			$stmt = parent::getStmt ( $qy );
			
			$stmt->bind_param ( "sssss", $id, $id, $start, $end, $ctgry );
			$stmt->execute ();
			$stmt->bind_result ( $r1, $r2, $r3, $r4, $r5, $r6, $r7, $r8 );
			
			$rslt = array ();
			while ( $stmt->fetch () ) {
				$item = new Hshld ();
				$item->setNdx ( $r1 );
				$item->setId ( $r2 );
				$item->setCd ( $r3 );
				$item->setTp ( $r4 );
				$item->setDt ( $r5 );
				$item->setCntxt ( $r6 );
				$item->setPrc ( $r7 );
				$item->setPdt ( $r8 );
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
	public function find($idx, $id) {
		$stmt = null;
		try {
			$qy = " select NDX, ID, CD, TP, DT, CNTXT, PRC, PDT from hshld ";
			$qy .= " where ndx = ? ";
			$qy .= " and  (id = ? or id in (select id from hshld_relation where rid = ?)) ";
			$stmt = parent::getStmt ( $qy );
			
			$stmt->bind_param ( "sss", $idx, $id, $id );
			$stmt->execute ();
			$stmt->bind_result ( $r1, $r2, $r3, $r4, $r5, $r6, $r7, $r8 );
			
			$stmt->fetch ();
			$item = new Hshld ();
			$item->setNdx ( $r1 );
			$item->setId ( $r2 );
			$item->setCd ( $r3 );
			$item->setTp ( $r4 );
			$item->setDt ( $r5 );
			$item->setCntxt ( $r6 );
			$item->setPrc ( $r7 );
			$item->setPdt ( $r8 );
			return $item;
		} catch ( Exception $e ) {
			echo $e;
			die ( $e );
		} finally {
			$stmt->close ();
			parent::close ();
		}
	}
	public function sum($id, $ctgry, $tp) {
		$stmt = null;
		try {
			$qy = " select IFNULL( sum(PRC), 0 ) from hshld ";
			$qy .= " where (id = ? or id in (select id from hshld_relation where rid = ?)) ";
			$qy .= " and cd = ? ";
			$qy .= " and tp = ? ";
			$stmt = parent::getStmt ( $qy );
			parent::setDebug ( $id );
			parent::setDebug ( $ctgry );
			parent::setDebug ( $tp );
			$stmt->bind_param ( "ssss", $id, $id, $ctgry, $tp );
			$stmt->execute ();
			$stmt->bind_result ( $rsum );
			
			$stmt->fetch ();
			return $rsum;
		} catch ( Exception $e ) {
			echo $e;
			die ( $e );
		} finally {
			$stmt->close ();
			parent::close ();
		}
	}
	public function insert($item) {
		$stmt = null;
		try {
			$qy = " insert into hshld( ID, CD, TP, DT, CNTXT, PRC, PDT)values ";
			$qy .= " ( ?, ?, ?, ?, ?, ?, now() ) ";
			$stmt = parent::getStmt ( $qy );
			$stmt->bind_param ( "ssssss", $p1, $p2, $p3, $p4, $p5, $p6 );
			$p1 = $item->getId ();
			$p2 = $item->getCd ();
			$p3 = $item->getTp ();
			$p4 = $item->getDt ();
			$p5 = $item->getCntxt ();
			$p6 = $item->getPrc ();
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
			$qy = " update hshld set id = ?, cd = ?, tp = ?, dt = ?, cntxt = ?, prc = ? , pdt=now() ";
			$qy .= " where ndx=?  ";
			$stmt = parent::getStmt ( $qy );
			$stmt->bind_param ( "sssssss", $p1, $p2, $p3, $p4, $p5, $p6, $p7 );
			
			$p1 = $item->getId ();
			$p2 = $item->getCd ();
			$p3 = $item->getTp ();
			$p4 = $item->getDt ();
			$p5 = $item->getCntxt ();
			$p6 = $item->getPrc ();
			$p7 = $item->getNdx ();
			
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
	public function delete($idx, $id) {
		$stmt = null;
		try {
			$qy = " delete from hshld ";
			$qy .= " where NDX = ? ";
			$stmt = parent::getStmt ( $qy );
			$stmt->bind_param ( "s", $p1 );
			$p1 = $idx;
			
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