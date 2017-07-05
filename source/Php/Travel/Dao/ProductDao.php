<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/AbstractDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/DefineCommon.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Entity/Product.php';
class ProductDao extends AbstractDao {
	public function select() {
		$stmt = null;
		try {
			$stmt = parent::getStmt ( "select PRODUCT_CODE,PLANNAME,START_LOCATION,ARRIVE_LOCATION,START_DATE,ARRIVE_DATE,PRICE,CREATEDATE,CREATER,STATE from tbl_tran_product where STATE='0'" );
			$stmt->execute ();
			$stmt->bind_result ( $product_code, $planname, $start_location, $arrive_location, $start_date, $arrive_date, $price, $createdate, $creater, $state );
			
			$rslt = array ();
			while ( $stmt->fetch () ) {
				$product = new Product ();
				$product->setProductCode ( $product_code );
				$product->setPlanname ( $planname );
				$product->setStartLocation ( $start_location );
				$product->setArriveLocation ( $arrive_location );
				$product->setStartDate ( $start_date );
				$product->setArriveDate ( $arrive_date );
				$product->setPrice ( $price );
				$product->setCreatedate ( $createdate );
				$product->setCreater ( $creater );
				$product->setState ( $state );
				array_push ( $rslt, $product );
			}
			return $rslt;
		} catch ( Exception $e ) {
			die ( $e );
		} finally {
			$stmt->close ();
			parent::close ();
		}
	}
	public function checkCode($code) {
		$stmt = null;
		try {
			$stmt = parent::getStmt ( "select count(1) from tbl_tran_product where PRODUCT_CODE=?" );
			$stmt->bind_param ( "s", $code );
			
			$stmt->execute ();
			$stmt->bind_result ( $count );
			if ($stmt->fetch ()) {
				if ($count < 1) {
					return DefineCommon::$CODE_CHECK_OK;
				}
			}
			return DefineCommon::$CODE_CHECK_NG;
		} catch ( Exception $e ) {
			die ( $e );
		} finally {
			$stmt->close ();
			parent::close ();
		}
	}
	public function insertProduct($product, $creater) {
		$stmt = null;
		try {
			$qy = "insert into tbl_tran_product(PRODUCT_CODE,PLANNAME,START_LOCATION,ARRIVE_LOCATION,START_DATE,ARRIVE_DATE,PRICE,CREATEDATE,CREATER,STATE)values";
			$qy .= "(?,?,?,?,?,?,?,now(),?,'0')";
			$stmt = parent::getStmt ( $qy );
			$stmt->bind_param ( "ssssssss", $product->getProductCode (), $product->getPlanname (), $product->getStartLocation (), $product->getArriveLocation (), $product->getStartDate (), $product->getArriveDate (), $product->getPrice (), $creater );
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
	public function productCdSelect($productCd) {
		$stmt = null;
		
		try {
			$stmt = parent::getStmt ( "select PRODUCT_CODE,PLANNAME,START_LOCATION,ARRIVE_LOCATION,START_DATE,ARRIVE_DATE,PRICE,CREATEDATE,CREATER,STATE from tbl_tran_product where PRODUCT_CODE=? AND STATE='0'" );
			$stmt -> bind_param ( "s", $productCd );
			
			$stmt -> execute ();
			$stmt -> bind_result ( $product_code, $planname, $start_location, $arrive_location, $start_date, $arrive_date, $price, $createdate, $creater, $state );
			if ($stmt->fetch ()) {
				$product = new Product ();
				$product->setProductCode ( $product_code );
				$product->setPlanname ( $planname );
				$product->setStartLocation ( $start_location );
				$product->setArriveLocation ( $arrive_location );
				$product->setStartDate ( $start_date );
				$product->setArriveDate ( $arrive_date );
				$product->setPrice ( $price );
				$product->setCreatedate ( $createdate );
				$product->setCreater ( $creater );
				$product->setState ( $state );
				return $product;
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