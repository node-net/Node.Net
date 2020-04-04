using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Node.Net.Test
{
    internal class Widget : ISerializable
    {
        public Widget() { }

        private Widget(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString("Name");
            Description = info.GetString("Description");
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("Description", Description);
        }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
