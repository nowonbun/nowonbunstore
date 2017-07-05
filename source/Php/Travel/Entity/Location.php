<?php
class Location {
	private $location_code;
	private $location_name;
	private $country_code;
	private $createdate;
	private $creater;
	private $state;
	public function getLocationCode() {
		return $this->location_code;
	}
	public function setLocationCode($location_code) {
		$this->location_code= $location_code;
	}
	public function getLocationName() {
		return $this->location_name;
	}
	public function setLocationName($location_name) {
		$this->location_name = $location_name;
	}
	public function getCountryCode() {
		return $this->country_code;
	}
	public function setCountryCode($country_code) {
		$this->country_code = $country_code;
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