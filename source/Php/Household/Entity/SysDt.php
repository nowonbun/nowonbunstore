<?php
class SysDt {
	private $kycd;
	private $dt;
	public function setKycd($kycd) {
		$this->kycd = $kycd;
	}
	public function getKycd() {
		return $this->kycd;
	}
	public function setDt($dt) {
		$this->dt = $dt;
	}
	public function getDt() {
		return $this->dt;
	}
	public function toString(){
		return array(
			"kycd" => $this->kycd,
			"dt" => $this->dt
		);
	}
}
?>
