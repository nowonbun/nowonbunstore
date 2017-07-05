<?php
class Country {
	private $country_code;
	private $country_name;
	private $createdate;
	private $creater;
	private $state;
	public function getCountryCode() {
		return $this->country_code;
	}
	public function setCountryCode($country_code) {
		$this->country_code = $country_code;
	}
	public function getCountryName() {
		return $this->country_name;
	}
	public function setCountryName($country_name) {
		$this->country_name = $country_name;
	}
	public function getCreatedate() {
		return $this->createdate;
	}
	public function setCreatedate($createdate) {
		$this->createdate = $createdate;
	}
	public function getCreater() {
		return $this->creater;
	}
	public function setCreater($creater) {
		$this->creater = $creater;
	}
	public function getState() {
		return $this->state;
	}
	public function setState($state) {
		$this->state = $state;
	}
}
?>