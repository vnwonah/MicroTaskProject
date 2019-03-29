using System;
namespace MT_NetCore_API.Interfaces
{
    public interface ITestService
    {
        string GetData();
    }

    public class TestService : ITestService
    {
        public string GetData()
        {
            return "some magic string";
        }
    }
}
