<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/AbstractDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/DefineCommon.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Entity/Member.php';
class MemberDao extends AbstractDao {
	public function login($pid, $pwd) {
		$stmt = null;
		try {
			$stmt = parent::getStmt ("select ID,PASSWORD,NAME,BIRTH,CREATEDATE,CREATER,STATE from tbl_tran_member where ID=? AND PASSWORD = ? AND STATE = '0'");
			$stmt->bind_param ( "ss", $pid, md5 ( $pwd ) );
			
			$stmt->execute ();
			$stmt->bind_result ( $rid, $rpwd, $name, $birth, $createdate, $creater, $state );
			
			if ($stmt->fetch ()) {
				$mem = new Member ();
				$mem->setId ( $rid );
				$mem->setPassword ( $rpwd );
				$mem->setName ( $name );
				$mem->setBirth ( $birth );
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
	public function checkId($pid) {
		$stmt = null;
		try {
			$stmt = parent::getStmt ( "select count(1) from tbl_tran_member where ID=?" );
			$stmt->bind_param ( "s", $pid );
			
			$stmt->execute ();
			$stmt->bind_result ( $count );
			if ($stmt->fetch ()) {
				if ($count < 1) {
					return DefineCommon::$LOGIN_CHECK_OK;
				}
			}
			return DefineCommon::$LOGIN_CHECK_NG;
		} catch ( Exception $e ) {
			die ( $e );
		} finally {
			$stmt->close ();
			parent::close ();
		}
	}
	public function insertMember($member) {
		$stmt = null;
		try {
			$qy = "insert into tbl_tran_member(ID,PASSWORD,NAME,BIRTH,CREATEDATE,CREATER,STATE)values";
			$qy .= "(?,?,?,?,now(),?,'0')";
			$stmt = parent::getStmt ( $qy );
			$stmt->bind_param ( "sssss", $member->getId (), md5($member->getPassword ()), $member->getName (), $member->getBirth (), $member->getId () );
			if($stmt->execute ()){
				return DefineCommon::$LOGIN_APPLY_OK;
			}
			return DefineCommon::$LOGIN_APPLY_NG;
		} catch ( Exception $e ) {
			die ( $e );
		} finally {
			$stmt->close ();
			parent::close ();
		}
	}
}
?>