using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;

public class HighScoreManager : MonoBehaviour {

    private string connectionString;
    private List<HighScore> highScores = new List<HighScore>();
    public GameObject scorePrefab;
    public Transform scoreParent;
    public int topRanks;

	// Use this for initialization
	void Start () {
        connectionString = "URI=file:" + Application.dataPath + "/HighScoreDB.sqlite";
        ShowScores();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InsertScore(string name, int newScore) {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuery = String.Format("INSERT INTO HighScores(Name,Score) VALUES(\"{0}\",\"{1}\")",name,newScore);
                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbConnection.Close();

            }
        }
    }

    private void GetScores() {
        highScores.Clear();
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuery = "SELECT * FROM HighScores ORDER BY Score";
                dbCmd.CommandText = sqlQuery;
                using (IDataReader reader = dbCmd.ExecuteReader()) {
                    while (reader.Read()) {
                        highScores.Add(new HighScore(reader.GetInt32(0), reader.GetInt32(2), reader.GetString(1), reader.GetDateTime(3)));
                    }

                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
    }

    private void DeleteScore(int id) {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuery = String.Format("DELETE FROM HighScores WHERE PlayerID = \"{0}\"", id);
                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbConnection.Close();

            }
        }
    }

    private void ShowScores() {
        GetScores();
        for (int i = 0; i< topRanks; i++) {

            if (i <= highScores.Count - 1) {

                GameObject tmpObjec = Instantiate(scorePrefab);

                HighScore tmpScore = highScores[i];

                // Get the length of the string
                int scoreTimeLength = tmpScore.Score.ToString().Length;
                string scoreTime;
                
                if (scoreTimeLength > 2)
                {
                    // Insert a ':' between the seconds and minutes of the score, then convert it to a string.
                    scoreTime = tmpScore.Score.ToString().Remove(scoreTimeLength - 2) + ":" + tmpScore.Score.ToString().Substring(scoreTimeLength - 2);
                } 
                else if (scoreTimeLength == 2)
                {
                    scoreTime = "00:" + tmpScore.Score.ToString();
                }
                else
                {
                    scoreTime = "00:0" + tmpScore.Score.ToString();
                }

                tmpObjec.GetComponent<HighScoreScript>().SetScore(tmpScore.Name, scoreTime, "#" + (i + 1).ToString());
                tmpObjec.transform.SetParent(scoreParent);
                tmpObjec.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
