using Newtonsoft.Json;
using System.Collections.Generic;

namespace RequestBody
{
    public class Rootobject
    {
        public List<Fieldsrequest> fieldsRequest { get; set; }
    }

    public class Fieldsrequest
    {
        public Field groupField { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] //NEW
        public List<Field> fields { get; set; }
       
    }

    public class Field
    {
        public string fieldId { get; set; }
        public string value { get; set; }
        public bool isGroup { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]  //NEW
        public List<Field> fields { get; set; } //NEW
    }

}
