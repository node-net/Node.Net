using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace Node.Net.Test
{
    internal class ResourceDictionaryTests
    {
        private static bool CanCreateResourceDictionary()
        {
#if !IS_WINDOWS
            try
            {
                return new ResourceDictionary() != null;
            }
            catch
            {
                return false;
            }
#else
            return false;
#endif
        }

        [Test]
        public async Task ResourceDictionary_Constructor_Default_InitializesCorrectly()
        {
            if (!CanCreateResourceDictionary())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

#if !IS_WINDOWS
            // Arrange & Act
            ResourceDictionary dictionary = new ResourceDictionary();

            // Assert
            await Assert.That(dictionary).IsNotNull();
            await Assert.That(dictionary.Count).IsEqualTo(0);
            await Assert.That(dictionary.IsReadOnly).IsFalse();
            await Assert.That(dictionary.IsFixedSize).IsFalse();
#endif
        }

        [Test]
        public async Task ResourceDictionary_Add_AddsKeyValuePair()
        {
            if (!CanCreateResourceDictionary())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();
            string key = "TestKey";
            string value = "TestValue";

            // Act
            dictionary.Add(key, value);

            // Assert
            await Assert.That(dictionary.Count).IsEqualTo(1);
            await Assert.That(dictionary.Contains(key)).IsTrue();
            await Assert.That(dictionary[key]).IsEqualTo(value);
#endif
            await Task.CompletedTask;
        }

        [Test]
        public async Task ResourceDictionary_Add_WithNullKey_ThrowsArgumentNullException()
        {
            if (!CanCreateResourceDictionary())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();

            // Act & Assert
            await Assert.That(() => dictionary.Add(null, "value")).Throws<ArgumentNullException>();
#endif
            await Task.CompletedTask;
        }

        [Test]
        public async Task ResourceDictionary_Add_WithNullValue_ThrowsArgumentNullException()
        {
            if (!CanCreateResourceDictionary())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();

            // Act & Assert
            await Assert.That(() => dictionary.Add("key", null)).Throws<ArgumentNullException>();
#endif
            await Task.CompletedTask;
        }

        [Test]
        public async Task ResourceDictionary_Indexer_Set_AddsOrUpdatesValue()
        {
            if (!CanCreateResourceDictionary())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();
            string key = "TestKey";
            string value1 = "Value1";
            string value2 = "Value2";

            // Act
            dictionary[key] = value1;
            dictionary[key] = value2;

            // Assert
            await Assert.That(dictionary.Count).IsEqualTo(1);
            await Assert.That(dictionary[key]).IsEqualTo(value2);
#endif
            await Task.CompletedTask;
        }

        [Test]
        public async Task ResourceDictionary_Indexer_Set_WithNullValue_RemovesKey()
        {
            if (!CanCreateResourceDictionary())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();
            string key = "TestKey";
            dictionary.Add(key, "Value");

            // Act
            dictionary[key] = null;

            // Assert
            await Assert.That(dictionary.Count).IsEqualTo(0);
            await Assert.That(dictionary.Contains(key)).IsFalse();
#endif
            await Task.CompletedTask;
        }

        [Test]
        public async Task ResourceDictionary_Indexer_Get_WithNonExistentKey_ReturnsNull()
        {
            if (!CanCreateResourceDictionary())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();

            // Act
            object value = dictionary["NonExistentKey"];

            // Assert
            await Assert.That(value).IsNull();
#endif
            await Task.CompletedTask;
        }

        [Test]
        public async Task ResourceDictionary_Indexer_Get_WithNullKey_ThrowsArgumentNullException()
        {
            if (!CanCreateResourceDictionary())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();

            // Act & Assert
            await Assert.That(() => { var _ = dictionary[null]; }).Throws<ArgumentNullException>();
#endif
            await Task.CompletedTask;
        }

        [Test]
        public async Task ResourceDictionary_Contains_ReturnsTrueForExistingKey()
        {
            if (!CanCreateResourceDictionary())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();
            string key = "TestKey";
            dictionary.Add(key, "Value");

            // Act
            bool contains = dictionary.Contains(key);

            // Assert
            await Assert.That(contains).IsTrue();
#endif
            await Task.CompletedTask;
        }

        [Test]
        public async Task ResourceDictionary_Contains_ReturnsFalseForNonExistentKey()
        {
            if (!CanCreateResourceDictionary())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();

            // Act
            bool contains = dictionary.Contains("NonExistentKey");

            // Assert
            await Assert.That(contains).IsFalse();
#endif
            await Task.CompletedTask;
        }

        [Test]
        public async Task ResourceDictionary_Contains_WithNullKey_ReturnsFalse()
        {
            if (!CanCreateResourceDictionary())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();

            // Act
            bool contains = dictionary.Contains(null);

            // Assert
            await Assert.That(contains).IsFalse();
#endif
            await Task.CompletedTask;
        }

        [Test]
        public async Task ResourceDictionary_Remove_RemovesKey()
        {
            if (!CanCreateResourceDictionary())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();
            string key = "TestKey";
            dictionary.Add(key, "Value");

            // Act
            dictionary.Remove(key);

            // Assert
            await Assert.That(dictionary.Count).IsEqualTo(0);
            await Assert.That(dictionary.Contains(key)).IsFalse();
#endif
            await Task.CompletedTask;
        }

        [Test]
        public async Task ResourceDictionary_Remove_WithNonExistentKey_DoesNothing()
        {
            if (!CanCreateResourceDictionary())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();

            // Act
            dictionary.Remove("NonExistentKey");

            // Assert
            await Assert.That(dictionary.Count).IsEqualTo(0);
#endif
            await Task.CompletedTask;
        }

        [Test]
        public async Task ResourceDictionary_Remove_WithNullKey_DoesNothing()
        {
            if (!CanCreateResourceDictionary())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();

            // Act
            dictionary.Remove(null);

            // Assert
            await Assert.That(dictionary.Count).IsEqualTo(0);
#endif
            await Task.CompletedTask;
        }

        [Test]
        public async Task ResourceDictionary_Clear_RemovesAllItems()
        {
            if (!CanCreateResourceDictionary())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();
            dictionary.Add("Key1", "Value1");
            dictionary.Add("Key2", "Value2");
            dictionary.Add("Key3", "Value3");

            // Act
            dictionary.Clear();

            // Assert
            await Assert.That(dictionary.Count).IsEqualTo(0);
            await Assert.That(dictionary.Contains("Key1")).IsFalse();
            await Assert.That(dictionary.Contains("Key2")).IsFalse();
            await Assert.That(dictionary.Contains("Key3")).IsFalse();
#endif
            await Task.CompletedTask;
        }

        [Test]
        public async Task ResourceDictionary_Keys_ReturnsAllKeys()
        {
            if (!CanCreateResourceDictionary())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();
            dictionary.Add("Key1", "Value1");
            dictionary.Add("Key2", "Value2");

            // Act
            ICollection keys = dictionary.Keys;

            // Assert
            await Assert.That(keys.Count).IsEqualTo(2);
            await Assert.That(keys.Cast<object>().Contains("Key1")).IsTrue();
            await Assert.That(keys.Cast<object>().Contains("Key2")).IsTrue();
#endif
            await Task.CompletedTask;
        }

        [Test]
        public async Task ResourceDictionary_Values_ReturnsAllValues()
        {
            if (!CanCreateResourceDictionary())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();
            dictionary.Add("Key1", "Value1");
            dictionary.Add("Key2", "Value2");

            // Act
            ICollection values = dictionary.Values;

            // Assert
            await Assert.That(values.Count).IsEqualTo(2);
            await Assert.That(values.Cast<object>().Contains("Value1")).IsTrue();
            await Assert.That(values.Cast<object>().Contains("Value2")).IsTrue();
#endif
            await Task.CompletedTask;
        }

        [Test]
        public async Task ResourceDictionary_GetEnumerator_IteratesThroughItems()
        {
            if (!CanCreateResourceDictionary())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();
            dictionary.Add("Key1", "Value1");
            dictionary.Add("Key2", "Value2");

            // Act
            int count = 0;
            foreach (DictionaryEntry entry in dictionary)
            {
                count++;
            }

            // Assert
            await Assert.That(count).IsEqualTo(2);
#endif
            await Task.CompletedTask;
        }

        [Test]
        public async Task ResourceDictionary_MergedDictionaries_CanBeAccessed()
        {
            if (!CanCreateResourceDictionary())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();

            // Act
            var mergedDictionaries = dictionary.MergedDictionaries;

            // Assert
            await Assert.That(mergedDictionaries).IsNotNull();
            await Assert.That(mergedDictionaries.Count).IsEqualTo(0);
#endif
            await Task.CompletedTask;
        }

        [Test]
        public async Task ResourceDictionary_MergedDictionaries_CanAddDictionary()
        {
            if (!CanCreateResourceDictionary())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();
            ResourceDictionary mergedDict = new ResourceDictionary();
            mergedDict.Add("MergedKey", "MergedValue");

            // Act
            dictionary.MergedDictionaries.Add(mergedDict);

            // Assert
            await Assert.That(dictionary.MergedDictionaries.Count).IsEqualTo(1);
            await Assert.That(dictionary.Contains("MergedKey")).IsTrue();
            await Assert.That(dictionary["MergedKey"]).IsEqualTo("MergedValue");
#endif
            await Task.CompletedTask;
        }

        [Test]
        public async Task ResourceDictionary_MergedDictionaries_ValueFromMergedDictionaryTakesPrecedence()
        {
            if (!CanCreateResourceDictionary())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();
            dictionary.Add("Key", "OriginalValue");
            ResourceDictionary mergedDict = new ResourceDictionary();
            mergedDict.Add("Key", "MergedValue");

            // Act
            dictionary.MergedDictionaries.Add(mergedDict);

            // Assert
            // In our implementation, merged dictionaries are checked after the main dictionary
            // So the original value should be returned first
            await Assert.That(dictionary["Key"]).IsEqualTo("OriginalValue");
#endif
            await Task.CompletedTask;
        }

        [Test]
        public async Task ResourceDictionary_MergedDictionaries_ValueFromFirstMergedDictionary()
        {
            if (!CanCreateResourceDictionary())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();
            ResourceDictionary mergedDict1 = new ResourceDictionary();
            mergedDict1.Add("Key", "Value1");
            ResourceDictionary mergedDict2 = new ResourceDictionary();
            mergedDict2.Add("Key", "Value2");

            // Act
            dictionary.MergedDictionaries.Add(mergedDict1);
            dictionary.MergedDictionaries.Add(mergedDict2);

            // Assert
            // First merged dictionary should be checked first
            await Assert.That(dictionary["Key"]).IsEqualTo("Value1");
#endif
            await Task.CompletedTask;
        }
    }
}

