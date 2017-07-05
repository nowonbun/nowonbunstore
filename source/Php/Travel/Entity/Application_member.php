<?php
class Application_member {
	private $idx;
	private $application_idx;
	private $name;
	private $birth;
	private $memo;
	private $createdate;
	private $state;
	public function getIdx() {
		return $this->idx;
	}
	public function setIdx($idx) {
		$this->idx = $idx;
	}
	public function getApplicationIdx() {
		return $this->application_idx;
	}
	public function setApplicationIdx($application_idx) {
		$this->application_idx = $application_idx;
	}
	public function getName() {
		return $this->name;
	}
	public function setName($name) {
		$this->name = $name;
	}
	public function getBirth() {
		return $this->birth;
	}
	public function setBirth($birth) {
		$this->birth = $birth;
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
		$this->state = $state;
	}
}
?>
