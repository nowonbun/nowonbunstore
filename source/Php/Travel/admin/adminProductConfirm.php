<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/AbstractAction.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/DefineMessage.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Dao/CountryDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Dao/LocationDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Dao/ProductDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Entity/Country.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Entity/Location.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Entity/Product.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Entity/Administrator.php';

class AdminProductConfirmAction extends AbstractAction {
	private $errorMsg;
	private $locationmap;
	private $locationListmap;
	private $locationKeyList;
	private $countrylist;
	private $countrymap;
	private $product;
	protected function initialize() {
		parent::checkAuthAdminUserToRedirect ();
		$this->load ();
		$this->product = parent::getBufferSessionUnserialize ();
		if ($this->product == null) {
			return false;
		}
		return true;
	}
	protected function main() {
		if (parent::isPostBack ()) {
			//parent::setBufferSession ( null );
			$admin = parent::getAdminUserSessionUnserialize ();
			$dao = new ProductDao ();
			if ($dao->insertProduct ( $this->product, $admin->getId () ) == DefineCommon::$PRODUCT_APPLY_OK) {
				parent::redirect ( "/admin/adminProductAddComplete.php" );
			}
			// parent::redirect ( "/error.php" );
		}
	}
	protected function error() {
		parent::redirect ( "/error.php" );
	}
	private function load() {
		$countrydao = new CountryDao ();
		$this->countrylist = $countrydao->select ();
		$this->countrymap = array ();
		for($i = 0; $i < count ( $this->countrylist ); $i ++) {
			$this->countrymap [$this->countrylist [$i]->getCountryCode ()] = $this->countrylist [$i];
		}
		$locationdao = new LocationDao ();
		$locationlist = $locationdao->select ();
		$this->locationmap = array ();
		$this->locationListmap = array ();
		$this->locationKeyList = array ();
		for($i = 0; $i < count ( $locationlist ); $i ++) {
			$location = $locationlist [$i];
			$countrycode = $location->getCountryCode ();
			if ($this->locationListmap [$countrycode] == null) {
				$this->locationListmap [$countrycode] = array ();
				array_push ( $this->locationKeyList, $countrycode );
			}
			array_push ( $this->locationListmap [$countrycode], $location );
			$this->locationmap [$location->getLocationCode ()] = $location;
		}
	}
	public function getProduct() {
		return $this->product;
	}
	public function getCountryList() {
		return $this->countrylist;
	}
	public function getCountry($code) {
		$location = $this->locationmap [$code];
		return $this->countrymap [$location->getCountryCode ()];
	}
	public function getLocationMap($code) {
		return $this->locationmap [$code];
	}
	public function getLocationListMap($code) {
		return $this->locationListmap [$code];
	}
	public function getLocationKeyList() {
		return $this->locationKeyList;
	}
	public function getErrorMsg() {
		return $this->errorMsg;
	}
}
$obj = new AdminProductConfirmAction ();
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
<link href="../css/adminProductConfirm.css" rel="stylesheet">
<script type="text/javascript" src="../js/adminProductConfirm.js"></script>
<title><?=$obj->getTitle()?></title>
</head>
<body>
	<header><?=$obj->getHeader()?>
	<div class="logout">
			<a href="/admin/adminLogout.php">Logout</a>
		</div>
	</header>
	<div class="main">
		<div class="productAdd">
			<form method="post">
				<table class="productAddBox">
					<thead>
						<tr>
							<th colspan="2">商品登録</th>
						</tr>
					</thead>
					<tbody>
						<tr>
							<td id="Lcode" class="label">商品コード</td>
							<td><?=$obj->getProduct()->getProductCode()?></td>
						</tr>
						<tr>
							<td id="Lplanname" class="label">プラン名</td>
							<td><?=$obj->getProduct()->getPlanname()?></td>
						</tr>
						<tr>
							<td id="LstartCountry" class="label">出発国家</td>
							<td><?=$obj->getCountry($obj->getProduct()->getStartLocation())->getCountryName()?></td>
						</tr>
						<tr>
							<td id="LstartLocation" class="label">出発地域</td>
							<td><?=$obj->getLocationMap($obj->getProduct()->getStartLocation())->getLocationName()?></td>
						</tr>
						<tr>
							<td id="LarriveCountry" class="label">到着国家</td>
							<td><?=$obj->getCountry($obj->getProduct()->getArriveLocation())->getCountryName()?></td>
						</tr>
						<tr>
							<td id="LarriveLocation" class="label">到着地域</td>
							<td><?=$obj->getLocationMap($obj->getProduct()->getArriveLocation())->getLocationName()?></td>
						</tr>
						<tr>
							<td id="Lstartdate" class="label">出発時間</td>
							<td><?=$obj->getProduct()->getStartDate()?></td>
						</tr>
						<tr>
							<td id="Larrivedate" class="label">到着時間</td>
							<td><?=$obj->getProduct()->getArriveDate()?></td>
						</tr>
						<tr>
							<td id="Lprice" class="label">価格</td>
							<td><?=$obj->getProduct()->getPrice()?></td>
						</tr>
					</tbody>
					<tfoot>
						<tr>
							<td colspan="2"><input type="submit" value="商品登録" /> <input
								type="button" id="cancel" value="キャンセル" /></td>
						</tr>
					</tfoot>
				</table>
			</form>
		</div>
	</div>
	<footer><?=$obj->getFooter()?></footer>
</body>
</html>