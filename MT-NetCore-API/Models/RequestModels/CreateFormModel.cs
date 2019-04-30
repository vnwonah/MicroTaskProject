using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MT_NetCore_API.Models.RequestModels
{
    public class CreateFormModel
    {
        [JsonProperty("form_name")]
        public string FormName { get; set; }

        public int ProjectId { get; set; }
    }
}
