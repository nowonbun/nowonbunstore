<?php
class Member {
	private $id;
	private $password;
	private $name;
	private $birth;
	private $createdate;
	private $creater;
	private $state;
	public function getId() {
		return $this->id;
	}
	public function setId($id) {
		$this->id = $id;
	}
	public function getPassword() {
		return $this->password;
	}
	public function setPassword($password) {
		$this->password = $password;
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
