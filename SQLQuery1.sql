CREATE DATABASE DB_HasilPanen;
GO
USE DB_HasilPanen;
GO

-- Tabel Admin
CREATE TABLE Admin (
    id_admin INT IDENTITY(1,1) PRIMARY KEY,
    nama_lengkap VARCHAR(100) NOT NULL,
    username VARCHAR(50) UNIQUE NOT NULL,
    password VARCHAR(50) NOT NULL,
    no_telp VARCHAR(15)
);

-- Tabel Petani
CREATE TABLE Petani (
    id_petani INT IDENTITY(1,1) PRIMARY KEY,
    nama_petani VARCHAR(100) NOT NULL,
    kelompok_tani VARCHAR(50) NOT NULL,
    no_telp VARCHAR(15),
    username VARCHAR(50) UNIQUE NOT NULL,
    password VARCHAR(50) NOT NULL
);

-- Tabel Tanaman
CREATE TABLE Tanaman (
    id_tanaman INT IDENTITY(1,1) PRIMARY KEY,
    nama_tanaman VARCHAR(100) NOT NULL,
    kategori VARCHAR(50) NOT NULL,
    satuan_hasil VARCHAR(20) NOT NULL
);

-- Tabel Hasil Panen
CREATE TABLE Hasil_Panen (
    id_panen INT IDENTITY(1,1) PRIMARY KEY,
    id_petani INT NOT NULL,
    id_tanaman INT NOT NULL,
    tanggal_panen DATE DEFAULT GETDATE(),
    jumlah_hasil FLOAT NOT NULL,
    kualitas VARCHAR(50),
  
    CONSTRAINT FK_Panen_Petani FOREIGN KEY (id_petani) REFERENCES Petani(id_petani),
	CONSTRAINT FK_Panen_Tanaman FOREIGN KEY (id_tanaman) REFERENCES Tanaman(id_tanaman)
);

-- Insert 1 Admin
INSERT INTO Admin (nama_lengkap, username, password, no_telp) 
VALUES ('Deva Aditya', 'admin', '123', '08123456789');

-- Insert 2 Petani
INSERT INTO Petani (nama_petani, kelompok_tani, no_telp, username, password) 
VALUES 
('Afdan Aiman', 'Padi', '085211112222', 'petani1', '123'),
('Ahmad Fadjar', 'Bawang', '085233334444', 'petani2', '123');

-- Insert Tanaman
INSERT INTO Tanaman (nama_tanaman, kategori, satuan_hasil) 
VALUES ('Padi', 'Tanaman Pangan', 'Kg');

INSERT INTO Tanaman (nama_tanaman, kategori, satuan_hasil) 
VALUES ('Bawang', 'Rempah Rempah', 'Kg');

INSERT INTO Tanaman (nama_tanaman, kategori, satuan_hasil) 
VALUES ('Jagung', 'Tanaman Pangan', 'Kg');

INSERT INTO Tanaman (nama_tanaman, kategori, satuan_hasil) 
VALUES ('Cabai', 'Sayuran', 'Kg');