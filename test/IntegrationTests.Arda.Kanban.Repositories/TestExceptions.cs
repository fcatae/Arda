using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public class TestFileNotFoundException : Exception
    {
        public TestFileNotFoundException(string message) : base(message)
        {
        }
    }

    public class FailedTestException : Exception
    {
        private string _expectedText;
        private string _message;
        private object _resultObject;
        private string _resultText;

        public FailedTestException(string message, object resultObject, string resultText, string expectedText) : base(message)
        {
            this._message = message;
            this._resultObject = resultObject;
            this._resultText = resultText;
            this._expectedText = expectedText;
        }
    }

}
