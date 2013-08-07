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

        public SqlToFormModelFactory(SqlDataReader reader)
        {
            this.reader = reader;
        }

        public RequestForm GetRequest(Guid requestID)
        {
            return null;
        }

        private CoyoteMovesFormEmployeeInfo MakeCurrentInfo()
        {
            return null;

        }

        private CoyoteMovesFormEmployeeInfo MakeFutureInfo()
        {
            return null;

        }

        private BazookaInfo MakeBazInfo()
        {
            return null;

        }

        private DeskInfo MakeDeskInfo()
        {
            return null;

        }

        private PhoneInfo MakePhoneInfo()
        {
            return null;

        }

        private UltiproInfo MakeUltiProInfo()
        {
            return null;

        }

        private EmailDistributionInfo MakeEmailInfo()
        {
            return null;

        }

        private EmployeeReviewFileInfo MakeReviewInfo()
        {
            return null;

        }

        private int GetCreateID()
        {
            return 0;

        }

        private int GetEmployeeID()
        {
            return 0;

        }
    }
}