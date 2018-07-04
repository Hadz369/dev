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

namespace RestServer
{
    public class TpService : IMemberData
    {
        private void Unauthorised()
        {
            FaultData fd = new FaultData(TpServiceRC.Unauthorised, "Attempt to access unauthorised method");
            WebFaultException<FaultData> ex = new WebFaultException<FaultData>(fd, HttpStatusCode.Unauthorized);
            throw ex;
        }

        public ServiceResponse UpdateAccount(string sessionKey, AccountTransaction trans)
        {
            ServiceResponse sr = new ServiceResponse();

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

        public ServiceResponse GetMyUserAgent(string sessionkey)
        {
            ServiceResponse rc = new ServiceResponse();

            Dictionary<string, string> list = new Dictionary<string, string>();

            try
            {
                HttpRequestMessageProperty prop;
                prop = (HttpRequestMessageProperty)OperationContext.Current.IncomingMessageProperties[HttpRequestMessageProperty.Name];
                var referer = prop.Headers[HttpRequestHeader.Referer];


                string accept = WebOperationContext.Current.IncomingRequest.Accept;
                string userAgentValue = WebOperationContext.Current.IncomingRequest.UserAgent;
                UserAgentInfo userAgent = new UserAgentInfo(userAgentValue);

                foreach (string s in prop.Headers.AllKeys)
                    list.Add(s, prop.Headers[s]);

                rc.ResponseData = list;

            }
            catch (Exception ex)
            {
                rc.ErrorMessage = ex.Message;
                rc.ReturnCode = 2765;
            }

            return rc;
        }

        public ServiceResponse SignOn(string vendor, string device)
        {
            ServiceResponse sr = new ServiceResponse();

            string key = Biz.Instance.SignOn(vendor, device);

            if (key != String.Empty)
            {
                sr.ResponseData = key;
            }
            else
            {
                FaultData fd = new FaultData(TpServiceRC.InvalidCredentials, "Invalid credentials supplied to the signon method");
                WebFaultException<FaultData> ex = new WebFaultException<FaultData>(fd, HttpStatusCode.Unauthorized);
                throw ex;
            }

            return sr;
        }

        public ServiceResponse SignOff(string sessionkey)
        {
            ServiceResponse sr = new ServiceResponse();

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

        public ServiceResponse GetData(string sessionkey, string method)
        {
            return GetData(sessionkey, method, "");
        }

        public ServiceResponse GetData(string sessionkey, string method, string parms)
        {
            ServiceResponse sr = new ServiceResponse();

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

        private ServiceResponse GetMember(string badgeNo)
        {
            ServiceResponse sr = new ServiceResponse();

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

        private ServiceResponse GetMembers(string surname)
        {
            ServiceResponse sr = new ServiceResponse();

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

        private ServiceResponse GetMemberAtLocation(string baseNo)
        {
            ServiceResponse sr = new ServiceResponse();

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

        private ServiceResponse GetTier(string parms)
        {
            int tierid;
            ServiceResponse sr = new ServiceResponse();

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

        private ServiceResponse GetTiers()
        {
            ServiceResponse sr = new ServiceResponse();

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

        private ServiceResponse GetMachines()
        {
            ServiceResponse sr = new ServiceResponse();

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

        private ServiceResponse GetCurrentMachineStats(string machineId)
        {
            ServiceResponse sr = new ServiceResponse();

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

        public ServiceResponse Test(string sessionkey)
        {
            ServiceResponse sr = new ServiceResponse();

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

        public ServiceResponse GetSessionInfo(string sessionkey)
        {
            ServiceResponse sr = new ServiceResponse();

            SessionCollection list = Biz.Instance.GetSessions();
            sr.ResponseData = list;

            return sr;
        }
    }
}
