create database online_store;

use online_store;

create table Goods (
	goods_id bigint identity(1, 1),
	name nvarchar(32) not null unique,
	description nvarchar(128) not null,
	price decimal(10,2) not null,

	constraint PK_goods_goods_id primary key (goods_id)
);

insert into goods
values ('����� 1', '�������� ������ 1', 9.99);

insert into goods
values ('����� 2', '�������� ������ 2', 10.99);

insert into goods
values ('����� 3', '�������� ������ 3', 11.99);
