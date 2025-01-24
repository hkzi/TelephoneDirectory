using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Http;
using TelephoneDirectory.Business.Abstract;
using TelephoneDirectory.Business.BusinessAspects.Autofac;
using TelephoneDirectory.Business.Concrete;
using TelephoneDirectory.Core.Utilities.Interceptors;
using TelephoneDirectory.Core.Utilities.Security.JWT;
using TelephoneDirectory.DataAccess.Abstract;
using TelephoneDirectory.DataAccess.Concrete;


namespace TelephoneDirectory.Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PersonManager>().As<IPersonService>().SingleInstance();
            builder.RegisterType<PersonDal>().As<IPersonDal>().SingleInstance();

            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<UserDal>().As<IUserDal>().SingleInstance();
            builder.RegisterType<UserOperationClaimManager>().As<IUserOperationClaimService>().SingleInstance();
            builder.RegisterType<UserOperationClaimDal>().As<IUserOperationClaimDal>().SingleInstance();
            builder.RegisterType<OperationClaimManager>().As<IOperationClaimService>().SingleInstance();
            builder.RegisterType<OperationClaimDal>().As<IOperationClaimDal>().SingleInstance();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>();
            builder.RegisterType<SecuredOperation>().As<IInterceptor>();
            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();
            builder.RegisterType<PhoneContext>().AsSelf().InstancePerLifetimeScope();
            



            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions { Selector = new AspectInterceptorSelector() })
                .SingleInstance();
          
        }
    }
}
