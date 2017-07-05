<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/AbstractDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/DefineCommon.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Entity/Location.php';
class LocationDao extends AbstractDao {
	public function select() {
		$stmt = null;
		try {
			$stmt = parent::getStmt ( "select LOCATION_CODE,LOCATION_NAME,COUNTRY_CODE,CREATEDATE,CREATER,STATE from tbl_mast_location where STATE='0'" );
			$stmt->execute ();
			$stmt->bind_result ( $location_code, $location_name, $country_code, $createdate, $creater, $state );
			
			$rslt = array ();
			while ( $stmt->fetch () ) {
				$location = new Location ();
				$location->setLocationCode ( $location_code );
				$location->setLocationName ( $location_name );
				$location->setCountryCode ( $country_code );
				$location->setCreatedate ( $createdate );
				$location->setCreater ( $creater );
				$location->setState ( $state );
				array_push ( $rslt, $location );
			}
			return $rslt;
		} catch ( Exception $e ) {
			die ( $e );
		} finally {
			$stmt->close ();
			parent::close ();
		}
	}
}
?>