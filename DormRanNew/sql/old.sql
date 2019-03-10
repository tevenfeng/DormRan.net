SET character_set_client = utf8;
SET character_set_connection = utf8;
SET character_set_database = utf8;
SET character_set_results = utf8;
SET character_set_server = utf8;

drop database if exists check_dorm_old;
create database check_dorm_old DEFAULT CHARSET utf8 COLLATE utf8_general_ci;

use check_dorm_old;

create table officer
(
    row_id   int auto_increment primary key,
    officer_id  varchar(12) unique not null,
    officer_department varchar(255) not null,
    officer_name varchar(255) not null,
    officer_gender enum('男','女') not null
)character set = utf8;

create table dorm
(
    row_id int auto_increment primary key,
    area  int not null check(area>0 and area<6),
    group_id int not null check(group_id>0 and group_id<3),
    dorm_name  varchar(255) unique not null,
    floor_number  int not null check(floor_number>0),
    gender enum('男','女') not null
)character set = utf8;

create table history
(
    row_id int auto_increment primary key,
    term  varchar(5) not null,
    check_id int not null,
    area  int not null,
    dorm_name  varchar(255) not null,
    floor_id  varchar(255) not null,  -- 1 2 3 4 5\
    insert_date  datetime
)character set = utf8;

create table checkin_history
(
    row_id int auto_increment primary key,
    term varchar(5) not null,
    officer_id varchar(12) references officer(officer_id),
    officer_name varchar(255) references officer(officer_name),
    insert_date  datetime
)character set = utf8;

insert into dorm(area, group_id, dorm_name, floor_number, gender) values(1, 1, '鹏翔5斋',12,'女');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(1, 1, '46斋',5,'女');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(1, 2, '鹏翔1斋',12,'男');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(1, 2, '35斋',4,'男');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(1, 2, '26斋',2,'男');

insert into dorm(area, group_id, dorm_name, floor_number, gender) values(2, 1, '鹏翔3斋',12,'女');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(2, 1, '23斋',2,'女');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(2, 1, '52斋B',7,'女');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(2, 2, '鹏翔2斋',12,'男');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(2, 2, '41斋',5,'男');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(2, 2, '24斋',2,'男');

insert into dorm(area, group_id, dorm_name, floor_number, gender) values(3, 1, '53斋',7,'女');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(3, 1, '37斋',4,'女');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(3, 1, '鹏翔4斋女生部',6,'女');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(3, 2, '鹏翔4斋',14,'男');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(3, 2, '49斋',6,'男');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(3, 3, '38斋',4,'男');

insert into dorm(area, group_id, dorm_name, floor_number, gender) values(4, 1, '48斋',6,'女');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(4, 1, '36斋',4,'女');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(4, 1, '25斋',2,'女');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(4, 2, '52斋A',6,'男');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(4, 2, '45斋',5,'男');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(4, 3, '47斋',6,'男');

set global optimizer_switch='derived_merge=off';
set optimizer_switch='derived_merge=off';

load data local infile "C:\\Users\\tevenfeng\\Desktop\\DormRanNew\\DormRanNew\\sql\\officer_info.csv" into table officer fields terminated by ',' lines terminated by '\r\n';

select * from dorm;

select * from officer;

select * from history;

select * from checkin_history;
