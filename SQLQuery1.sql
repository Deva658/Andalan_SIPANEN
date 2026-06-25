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
('Ahmad Fadjar', 'Bawang', '085233334444', 'petani2', '123'),
('Depa Aditya', 'Jagung', '085876545678', 'petani3', '123'),
('Farhan Arkabima', 'Cabai', '0851234543', 'petani4', '123');

-- Insert Tanaman
INSERT INTO Tanaman (nama_tanaman, kategori, satuan_hasil) 
VALUES ('Padi', 'Tanaman Pangan', 'Kg');

INSERT INTO Tanaman (nama_tanaman, kategori, satuan_hasil) 
VALUES ('Bawang', 'Rempah Rempah', 'Kg');

INSERT INTO Tanaman (nama_tanaman, kategori, satuan_hasil) 
VALUES ('Jagung', 'Tanaman Pangan', 'Kg');

INSERT INTO Tanaman (nama_tanaman, kategori, satuan_hasil) 
VALUES ('Cabai', 'Sayuran', 'Kg');

CREATE VIEW vw_RiwayatPanen AS
SELECT 
    h.id_panen, 
    h.id_petani, 
    p.nama_petani, 
    p.no_telp, 
    t.id_tanaman,
    t.nama_tanaman, 
    h.tanggal_panen, 
    h.jumlah_hasil, 
    t.satuan_hasil, 
    h.kualitas
FROM Hasil_Panen h
JOIN Petani p ON h.id_petani = p.id_petani
JOIN Tanaman t ON h.id_tanaman = t.id_tanaman;

SELECT * INTO Hasil_Panen_Backup 
FROM Hasil_Panen;

SELECT * FROM Hasil_Panen

CREATE PROCEDURE sp_InsertHasilPanen
    @IdPetani INT,
    @IdTanaman INT,
    @TanggalPanen DATE,
    @JumlahHasil FLOAT,
    @Kualitas VARCHAR(50)
AS
BEGIN
    BEGIN TRY
        IF @JumlahHasil <= 0
        BEGIN
            THROW 51000, 'Error DB: Jumlah hasil panen tidak boleh nol atau negatif.', 1;
        END

        INSERT INTO Hasil_Panen (id_petani, id_tanaman, tanggal_panen, jumlah_hasil, kualitas)
        VALUES (@IdPetani, @IdTanaman, @TanggalPanen, @JumlahHasil, @Kualitas);
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END

CREATE PROCEDURE sp_UpdateHasilPanen
    @IdPanen INT,
    @IdPetani INT,
    @IdTanaman INT,
    @TanggalPanen DATE,
    @JumlahHasil FLOAT,
    @Kualitas VARCHAR(50)
AS
BEGIN
    BEGIN TRY
        IF @JumlahHasil <= 0
            THROW 51000, 'Error DB: Jumlah hasil tidak valid.', 1;

        UPDATE Hasil_Panen 
        SET id_tanaman = @IdTanaman, tanggal_panen = @TanggalPanen, 
            jumlah_hasil = @JumlahHasil, kualitas = @Kualitas 
        WHERE id_panen = @IdPanen AND id_petani = @IdPetani;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END

CREATE PROCEDURE sp_DeleteHasilPanen
    @IdPanen INT
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Hasil_Panen WHERE id_panen = @IdPanen)
        BEGIN
            THROW 51001, 'Error DB: Data panen tidak ditemukan untuk dihapus.', 1;
        END

        DELETE FROM Hasil_Panen WHERE id_panen = @IdPanen;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END

CREATE PROCEDURE sp_SearchPanen
    @Keyword VARCHAR(100)
AS
BEGIN
    BEGIN TRY
        IF LTRIM(RTRIM(@Keyword)) = ''
        BEGIN
            SELECT * FROM vw_RiwayatPanen;
        END
        ELSE
        BEGIN
            SELECT * FROM vw_RiwayatPanen
            WHERE nama_petani LIKE '%' + @Keyword + '%' 
               OR nama_tanaman LIKE '%' + @Keyword + '%';
        END
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END

DROP PROCEDURE sp_SearchPanen;
GO

CREATE TABLE LogError (
    id_log INT IDENTITY(1,1) PRIMARY KEY,
    waktu DATETIME DEFAULT GETDATE(),
    pesan_error VARCHAR(MAX)
);
GO

CREATE TABLE LogAktivitas (
    id_log INT IDENTITY(1,1) PRIMARY KEY,
    aktivitas VARCHAR(100),
    waktu DATETIME DEFAULT GETDATE()
);
GO

