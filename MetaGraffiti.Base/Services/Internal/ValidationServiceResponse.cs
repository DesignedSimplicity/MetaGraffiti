using System;
using System.Collections.Generic;
using System.Text;

namespace MetaGraffiti.Base.Services.Internal
{
	public class ValidationServiceResponse<T>
	{
		public bool OK { get { return !HasErrors; } }

		public bool HasErrors { get { return ValidationErrors.Count > 0; } }

		public T Data { get; private set; }

		public List<ValidationServiceResponseError> ValidationErrors { get; private set; } = new List<ValidationServiceResponseError>();

		public ValidationServiceResponse(T data) { Data = data; }

		public void AddError(string field, string message) { ValidationErrors.Add(new ValidationServiceResponseError() { Field = field, Message = message }); }
	}

	public class ValidationServiceResponseError
	{
		public string Field { get; set; }
		public string Message { get; set; }
	}
}
