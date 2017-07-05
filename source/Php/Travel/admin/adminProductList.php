<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/AbstractAction.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Dao/CountryDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Dao/LocationDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Dao/ProductDao.php';
class AdminProductListAction extends AbstractAction {
	private $countrylist;
	private $countrymap;
	private $locationlist;
	private $locationmap;
	private $productlist;
	protected function initialize() {
		parent::checkAuthAdminUserToRedirect ();
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
$obj = new AdminProductListAction ();
$obj->run ();
?>
<!DOCTYPE html>
<html>
<meta content='text/html; charset=UTF-8' http-equiv='Content-Type' />
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<meta name="viewport"
	content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
<script
	src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
<link href="../css/common.css" rel="stylesheet">
<link href="../css/adminProductList.css" rel="stylesheet">
<script type="text/javascript" src="../js/adminProductList.js"></script>
<title><?=$obj->getTitle()?></title>
</head>
<body>
	<header><?=$obj->getHeader()?>
	<div class="logout">
			<a href="/admin/adminLogout.php">Logout</a>
		</div>
	</header>
	<div class=main>
		<div>
			<div class="country">
				<table>
					<thead>
						<tr>
							<th colspan="2">国家マスタ</th>
						</tr>
						<tr>
							<th width="220px">国家コード</th>
							<th width="280px">国家名</th>
						</tr>
					</thead>
					<tbody>
					<?php for($i=0;$i<count($obj->getCountryList());$i++){?>
						<tr>
							<td><?=$obj->getCountryList()[$i]->getCountryCode()?></td>
							<td><?=$obj->getCountryList()[$i]->getCountryName()?></td>
						</tr>
					<?php }?>
					<?php if(count($obj->getCountryList()) < 1){?>
						<tr>
							<td colspan="2">検索結果はありません。</td>
						</tr>
					<?php }?>
				</tbody>
				</table>
			</div>
		</div>
		<div>
			<div class="location">
				<table>
					<thead>
						<tr>
							<th colspan="3">地域マスタ</th>
						</tr>
						<tr>
							<th width="140px">地域コード</th>
							<th width="180px">地域名</th>
							<th width="180px">国家</th>
						</tr>
					</thead>
					<tbody>
					<?php for($i=0;$i<count($obj->getLocationList());$i++){?>
						<tr>
							<td><?=$obj->getLocationList()[$i]->getLocationCode()?></td>
							<td><?=$obj->getLocationList()[$i]->getLocationName()?></td>
							<td><?=$obj->getCountry($obj->getLocationList()[$i]->getCountryCode())->getCountryName()?></td>
						</tr>
					<?php }?>
					<?php if(count($obj->getLocationList()) < 1){?>
						<tr>
							<td colspan="3">検索結果はありません。</td>
						</tr>
					<?php }?>
					</tbody>
				</table>
			</div>
		</div>
		<div>
			<div class="product">
				<table>
					<thead>
						<tr>
							<th colspan="7">商品リスト</th>
						</tr>
						<tr>
							<th width="100px">商品コード</th>
							<th width="250px">プラン名</th>
							<th width="130px">出発(国 / 地域)</th>
							<th width="130px">到着(国 / 地域)</th>
							<th width="120px">出発時間</th>
							<th width="120px">到着時間</th>
							<th width="150px">価格</th>
						</tr>
					</thead>
					<tbody>
						<?php for($i=0;$i<count($obj->getProductList());$i++){?>
						<tr>
							<td><?=$obj->getProductList()[$i]->getProductCode()?></td>
							<td><?=$obj->getProductList()[$i]->getPlanname()?></td>
							<td><?=$obj->getCountryNLocation($obj->getProductList()[$i]->getStartLocation())?></td>
							<td><?=$obj->getCountryNLocation($obj->getProductList()[$i]->getArriveLocation())?></td>
							<td><?=$obj->getProductList()[$i]->getStartDate()?></td>
							<td><?=$obj->getProductList()[$i]->getArriveDate()?></td>
							<td>JPT ￥ <?=number_format( $obj->getProductList()[$i]->getPrice(),0)?></td>
						</tr>
						<?php }?>
					<?php if(count($obj->getProductList()) < 1){?>
						<tr>
							<td colspan="7">検索結果はありません。</td>
						</tr>
					<?php }?>
					</tbody>
				</table>
			</div>
		</div>
		<div>
			<input type="button" id="addProductBtn" value="商品追加">
		</div>
	</div>
	<footer><?=$obj->getFooter()?></footer>
</body>
</html>