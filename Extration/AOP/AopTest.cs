using Castle.DynamicProxy;

namespace Extration
{
    public class AopTest : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("执行前");
            Console.WriteLine("方法名："+invocation.Method.Name);
            Console.WriteLine("参数："+invocation.Arguments);

            invocation.Proceed(); //执行

            Console.WriteLine("返回"+invocation.ReturnValue);
            Console.WriteLine("执行后");
        }
    }
}