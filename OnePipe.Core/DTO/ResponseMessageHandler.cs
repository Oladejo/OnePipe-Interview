using System.Collections.Generic;

namespace OnePipe.Core.DTO
{
    public class ResponseMessageHandler
    {
        public ResponseMessageHandler()
        {
            ErrorMessages = new List<string>();
        }
        public string status { get; set; }


        public string UserId { get; set; }

        public List<string> ErrorMessages { get; set; }

        public object data { get; set; }
    }
}
