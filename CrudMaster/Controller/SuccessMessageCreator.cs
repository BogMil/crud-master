namespace CrudMaster.Controller
{
    public static class SuccessMessageCreator
    {
        public static object GetMessage()
        {
            return new
            {
                success = "Success message from abstract class"
            };
        }

        public static object Message(string message)
        {
            return new
            {
                success = message
            };
        }
    }
}