CREATE KEYSPACE scraping_dev WITH REPLICATION = {'class' : 'SimpleStrategy', 'replication_factor': 3};

use scraping_dev;

CREATE TABLE scraping_dev.broker(
	pkey INT,
    key VARCHAR,
    ip VARCHAR,
    count INT,
    active INT,
    connected TIMESTAMP,
    disconnected TIMESTAMP,
    lastpingupdated TIMESTAMP,
    PRIMARY KEY ((pkey),key)
);

CREATE TABLE scraping_dev.commondata(
	mallkey int,
	key VARCHAR,
	idx int,
	data text,
	PRIMARY KEY ((mallkey),key, idx)
);

CREATE TABLE scraping_dev.packagedata(
	mallkey int,
	key VARCHAR,
	idx int,
	separation int,
	data text,
	PRIMARY KEY ((mallkey),key, idx,separation)
);

CREATE TABLE scraping_dev.requestData (
	mallkey int,
    key VARCHAR,
    sdate VARCHAR,
    edate VARCHAR,
    scraptype VARCHAR,
    exec VARCHAR,
    id1 VARCHAR,
    id2 VARCHAR,
    id3 VARCHAR,
    pw1 VARCHAR,
    pw2 VARCHAR,
    pw3 VARCHAR,
    option1 VARCHAR,
    option2 VARCHAR,
    option3 VARCHAR,
    createdDate timestamp,
    apikey text,
    PRIMARY KEY ((mallkey),key)
);

CREATE TABLE scraping_dev.resultData (
	mallkey int,
	key VARCHAR,
	reqno VARCHAR,
	resultcd VARCHAR,
	resultmsg VARCHAR,
	starttime timestamp,
	endtime timestamp,
	server VARCHAR,
	PRIMARY KEY ((mallkey),key)
);

CREATE TABLE scraping_dev.keydata (
	pkey int,
	key text,
	bizno text,
	name text,
	ip text,
	callback text,
	PRIMARY KEY ((pkey),key)
);

CREATE TABLE scraping_dev.pingpong (
	pkey int,
    key VARCHAR,
    ip VARCHAR,
    lastupdated timestamp,
    PRIMARY KEY ((pkey),key)
);

CREATE TABLE scraping_dev.keynode (
	pkey int,
	key VARCHAR,
	mallkey int,
	childrunkey VARCHAR,
	PRIMARY KEY ((pkey),key)
);
