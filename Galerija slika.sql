insert into tbl_Autor
values('Milos','Aleksic','Rodjen u Srbiji');
insert into tbl_Autor
values('Aleksic','Milosevic','Rodjen u Bosni');
insert into tbl_Autor
values('Bojan','Bojanovic','Rodjen u Hrvatskoj');
insert into tbl_Autor
values('Maja','Ristic','Rodjen u Srbiji');
insert into tbl_Autor
values('Lena','Kovacevic','Rodjena u Makedoniji');

select ime
from tbl_Autor

update tbl_Autor
set prezime='Antic'
where autorID=3

delete from tbl_Autor
where prezime='Antic'


insert into tbl_Kustos
values('Nikola', 'Jovanovic', '345-222');
insert into tbl_Kustos
values('Milos', 'Markovic', '666-333');
insert into tbl_Kustos
values('Ana', 'Ilic', '888-6666');
insert into tbl_Kustos
values('Ivan', 'Petrovic', '999-000');
insert into tbl_Kustos
values('Marija', 'Lazic', '423-111');
insert into tbl_Kustos
values('Marija', 'Lajic', '423-111');

select ime, prezime
from tbl_Kustos;

update tbl_Kustos
set kontaktInformacije = '111-111'
where kustosID = 6;

delete from tbl_Kustos
where ime = 'Marija' and prezime = 'Lajic';

insert into tbl_Izlozba
values('Prolećna','05.20.2024','05.21.2024','Moderna', 6);
insert into tbl_Izlozba
values('Letnja', '04.20.2024','04.21.2024', 'Savremena', 8);
insert into tbl_Izlozba
values('Jesenja', '03.20.2024','03.30.2024', 'Klasicna', 7);
insert into tbl_Izlozba
values('Zimska', '06.20.2024','06.30.2024', 'Skulpture', 9);
insert into tbl_Izlozba
values('Festival', '07.20.2024','07.30.2024', 'Domaca', 10);
insert into tbl_Izlozba
values('Magicna', '10.20.2024','10.30.2024', 'Domaca', 10);


select nazivIzlozbe, datumPocetka, datumZavrsetka
from tbl_Izlozba;


update tbl_Izlozba
set opis = 'Nova'
where izlozbaID = 15;

delete from tbl_Izlozba
where nazivIzlozbe = 'Magicna';


insert into tbl_Korisnik
values('Jovana', 'Nikolic', 'jovana.n', 'pass123', 'Posetilac');
insert into tbl_Korisnik
values('Marko', 'Vasic', 'marko.v', 'pass456', 'Clan');
insert into tbl_Korisnik
values('Ana', 'Peric', 'ana.p', 'pass789', 'Admin');
insert into tbl_Korisnik
values('Mina', 'Jovic', 'mina.j', 'pass321', 'Posetilac');
insert into tbl_Korisnik
values('Luka', 'Petrovic', 'luka.p', 'pass654', 'Clan');

select ime, prezime, tipKorisnika
from tbl_Korisnik
where prezime='Petrovic';


update tbl_Korisnik
set lozinka = 'new123'
where korisnikID = 4;


delete from tbl_Korisnik
where korisnickoIme = 'marko';

insert into tbl_Ulaznica
values('04.20.2024', 500, 'Standard', 11, 13);
insert into tbl_Ulaznica
values('03.20.2024', 700, 'VIP', 12, 14);
insert into tbl_Ulaznica
values('07.20.2024', 500, 'Standard', 20, 15);
insert into tbl_Ulaznica
values('06.20.2024', 800, 'VIP', 14, 16);
insert into tbl_Ulaznica
values('05.20.2024', 600, 'Standard', 15, 17);

select datumKupovine, cena, tipUlaznice
from tbl_Ulaznica;

update tbl_Ulaznica
set cena = 750.0
where ulaznicaID = 3;


delete from tbl_Ulaznica
where tipUlaznice = 'VIP' and cena > 700.0;


insert into tbl_Kategorije
values('Portreti', 'Slike ljudi i lica');
insert into tbl_Kategorije
values('Pejzaži', 'Prikazi prirode');
insert into tbl_Kategorije
values('Apstraktna', 'Boje, oblici i linije');
insert into tbl_Kategorije
values('Mrtva', 'Neživih objekati');
insert into tbl_Kategorije
values('Animalizam', 'Slike sa motivima životinja');
insert into tbl_Kategorije
values('Animaliza', 'Slike sa motivima životinja');

select nazivKategorije, opis
from tbl_Kategorije;


update tbl_Kategorije
set opis = 'Prikazi prirode i pejzaža'
where kategorijaID = 2;


delete from tbl_Kategorije
where nazivKategorije = 'Animaliza';


insert into tbl_Recenzija
values('Izložba je bila veoma inspirativna!', 5, '05.20.2024', 16, 13);
insert into tbl_Recenzija
values('Svi radovi su bili impresivni, ali očekivao sam više.', 4, '03.20.2024', 17, 14);
insert into tbl_Recenzija
values('Umetnička dela su ostavila snažan utisak na mene.', 5, '04.20.2024', 18, 15);
insert into tbl_Recenzija
values('Prelepa organizacija i odabir radova.', 5, '07.20.2024', 19, 16);
insert into tbl_Recenzija
values('Dobra izložba, ali malo preskupa ulaznica.', 3, '06.20.2024', 20, 17);

select textRecenzije, ocena
from tbl_Recenzija;


update tbl_Recenzija
set ocena = 4
where recenzijaID = 3;


delete from tbl_Recenzija
where ocena < 5;

insert into tbl_Slika
values('Sunce', 2021, 'Akvarel', '50x70 cm', 1, 16, 5);
insert into tbl_Slika
values('Cviet', 2020, 'Ulje ', '40x60 cm', 2, 17, 6);
insert into tbl_Slika
values('More', 2019, 'Pastel', '60x80 cm', 2, 13, 7);
insert into tbl_Slika
values('Zima', 2022, 'Akril', '70x90 cm', 4, 14, 10);
insert into tbl_Slika
values('Ulica', 2023, 'Grafit', '30x40 cm', 5, 15, 11);



select nazivSlike, godinaNastanka, tehnika
from tbl_Slika;


update tbl_Slika
set dimenzije = '60x90 cm'
where slikaID = 7;


delete from tbl_Slika
where nazivSlike = 'Ulica';

select nazivSlike, nazivKategorije,nazivIzlozbe,ime +' '+ prezime as 'ime i prezime'
from tbl_Slika  join tbl_Autor on tbl_Slika.autorID = tbl_Autor.autorID
				join tbl_Kategorije on tbl_Slika.kategorijaID=tbl_Kategorije.kategorijaID
				join tbl_Izlozba on tbl_Slika.izlozbaID=tbl_Izlozba.izlozbaID


USE [Galerija slika]
GO
delete from tbl_Slika
delete from tbl_Autor
delete from tbl_Izlozba
delete from tbl_Kategorije
delete from tbl_Korisnik
delete from tbl_Kustos
delete from tbl_Recenzija
delete from tbl_Ulaznica


