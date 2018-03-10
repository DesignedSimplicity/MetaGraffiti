using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Modules.Crypto
{
    public class CryptoGraffiti
    {
		public static string NewHashID() { return Guid.NewGuid().ToString("N").ToUpper(); }
    }
}
