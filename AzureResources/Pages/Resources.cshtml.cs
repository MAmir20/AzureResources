using Microsoft.AspNetCore.Mvc;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureResources.Pages
{
    public class ResourcesModel : PageModel
    {
        private readonly ArmClient _armClient;

        public ResourcesModel(ArmClient armClient)
        {
            _armClient = armClient;
        }

        public List<ResourceItem> Resources { get; private set; } = new List<ResourceItem>();

        public async Task OnGetAsync()
        {
            var subscription = await _armClient.GetDefaultSubscriptionAsync();
            if (subscription != null)
            {
                await foreach (var resource in subscription.GetGenericResourcesAsync())
                {
                    var resourceData = resource.Data;

                    Resources.Add(new ResourceItem
                    {
                        Id = resourceData.Id.ToString(),
                        Name = resourceData.Name,
                        Type = resourceData.ResourceType.ToString(),
                        Location = resourceData.Location,
                        Tags = resourceData.Tags,
                        ManagedBy = resourceData.ManagedBy,
                        Kind = resourceData.Kind
                    });
                }
            }
        }

        public class ResourceItem
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string Location { get; set; }
            public IDictionary<string, string> Tags { get; set; }
            public string ManagedBy { get; set; }
            public string Kind { get; set; }
        }
    }
}
