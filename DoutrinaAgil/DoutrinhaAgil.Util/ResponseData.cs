using System;
using DoutrinaAgil.Util.Enums;

namespace DoutrinaAgil.Util
{
    public class ResponseData
    {
        public string Message { get; set; }
        public object Data { get; set; }
        public EResponse Response { get; set; }

        public static ResponseData GetResponseSuccess(string message = null)
        {
            return new ResponseData
            {
                Response = EResponse.Success,
                Message = message
            };
        }
        public static ResponseData GetResponseSuccess(object data, string message = null)
        {
            return new ResponseData
            {
                Response = EResponse.Success,
                Message = message,
                Data = data
            };
        }
        public static ResponseData GetResponseError(string message)
        {
            return new ResponseData
            {
                Response = EResponse.Error,
                Message = message
            };
        }

        public T Cast<T>()
        {
            return (T)Convert.ChangeType(Data, typeof(T));
        }
    }
}
