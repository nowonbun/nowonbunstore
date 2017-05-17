<?php
class Tp {
	private $tp;
	private $nm;
	private $cd;
	public function setTp($tp) {
		$this->tp = $tp;
	}
	public function getTp() {
		return $this->tp;
	}
	public function setNm($nm) {
		$this->nm = $nm;
	}
	public function getNm() {
		return $this->nm;
	}
	public function setCd($cd) {
		$this->cd = $cd;
	}
	public function getCd() {
		return $this->cd;
	}
	public function toArray() {
		return array (
				"tp" => $this->tp,
				"nm" => $this->nm,
				"cd" => $this->cd 
		);
	}
}
?>
