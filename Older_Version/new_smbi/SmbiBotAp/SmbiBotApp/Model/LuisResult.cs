using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Microsoft.Rest;
using Microsoft.Rest.Serialization;
using Microsoft.Bot.Builder.Luis.Models;

namespace SmbiBotApp.Model
{
    public partial class LuisResult
    {
           public LuisResult() { }

           public LuisResult(string query, IList<IntentRecommendation> intents, IList<EntityRecommendation> entities)
           {
            Query = query;
            this.Intents = intents;
            this.Entities = entities;
            }
             
            [JsonProperty(PropertyName = "query")]
            public string Query { get; set; }
    
            [JsonProperty(PropertyName = "intents")]
            public IList<IntentRecommendation> Intents { get; set; }

        [JsonProperty(PropertyName = "entities")]
            public IList<EntityRecommendation> Entities { get; set; }
            public virtual void Validate()
            {
                if (Query == null)
                {
                    throw new ValidationException(ValidationRules.CannotBeNull, "Query");
                }
                if (Intents == null)
                {
                    throw new ValidationException(ValidationRules.CannotBeNull, "Intents");
                }
                if (Entities == null)
                {
                    throw new ValidationException(ValidationRules.CannotBeNull, "Entities");
                }
            }
        }       
}