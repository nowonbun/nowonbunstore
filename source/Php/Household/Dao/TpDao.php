<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Common/AbstractDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Entity/Tp.php';
class TpDao extends AbstractDao {
	public function findAll() {
		$stmt = null;
		try {
			$stmt = parent::getStmt ( " select TP,NM,CD from tp " );
			$stmt->execute ();
			$stmt->bind_result ( $rtp, $rnm, $rcd );
			
			$rslt = array ();
			while ( $stmt->fetch () ) {
				$item = new Tp ();
				$item->setTp ( $rtp );
				$item->setNm ( $rnm );
				$item->setCd ( $rcd );
				array_push ( $rslt, $item );
			}
			return $rslt;
		} catch ( Exception $e ) {
			echo $e;
			http_response_code ( 407 );
			die ( $e );
		} finally {
			$stmt->close ();
			parent::close ();
		}
	}
}
?>