using System;

namespace CrudMaster.Controller
{
    public static class ErrorMessageCreator
    {
        public static object GetMessage(Exception e)
        {
            while (e.InnerException != null)
                e = e.InnerException;

            var message = e.Message;

            return new
            {
                error = "Десила се грешка:\n" + message
            };
        }

        public static string GetMessageAsString(Exception e)
        {
            while (e.InnerException != null)
                e = e.InnerException;

            var message = e.Message;

            return "Десила се грешка:\n" + message;
        }

        public static string GetMessageAsHttpsStatusCode(Exception e)
        {
            while (e.InnerException != null)
                e = e.InnerException;

            var message = e.Message;

            message =  message.Replace("\n"," ");
            message = message.Replace("\r", " ");

            return Uri.EscapeUriString(message);
        }

        public static object GetUnauthorizedMessage()
        {
            return new
            {
                error = "Нисте ауторизовани за ову акцију!"
            };
        }
    }
}