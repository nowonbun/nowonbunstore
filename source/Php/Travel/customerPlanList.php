<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/AbstractAction.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Dao/ProductDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Dao/LocationDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Dao/CountryDao.php';
class customerPlanListAction extends AbstractAction {
	private $productlist;
	private $countrylist;
	private $countrymap;
	private $locationlist;
	private $locationmap;
	private $member;
	protected function initialize() {
		parent::checkAuthUserToRedirect ();
		return true;
	}
	protected function main() {
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
		$this->productlist = $productdao->select ();
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

$obj = new customerPlanListAction ();
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
<link href="css/common.css" rel="stylesheet">
<link href="css/customerPlanList.css?ver=2" rel="stylesheet" type="text/css">
<script type="text/javascript" src="js/customerPlanList.js"></script>
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
		<div class="list">
			<table>
				<thead>
					<tr class="trHeader">
						<th colspan="9"><p class="header">旅行商品リスト</p></th>
					</tr>
					<tr class="mainHeader">
						<td>商品コード</td>
						<td>商品名</td>
						<td>出発地</td>
						<td>到着地</td>
						<td>出発時間</td>
						<td>到着時間</td>
						<td colspan="2">価格</td>
						<td>予約</td>
					</tr>
				</thead>
				<tbody>
					<?php for($i=0;$i<count($obj->getProductList());$i++){?>
					<tr class="mainList">
						<td><?=$obj->getProductList()[$i]->getProductCode()?></td>
						<td><?=$obj->getProductList()[$i]->getPlanname()?></td>
						<td><?=$obj->getCountryNLocation($obj->getProductList()[$i]->getStartLocation())?></td>
						<td><?=$obj->getCountryNLocation($obj->getProductList()[$i]->getArriveLocation())?></td>
						<td><?=$obj->getProductList()[$i]->getStartDate()?></td>
						<td><?=$obj->getProductList()[$i]->getArriveDate()?></td>
						<td class="jpt">JPT ￥</td>
						<td class="price"><?=number_format( $obj->getProductList()[$i]->getPrice(),0)?></td>
						<td><input type="button" id="selectBtn" value="予約"
							onclick="location.href='/customerPlanBooking.php/?productCode=<?=$obj->getProductList()[$i]->getProductCode()?>&productName=<?=$obj->getProductList()[$i]->getPlanname()?>'"></td>
					</tr>
					<?php }?>
					<?php if(count($obj->getProductList()) < 1){?>
					<tr>
						<td colspan="8">検索結果はありません。</td>
					</tr>
					<?php }?>
					</tbody>
				<tfoot></tfoot>
			</table>
		</div>
	</div>
	<footer><?=$obj->getFooter()?> </footer>
</body>
</html>