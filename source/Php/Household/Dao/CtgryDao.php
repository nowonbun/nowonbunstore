<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Common/AbstractDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Entiry/Ctgry.php';
class CtgryDao extends AbstractDao {
	public function findAll() {
		$stmt = null;
		try {
			$stmt = parent::getStmt ( " select CD,NM from ctgry " );
			$stmt->execute ();
			$stmt->bind_result ( $rcd, $rnm );
			
			$rslt = array ();
			while ( $stmt->fetch () ) {
				$item = new Ctgry ();
				$item->setCd ( $rcd );
				$item->setNm ( $rnm );
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