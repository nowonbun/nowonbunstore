<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Common/AbstractDao.php';
class HshldLogDao extends AbstractDao {
	public function insert($item) {
		$stmt = null;
		try {
			$qy = " insert into hshld_log( NDX2, ID, CD, TP, DT, CNTXT, PRC, PDT)values ";
			$qy .= " ( ?, ?, ?, ?, ?, ?, ?, now() ) ";
			$stmt = parent::getStmt ( $qy );
			$stmt->bind_param ( "sssssss", $p1, $p2, $p3, $p4, $p5, $p6, $p7 );
			$p1 = $item->getNdx ();
			$p2 = $item->getId ();
			$p3 = $item->getCd ();
			$p4 = $item->getTp ();
			$p5 = $item->getDt ();
			$p6 = $item->getCntxt ();
			$p7 = $item->getPrc ();
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