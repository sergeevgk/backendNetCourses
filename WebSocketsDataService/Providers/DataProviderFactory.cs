using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WebSocketsDataService.Providers
{
    public class DataProviderFactory : IDataProviderFactory
    {
        public IDataProvider Create(Type type)
        {
            var availableProviderTypes = Assembly.GetAssembly(GetType()).GetTypes()
                .Where(t => t.IsAssignableTo(typeof(IDataProvider)) && !t.IsAbstract);

            var providerType = availableProviderTypes.FirstOrDefault(t => t.Name == $"{type.Name}Provider");
            if (providerType == null)
                throw new Exception("Required provider type cannot be instantiated");

            return Activator.CreateInstance(providerType) as IDataProvider;
        }
    }

}
