<?php
class Ctgry {
	private $cd;
	private $nm;
	public function setCd($cd) {
		$this->cd = $cd;
	}
	public function getCd() {
		return $this->cd;
	}
	public function setNm($nm) {
		$this->nm = $nm;
	}
	public function getNm() {
		return $this->nm;
	}
	public function toString(){
		return array(
			"cd" => $this->cd,
			"nm" => $this->nm
		);
	}
}
?>