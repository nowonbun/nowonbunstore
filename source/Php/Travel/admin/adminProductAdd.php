<?php
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/AbstractAction.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/common/DefineMessage.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Dao/CountryDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Dao/LocationDao.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Entity/Country.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Entity/Location.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Entity/Product.php';
class AdminProductAddAction extends AbstractAction {
	private $errorMsg;
	private $locationmap;
	private $locationListmap;
	private $locationKeyList;
	private $countrylist;
	private $countrymap;
	private $code;
	private $planname;
	private $startCountry;
	private $startLocation;
	private $arriveCountry;
	private $arriveLocation;
	private $startDate;
	private $arriveDate;
	private $price;
	protected function initialize() {
		parent::checkAuthAdminUserToRedirect ();
		$this->load ();
		if (parent::isPostBack ()) {
			if ($_POST ["code"] == null) {
				$this->errorMsg = DefineMessage::$PRODUCT_ERROR1;
				return false;
			}
			$this->code = $_POST ["code"];
			if ($_POST ["codeChecker"] != "1") {
				$this->errorMsg = DefineMessage::$PRODUCT_ERROR2;
				return false;
			}
			if ($_POST ["planname"] == null) {
				$this->errorMsg = DefineMessage::$PRODUCT_ERROR3;
				return false;
			}
			$this->planname = $_POST ["planname"];
			if ($_POST ["startCountry"] == null) {
				$this->errorMsg = DefineMessage::$PRODUCT_ERROR4;
				return false;
			}
			$this->startCountry = $_POST ["startCountry"];
			if ($_POST ["startLocation"] == null) {
				$this->errorMsg = DefineMessage::$PRODUCT_ERROR5;
				return false;
			}
			$this->startLocation = $_POST ["startLocation"];
			if ($_POST ["arriveCountry"] == null) {
				$this->errorMsg = DefineMessage::$PRODUCT_ERROR6;
				return false;
			}
			$this->arriveCountry = $_POST ["arriveCountry"];
			if ($_POST ["arriveLocation"] == null) {
				$this->errorMsg = DefineMessage::$PRODUCT_ERROR7;
				return false;
			}
			$this->arriveLocation = $_POST ["arriveLocation"];
			
			$countrytemp = $this->getCountry ( $this->startCountry );
			$locationtemp = $this->getLocationMap ( $this->startLocation );
			if ($countrytemp == null || $locationtemp == null || $countrytemp->getCountryCode () != $locationtemp->getCountryCode ()) {
				$this->errorMsg = DefineMessage::$PRODUCT_ERROR8;
				return false;
			}
			$countrytemp = $this->getCountry ( $this->arriveCountry );
			$locationtemp = $this->getLocationMap ( $this->arriveLocation );
			if ($countrytemp == null || $locationtemp == null || $countrytemp->getCountryCode () != $locationtemp->getCountryCode ()) {
				$this->errorMsg = DefineMessage::$PRODUCT_ERROR9;
				return false;
			}
			if ($_POST ["startdate"] == null) {
				$this->errorMsg = DefineMessage::$PRODUCT_ERROR10;
				return false;
			}
			$this->startDate = $this->transTime ( $_POST ["startdate"] );
			if ($this->startDate == false) {
				$this->errorMsg = DefineMessage::$PRODUCT_ERROR10;
				return false;
			}
			if ($_POST ["arrivedate"] == null) {
				$this->errorMsg = DefineMessage::$PRODUCT_ERROR11;
				return false;
			}
			$this->arriveDate = $this->transTime ( $_POST ["arrivedate"] );
			if ($this->arriveDate == false) {
				$this->errorMsg = DefineMessage::$PRODUCT_ERROR11;
				return false;
			}
			if ($this->transTimeTick ( $this->startDate ) >= $this->transTimeTick ( $this->arriveDate )) {
				$this->errorMsg = DefineMessage::$PRODUCT_ERROR12;
				return false;
			}
			if ($_POST ["price"] == null) {
				$this->errorMsg = DefineMessage::$PRODUCT_ERROR13;
				return false;
			}
			$this->price = str_replace ( ",", "", $_POST ["price"] );
			try {
				if (intval ( $this->price ) <= 0) {
					$this->errorMsg = DefineMessage::$PRODUCT_ERROR13;
					return false;
				}
			} catch ( Exception $e ) {
				return false;
			}
			return true;
		}
		return false;
	}
	protected function main() {
		$product = new Product ();
		$product->setProductCode ( $this->code );
		$product->setPlanname ( $this->planname );
		$product->setStartLocation ( $this->startLocation );
		$product->setArriveLocation ( $this->arriveLocation );
		$product->setStartDate ( $this->startDate );
		$product->setArriveDate ( $this->arriveDate );
		$product->setPrice ( $this->price );
		parent::setBufferSessionSerialize ( $product );
		parent::redirect ( "/admin/adminProductConfirm.php" );
	}
	protected function error() {
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
	private function transTime($val) {
		try {
			$pm = false;
			if (strpos ( $val, "pm" ) != false) {
				$pm = true;
			}
			$val = str_replace ( "am", "", $val );
			$val = str_replace ( "pm", "", $val );
			$buffer = explode ( ":", $val );
			$hour = intval ( $buffer [0] );
			$min = intval ( $buffer [1] );
			if ($pm) {
				$hour += 12;
			}
			return $hour . ":" . $min;
		} catch ( Exception $e ) {
			return false;
		}
	}
	private function transTimeTick($val) {
		try {
			$buffer = explode ( ":", $val );
			$ret = 0;
			$ret += intval ( $buffer [0] ) * 60;
			$ret += intval ( $buffer [1] );
			return $ret;
		} catch ( Exception $e ) {
			return false;
		}
	}
	public function getCountryList() {
		return $this->countrylist;
	}
	public function getCountry($code) {
		return $this->countrymap [$code];
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
$obj = new AdminProductAddAction ();
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
<script
	src="http://jonthornton.github.io/jquery-timepicker/jquery.timepicker.js"></script>
<link
	href="http://jonthornton.github.io/jquery-timepicker/jquery.timepicker.css"
	rel="stylesheet">
<link href="../css/common.css" rel="stylesheet">
<link href="../css/adminProductAdd.css" rel="stylesheet">
<script type="text/javascript" src="../js/adminProductAdd.js"></script>
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
							<td><input type="text" maxlength="4" id="code" name="code"
								class="btnText"> <input type="button" id="checkerCode"
								value="コードチェック" /> <input type="hidden" id="codeChecker"
								name="codeChecker" value="0" /></td>
						</tr>
						<tr>
							<td id="Lplanname" class="label">プラン名</td>
							<td><input type="text" maxlength="100" id="planname"
								name="planname"></td>
						</tr>
						<tr>
							<td id="LstartCountry" class="label">出発国家</td>
							<td><select id="startCountry" name="startCountry"
								class="selectCountry">
									<?php for($i=0;$i<count($obj->getCountryList());$i++){?>
										<option
										value="<?=$obj->getCountryList()[$i]->getCountryCode()?>">
											<?=$obj->getCountryList()[$i]->getCountryName()?>
										</option>
									<?php }?>
								</select></td>
						</tr>
						<tr>
							<td id="LstartLocation" class="label">出発地域</td>
							<td><select id="startLocation" name="startLocation"></select></td>
						</tr>
						<tr>
							<td id="LarriveCountry" class="label">到着国家</td>
							<td><select id="arriveCountry" name="arriveCountry"
								class="selectCountry">
									<?php for($i=0;$i<count($obj->getCountryList());$i++){?>
										<option
										value="<?=$obj->getCountryList()[$i]->getCountryCode()?>">
											<?=$obj->getCountryList()[$i]->getCountryName()?>
										</option>
									<?php }?>
								</select></td>
						</tr>
						<tr>
							<td id="LarriveLocation" class="label">到着地域</td>
							<td><select id="arriveLocation" name="arriveLocation"></select></td>
						</tr>
						<tr>
							<td id="Lstartdate" class="label">出発時間</td>
							<td><input type="text" id="startdate" name="startdate"
								class="btnText"> <input type="button" id="startdateButton"
								value="現在時間" /></td>
						</tr>
						<tr>
							<td id="Larrivedate" class="label">到着時間</td>
							<td><input type="text" id="arrivedate" name="arrivedate"
								class="btnText"> <input type="button" id="arrivedateButton"
								value="現在時間" /></td>
						</tr>
						<tr>
							<td id="Lprice" class="label">価格</td>
							<td><input type="tel" id="price" name="price" value="0"></td>
						</tr>
						<tr>
							<td colspan="2" class="errorLine"><span id="errorMsg"
								class="errorMsg"><?=$obj->getErrorMsg()?></span></td>
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
	<div class="template">
		<div id="locationselect">
			<?php
			
			for($i = 0; $i < count ( $obj->getLocationKeyList () ); $i ++) {
				$code = $obj->getLocationKeyList () [$i];
				?>
			<select id="location_key_<?=$code?>">
				<?php for($j=0;$j<count($obj->getLocationListMap($code));$j++){?>
					<option
					value="<?=$obj->getLocationListMap($code)[$j]->getLocationCode()?>"><?=$obj->getLocationListMap($code)[$j]->getLocationName()?></option>
				<?php }?>
			</select>
			<?php }?>
		</div>
	</div>
	<footer><?=$obj->getFooter()?></footer>
</body>
</html>