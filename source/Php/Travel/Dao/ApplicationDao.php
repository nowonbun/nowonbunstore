<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/AbstractDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/DefineCommon.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Entity/Application.php';

class ApplicationDao extends AbstractDao{
	public function insert($Application){
		$stmt = null;
		try {
			$qy = "insert into tbl_tran_Application(MEMBER_ID,PRODUCT_CODE,MEMO,CREATEDATE,STATE)values";
			$qy .= "(?,?,?,now(),'0')";
			$stmt = parent::getStmt ( $qy );
			$stmt->bind_param ( "sss",$Application->getMemberId(),$Application->getProductCode(),$Application->getMemo());
			if ($stmt->execute ()) {
				return DefineCommon::$PRODUCT_APPLY_OK;
			}
			return DefineCommon::$PRODUCT_APPLY_NG;
		} catch ( Exception $e ) {
			die ( $e );
		} finally {
			$stmt->close ();
			parent::close ();
		}
	}
}
?>