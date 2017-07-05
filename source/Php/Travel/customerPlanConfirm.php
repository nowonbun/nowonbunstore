<?php
/**
 * 商品詳細画面
 */
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/AbstractAction.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Entity/Application.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Dao/ApplicationDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Dao/ProductDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Dao/CountryDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Dao/LocationDao.php';
class customerPlanConfirmtAction extends AbstractAction {
	private $application;
	private $product;
	private $getUser;
	private $countrymap;
	private $countrylist;
	private $locationmap;
	private $locationlist;
	protected function initialize() {
		parent::checkAuthUserToRedirect ();
		return true;
	}
	protected function main() {
		$user = parent::getUserSessionUnserialize ();
		$this->getUser = $user->getId ();
		
		$countryDao = new CountryDao ();
		$this->countrylist = $countryDao->select ();
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
		
		$productDao = new ProductDao ();
		$this->product = $productDao->productCdSelect ( $_POST ['productCode'] );
	}
	protected function error() {
	}
	public function getProduct() {
		return $this->product;
	}
	public function getId() {
		return $this->getUser;
	}
	public function getLocationList() {
		return $this->locationlist;
	}
	public function getCountryList() {
		return $this->countrylist;
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
		return $country->getCountryName () . "/" . $location->getLocationName ();
	}
}
$obj = new customerPlanConfirmtAction ();
$obj->run ();
?>

<!DOCTYPE HTML>
<html>
<head>
<meta content='text/html; charset=UTF-8' http-equiv='Content-Type' />
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<meta name="viewport"
	content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
<script
	src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
<link href="../css/common.css" rel="stylesheet">
<link href="../css/customerPlanConfirm.css?ver=4" rel="stylesheet">
<title><?=$obj->getTitle()?></title>
<style></style>
</head>
<body>
	<header><?=$obj->getHeader()?>
	<div class="logout">
			<a href="/logout.php">Logout</a>
		</div>
	</header>
	<form method="post" action="../customerPlanComplete.php">
		<div class="main">
			<div class="inline">
				<table class="mainTable">
					<thead>
						<tr>
							<th colspan="8"><p class="header">予約情報</p></th>
						</tr>
					</thead>
					<tbody>
						<tr class="mainHeader">
							<td>商品名</td>
							<td>ID</td>
							<td>出発</td>
							<td>出発時刻</td>
							<td>到着</td>
							<td>到着時刻</td>
							<td colspan="2">金額</td>
						</tr>
						<tr>
							<td><?=$obj->getProduct()->getPlanname() ?></td>
							<td><?=$obj->getId()?></td>
							<td><?=$obj->getCountryNLocation($obj->getProduct()->getStartLocation()) ?></td>
							<td><?=$obj->getProduct()->getStartDate() ?></td>
							<td><?=$obj->getCountryNLocation($obj->getProduct()->getArriveLocation()) ?></td>
							<td><?=$obj->getProduct()->getArriveDate() ?></td>
							<td class="jpt">JPT ￥</td>
							<td class="price"><?=number_Format($obj->getProduct()->getPrice())?></td>
						</tr>
					</tbody>
				</table>
				<input type="hidden" name="productCode"	value="<?=$_POST['productCode'] ?>">
				<input type="hidden" name="memo" value="<?=$_POST['memo'] ?>"> 
				<input type="hidden" name="peopleNumber" value="<?=$_POST['peopleNumber'] ?>"> 
				<input type="hidden" name="name" value="<?=$_POST['name'] ?>">
				<input type="hidden" name="birth" value="<?=$_POST['birth'] ?>"> 
				<input type="submit" value="予約"> 
				<input type="button" value="キャンセル" onclick="location.href='/customerPlanList.php'">
			</div>
		</div>
	</form>
	<footer><?=$obj->getFooter()?></footer>
</body>
</html>