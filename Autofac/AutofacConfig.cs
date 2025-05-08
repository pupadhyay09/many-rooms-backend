using Autofac;
using ManyRoomStudio.Gateways;
using ManyRoomStudio.Infrastructure.RazorPartial;
using ManyRoomStudio.UseCases;
using System.Reflection;
using Module = Autofac.Module;

namespace ManyRoomStudio.Autofac
{
    public class AutofacConfig : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var webAssembly = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(typeof(UserGateway).Assembly)
                  .Where(t => t.Name.EndsWith("Gateway"))
                  .AsImplementedInterfaces()
                  .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(UserBookingUsecase).Assembly)
                   .Where(t => t.Name.EndsWith("Usecase"))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterType<RazorPartialToString>()
                   .As<IRazorPartialToString>()
                   .SingleInstance();

            builder.Register(c => new HttpContextAccessor())
                   .As<IHttpContextAccessor>()
                   .SingleInstance();
        }
    }
}
