<?php
class System_code {
	private $code;
	private $value;
	private $createdate;
	private $creater;
	private $state;
	public function getCode() {
		return $this->code;
	}
	public function setCode($code) {
		$this->code = $code;
	}
	public function getValue() {
		return $this->value;
	}
	public function setValue($value) {
		$this->value = $value;
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