<?php
error_reporting ( E_ALL ^ E_NOTICE || E_WARNING );
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Common/DefineMessage.php';
include_once $_SERVER ['DOCUMENT_ROOT'] . '/Household/Common/SessionClass.php';
abstract class AbstractController extends SessionClass {
	private $param;
	public function run() {
		try {
			date_default_timezone_set ( "Asis/Tokyu" );
			
			if (! $this->isPostBack ()) {
				throw new Exception ();
			}
			
			// http://localhost/Household/GetMaster.php?p=aaeyJnaWQiOiJ0ZXN0In0=
			if ($_POST ["p"] == null) {
				throw new Exception ();
			}
			$this->param = json_decode ( base64_decode ( substr ( $_POST ["p"], 2 ) ) );
			
			$this->initialize ();
			
			if ($this->validate () == false) {
				throw new Exception ();
			}
			
			echo "=A" . base64_encode ( json_encode ( $this->main () ) );
		} catch ( Exception $e ) {
			$this->error ();
		}
	}
	protected function isPostBack() {
		return strtoupper ( $_SERVER ['REQUEST_METHOD'] ) == 'POST';
	}
	protected function getParam($key) {
		$ret = $this->param->{$key};
		if ($ret == NULL) {
			throw new Exception ();
		}
		return $ret;
	}
	protected function redirect($url) {
		header ( "Location: " . $url );
		die ();
	}
	protected function connectHttp($url, $post, $header, $param) {
		// http://php.net/manual/en/function.curl-setopt.php
		$handle = curl_init ();
		try {
			curl_setopt ( $handle, CURLOPT_URL, $url );
			curl_setopt ( $handle, CURLOPT_FOLLOWLOCATION, TRUE );
			curl_setopt ( $handle, CURLOPT_AUTOREFERER, TRUE );
			curl_setopt ( $handle, CURLOPT_POST, $post );
			curl_setopt ( $handle, CURLOPT_POSTFIELDS, $param );
			curl_setopt ( $handle, CURLOPT_VERBOSE, TRUE );
			curl_setopt ( $handle, CURLOPT_SSL_VERIFYPEER, FALSE );
			curl_setopt ( $handle, CURLOPT_RETURNTRANSFER, TRUE );
			curl_setopt ( $handle, CURLOPT_HEADER, TRUE );
			curl_setopt ( $handle, CURLOPT_HTTPHEADER, $header );
			$response = curl_exec ( $handle );
			$header_size = curl_getinfo ( $handle, CURLINFO_HEADER_SIZE );
			$header = substr ( $response, 0, $header_size );
			$header_map = array ();
			$temp = split ( "\r\n", $header );
			array_push ( $header_map, $temp [0] );
			for($i = 1; $i < count ( $temp ); $i ++) {
				$temp1 = $temp [$i];
				$temp2 = split ( ":", $temp1 );
				$header_map [$temp2 [0]] = str_replace ( $temp2 [0] . ":", "", $temp1 );
			}
			$body = substr ( $response, $header_size );
			return array (
					"header" => $header_map,
					"body" => $body 
			);
		} finally{
			curl_close ( $handle );
		}
	}
	protected function createGUID() {
		if (function_exists ( 'com_create_guid' )) {
			return com_create_guid ();
		} else {
			mt_srand ( ( double ) microtime () * 10000 );
			$charid = strtoupper ( md5 ( uniqid ( rand (), true ) ) );
			$uuid = substr ( $charid, 0, 8 ) . chr ( 45 );
			$uuid .= substr ( $charid, 8, 4 ) . chr ( 45 );
			$uuid .= substr ( $charid, 12, 4 ) . chr ( 45 );
			$uuid .= substr ( $charid, 16, 4 ) . chr ( 45 );
			$uuid .= substr ( $charid, 20, 12 );
			return $uuid;
		}
	}
	protected function now() {
		return date ( 'Ymdhis', time () ) . gettimeofday () ["usec"];
	}
	protected function setHeaderError($code, $msg) {
		header ( "HTTP/1.1 " . $code . " " . $msg );
		http_response_code ( 406 );
		die ();
	}
	protected function writeFile($filepath, $data) {
		$file = fopen ( $filepath, "w" );
		try {
			fwrite ( $file, $data );
		} finally {
			fclose ( $file );
		}
	}
	protected function readFile($filepath) {
		$file = fopen ( $filepath, "r" );
		try {
			return fread ( $file, filesize ( $filepath ) );
		} finally {
			fclose ( $file );
		}
	}
	protected function moveFile($source, $destination) {
		rename ( $source, $destination );
	}
	protected abstract function initialize();
	protected abstract function main();
	protected abstract function error();
	protected abstract function validate();
}
?>