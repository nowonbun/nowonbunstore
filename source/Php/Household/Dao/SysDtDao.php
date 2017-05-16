<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Common/AbstractDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Entity/SysDt.php';
class SysDtDao extends AbstractDao {
	public function findAll() {
		$stmt = null;
		try {
			$stmt = parent::getStmt ( "select KYCD,DT from sys_dt" );
			$stmt->execute ();
			$stmt->bind_result ( $rkycd, $rdt );
			
			$rslt = array ();
			while ( $stmt->fetch () ) {
				$item = new SysDt ();
				$item->setKycd ( $rkycd );
				$item->setDt ( $rdt );
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
}
?>