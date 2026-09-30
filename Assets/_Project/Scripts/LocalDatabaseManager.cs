using System;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

public class LocalDatabaseManager : MonoBehaviour
{
    private string dbPath;

    void Awake()
    {
        dbPath = "URI=file:" + Application.persistentDataPath + "/CalmAppDatabase.db";
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS BreathingSessions (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        timestamp TEXT NOT NULL,
                        durationSeconds INTEGER NOT NULL
                    );";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    public void SaveSession(int durationInSeconds)
    {
        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO BreathingSessions (timestamp, durationSeconds) VALUES (@time, @duration);";
                command.Parameters.Add(new SqliteParameter("@time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                command.Parameters.Add(new SqliteParameter("@duration", durationInSeconds));
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
        Debug.Log("Sessão registrada localmente no banco SQLite.");
    }
}