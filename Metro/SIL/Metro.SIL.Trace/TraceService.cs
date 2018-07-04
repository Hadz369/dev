using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;
using System.ServiceModel.Activation;

using System.Net;
using Metro;

namespace RestServer
{
    public class TraceService : ITraceContract
    {
        private void Unauthorised()
        {
            FaultData fd = new FaultData(Metro.FaultCode.Unauthorised, "Attempt to access unauthorised method");
            WebFaultException<FaultData> ex = new WebFaultException<FaultData>(fd, HttpStatusCode.Unauthorized);
            throw ex;
        }

        public Response UpdateAccount(string sessionKey, AccountTransaction trans)
        {
            Response sr = new Response();

            if (Biz.Instance.ValidateSession(sessionKey))
            {
                try
                {
                    AccountUpdateType t = (AccountUpdateType)Enum.Parse(typeof(AccountUpdateType), trans.ChangeType, true);
                    sr = Biz.Instance.UpdatePoints(trans.MemberId, t, trans.Amount);
                }
                catch (Exception ex)
                {
                    sr.ReturnCode = -1;
                    sr.ErrorMessage = String.Format("Error updating account: Msg={0}", ex.Message);
                }
            }
            else Unauthorised();

            return sr;
        }

        public Response SignOn(string vendor, string device)
        {
            Response sr = new Response();

            string key = Biz.Instance.SignOn(vendor, device);

            if (key != String.Empty)
            {
                sr.Data = key;
            }
            else
            {
                FaultData fd = new FaultData(Metro.FaultCode.Unauthorised, "Invalid credentials supplied to the signon method");
                WebFaultException<FaultData> ex = new WebFaultException<FaultData>(fd, HttpStatusCode.Unauthorized);
                throw ex;
            }

            return sr;
        }

        public Response SignOff(string sessionkey)
        {
            Response sr = new Response();

            if (Biz.Instance.ValidateSession(sessionkey))
            {
                Biz.Instance.SignOff(sessionkey);
            }
            else
            {
                sr.ReturnCode = TpServiceRC.InvalidSessionKey;
                sr.ErrorMessage = String.Concat("Invalid SessionID: ", sessionkey);
            }

            return sr;
        }

        public Response GetData(string sessionkey, string method)
        {
            return GetData(sessionkey, method, "");
        }

        public Response GetData(string sessionkey, string method, string parms)
        {
            Response sr = new Response();

            if (Biz.Instance.ValidateSession(sessionkey))
            {
                try
                {
                    switch (method.ToLower())
                    {
                        case "member":
                            sr = GetMember(parms);
                            break;
                        case "members":
                            sr = GetMembers(parms);
                            break;
                        case "memberatlocation":
                            sr = GetMemberAtLocation(parms);
                            break;
                        case "tier":
                            sr = GetTier(parms);
                            break;
                        case "tiers":
                            sr = GetTiers();
                            break;
                        case "machines":
                            sr = GetMachines();
                            break;
                        case "machinestats":
                            sr = GetCurrentMachineStats(parms);
                            break;
                        default:
                            sr.ReturnCode = -1;
                            sr.ErrorMessage = String.Concat("Unknown method: ", method);
                            break;
                    }
                }
                catch (SqlException ex)
                {
                    sr.ReturnCode = TpServiceRC.SqlError;
                    sr.ErrorMessage = ex.Message;
                }
                catch (Exception ex)
                {
                    sr.ReturnCode = TpServiceRC.UnhandledError;
                    sr.ErrorMessage = ex.Message;
                }
            }
            else Unauthorised();

            return sr;
        }

        private Response GetMember(string badgeNo)
        {
            Response sr = new Response();

            Member member = Biz.Instance.GetMember(badgeNo);

            if (member == null)
            {
                sr.ErrorMessage = "No records found";
                sr.ReturnCode = TpServiceRC.NoRecordsFound;
            }
            else
            {
                sr.ResponseData = member;
            }

            return sr;
        }

        private Response GetMembers(string surname)
        {
            Response sr = new Response();

            MemberList members = Biz.Instance.GetMembers(surname);

            if (members == null)
            {
                sr.ErrorMessage = "No records found";
                sr.ReturnCode = TpServiceRC.NoRecordsFound;
            }
            else
            {
                sr.ResponseData = members;
            }

            return sr;
        }

        private Response GetMemberAtLocation(string baseNo)
        {
            Response sr = new Response();

            Member member = Biz.Instance.GetMemberAtLocation(baseNo);

            if (member == null)
            {
                sr.ErrorMessage = "No records found";
                sr.ReturnCode = TpServiceRC.NoRecordsFound;
            }
            else
            {
                sr.ResponseData = member;
            }

            return sr;
        }

        private Response GetTier(string parms)
        {
            int tierid;
            Response sr = new Response();

            if (Int32.TryParse(parms, out tierid))
            {
                Tier tier = Biz.Instance.GetTier(tierid);

                if (tier == null)
                {
                    sr.ErrorMessage = "No records found";
                    sr.ReturnCode = TpServiceRC.NoRecordsFound;
                }
                else
                {
                    sr.ResponseData = tier;
                }
            }
            else
            {
                sr.ErrorMessage = String.Concat("Invalid parameter: ", parms);
                sr.ReturnCode = TpServiceRC.InvalidParameter;
            }

            return sr;
        }

        private Response GetTiers()
        {
            Response sr = new Response();

            TierList tiers = Biz.Instance.GetTiers();

            if (tiers == null)
            {
                sr.ErrorMessage = "No records found";
                sr.ReturnCode = TpServiceRC.NoRecordsFound;
            }
            else
            {
                sr.ResponseData = tiers;
            }

            return sr;
        }

        private Response GetMachines()
        {
            Response sr = new Response();

            MachineList list = Biz.Instance.GetMachines();

            if (list == null)
            {
                sr.ErrorMessage = "No records found";
                sr.ReturnCode = TpServiceRC.NoRecordsFound;
            }
            else
            {
                sr.ResponseData = list;
            }

            return sr;
        }

        private Response GetCurrentMachineStats(string machineId)
        {
            Response sr = new Response();

            int id;

            if (Int32.TryParse(machineId, out id))
            {
                MachineStats stats = Biz.Instance.GetCurrentMachineStats(id);

                if (stats == null)
                {
                    sr.ErrorMessage = "No records found";
                    sr.ReturnCode = TpServiceRC.NoRecordsFound;
                }
                else
                {
                    sr.ResponseData = stats;
                }
            }
            else
            {
                sr.ErrorMessage = String.Concat("Invalid parameter: ", machineId);
                sr.ReturnCode = TpServiceRC.InvalidParameter;
            }

            return sr;
        }

        public Response Test(string sessionkey)
        {
            Response sr = new Response();

            MachineList list = Biz.Instance.GetMachines();

            if (list == null)
            {
                sr.ErrorMessage = "No records found";
                sr.ReturnCode = TpServiceRC.NoRecordsFound;
            }
            else
            {
                sr.ResponseData = list;
            }

            return sr;
        }

        public Response GetSessionInfo(string sessionkey)
        {
            Response sr = new Response();

            SessionCollection list = Biz.Instance.GetSessions();
            sr.ResponseData = list;

            return sr;
        }
    }
}
