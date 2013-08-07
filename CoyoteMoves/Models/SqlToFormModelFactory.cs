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

        //public GetRequest(Guid requestID)
        //{

        //}

        //private CoyoteMovesFormEmployeeInfo MakeCurrentInfo()
        //{

        //}

        //private CoyoteMovesFormEmployeeInfo MakeFutureInfo()
        //{

        //}

        //private BazookaInfo MakeBazInfo()
        //{

        //}

        //private DeskInfo MakeDeskInfo()
        //{

        //}

        //private PhoneInfo MakePhoneInfo()
        //{

        //}

        //private UltiproInfo MakeUltiProInfo()
        //{

        //}

        //private EmailDistributionInfo MakeEmailInfo()
        //{

        //}

        //private EmployeeReviewFileInfo MakeReviewInfo()
        //{

        //}

        //private int GetCreateID()
        //{

        //}

        //private int GetEmployeeID()
        //{

        //}
    }
}