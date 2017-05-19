<?php
class Hshld {
	private $ndx;
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
	public function toArray() {
		return array (
				"ndx" => $this->ndx,
				"id" => $this->id,
				"cd" => $this->cd,
				"tp" => $this->tp,
				"dt" => $this->dt,
				"cntxt" => $this->cntxt,
				"prc" => $this->prc,
				"pdt" => $this->pdt 
		);
	}
}
?>