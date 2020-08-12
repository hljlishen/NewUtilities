﻿using Unity;
using Unity.Interception;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.Interceptors.TypeInterceptors.VirtualMethodInterception;
using Unity.Lifetime;
using Utilities.IniBind.InterfaceInterception;
using Utilities.IniBind.AttributeInterception;

namespace Utilities.IniBind
{
    public class IniFileObjectFactory<T>
    {
        private static readonly IUnityContainer container;
        static IniFileObjectFactory()
        {
            container = new UnityContainer();
            container.AddNewExtension<Interception>();
        }

        public static T CreateObject()
        {
            if (typeof(IIniBindInterface).IsAssignableFrom(typeof(T)))
            {
                if (!container.IsRegistered<T>())
                {
                    container.RegisterType<T>(new SingletonLifetimeManager(), new Interceptor<VirtualMethodInterceptor>(), new InterceptionBehavior<BindIniBehavior>());
                    container.RegisterType<BindStrategy, InterfaceStrategy>();
                }
            }
            else
            {
                if (!container.IsRegistered<T>())
                {
                    container.RegisterType<T>(new SingletonLifetimeManager(), new Interceptor<VirtualMethodInterceptor>(), new InterceptionBehavior<BindIniBehavior>());
                    container.RegisterType<BindStrategy, AttributeStrategy>();
                }
            }
            var obj = container.Resolve<T>();
            CallAPropertyGetter(obj, container.Resolve<BindStrategy>());
            return obj;
        }
        private static void CallAPropertyGetter(T ob, BindStrategy strategy)
        {
            var ps = ob.GetType().GetProperties();
            foreach (var p in ps)
            {
                if (strategy.IsIniBindProperty(p))
                {
                    _ = p.GetValue(ob);
                    return;
                }
            }
            throw new System.Exception("类型中没有与Ini文件绑定的字段");
        }
    }
}