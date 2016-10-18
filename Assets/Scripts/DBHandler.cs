using UnityEngine;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.Data;
using System;
using System.Collections;
using System.IO;

public class DBHandler : MonoBehaviour
{
    private string connString;
    string path;
    IDbConnection dbconn;
    IDbCommand dbCmd;
    IDataReader reader;

    bool sessionChecked = false;
    bool savedCurrentSession = false;
    int sessionNo;

    // Use this for initialization
    void Awake()
    {
        //telling it that its a path and assigning path 
        path = Application.dataPath + "/ObjScanDatabase.db";
        connString = "URI=file:" + path;
        UpdateSession();
    }
    public int GetSessionNo() { return sessionNo; }
    public bool SavedCurSess() { return savedCurrentSession; }
    public void Write(List<LookedAt> laList)
    {
        if (!sessionChecked)
        {
            UpdateSession();
        }
        string values = "";
        foreach (LookedAt la in laList)
        {
            if (la.WrittenToDB == false) { 
                values += "(\"" + la.GetID() + "\",\"" + la.GetTime() + "\",\" " + 
                    la.GetHitPoint().ToString()
                + "\",\"" + sessionNo + "\"),";
                la.WrittenToDB = true;
            }
        }
        if (values == "")
        {
            Debug.LogAssertion("no list of LookedAts!");
            return;
        }
        values = RemoveLastChar(values);
        string queryStr = "INSERT INTO ObjectsLookedAt(objID,Time, HitPoint, SessionID) VALUES" + values;
        //give connection the command text
        OpenDB(queryStr);       
        dbCmd.ExecuteScalar();
        savedCurrentSession=true; // so that can access saved info from within same session/create more sessions
        CloseDB();

    }
    string RemoveLastChar(string str)
    {
        print("str last char");
        if (str != null && str.Length > 0 && str.Substring(str.Length - 1) == ",")
        {
            str = str.Substring(0, str.Length - 1);
            print("str ed");
        }
        else { print("no str"); }
        return str;
    }

    void OpenDB(string _queryStr)
    {
        dbconn = new SqliteConnection(connString);
        dbconn.Open();
        dbCmd = dbconn.CreateCommand();
        dbCmd.CommandText = _queryStr;
    }
    void CloseDB()
    {
        //close the databse

        dbCmd.Dispose();
        dbCmd = null;
        dbconn.Close();
        dbconn = null;
    }
    public void GetSession(int _sessionID, List<LookedAt>[] _sessions)
    {
        string querystr = "SELECT * FROM ObjectsLookedAt WHERE SessionID = " + _sessionID;
        OpenDB(querystr);

        //int rowCount = (int)dbCmd.ExecuteScalar(); //get number of rows for length of array
        //print(rowCount);
        //sessions[sessionNo] = new LookedAt[rowCount];
        IDataReader reader = dbCmd.ExecuteReader();
        _sessions[_sessionID] = new List<LookedAt>();
       while(reader.Read())
        {
            //print("sc"+ _sessions[_sessionID].Count);
            _sessions[_sessionID].Add( new LookedAt( reader.GetInt32(1), 
                                       reader.GetFloat(2), 
                                       GetVec(reader.GetString(3)) ) );
        }
        reader.Close();
        reader = null;
        CloseDB();
    }
    Vector3 GetVec(string str)
    {
        str = str.Substring(2, str.Length - 3);
        string[] strArr = str.Split(',');
        if (strArr.Length < 2) {
            Debug.LogError("Vector 3 was not passed");
            return Vector3.zero;
        }

        Vector3 d = new Vector3(float.Parse(strArr[0]),
                           float.Parse(strArr[1]),
                           float.Parse(strArr[2]));

        return d;
    }
    void UpdateSession()
    {
        if (File.Exists(path))
        {
            string queryStr = "SELECT * FROM ObjectsLookedAt WHERE objLookedAtID = (SELECT MAX(objLookedAtID) FROM ObjectsLookedAt)"; //get last row  //give connection the command text                                                                                                                          
            OpenDB(queryStr);
            //need to use reader to get info
            IDataReader reader = dbCmd.ExecuteReader();
            reader.Read();
            sessionNo = reader.GetInt32(4) + 1;
            sessionChecked = true;
            reader.Close();
            reader = null;
            CloseDB();
        }else {
            print("no exist");
        }
    }
    //void Read()
    //{

    //    string conn = "URI=file:" + Application.dataPath + "/ObjScanDatabase.s3db"; //Path to database.
    //    if (File.Exists(Application.dataPath + "/ObjScanDatabase.s3db"))
    //    {
    //        print("exists!");
    //    }
    //        print("conn " + conn);
    //    IDbConnection dbconn;
    //    dbconn = (IDbConnection)new SqliteConnection(conn);
    //    dbconn.Open(); //Open connection to the database.
    //    IDbCommand dbcmd = dbconn.CreateCommand();
    //    //get random data
    //    string sqlQuery = "SELECT NAME FROM sqlite_master";//"SELECT value,name, randomSequence " + "FROM ObjectsLookedAt";
    //    dbcmd.CommandText = sqlQuery;
    //    IDataReader reader = dbcmd.ExecuteReader();
    //    print("reader "+ reader.IsDBNull(0));
    //    while (reader.Read())
    //    {
    //        //int value = reader.GetInt32(0);
    //        //string name = reader.GetString(1);
    //        //int rand = reader.GetInt32(2);

    //        Debug.Log(reader.GetName(0));
    //    }


    //    //close the databse
    //    reader.Close();
    //    reader = null;
    //    dbcmd.Dispose();
    //    dbcmd = null;
    //    dbconn.Close();
    //    dbconn = null;
    //}

    //public ArrayList GetTables()
    //{
    //    ArrayList list = new ArrayList();

    //    // executes query that select names of all tables in master table of the database
    //    String query = "SELECT name FROM sqlite_master " +
    //            "WHERE type = 'table'" +
    //            "ORDER BY 1";
    //    try
    //    {

    //        DataTable table = GetDataTable(query);

    //        // Return all table names in the ArrayList

    //        foreach (DataRow row in table.Rows)
    //        {
    //            list.Add(row.ItemArray[0].ToString());
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        Console.WriteLine(e.Message);
    //    }
    //    return list;
    //}

    //public DataTable GetDataTable(string sql)
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();
    //        using (var c = new SqliteConnection(dbConnection))
    //        {
    //            c.Open();
    //            using (SqliteConnection cmd = new SqliteConnection(sql, c))
    //            {
    //                using (SqliteConnection rdr = cmd.ExecuteReader())
    //                {
    //                    dt.Load(rdr);
    //                    return dt;
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        Console.WriteLine(e.Message);
    //        return null;
    //    }
    //}
}
