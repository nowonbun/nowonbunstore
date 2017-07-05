<?php
class Product {
	private $product_code;
	private $planname;
	private $start_location;
	private $arrive_location;
	private $start_date;
	private $arrive_date;
	private $price;
	private $createdate;
	private $creater;
	private $state;
	public function getProductCode() {
		return $this->product_code;
	}
	public function setProductCode($product_code) {
		$this->product_code = $product_code;
	}
	public function getPlanname() {
		return $this->planname;
	}
	public function setPlanname($planname) {
		$this->planname = $planname;
	}
	public function getStartLocation() {
		return $this->start_location;
	}
	public function setStartLocation($start_location) {
		$this->start_location = $start_location;
	}
	public function getArriveLocation() {
		return $this->arrive_location;
	}
	public function setArriveLocation($arrive_location) {
		$this->arrive_location = $arrive_location;
	}
	public function getStartDate() {
		return $this->start_date;
	}
	public function setStartDate($start_date) {
		$this->start_date = $start_date;
	}
	public function getArriveDate() {
		return $this->arrive_date;
	}
	public function setArriveDate($arrive_date) {
		$this->arrive_date = $arrive_date;
	}
	public function getPrice() {
		return $this->price;
	}
	public function setPrice($price) {
		$this->price = $price;
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
