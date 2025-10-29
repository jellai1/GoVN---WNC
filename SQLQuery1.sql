create database dbCars

use dbCars

create table tblMembers(
MaUser int Identity primary key,
TenDN nvarchar(299) not null,
MatKhau varchar(299) not null,
Email varchar(299) not null,
SDT varchar(12) not null,
VaiTro nvarchar(30) not null,
);
drop table tblMembers
select * from tblMembers
delete  from tblMembers

CREATE TABLE tblXe (
    MaXe INT IDENTITY PRIMARY KEY,
    MaChuXe INT NOT NULL,
    TenXe NVARCHAR(299) NOT NULL,
    LoaiXe NVARCHAR(299) NOT NULL,
    BienSo VARCHAR(299) NOT NULL,
    GiaThueNgay Real NOT NULL,
    MoTa NVARCHAR(299),
    TrangThai NVARCHAR(299),
    AnhXe VARCHAR(299) NOT NULL,
    CONSTRAINT FK_tblXe_tblMembers FOREIGN KEY (MaChuXe) REFERENCES tblMembers(MaUser)
);

select * from tblXe
