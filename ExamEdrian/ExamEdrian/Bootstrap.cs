using ExamEdrian.Services;
using ExamEdrian.ViewModel;
using ExamEdrian.Views;
using MvvmAspire;
using MvvmAspire.Services;
using MvvmAspire.Unity;
using Unity;
using Unity.Lifetime;

namespace ExamEdrian
{
    public static class AppBootstrap
    {
        static UnityContainer container;

        public static UnityDependencyResolver Init(bool forBackgroundService = false)
        {
            container = new UnityContainer();
            var resolver = new UnityDependencyResolver(container);
            container.RegisterType<IDataService, DataService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISearchService, SearchService>(new ContainerControlledLifetimeManager());

            if (!forBackgroundService)
            {
                Resolver.SetResolver(resolver);
                container.RegisterInstance<INavigation>(RegisterNavigation());
                RegisterMapping();
            }
            return resolver;
        }

        public static void RegisterOverrides() => RegisterOverrides(Resolver.Current);

        public static void RegisterOverrides(IDependencyResolver resolver) => resolver.GetContainer();

        /// <summary>
        /// Registration of Navigation MVVM
        /// </summary>
        /// <returns></returns>
        public static INavigation RegisterNavigation()
        {
            var navigation = new XamarinFormsNavigation(() => App.Current);
            navigation.Register<SearchViewModel, SearchPage>();
            navigation.Register<ReserveViewModel, ReservePage>();
            return navigation;
        }

        /// <summary>
        /// Mapping DataModels
        /// </summary>
        static void RegisterMapping()
        {

        }
    }
}
