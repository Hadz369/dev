using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;

namespace Metro.Data
{
    /// <summary>
    /// The core data class handles tables such as object, code and option
    /// </summary>
    public class Core
    {
        DbHandler _db;

        public Core(DbHandler db)
        {
            _db = db;
        }

        internal int InsertObject(int objType, int objState, int objOwner, bool isLocal )
        {
            int id = 0;

            SqlCommand cmd = _db.GetCommand(CommandType.Text, "" +
                "INSERT INTO met.Object (ObjxType, ObjxState, ObjxOwner, IsLocal, UID, MID) " +
                "VALUES (@type, @state, @owner, @islocal, @uid, @mid); " +
                "SELECT SCOPE_IDENTITY()");

            cmd.Parameters.AddWithValue("objType", objType);
            cmd.Parameters.AddWithValue("objState", objState);
            cmd.Parameters.AddWithValue("objOwner", objOwner);
            cmd.Parameters.AddWithValue("isLocal", isLocal);
            cmd.Parameters.AddWithValue("pid", _db.UID);
            cmd.Parameters.AddWithValue("mid", _db.MID);
            cmd.Parameters.AddWithValue("updated", DateTime.Now);

            object o = _db.ExecuteScalar(cmd);

            if (o != null) id = Convert.ToInt32(o);

            return id;
        }

        internal int SetObjectState(int objId, int objState)
        {
            int id = 0;

            SqlCommand cmd = _db.GetCommand(CommandType.Text, "" +
                "UPDATE met.Object " +
                "SET" +
                " objState = @objState," +
                " UID = @uid," +
                " MID = @mid" +
                " Updated = getdate() " +
                "WHERE ObjxId = @id");

            cmd.Parameters.AddWithValue("objId", objId);
            cmd.Parameters.AddWithValue("objState", objState);
            cmd.Parameters.AddWithValue("uid", _db.UID);
            cmd.Parameters.AddWithValue("mid", _db.MID);

            return id;
        }

        internal int SetObjectLocal(int objId, bool isLocal)
        {
            int id = 0;

            SqlCommand cmd = _db.GetCommand(CommandType.Text, "" +
                "UPDATE met.Object " +
                "SET" +
                " isLocal = @isLocal," +
                " UID = @uid," +
                " MID = @mid" +
                " Updated = getdate() " +
                "WHERE ObjxId = @id");

            cmd.Parameters.AddWithValue("objId", objId);
            cmd.Parameters.AddWithValue("isLocal", isLocal);
            cmd.Parameters.AddWithValue("pid", _db.UID);
            cmd.Parameters.AddWithValue("mid", _db.MID);
            cmd.Parameters.AddWithValue("updated", DateTime.Now);

            return id;
        }

        internal int InsertVenue(int venueId, string name, string desc, bool isMaster, bool isClient)
        {
            int id = 0;

            SqlCommand cmd = _db.GetCommand(CommandType.Text, "" +
                "INSERT INTO dev.Venue (VenueId, Name, Description, IsWanMaster, IsWanClient, UID, MID, Updated) " +
                "VALUES (@id, @name, @desc, @master, @client, @uid, @mid, getdate()); " +
                "SELECT SCOPE_IDENTITY()");

            cmd.Parameters.AddWithValue("id", id); 
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("desc", desc);
            cmd.Parameters.AddWithValue("master", isMaster);
            cmd.Parameters.AddWithValue("client", isClient);
            cmd.Parameters.AddWithValue("uid", _db.UID);
            cmd.Parameters.AddWithValue("mid", _db.MID);

            object o = _db.ExecuteScalar(cmd);
            
            if (o != null) id = Convert.ToInt32(o);

            return id;
        }

        internal void UpdateVenue(int venueId, string name)
        {
            SqlCommand cmd = _db.GetCommand(CommandType.Text, "" +
                "UPDATE dev.Venue " +
                "SET " +
                " VenueName    = @name," +
                " UID          = @uid," +
                " MID          = @mid," +
                " Updated      = getdate() " +
                "WHERE VenueId = @id");

            cmd.Parameters.AddWithValue("id", venueId); 
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("uid", _db.UID);
            cmd.Parameters.AddWithValue("mid", _db.MID);

            _db.ExecuteNonQuery(cmd);
        }
    }
}
