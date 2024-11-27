using System;
using System.Diagnostics;
using System.IO;
using frogaDB8.Models;
using SQLite;

namespace frogaDB8.Services
{
    public static class DatabaseInitializer
    {
        // Datu basearen fitxategiaren izena
        private static string DbFileName => "data.sqlite";

        // Datu basearen bide osoa lortzen du
        public static string DbPath => Path.Combine(FileSystem.AppDataDirectory, DbFileName);

        /// <summary>
        /// Datu basea inicializatzen du eta beharrezko taulak sortzen ditu.
        /// </summary>
        public static void InitializeDatabase()
        {
            try
            {
                // Datu basearekin konexioa sortu
                using var connection = new SQLiteConnection(DbPath);

                // Taulak sortu datu basean
                connection.CreateTable<Hizkuntzak>();
                connection.CreateTable<Argitaletxeak>();
                connection.CreateTable<Egileak>();
                connection.CreateTable<Bazkideak>();
                connection.CreateTable<Liburuak>();
                connection.CreateTable<Maileguak>();

                // Mezua erroreak ez badaude
                Debug.WriteLine("Datu basea ondo hasi da");
            }
            catch (Exception ex)
            {
                // Errorea atzematen bada, mezua erakutsi
                Debug.WriteLine("Errorea datu basea hasterako garaian: " + ex.Message);
                throw;
            }
        }
    }
}
