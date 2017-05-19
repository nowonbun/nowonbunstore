<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Common/AbstractDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Entity/Hshld.php';
class HshldDao extends AbstractDao {
	public function findList($id, $start, $end) {
		$stmt = null;
		try {
			$qy = " select NDX,ID,CD,TP,DT,CNTXT,PRC,PDT from hshld ";
			$qy .= " where (id = ? or id in (select id from hshld_relation where rid = ?)) ";
			$qy .= " and dt >= ? and dt < ? ";
			$stmt = parent::getStmt ( $qy );
			parent::setDebug($start);
			parent::setDebug($end);
			$stmt->bind_param ( "ssss", $id, $id, $start, $end );
			$stmt->execute ();
			$stmt->bind_result ( $rndx, $rid, $rcd, $rtp, $rdt, $rcntxt, $rprc, $rpdt );
			
			$rslt = array ();
			while ( $stmt->fetch () ) {
				$item = new Hshld ();
				$item->setNdx ( $rndx );
				$item->setId ( $rid );
				$item->setCd ( $rcd );
				$item->setTp ( $rtp );
				$item->setDt ( $rdt );
				$item->setCntxt ( $rcntxt );
				$item->setPrc ( $rprc );
				$item->setPdt ( $rpdt );
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
			$qy = " select NDX,ID,CD,TP,DT,CNTXT,PRC,PDT from hshld ";
			$qy .= " where (id = ? or id in (select id from hshld_relation where rid = ?)) ";
			$qy .= " and dt >= ? and dt < ? ";
			$qy .= " and cd = ? ";
			$stmt = parent::getStmt ( $qy );
			
			$stmt->bind_param ( "sssss", $id, $id, $start, $end, $ctgry );
			$stmt->execute ();
			$stmt->bind_result ( $rndx, $rid, $rcd, $rtp, $rdt, $rcntxt, $rprc, $rpdt );
			
			$rslt = array ();
			while ( $stmt->fetch () ) {
				$item = new Hshld ();
				$item->setNdx ( $rndx );
				$item->setId ( $rid );
				$item->setCd ( $rcd );
				$item->setTp ( $rtp );
				$item->setDt ( $rdt );
				$item->setCntxt ( $rcntxt );
				$item->setPrc ( $rprc );
				$item->setPdt ( $rpdt );
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
			$qy = " select NDX,ID,CD,TP,DT,CNTXT,PRC,PDT from hshld ";
			$qy .= " where ndx = ? ";
			$qy .= " and  (id = ? or id in (select id from hshld_relation where rid = ?)) ";
			$stmt = parent::getStmt ( $qy );
			
			$stmt->bind_param ( "sss", $idx, $id, $id );
			$stmt->execute ();
			$stmt->bind_result ( $rndx, $rid, $rcd, $rtp, $rdt, $rcntxt, $rprc, $rpdt );
			
			$stmt->fetch ();
			$item = new Hshld ();
			$item->setNdx ( $rndx );
			$item->setId ( $rid );
			$item->setCd ( $rcd );
			$item->setTp ( $rtp );
			$item->setDt ( $rdt );
			$item->setCntxt ( $rcntxt );
			$item->setPrc ( $rprc );
			$item->setPdt ( $rpdt );
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
			$qy = " select IFNULL(sum(PRC),0) from hshld ";
			$qy .= " where (id = ? or id in (select id from hshld_relation where rid = ?)) ";
			$qy .= " and cd = ? ";
			$qy .= " and tp = ? ";
			$stmt = parent::getStmt ( $qy );
			parent::setDebug($id);
			parent::setDebug($ctgry);
			parent::setDebug($tp);
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
}
?>