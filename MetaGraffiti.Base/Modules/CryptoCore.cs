using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaGraffiti.Base.Modules
{
    public class CryptoCore
    {
		public string GetNewHash() { return Guid.NewGuid().ToString("N").ToUpper(); }
	}
}
