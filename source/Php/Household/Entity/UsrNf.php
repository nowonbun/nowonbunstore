<?php
class UsrNf {
	private $id;
	private $name;
	private $email;
	private $createdate;
	public function setId($id) {
		$this->id = $id;
	}
	public function getId() {
		return $this->id;
	}
	public function setName($name) {
		$this->name = $name;
	}
	public function getName() {
		return $this->name;
	}
	public function setEmail($email) {
		$this->email = $email;
	}
	public function getEmail() {
		return $this->email;
	}
	public function setCreatedate($createdate) {
		$this->createdate = $createdate;
	}
	public function getCreatedate() {
		return $this->createdate;
	}
}