using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace WpfSearchControls
{
    public class Data
    {
        #region Initialisation

        SqlConnection _con;

        public Data(string conString)
        {
            _con = new SqlConnection(conString);
            _con.Open();
        }

        #endregion

        #region Public Methods

        public int? GetDefaultCountry()
        {
            SqlCommand cmd;

            cmd = new SqlCommand("" +
                "select CountryPk from css.PostalCountry " +
                "where country = @name", _con);

            cmd.Parameters.AddWithValue("@name", "Australia");
            
            object o = cmd.ExecuteScalar();

            if (o != null)
                return (int)o;
            else
                return null;
        }

        public int? GetDefaultProvince(int? countryId)
        {
            SqlCommand cmd = new SqlCommand("" +
                "select ProvincePk from css.PostalProvince " +
                "where CountryId = @id and Province = @name", _con);

            cmd.Parameters.AddWithValue("@id", countryId);
            cmd.Parameters.AddWithValue("@name", "New South Wales");

            object o = cmd.ExecuteScalar();

            if (o != null)
                return (int)o;
            else
                return null;
        }

        public DataTable GetCountries()
        {
            string sql1 = "" +
                "select CountryPk, Country " +
                "from css.PostalCountry " +
                "order by 2";

            SqlCommand cmd = new SqlCommand(sql1, _con);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            return dt;
        }

        public DataTable GetProvinces(int countryId)
        {
            string sql1 = "" +
                "select ProvincePk, Province " +
                "from css.PostalProvince " +
                "where CountryId = @id " +
                "order by 2";

            SqlCommand cmd = new SqlCommand(sql1, _con);
            cmd.Parameters.Add(new SqlParameter("@id", countryId));

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            return dt;
        }

        public DataTable GetDistricts(int provinceId)
        {
            string sql1 = "" +
                "select DistrictPk, Suburb, PostCode " +
                "from css.PostalDistrict " +
                "where ProvinceId = @id " +
                "order by 2";

            SqlCommand cmd = new SqlCommand(sql1, _con);
            cmd.Parameters.Add(new SqlParameter("@id", provinceId));

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            return dt;
        }

        public DataTable GetAddressTypes()
        {
            string sql1 = "" +
                "select CodeSuidPk, CodeDefn.ToString() as 'Value' " +
                "from obj.Code  " +
                "where codetype = obj.GetTypeSuid(@type) " +
                "order by sequence";

            SqlCommand cmd = new SqlCommand(sql1, _con);
            cmd.Parameters.AddWithValue("@type", "ADDRESSTYPE");

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            return dt;
        }

        public DataTable GetStreetTypes()
        {
            string sql1 = "" +
                "select StreetPk, Name, Abbreviation " +
                "from css.PostalStreet " +
                "order by Name";

            SqlCommand cmd = new SqlCommand(sql1, _con);

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            return dt;
        }

        #endregion
    }
}
