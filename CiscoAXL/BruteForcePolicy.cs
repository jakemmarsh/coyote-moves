using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiscoAXL
{
    class BruteForcePolicy : System.Net.ICertificatePolicy
        
    {
        public bool CheckValidationResult(System.Net.ServicePoint sp, System.Security.Cryptography.X509Certificates.X509Certificate cert, System.Net.WebRequest request, int problem)
        {
            return true;
        }
    }
}
