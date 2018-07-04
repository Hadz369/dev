using System;
using System.Data;
using System.Data.SqlClient;
using System.ServiceModel;

namespace fnDataServer
{
    public enum MeterSetType
    {
        Daily,
        Snapshot
    }

    public class DataLayer : IDisposable
    {
        SqlConnection _con;

        public DataLayer()
        {
            _con = new SqlConnection("Server=server.flexinetsystems.com.au;Database=TimeTracker; User Id=flexinet;Password=s3cr3t;");
            _con.Open();
        }

        public bool ValidateSignOn(SignonData sdata, out ErrorMessage emsg)
        {
            bool ok = false;

            ErrorMessage e = null;

            SqlCommand cmd = new SqlCommand("select Id, Description as 'Name' from dbo.Sites where DeviceId = @id and Description = @desc", _con);
            cmd.Parameters.Add( new SqlParameter("@id", sdata.DeviceId));
            cmd.Parameters.Add(new SqlParameter("@desc", sdata.VendorName));

            using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow))
            {
                while (dr.Read())
                {
                    if (dr.GetInt32(0) == sdata.DeviceId)
                        if (String.Compare(dr.GetString(1).Trim(), sdata.VendorName.Trim(), true) == 0)
                            ok = true;
    
                    if (!ok) 
                        e = new ErrorMessage(2, "The site name recorded in the server differs from the name provided");

                    break;
                }
            }

            if (!ok && e == null) e = new ErrorMessage(1, "Site not found for Id=" + sdata.DeviceId.ToString());

            emsg = e;

            return ok;
        }

        public DataTable GetPeople()
        {
            SqlCommand c = new SqlCommand("select * from dbo.People", _con);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(c);
            da.Fill(dt);

            return dt;
        }

        public void Dispose()
        {
            if (_con != null && _con.State != ConnectionState.Closed)
                _con.Close();

            _con = null;
        }
    }
}