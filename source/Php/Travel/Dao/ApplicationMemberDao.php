<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/AbstractDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/DefineCommon.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Entity/Application_Member.php';

class ApplicationMemberDao extends AbstractDao {
	public function insert($application_member) {
		$stmt = null;
		try {	
			$qy = "insert into tbl_tran_application_member(APPLICATION_IDX,NAME,BIRTH,MEMO,CREATEDATE,STATE) values";
			$qy .= "(?,?,?,?,now(),0)";
			$stmt = parent::getStmt ( $qy );
			$stmt->bind_param ("ssss",$application_member->getApplicationIdx (), $application_member->getName (), $application_member->getBirth (), $application_member->getMemo ());
			
			if ($stmt->execute ()) {
				return DefineCommon::$PRODUCT_APPLY_OK;
			} 
			return DefineCommon::$PRODUCT_APPLY_NG;
		} catch ( Excetion $e ) {
			die ( $e );
		} finally {
			$stmt->close ();
			parent::close ();
		}
	}
}
?>