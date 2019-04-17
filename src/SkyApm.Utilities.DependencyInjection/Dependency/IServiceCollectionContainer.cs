#if NET_FX
namespace SkyApm.Utilities.DependencyInjection
{
    using System;

    /// <summary>
    /// 依赖注入容器
    /// </summary>
    public interface IServiceCollectionContainer : IDisposable
    {
        /// <summary>
        /// 注册实例
        /// </summary>
        /// <returns></returns>
        IServiceCollectionContainer AddSingleton<TService, TImplementation>();

        /// <summary>
        /// 注册实例
        /// </summary>
        /// <returns></returns>
        IServiceCollectionContainer AddSingleton<TService>();

        /// <summary>
        /// 注册实例
        /// </summary>
        /// <returns></returns>
        IServiceCollectionContainer AddSingleton<TService>(TService implementationType)where TService : class;

        /// <summary>
        /// 注册实例
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="resolveType"></param>
        /// <returns></returns>
        IServiceCollectionContainer AddSingletonAfterResolve<TService>(Type resolveType);


        IServiceProvider BuildServiceProvider();
    }
}
#endif