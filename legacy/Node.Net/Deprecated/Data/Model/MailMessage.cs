using System.Collections.Generic;

namespace Node.Net.Data.Model
{
    public class MailMessage : Dictionary<string, dynamic>, IModel
    {
        public MailMessage() { this.SetTypeName(); }

        public string From
        {
            get { return this.Get<string>(nameof(From)); }
            set { this.Set(nameof(From), value); }
        }
    }
}
