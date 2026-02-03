using System.Collections.Generic;
using BlazorDemo.Data;

namespace BlazorDemo.DataProviders.Implementation {
    public class MapApiKeyProvider : IMapApiKeyProvider {
        public string GetBingProviderKey() => "YOUR_BING_MAPS_API_KEY_HERE";
        public string GetAzureProviderKey() => "YOUR_AZURE_MAPS_API_KEY_HERE";
    }
}
