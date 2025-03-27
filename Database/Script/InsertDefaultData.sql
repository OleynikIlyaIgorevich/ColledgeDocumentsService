use colledge_document_db;

insert into roles(id, title)
values (1, 'Студент'), (2, 'Оператор справок'), (3, 'Администратор');

insert into users(role_id, lastname, firstname, middlename, phone, username, password_hash)
values (3, 'Олейник', 'Илья', 'Игоревич', '+791231231313', 'ilya123', '$2a$12$fLoF5pSPhm91lL96p1QvEOT7zDN4d3SBHKyHPnELqH33dcQvYFfP.');

insert into document_types(id, title)
values (1, 'Справка с места учёбы');

insert into order_statuses(id, title)
values (1, 'Создана'), (2, 'Отменена'), (3, 'Принята в работу'), (4, 'Ожидает получателя'), (5, 'Завершена');

insert into departaments(id, title)
values (1, 'Деловая улица, 15'), (2, 'Рихарда Зорге, 13А');