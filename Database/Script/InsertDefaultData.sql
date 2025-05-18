use colledge_document_db;

insert into roles(id, title)
values (1, 'Студент'), (2, 'Оператор справок'), (3, 'Администратор');

insert into users(role_id, lastname, firstname, middlename, phone, username, password_hash)
values
	(3, 'Шайдулин', 'Даниил', null, '+791231231313', 'danil123', '$2a$12$DqwVbZEGG0jz.W6ar/JQKuCNYw8Eot1AB8.Mzr/T.fvVYX3Xc9G.O'),
	(3, 'Гизатуллин', 'Ильяс', null, '+791231231312', 'ilyas123', '$2a$12$DqwVbZEGG0jz.W6ar/JQKuCNYw8Eot1AB8.Mzr/T.fvVYX3Xc9G.O');

insert into document_types(id, title)
values (1, 'Справка с места учёбы');

insert into order_statuses(id, title)
values (1, 'Создана'), (2, 'Отменена'), (3, 'Принята в работу'), (4, 'Ожидает получателя'), (5, 'Завершена');

insert into departaments(id, title)
values (1, 'Деловая улица, 15'), (2, 'Рихарда Зорге, 13А');