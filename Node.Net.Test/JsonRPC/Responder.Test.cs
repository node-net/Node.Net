using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

namespace Node.Net.JsonRPC
{
    [TestFixture]
    internal class ResponderTest
    {
        [Test]
        public void Respond()
        {
            var responder = GetTestResponder();
            var response = responder.Respond(new Request("say_hello"));
            Assert.AreEqual("hello", response.Result.ToString());

            var stream = typeof(ResponderTest).Assembly.GetManifestResourceStream(
                "Node.Net.Test.JsonRPC.Responder.Test.Data.json");
            var test_data = new Node.Net.Reader().Read<IDictionary>(stream);
            Assert.NotNull(test_data, nameof(test_data));

            foreach (string key in test_data.Keys)
            {
                if (key.Contains("get_properties"))
                {
                    int x = 0;
                }
                if (key.Contains("_request"))
                {
                    try
                    {
                        var request_data = (test_data[key] as IDictionary)?.ToJson();
                        var response_text = responder.Respond(request_data);
                        if (test_data.Contains(key.Replace("_request", "_response")))
                        {
                            var response_json = (test_data[key.Replace("_request", "_response")] as IDictionary)?.ToJson();
                            Assert.AreEqual(response_json, response_text, key);
                        }
                    }
                    catch (System.Exception e)
                    {
                        throw new System.InvalidOperationException(key, e);
                    }
                }
            }
        }

        public static Responder GetTestResponder()
        {
            return new Responder
            {
                Methods = new Dictionary<string, IResponder>
                {
                    {"say_hello", new JsonRPC.Function<string>(SayHello) },
                    {"action3",new JsonRPC.Action<string,string,string>(Action3) },
                    {"bad_action",new JsonRPC.Action(BadAction) },
                    { "add_multiply",new JsonRPC.Function<int,int,int,int>(AddMultiply,"a","b","c") },
                    {"set_properties",new JsonRPC.Action<string,IDictionary<string,string>>(SetProperties) },
                    {"get_properties",new JsonRPC.Function<string,string[],IDictionary<string,string>>(GetProperties) }
                }
            };
        }

        public static string SayHello()
        {
            return "hello";
        }

        public static void Action3(string a, string b, string c)
        {
        }

        public static void BadAction()
        {
            throw new System.InvalidOperationException("BadAction");
        }

        public static int AddMultiply(int a, int b, int c) { return (a + b) * c; }

        public static void SetProperties(string name, IDictionary<string, string> properties)
        {
            if (!_properties.ContainsKey(name)) { _properties.Add(name, new Dictionary<string, string>()); }

            foreach (var property in properties.Keys)
            {
                _properties[name][property] = properties[property];
            }
        }

        public static IDictionary<string, string> GetProperties(string name, string[] property_names)
        {
            var result = new Dictionary<string, string>();
            foreach (var property_name in property_names)
            {
                string value = string.Empty;
                if (_properties.ContainsKey(name) && _properties[name].ContainsKey(property_name))
                {
                    value = _properties[name][property_name];
                    result.Add(property_name, value);
                }
            }
            return result;
        }

        public static Dictionary<string, Dictionary<string, string>> _properties = new Dictionary<string, Dictionary<string, string>>();
    }
}