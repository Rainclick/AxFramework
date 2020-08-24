using DynamicExpresso;

namespace Common
{
    public class CodeUtil
    {
        public static T Execute<T>(string csharpCode)
        {
            var interpreter = new Interpreter();
            var result = interpreter.Eval(csharpCode);
            return (T)result;
        }
    }
}
