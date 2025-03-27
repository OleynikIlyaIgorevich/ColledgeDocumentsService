drop database if exists colledge_document_db;
create database colledge_document_db;
use colledge_document_db;

create table roles(
	id int primary key auto_increment,
    title varchar(32) not null unique,
	created_at datetime not null default now(),
    updated_at datetime
);

create table users(
	id int primary key auto_increment,
	role_id int not null,
    lastname varchar(32) not null,
    firstname varchar(32) not null,
    middlename varchar(32),
    phone varchar(20) not null unique,
    username varchar(32) not null unique,
    password_hash varchar(256) not null,
	created_at datetime not null default now(),
    updated_at datetime,
    foreign key (role_id) references roles(id) ON DELETE CASCADE
);

create table refresh_sessions(
	id int primary key auto_increment,
    user_id int not null,
    refresh_token varchar(128) not null,
    expires_at datetime not null,
    created_at datetime default now(),
    foreign key (user_id) references users(id) ON DELETE CASCADE
);

create table order_statuses(
	id int primary key auto_increment,
	title varchar(32) not null unique,
	created_at datetime not null default now(),
    updated_at datetime
);

create table document_types(
	id int primary key auto_increment,
	title varchar(32) not null unique,
	created_at datetime not null default now(),
    updated_at datetime
);

create table departaments(
	id int primary key auto_increment,
	title varchar(32) not null unique,
	created_at datetime not null default now(),
    updated_at datetime
);

create table document_orders(
	id int primary key auto_increment,
    user_id int not null,
    document_type_id int not null,
    order_status_id int not null,
    departament_id int not null,
    quantity int not null,
    created_at datetime not null default now(),
    updated_at datetime,
    foreign key (user_id) references users(id),
    foreign key (document_type_id) references document_types(id) ON DELETE CASCADE,
    foreign key (order_status_id) references order_statuses(id) ON DELETE CASCADE,
    foreign key (departament_id) references departaments(id) ON DELETE CASCADE
);




