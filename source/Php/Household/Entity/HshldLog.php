<?php
class HshldLog {
	private $ndx;
	private $ndx2;
	private $id;
	private $cd;
	private $tp;
	private $dt;
	private $cntxt;
	private $prc;
	private $pdt;
	public function setNdx($ndx) {
		$this->ndx = $ndx;
	}
	public function getNdx() {
		return $this->ndx;
	}
	public function setNdx2($ndx2) {
		$this->ndx2 = $ndx2;
	}
	public function getNdx2() {
		return $this->ndx2;
	}
	public function setId($id) {
		$this->id = $id;
	}
	public function getId() {
		return $this->id;
	}
	public function setCd($cd) {
		$this->cd = $cd;
	}
	public function getCd() {
		return $this->cd;
	}
	public function setTp($tp) {
		$this->tp = $tp;
	}
	public function getTp() {
		return $this->tp;
	}
	public function setDt($dt) {
		$this->dt = $dt;
	}
	public function getDt() {
		return $this->dt;
	}
	public function setCntxt($cntxt) {
		$this->cntxt = $cntxt;
	}
	public function getCntxt() {
		return $this->cntxt;
	}
	public function setPrc($prc) {
		$this->prc = $prc;
	}
	public function getPrc() {
		return $this->prc;
	}
	public function setPdt($pdt) {
		$this->pdt = $pdt;
	}
	public function getPdt() {
		return $this->pdt;
	}
}
?>