<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Common/AbstractDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Entity/UsrNf.php';
class UsrNfDao extends AbstractDao {
	public function find($id) {
		$stmt = null;
		try {
			$qy = " select ID,NAME,EMAIL,CREATEDATE from usr_nf ";
			$qy .= " where ID = ? ";
			$stmt = parent::getStmt ( $qy );
			
			$stmt->bind_param ( "?", $id );
			$stmt->execute ();
			$stmt->bind_result ( $rid, $rname, $remail, $rcreatedate );
			
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
			echo $e;
			die ( $e );
		} finally {
			$stmt->close ();
			parent::close ();
		}
	}
}
?>