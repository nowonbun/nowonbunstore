<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/AbstractDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/DefineCommon.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Entity/Administrator.php';
class AdministratorDao extends AbstractDao {
	public function login($pid, $pwd) {
		$stmt = null;
		try {
			$stmt = parent::getStmt ( "select ID,PASSWORD,NAME,CREATEDATE,CREATER,STATE from tbl_tran_administrator where ID=? AND PASSWORD = ? AND STATE = '0'" );
			$stmt->bind_param ( "ss", $pid, md5 ( $pwd ) );
			$stmt->execute ();
			$stmt->bind_result ( $rid, $rpwd, $name, $createdate, $creater, $state );
			
			if ($stmt->fetch ()) {
				$mem = new Administrator ();
				$mem->setId ( $rid );
				$mem->setPassword ( $rpwd );
				$mem->setName ( $name );
				$mem->setCreatedate ( $createdate );
				$mem->setCreater ( $creater );
				$mem->setState ( $state );
				return $mem;
			} else {
				return DefineCommon::$LOGIN_ERROR;
			}
		} catch ( Exception $e ) {
			die ( $e );
		} finally {
			$stmt->close ();
			parent::close ();
		}
	}
}
?>