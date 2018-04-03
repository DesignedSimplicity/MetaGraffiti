using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Services.Internal
{
	public class BasicServiceResponse<T>
	{
		public bool OK { get; private set; } = true;

		public T Data { get; private set; }

		public string Message { get; private set; }

		public string Error { get; private set; }
		public Exception Exception { get; private set; }

		public BasicServiceResponse(bool ok, string message = "") { Message = message; OK = ok; }
		public BasicServiceResponse(T data, string message = "") { Data = data; Message = message; OK = true; }
		public BasicServiceResponse(string error) { Error = error; OK = false; }
		public BasicServiceResponse(Exception ex) { Exception = ex; Error = ex.ToString(); OK = false; }
	}
}
