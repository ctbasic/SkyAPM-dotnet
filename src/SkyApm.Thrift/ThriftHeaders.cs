using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SkyApm.Agent.Thrift
{
    public class ThriftHeaders : IEnumerable<KeyValuePair<string, string>>
    {
        private List<KeyValuePair<string, string>> dataStore;

        public ThriftHeaders()
        {
            if (dataStore == null)
            {
                dataStore = new List<KeyValuePair<string, string>>();
            }
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return dataStore.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(string name, string value)
        {
            dataStore.Add(new KeyValuePair<string, string>(name, value));
        }

        public bool Contains(string name)
        {
            return dataStore != null && dataStore.Any(x => x.Key == name);
        }

        public void Remove(string name)
        {
            dataStore?.RemoveAll(x => x.Key == name);
        }

        public void Cleaar()
        {
            dataStore?.Clear();
        }
    }
}
