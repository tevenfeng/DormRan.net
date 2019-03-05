SET character_set_client = utf8;
SET character_set_connection = utf8;
SET character_set_database = utf8;
SET character_set_results = utf8;
SET character_set_server = utf8;

drop database if exists check_dorm_New;
create database check_dorm_New DEFAULT CHARSET utf8 COLLATE utf8_general_ci;

use check_dorm_New;

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
    area  int not null check(area>0 and area<5),
    group_id int not null check(group_id>0 and group_id<4),
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
    insert_date  date
)character set = utf8;

insert into dorm(area, group_id, dorm_name, floor_number, gender) values(1, 1, '齐园13斋',5,'女');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(1, 1, '诚园7斋A',5,'女');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(1, 1, '留园B',5,'女');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(1, 2, '修园11斋',4,'男');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(1, 2, '知园5斋B',5,'男');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(1, 2, '平园21斋B',4,'男');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(1, 3, '诚园8斋B',5,'男');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(1, 3, '治园18斋',5,'男');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(1, 3, '平园23斋A',5,'男');

insert into dorm(area, group_id, dorm_name, floor_number, gender) values(2, 1, '格园3斋',6,'女');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(2, 1, '知园4斋A',5,'女');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(2, 1, '治园17斋',5,'女');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(2, 2, '修园12斋',4,'男');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(2, 2, '诚园6斋A',5,'男');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(2, 2, '平园23斋B',4,'男');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(2, 3, '诚园8斋A',5,'男');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(2, 3, '格园1斋',5,'男');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(2, 3, '平园24斋B',4,'男');

insert into dorm(area, group_id, dorm_name, floor_number, gender) values(3, 1, '正园10斋',4,'女');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(3, 1, '知园4斋B',5,'女');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(3, 1, '齐园16斋',5,'女');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(3, 2, '齐园15斋',5,'男');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(3, 2, '治园20斋',5,'男');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(3, 3, '诚园6斋B',5,'男');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(3, 3, '知园5斋A',5,'男');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(3, 3, '平园21斋A',5,'男');

insert into dorm(area, group_id, dorm_name, floor_number, gender) values(4, 1, '齐园14斋',5,'女');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(4, 1, '诚园7斋B',5,'女');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(4, 1, '平园22斋B',5,'女');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(4, 2, '正园9斋',4,'男');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(4, 2, '治园19斋',5,'男');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(4, 3, '格园2斋',5,'男');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(4, 3, '平园22斋A',5,'男');
insert into dorm(area, group_id, dorm_name, floor_number, gender) values(4, 3, '平园24斋A',5,'男');

load data local infile "C:\\Users\\tevenfeng\\Desktop\\DormRanNew\\DormRanNew\\sql\\officer_info.csv" into table officer fields terminated by ',' lines terminated by '\r\n';

set global optimizer_switch='derived_merge=off';
set optimizer_switch='derived_merge=off';

select * from dorm;

select * from officer;

select * from history;
