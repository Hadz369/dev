using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace RestServer
{
    public class Data : IDisposable
    {
        string _conString;
        SqlConnection _con;
        Dictionary<string, string> _options = new Dictionary<string, string>();

        public Data(string conString)
        {
            _conString = conString;
            _con = new SqlConnection(conString);
        }

        private SqlConnection Connect()
        {
            //SqlConnection con = new SqlConnection(_conString);
            if (_con.State != ConnectionState.Open)
                _con.Open();

            return _con;
        }

        /// <summary>
        /// Gets the first valid member found with a given badge number.
        /// </summary>
        /// <param name="badgeNo"></param>
        /// <returns></returns>
        public Member GetMember(string badgeNo)
        {
            Member member = null;

            using (SqlCommand cmd = new SqlCommand() { Connection = Connect() })
            {
                cmd.CommandText = "" +
                    "select" +
                    "  MBAI, MEMBER, FNAME, LNAME, BDAY, VALIDTO, CAST(POINTS as DECIMAL(18,3))/1000, STATUS, PREV " +
                    "from dbo.Members " +
                    "where Member = @mem";

                cmd.Parameters.AddWithValue("@mem", badgeNo);

                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        member = new Member(r);
                        break;
                    }
                }
            }

            return member;
        }

        /// <summary>
        /// Gets the id of the member at device location
        /// </summary>
        /// <param name="badgeNo"></param>
        /// <returns></returns>
        public Member GetMemberAtLocation(string baseNo)
        {
            Member member = null;

            using (SqlCommand cmd = new SqlCommand() { Connection = Connect() })
            {
                cmd.CommandText = "" +
                    "select" +
                    "  MEMB.MBAI, MEMB.MEMBER, MEMB.FNAME, MEMB.LNAME, MEMB.BDAY, MEMB.VALIDTO, CAST(MEMB.POINTS as DECIMAL(18,3))/1000, MEMB.STATUS, MEMB.PREV " +
                    "from dbo.Members MEMB " +
                    "inner join dbo.CurrentCards CC on MEMB.MBAI = CC.MemberID " +
                    "inner join dbo.Machines MACH ON CC.DeviceID = MACH.DeviceID " +
                    "WHERE MACH.BaseNo = @baseno";

                cmd.Parameters.AddWithValue("@baseno", baseNo);

                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        member = new Member(r);
                        break;
                    }
                }
            }

            return member;
        }

        public MemberList GetMembers(string surname)
        {
            MemberList members = null;

            using (SqlCommand cmd = new SqlCommand() { Connection = Connect() })
            {
                cmd.CommandText = String.Format("" +
                    "select" +
                    " MBAI, MEMBER, FNAME, LNAME, BDAY, VALIDTO, CAST(POINTS as DECIMAL(18,3))/1000, STATUS, PREV " +
                    "from dbo.Members " +
                    "where LNAME LIKE('%{0}%')", surname);

                Console.WriteLine(cmd.CommandText);

                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        if (members == null) members = new MemberList();

                        Member member = new Member(r);
                        members.Add(member);
                    }
                }
            }

            return members;
        }

        public Tier GetTier(int tierId)
        {
            Tier tier = null;

            using (SqlCommand cmd = new SqlCommand() { Connection = Connect() })
            {
                cmd.CommandText = "" +
                    "select [TierID], [Name], [Description], [Factor], [PostFactor], [Colour] " +
                    "from dbo.MemberTiers " +
                    "where TierId = @tier";

                cmd.Parameters.AddWithValue("@tier", tierId);

                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        tier = new Tier(r);
                        break;
                    }
                }
            }

            return tier;
        }

        public TierList GetTiers()
        {
            TierList list = null;

            using (SqlCommand cmd = new SqlCommand() { Connection = Connect() })
            {
                cmd.CommandText = "" +
                    "select [TierID], [Name], [Description], [Factor], [PostFactor], [Colour] " +
                    "from dbo.MemberTiers";

                Console.WriteLine(cmd.CommandText);

                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        if (list == null) list = new TierList();

                        Tier tier = new Tier(r);
                        list.Add(tier);
                    }
                }
            }

            return list;
        }

        public MachineList GetMachines()
        {
            MachineList list = null;

            using (SqlCommand cmd = new SqlCommand() { Connection = Connect() })
            {
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "exec dbo.tpGetMachineList";

                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        // Create the list when the first row is loaded
                        if (list == null) list = new MachineList();

                        list.Add(new Machine(r));
                    }
                }
            }

            return list;
        }

        public MachineStats GetCurrentMachineStats(int machineId)
        {
            MachineStats stats = null;

            using (SqlCommand cmd = new SqlCommand() { Connection = Connect() })
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandText = "dbo.tpGetCurrentMachineStats";
                cmd.Parameters.AddWithValue("@DeviceId", machineId);

                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        stats = new MachineStats(r);
                        break;
                    }
                }
            }

            return stats;
        }

        public void UpdatePoints(int memberId, AccountUpdateType updType, int amount)
        {
            string utype;

            if (updType == AccountUpdateType.Earn) utype = "EARNED";
            else if (updType == AccountUpdateType.Issue) utype = "ISSUED";
            else utype = "CLAIMED";

            using (SqlCommand cmd = new SqlCommand() { Connection = Connect() })
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "pro.AccountUpdate";

                cmd.Parameters.AddWithValue("@PersonId", memberId);
                cmd.Parameters.AddWithValue("@PrizeId", 6);
                cmd.Parameters.AddWithValue("@PromName", "TpMemberService");
                cmd.Parameters.AddWithValue("@ChgName", utype);
                cmd.Parameters.AddWithValue("@ChgAmt", amount);

                cmd.ExecuteNonQuery();
            }
        }

        public string GetOption(string path, string name)
        {
            string key = String.Concat(path, ":", name);
            string option = "";

            if (_options.ContainsKey(key))
            {
                option = _options[key];
            }
            else
            {
                using (SqlCommand cmd = new SqlCommand() { Connection = Connect() })
                {
                    cmd.CommandText = String.Format("select ValueDefault from [css].[udf_GetOption]('{0}', '{1}')", path, name);

                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            option = r.GetString(0);
                            _options.Add(key, option);
                            break;
                        }
                    }
                }
            }

            return option;
        }

        public void Dispose()
        {
            if (_con.State != ConnectionState.Closed) _con.Close();
            _con.Dispose();

            _options = null;
        }
    }
}
