
using Autofac;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rahhal.Common.BaseEndpoint;
using Rahhal.Common.BaseHandler;
using Rahhal.Common.Views;
using Rahhal.Data.Repository;
using System.Reflection;



namespace Rahhal.Configration
{
    public class ApplicationModule : Autofac.Module
    {

        protected override void Load(ContainerBuilder builder)
        {
          

               

                #region Services Registration
                builder.RegisterType<UserState>().AsSelf().InstancePerLifetimeScope();
                builder.RegisterType<TokenHelper>().AsSelf().InstancePerLifetimeScope();
                builder.RegisterType<UserInfoProvider>().AsSelf().InstancePerLifetimeScope();
                #endregion


                #region MediatR Handlers Registration
                // Register MediatR request handlers
                builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                    .AsClosedTypesOf(typeof(IRequestHandler<,>))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();
                #endregion

                #region FluentValidation Registration
                // Register FluentValidation validators
                builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                    .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();
                #endregion

                #region Endpoint Registration
                // Register specific endpoint parameters
                builder.RegisterGeneric(typeof(BaseEndpointParameters<>))
                    .AsSelf()
                    .InstancePerLifetimeScope();
                #endregion



                #region Repository Registration
                builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
                #endregion

                builder.RegisterGeneric(typeof(BaseRequestHandlerParameter))
                     .AsSelf()
                     .InstancePerLifetimeScope();



            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
        }
    } 
}