<?php
class TknDmn {
	private $ndx;
	private $id;
	private $access_token;
	private $apply_date;
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
	public function setAccess_token($access_token) {
		$this->access_token = $access_token;
	}
	public function getAccess_token() {
		return $this->access_token;
	}
	public function setApply_date($apply_date) {
		$this->apply_date = $apply_date;
	}
	public function getApply_date() {
		return $this->apply_date;
	}
}
?>