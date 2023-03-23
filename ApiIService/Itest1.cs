using Autofac.Extras.DynamicProxy;
using Extration;

namespace ApiIService
{
    [Intercept(typeof(AopTest))]
    public interface Itest1
    {
        void ACT();
    }
}