CREATE TABLE LogKeamanan (
    id_log INT IDENTITY(1,1) PRIMARY KEY,
    aktivitas VARCHAR(200),
    jumlah_data INT,
    waktu DATETIME DEFAULT GETDATE()
);
GO

CREATE TRIGGER trg_InsertHasilPanen
ON Hasil_Panen
AFTER INSERT
AS
BEGIN
    INSERT INTO LogAktivitas (aktivitas, waktu)
    VALUES ('Tambah data hasil panen', GETDATE());
END;
GO

CREATE TRIGGER trg_DeleteHasilPanen
ON Hasil_Panen
AFTER DELETE
AS
BEGIN
    INSERT INTO LogAktivitas (aktivitas, waktu)
    VALUES ('Hapus data hasil panen', GETDATE());
END;
GO

CREATE TRIGGER trg_UpdatePanen
ON Hasil_Panen
AFTER UPDATE
AS
BEGIN
    DECLARE @jumlah INT;
    
    SELECT @jumlah = COUNT(*) FROM inserted; 

    IF @jumlah > 1
    BEGIN
        -- Simpan ke log keamanan
        INSERT INTO LogKeamanan (aktivitas, jumlah_data, waktu)
        VALUES ('WARNING: Update massal terdeteksi pada tabel Hasil_Panen', @jumlah, GETDATE()); 

        -- Membatalkan transaksi transaksi
        ROLLBACK TRANSACTION; 

        -- Menampilkan pesan error ke sistem/aplikasi
        RAISERROR('Update dibatalkan! Terlalu banyak data diubah sekaligus (Indikasi Ancaman Keamanan).', 16, 1); 
    END
END;
GO

ALTER PROCEDURE sp_ReportPanen
    @inNamaPetani VARCHAR(100),
    @inTahunPanen CHAR(4)
AS
BEGIN
    IF @inNamaPetani = 'Semua' OR @inNamaPetani = ''
    BEGIN
        SELECT 
            h.id_panen AS IdPanen,
            p.nama_petani AS NamaPetani,
            t.nama_tanaman AS NamaTanaman,
            h.tanggal_panen AS TanggalPanen,
            h.jumlah_hasil AS JumlahHasil,
            t.satuan_hasil AS SatuanHasil,
            h.kualitas AS Kualitas
        FROM Hasil_Panen h
        JOIN Petani p ON h.id_petani = p.id_petani
        JOIN Tanaman t ON h.id_tanaman = t.id_tanaman
        WHERE YEAR(h.tanggal_panen) = @inTahunPanen;
    END
    ELSE
    BEGIN
        SELECT 
            h.id_panen AS IdPanen,
            p.nama_petani AS NamaPetani,
            t.nama_tanaman AS NamaTanaman,
            h.tanggal_panen AS TanggalPanen,
            h.jumlah_hasil AS JumlahHasil,
            t.satuan_hasil AS SatuanHasil,
            h.kualitas AS Kualitas
        FROM Hasil_Panen h
        JOIN Petani p ON h.id_petani = p.id_petani
        JOIN Tanaman t ON h.id_tanaman = t.id_tanaman
        WHERE p.nama_petani = @inNamaPetani
          AND YEAR(h.tanggal_panen) = @inTahunPanen;
    END
END;
GO

CREATE PROCEDURE sp_DashboardAdmin
    @inTahun CHAR(4)
AS
BEGIN
    SELECT 
        p.nama_petani AS NamaSumbuX,
        SUM(h.jumlah_hasil) AS TotalPanen
    FROM Hasil_Panen h
    JOIN Petani p ON h.id_petani = p.id_petani
    WHERE YEAR(h.tanggal_panen) = @inTahun
    GROUP BY p.nama_petani;
END;
GO

CREATE PROCEDURE sp_DashboardPetani
    @inNamaPetani VARCHAR(100),
    @inTahun CHAR(4)
AS
BEGIN
    SELECT 
        t.nama_tanaman AS NamaSumbuX,
        SUM(h.jumlah_hasil) AS TotalPanen
    FROM Hasil_Panen h
    JOIN Petani p ON h.id_petani = p.id_petani
    JOIN Tanaman t ON h.id_tanaman = t.id_tanaman
    WHERE p.nama_petani = @inNamaPetani
      AND YEAR(h.tanggal_panen) = @inTahun
    GROUP BY t.nama_tanaman;
END;
GO

select * from LogAktivitas