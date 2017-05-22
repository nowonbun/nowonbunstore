<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Common/AbstractDao.php';
class HshldLogDao extends AbstractDao {
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
}
?>