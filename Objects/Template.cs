using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace TEMPLATE.Objects
{
  public class TEMPLATE_OBJECT
  {
    // where TEMPLATE_OBJECTId references a property of the object
    public int TEMPLATE_OBJECTId {get; set;}
    public string TEMPLATE_OBJECTDescription {get; set;}
    private List<string> TEMPLATE = new List<string> {};
    //where TEMPLATE_OBJECT references the Object

    public TEMPLATE_OBJECT(int TEMPLATEuserdata)
    {
      this.TEMPLATEproperty = TEMPLATEuserdata;
    }

    public override bool Equals(System.Object TEMPLATE_OTHER_OBJECT)
    {
      if (!(otherTask is Task))
      {
        return false;
      }
      else
      {
        TEMPLATE_OBJECT newTEMPLATE_OBJECT = (TEMPLATE_OBJECT) TEMPLATE_OTHER_OBJECT;
        bool TEMPLATEpropertyEquality = (this.TEMPLATEproperty() == newTEMPLATE_OBJECT.TEMPLATEproperty());
        return (TEMPLATEpropertyEquality);
      }
    }
    public override int GetHashCode()
    {
      return this.TEMPLATE_OBJECTDescription.GetHashCode();
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO template (TEMPLATE_OBJECTdescription) OUTPUT INSERTED.id VALUES (@TEMPLATE_OBJECTDescription);", conn);

      SqlParameter TEMPLATE_OBJECTdescriptionParameter = new SqlParameter("@TEMPLATE_OBJECTDescription", this.TEMPLATE_OBJECTdescription);
      cmd.Parameters.Add(TEMPLATE_OBJECTdescriptionParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this.Id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static List<Task> GetAll()
    {
      List<TEMPLATE_OBJECT> TEMPLATE_OBJECT_LIST = new List<TEMPLATE_OBJECT>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM TEMPLATE_OBJECTS;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int TEMPLATE_OBJECTId = rdr.GetInt32(0);
        string TEMPLATE_OBJECTDescription = rdr.GetString(1);
        TEMPLATE_OBJECT newTEMPLATE_OBJECT = new TEMPLATE_OBJECT(TEMPLATE_OBJECTDescription, taskId);
        TEMPLATE_OBJECT_LIST.Add(newTEMPLATE_OBJECT);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allTasks;
    }

    public static TEMPLATE Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM template WHERE id = @TEMPLATEId;", conn);
      SqlParameter TEMPLATEIdParameter = new SqlParameter("@TEMPLATEId", id.ToString());
      cmd.Parameters.Add(TEMPLATEIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundTEMPLATEId = 0;
      string foundTEMPLATEDescription = null;
      while(rdr.Read())
      {
        foundTEMPLATEId = rdr.GetInt32(0);
        foundTEMPLATEDescription = rdr.GetString(1);
      }
      TEMPLATE foundTEMPLATE = new TEMPLATE(foundTEMPLATEDescription, foundTEMPLATEId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return foundTEMPLATE;
    }
    public void Edit(string description)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      Console.WriteLine(this.TEMPLATEdescription);

      SqlCommand cmd = new SqlCommand("UPDATE template SET TEMPLATEdescription = @TEMPLATEdescription WHERE id = @TEMPLATEId;", conn);

      SqlParameter TEMPLATEParameter = new SqlParameter("@TEMPLATEId", this.Id);

       SqlParameter TEMPLATEdescriptionParameter = new SqlParameter("TEMPLATEdescription", description);

       cmd.Parameters.Add(TEMPLATEParameter);
       cmd.Parameters.Add(TEMPLATEdescriptionParameter);

       SqlDataReader rdr = cmd.ExecuteReader();

       while(rdr.Read())
       {
         this.Id = rdr.GetInt32(0);
         this.TEMPLATEdescription = rdr.GetString(1);
       }
       if (rdr != null)
       {
         rdr.Close();
       }
       if (conn != null)
       {
         conn.Close();
       }
     }

    public static List<TEMPLATE> Sort()
    {
      List<Task> allTEMPLATE = new List<Task>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM template ORDER BY TEMPLATEdate;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int TEMPLATEId = rdr.GetInt32(0);
        string TEMPLATEDescription = rdr.GetString(1);
        TEMPLATE newTEMPLATE = new TEMPLATE(TEMPLATEDescription, TEMPLATEId);
        allTEMPLATE.Add(newTEMPLATE);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allTEMPLATE;
    }
    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM template WHERE id = @TEMPLATEId; DELETE FROM join_table WHERE template_id = @TEMPLATEId", conn);
      SqlParameter TEMPLATEIdParameter = new SqlParameter("@TEMPLATEId", this.Id);
      cmd.Parameters.Add(TEMPLATEIdParameter);
      cmd.ExecuteNonQuery();

      if(conn!=null)
      {
        conn.Close();
      }
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM template;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

  }
}
