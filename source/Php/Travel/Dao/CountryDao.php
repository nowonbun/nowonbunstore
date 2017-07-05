<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/AbstractDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/DefineCommon.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Entity/Country.php';
class CountryDao extends AbstractDao {
	public function select(){
		$stmt = null;
		try{
			$stmt = parent::getStmt ( "select COUNTRY_CODE,COUNTRY_NAME,CREATEDATE,CREATER,STATE from tbl_mast_country where STATE='0'" );
			$stmt->execute ();
			$stmt->bind_result ( $country_code, $country_name, $createdate, $creater, $state );
			
			$rslt = array();
			while ($stmt->fetch ()) {
				$country = new Country();
				$country->setCountryCode($country_code);
				$country->setCountryName($country_name);
				$country->setCreatedate($createdate);
				$country->setCreater($creater);
				$country->setState($state);
				array_push($rslt,$country);
			} 
			return $rslt;
			
		}catch ( Exception $e ) {
			die ( $e );
		} finally {
			$stmt->close ();
			parent::close ();
		}
	}
}
?>