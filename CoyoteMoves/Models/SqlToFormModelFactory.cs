using CoyoteMoves.Data_Access;
using CoyoteMoves.Models.RequestItems;
using CoyoteMoves.Models.RequestItems.RequestTypes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CoyoteMoves.Models
{
    public class SqlToFormModelFactory
    {
        public SqlDataReader reader;
        private EmployeeDB _edb = new EmployeeDB();
        private string _connectionString;

        public SqlToFormModelFactory(SqlDataReader reader)
        {
            this.reader = reader;
            this._connectionString = (string)System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DataClientRead"].ConnectionString;
        }

        public RequestForm GetRequest(Guid requestID)
        {
            if (this.reader == null)
            {
                return null;
            }

            RequestForm temp = null;
            while (this.reader.Read())
            {
                temp =  new RequestForm()
                {
                    CreatedByID = GetCreateByID(),
                    Current = MakeCurrentInfo(),
                    Future = MakeFutureInfo(),
                    EmailInfo = MakeEmailInfo(),
                    EmployeeId = GetEmployeeID(),
                    ReviewInfo = MakeReviewInfo(),
                    UniqueId = requestID,
                };
                break;
            }
            return temp;
        }

        private CoyoteMovesFormEmployeeInfo MakeCurrentInfo()
        {
            return new CoyoteMovesFormEmployeeInfo()
            {
                BazookaInfo = MakeCurrentBazInfo(),
                DeskInfo = MakeCurrentDeskInfo(),
                PhoneInfo = MakeCurrentPhoneInfo(),
                UltiproInfo = MakeCurrentUltiProInfo()
            };
        }

        private CoyoteMovesFormEmployeeInfo MakeFutureInfo()
        {
            return new CoyoteMovesFormEmployeeInfo()
            {
                BazookaInfo = MakeFutureBazInfo(),
                DeskInfo = MakeFutureDeskInfo(),
                PhoneInfo = MakeFuturePhoneInfo(),
                UltiproInfo = MakeFutureUltiProInfo()
            };
        }

        private BazookaInfo MakeCurrentBazInfo()
        {
            return new BazookaInfo()
            {
                Department = reader["C_Department"].ToString(),
                Group = reader["C_Group"].ToString(),
                JobTemplate = reader["C_JobTemplate"].ToString(),
                JobTitle = reader["C_JobTitle"].ToString(),
                ManagerID = (int)reader["C_ManagerID"],
                SecurityItemRights = reader["C_SecurityItemRights"].ToString()
            };
        }

        private BazookaInfo MakeFutureBazInfo()
        {
            return new BazookaInfo()
            {
                Department = reader["F_Department"].ToString(),
                Group = reader["F_Group"].ToString(),
                JobTemplate = reader["F_JobTemplate"].ToString(),
                JobTitle = reader["F_JobTitle"].ToString(),
                ManagerID = (int)reader["F_ManagerID"],
                SecurityItemRights = reader["F_SecurityItemRights"].ToString()
            };
        }

        private DeskInfo MakeCurrentDeskInfo()
        {
            return new DeskInfo()
            {
                DeskNumber = reader["C_DeskNumber"].ToString(),
                Office = reader["C_Office"].ToString()
            };
        }

        private DeskInfo MakeFutureDeskInfo()
        {
            return new DeskInfo()
            {
                DeskNumber = reader["F_DeskNumber"].ToString(),
                Office = reader["F_Office"].ToString()
            };
        }

        private PhoneInfo MakeCurrentPhoneInfo()
        {
            return new PhoneInfo()
            {
                PhoneNumber = reader["C_PhoneNumber"].ToString()
            };
        }

        private PhoneInfo MakeFuturePhoneInfo()
        {
            return new PhoneInfo()
            {
                PhoneNumber = reader["F_PhoneNumber"].ToString()
            };
        }

        private UltiproInfo MakeCurrentUltiProInfo()
        {
            return new UltiproInfo()
            {
                Department = reader["C_Department"].ToString(),
                JobTitle = reader["C_JobTitle"].ToString(),
                Supervisor = _edb.GetFullNameById((int)reader["C_ManagerID"]),
                Other = reader["C_Other"].ToString() 
            };
        }

        private UltiproInfo MakeFutureUltiProInfo()
        {
            return new UltiproInfo()
            {
                Department = reader["F_Department"].ToString(),
                JobTitle = reader["F_JobTitle"].ToString(),
                Supervisor = _edb.GetFullNameById((int)reader["F_ManagerID"]),
                Other = reader["F_Other"].ToString()
            };
        }

        private EmailDistributionInfo MakeEmailInfo()
        {
            return new EmailDistributionInfo()
            {
                GroupsToBeAddedTo = reader["GroupsToBeAddedTo"].ToString(),
                GroupsToBeRemovedFrom = reader["GroupsToBeRemovedFrom"].ToString()
            };
        }

        private EmployeeReviewFileInfo MakeReviewInfo()
        {
            return new EmployeeReviewFileInfo()
            {
                FilesToBeAddedTo = reader["FilesToBeAddedTo"].ToString(),
                FilesToBeRemovedFrom = reader["FilesToBeRemovedFrom"].ToString()
            };
        }

        private int GetEmployeeID()
        {
            return (int)reader["EmployeeID"];
        }

        private int GetCreateByID()
        {
            return (int)reader["CreateByID"];
        }
    }
}