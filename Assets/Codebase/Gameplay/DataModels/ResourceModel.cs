using System.Collections.Generic;
using ACS.Data.DataService.Model;
using Codebase.Gameplay.Resources;
using Newtonsoft.Json;

namespace Codebase.Gameplay.DataModels
{
    [UnityEngine.Scripting.Preserve]
    public class ResourceModel : ProgressModel
    {
        [JsonProperty] private Dictionary<GameplayResourceType, long> _resources = new Dictionary<GameplayResourceType, long>();

        public long Get(GameplayResourceType type) => _resources.ContainsKey(type) ? _resources[type] : 0;

        public void Set(GameplayResourceType type, long amount)
        {
            if (!_resources.ContainsKey(type)) _resources.Add(type, amount);
            else _resources[type] = amount;
            
            MarkAsDirty();
        }
    }
}
