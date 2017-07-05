<?php
class Application {
	private $application_idx;
	private $member_id;
	private $product_code;
	private $memo;
	private $createdate;
	private $state;
	public function getApplicationIdx() {
		return $this->application_idx;
	}
	public function setApplicationIdx($application_idx) {
		$this->application_idx = $application_idx;
	}
	public function getMemberId() {
		return $this->member_id;
	}
	public function setMemeberId($member_id) {
		$this->member_id = $member_id;
	}
	public function getProductCode() {
		return $this->product_code;
	}
	public function setProductCode($product_code) {
		$this->product_code = $product_code;
	}
	public function getMemo() {
		return $this->memo;
	}
	public function setMemo($memo) {
		$this->memo = $memo;
	}
	public function getCreatedate() {
		return $this->createdate;
	}
	public function setCreatedate($createdate) {
		$this->createdate = $createdate;
	}
	public function getState() {
		return $this->state;
	}
	public function setState($state) {
		$this->state　 = $state;
	}
}
?>
