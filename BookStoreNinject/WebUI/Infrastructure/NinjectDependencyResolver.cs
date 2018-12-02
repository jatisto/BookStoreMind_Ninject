using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Concrete;
using Domain.Entities;
using Moq;
using Ninject;

namespace WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            this.kernel = kernel;
            AddBinding();
        }

        private void AddBinding()
        {
            /*Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>()
            {
                new Book(){Name = "Чистый код", Author = "Роберт Мартин", Price = 1795},
                new Book(){Name = "Совершенный код", Author = "Стив Макконнелл", Price = 1500},
                new Book(){Name = "Язык программирования С", Author = "Брайан Керниган, Деннис Ритчи", Price = 1350}
            });
            kernel.Bind<IBookRepository>().ToConstant(mock.Object);*/
            kernel.Bind<IBookRepository>().To<EFBookRepository>();

            EmailSettings emailSettings = new EmailSettings()
            {
                WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
            };

            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>();

        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
    }
}