<?php
class HshldRelation {
	private $ndx;
	private $id;
	private $rid;
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
	public function setRid($rid) {
		$this->rid = $rid;
	}
	public function getRid() {
		return $this->rid;
	}
}
?>