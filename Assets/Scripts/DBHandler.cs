using UnityEngine;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.Data;
using System;
using System.Collections;
using System.IO;  

public class DBHandler : MonoBehaviour {
    private string connString;
    string path;
    bool sessionChecked = false;
    int sessionNo;

    // Use this for initialization
    void Start () {
        //telling it that its a path and assigning path 
        path = Application.dataPath + "/ObjScanDatabase.db";
        connString = "URI=file:" + path;
       // NewRead();
        //Write(2, 4.2f);
    }

    public void Write(List<LookedAt> laList)
    {
        if (!sessionChecked) {
            Read();
        }
        string conn = "URI=file:" + Application.dataPath + "/ObjScanDatabase.s3db"; //Path to database.
        using (IDbConnection dbconn = (IDbConnection)new SqliteConnection(connString))
        {
            dbconn.Open(); //Open connection to the database.
            using (IDbCommand dbCmd = dbconn.CreateCommand())
            {
                string values="";
                foreach (LookedAt la in laList)
                {
                    values += "(\"" + la.GetID() + "\",\"" + la.GetTime() + "\",\""+sessionNo+"\"),";
                }
                values = RemoveLastChar(values);
                string queryStr = /*string.Format(*/"INSERT INTO ObjectsLookedAt(objID,Time, SessionID) VALUES"+values;
                //give connection the command text
                dbCmd.CommandText = queryStr;
                dbCmd.ExecuteScalar();
                dbconn.Close();
            }
        }
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
    public void Read()
    {
        if (File.Exists(path))
        {
            print("exists");
            //the using means that the database connection closes itself
            using (IDbConnection dbconn = new SqliteConnection(connString))
            {
                dbconn.Open();
                using (IDbCommand dbCmd = dbconn.CreateCommand())
                {
                    string queryStr = "SELECT * FROM ObjectsLookedAt WHERE objLookedAtID = (SELECT MAX(objLookedAtID) FROM ObjectsLookedAt)"; //get last row
                                                                                                       //give connection the command text
                    dbCmd.CommandText = queryStr;

                    //need to use reader to get info
                    using (IDataReader reader = dbCmd.ExecuteReader())
                    {
                        reader.Read();
                        sessionNo = reader.GetInt32(3)+1;
                        sessionChecked = true;
                        print("sess checked: " + sessionNo);

                        while(reader.Read())
                        {
                            
                        }
                        dbconn.Close();
                        reader.Close();
                    }
                }
            }
        }
        else {
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
