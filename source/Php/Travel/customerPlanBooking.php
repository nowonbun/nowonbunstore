<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/AbstractAction.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Dao/ProductDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Dao/LocationDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Dao/CountryDao.php';
class customerPlanBookingAction extends AbstractAction {
	private $productlist;
	private $countrylist;
	private $countrymap;
	private $locationlist;
	private $locationmap;
	protected function initialize() {
		parent::checkAuthUserToRedirect ();
		return true;
	}
	protected function main() {
		$user = parent::getUserSessionUnserialize ();
		
		$countrydao = new CountryDao ();
		$this->countrylist = $countrydao->select ();
		$this->countrymap = array ();
		for($i = 0; $i < count ( $this->countrylist ); $i ++) {
			$this->countrymap [$this->countrylist [$i]->getCountryCode ()] = $this->countrylist [$i];
		}
		$locationdao = new LocationDao ();
		$this->locationlist = $locationdao->select ();
		$this->locationmap = array ();
		for($i = 0; $i < count ( $this->locationlist ); $i ++) {
			$this->locationmap [$this->locationlist [$i]->getLocationCode ()] = $this->locationlist [$i];
		}
		$productdao = new ProductDao ();
		$this->productlist = $productdao->productCdSelect ( addslashes ( $_GET ['productCode'] ) );
	}
	protected function error() {
	}
	public function getCountryList() {
		return $this->countrylist;
	}
	public function getLocationList() {
		return $this->locationlist;
	}
	public function getProductList() {
		return $this->productlist;
	}
	public function getCountry($code) {
		return $this->countrymap [$code];
	}
	public function getLocation($code) {
		return $this->locationmap [$code];
	}
	public function getCountryNLocation($code) {
		$location = $this->getLocation ( $code );
		$country = $this->getCountry ( $location->getCountryCode () );
		return $country->getCountryName () . " / " . $location->getLocationName ();
	}
}
$obj = new customerPlanBookingAction ();
$obj->run ();
?>
<!DOCTYPE html>
<html>
<head>
<meta content='text/html; charset=UTF-8' http-equiv='Content-Type' />
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<meta name="viewport"
	content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
<script
	src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
<link href="../css/common.css" rel="stylesheet">
<link href="../css/customerPlanBooking.css?ver=2" rel="stylesheet">
<title><?=$obj->getTitle()?></title>
<style>
</style>
</head>
<body>
	<header><?=$obj->getHeader()?>
		<div class="logout">
			<a href="/logout.php">Logout</a>
		</div>
	</header>
	<div class="main">
		<table>
			<thead>
				<tr>
					<th class="mainHead"><p class="header">予約</p></th>
				</tr>
			</thead>
			<tbody>
				<tr>
					<td>
						<p>[ <?=$_GET['productName'] ?> ]商品はこちらになります。</p>
					</td>
				</tr>
			</tbody>
		</table>
		<div class="start">
			<table>
				<thead>
					<tr>
						<th colspan="2">出発</th>
					</tr>
					<tr>
						<th width="220px">国家名/空港</th>
						<th width="220px">出発時間</th>
					</tr>
				</thead>
				<tbody>
					<tr>
						<td><?=$obj->getCountryNLocation($obj->getProductList()->getStartLocation())?></td>
						<td><?=$obj->getProductList()->getStartDate()?></td>
					</tr>
				</tbody>
				<tfoot></tfoot>
			</table>
		</div>
		<div class="arrive">
			<table>
				<thead>
					<tr>
						<th colspan="2">到着</th>
					</tr>
					<tr>
						<th width="220px">国家名/空港</th>
						<th width="220px">到着時間</th>
					</tr>
				</thead>
				<tbody>
					<tr>
						<td><?=$obj->getCountryNLocation($obj->getProductList()->getArriveLocation())?></td>
						<td><?=$obj->getProductList()->getArriveDate()?></td>
					</tr>
				</tbody>
				<tfoot></tfoot>
			</table>

		</div>
		<div class="buttonArea">
			<input type="button" value="詳細情報登録"
				onclick="location.href='/customerPlanSelect.php/?productCode=<?=$_GET['productCode'] ?>&productName=<?=$_GET['productName'] ?>'">
			<input type="button" value="キャンセル"
				onclick="location.href='/customerPlanList.php'">
		</div>
	</div>
	<footer><?=$obj->getFooter()?> </footer>
</body>
</html>