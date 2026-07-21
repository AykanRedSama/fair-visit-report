create table if not exists visit_reports (
    id bigserial primary key,
    name varchar(255) not null,
    position varchar(255),
    company varchar(255),
    mail_address varchar(320),
    phone_number varchar(100),
    report_text text not null,
    created_at timestamp with time zone not null,
    updated_at timestamp with time zone not null,
    exported boolean not null default false,
    exported_at timestamp with time zone null
);