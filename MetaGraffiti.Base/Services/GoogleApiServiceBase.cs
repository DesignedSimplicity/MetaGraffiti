using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Services
{
    public class GoogleApiServiceBase
    {
		protected string _apiKey;

		public GoogleApiServiceBase(string apiKey) { _apiKey = apiKey; }
	}
}
