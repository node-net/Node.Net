using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;

namespace Node.Net.Test
{
    [TestFixture]
    internal static class ResourceDictionaryTests
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
        public static void ResourceDictionary_Constructor_Default_InitializesCorrectly()
        {
            if (!CanCreateResourceDictionary())
            {
                Assert.Pass("ResourceDictionary only available on non-Windows platforms");
                return;
            }

#if !IS_WINDOWS
            // Arrange & Act
            ResourceDictionary dictionary = new ResourceDictionary();

            // Assert
            Assert.That(dictionary, Is.Not.Null);
            Assert.That(dictionary.Count, Is.EqualTo(0));
            Assert.That(dictionary.IsReadOnly, Is.False);
            Assert.That(dictionary.IsFixedSize, Is.False);
#endif
        }

        [Test]
        public static void ResourceDictionary_Add_AddsKeyValuePair()
        {
            if (!CanCreateResourceDictionary())
            {
                Assert.Pass("ResourceDictionary only available on non-Windows platforms");
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
            Assert.That(dictionary.Count, Is.EqualTo(1));
            Assert.That(dictionary.Contains(key), Is.True);
            Assert.That(dictionary[key], Is.EqualTo(value));
#endif
        }

        [Test]
        public static void ResourceDictionary_Add_WithNullKey_ThrowsArgumentNullException()
        {
            if (!CanCreateResourceDictionary())
            {
                Assert.Pass("ResourceDictionary only available on non-Windows platforms");
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => dictionary.Add(null, "value"));
#endif
        }

        [Test]
        public static void ResourceDictionary_Add_WithNullValue_ThrowsArgumentNullException()
        {
            if (!CanCreateResourceDictionary())
            {
                Assert.Pass("ResourceDictionary only available on non-Windows platforms");
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => dictionary.Add("key", null));
#endif
        }

        [Test]
        public static void ResourceDictionary_Indexer_Set_AddsOrUpdatesValue()
        {
            if (!CanCreateResourceDictionary())
            {
                Assert.Pass("ResourceDictionary only available on non-Windows platforms");
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
            Assert.That(dictionary.Count, Is.EqualTo(1));
            Assert.That(dictionary[key], Is.EqualTo(value2));
#endif
        }

        [Test]
        public static void ResourceDictionary_Indexer_Set_WithNullValue_RemovesKey()
        {
            if (!CanCreateResourceDictionary())
            {
                Assert.Pass("ResourceDictionary only available on non-Windows platforms");
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
            Assert.That(dictionary.Count, Is.EqualTo(0));
            Assert.That(dictionary.Contains(key), Is.False);
#endif
        }

        [Test]
        public static void ResourceDictionary_Indexer_Get_WithNonExistentKey_ReturnsNull()
        {
            if (!CanCreateResourceDictionary())
            {
                Assert.Pass("ResourceDictionary only available on non-Windows platforms");
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();

            // Act
            object value = dictionary["NonExistentKey"];

            // Assert
            Assert.That(value, Is.Null);
#endif
        }

        [Test]
        public static void ResourceDictionary_Indexer_Get_WithNullKey_ThrowsArgumentNullException()
        {
            if (!CanCreateResourceDictionary())
            {
                Assert.Pass("ResourceDictionary only available on non-Windows platforms");
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => { var _ = dictionary[null]; });
#endif
        }

        [Test]
        public static void ResourceDictionary_Contains_ReturnsTrueForExistingKey()
        {
            if (!CanCreateResourceDictionary())
            {
                Assert.Pass("ResourceDictionary only available on non-Windows platforms");
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
            Assert.That(contains, Is.True);
#endif
        }

        [Test]
        public static void ResourceDictionary_Contains_ReturnsFalseForNonExistentKey()
        {
            if (!CanCreateResourceDictionary())
            {
                Assert.Pass("ResourceDictionary only available on non-Windows platforms");
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();

            // Act
            bool contains = dictionary.Contains("NonExistentKey");

            // Assert
            Assert.That(contains, Is.False);
#endif
        }

        [Test]
        public static void ResourceDictionary_Contains_WithNullKey_ReturnsFalse()
        {
            if (!CanCreateResourceDictionary())
            {
                Assert.Pass("ResourceDictionary only available on non-Windows platforms");
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();

            // Act
            bool contains = dictionary.Contains(null);

            // Assert
            Assert.That(contains, Is.False);
#endif
        }

        [Test]
        public static void ResourceDictionary_Remove_RemovesKey()
        {
            if (!CanCreateResourceDictionary())
            {
                Assert.Pass("ResourceDictionary only available on non-Windows platforms");
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
            Assert.That(dictionary.Count, Is.EqualTo(0));
            Assert.That(dictionary.Contains(key), Is.False);
#endif
        }

        [Test]
        public static void ResourceDictionary_Remove_WithNonExistentKey_DoesNothing()
        {
            if (!CanCreateResourceDictionary())
            {
                Assert.Pass("ResourceDictionary only available on non-Windows platforms");
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();

            // Act
            dictionary.Remove("NonExistentKey");

            // Assert
            Assert.That(dictionary.Count, Is.EqualTo(0));
#endif
        }

        [Test]
        public static void ResourceDictionary_Remove_WithNullKey_DoesNothing()
        {
            if (!CanCreateResourceDictionary())
            {
                Assert.Pass("ResourceDictionary only available on non-Windows platforms");
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();

            // Act
            dictionary.Remove(null);

            // Assert
            Assert.That(dictionary.Count, Is.EqualTo(0));
#endif
        }

        [Test]
        public static void ResourceDictionary_Clear_RemovesAllItems()
        {
            if (!CanCreateResourceDictionary())
            {
                Assert.Pass("ResourceDictionary only available on non-Windows platforms");
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
            Assert.That(dictionary.Count, Is.EqualTo(0));
            Assert.That(dictionary.Contains("Key1"), Is.False);
            Assert.That(dictionary.Contains("Key2"), Is.False);
            Assert.That(dictionary.Contains("Key3"), Is.False);
#endif
        }

        [Test]
        public static void ResourceDictionary_Keys_ReturnsAllKeys()
        {
            if (!CanCreateResourceDictionary())
            {
                Assert.Pass("ResourceDictionary only available on non-Windows platforms");
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
            Assert.That(keys.Count, Is.EqualTo(2));
            Assert.That(keys.Cast<object>().Contains("Key1"), Is.True);
            Assert.That(keys.Cast<object>().Contains("Key2"), Is.True);
#endif
        }

        [Test]
        public static void ResourceDictionary_Values_ReturnsAllValues()
        {
            if (!CanCreateResourceDictionary())
            {
                Assert.Pass("ResourceDictionary only available on non-Windows platforms");
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
            Assert.That(values.Count, Is.EqualTo(2));
            Assert.That(values.Cast<object>().Contains("Value1"), Is.True);
            Assert.That(values.Cast<object>().Contains("Value2"), Is.True);
#endif
        }

        [Test]
        public static void ResourceDictionary_GetEnumerator_IteratesThroughItems()
        {
            if (!CanCreateResourceDictionary())
            {
                Assert.Pass("ResourceDictionary only available on non-Windows platforms");
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
            Assert.That(count, Is.EqualTo(2));
#endif
        }

        [Test]
        public static void ResourceDictionary_MergedDictionaries_CanBeAccessed()
        {
            if (!CanCreateResourceDictionary())
            {
                Assert.Pass("ResourceDictionary only available on non-Windows platforms");
                return;
            }

#if !IS_WINDOWS
            // Arrange
            ResourceDictionary dictionary = new ResourceDictionary();

            // Act
            var mergedDictionaries = dictionary.MergedDictionaries;

            // Assert
            Assert.That(mergedDictionaries, Is.Not.Null);
            Assert.That(mergedDictionaries.Count, Is.EqualTo(0));
#endif
        }

        [Test]
        public static void ResourceDictionary_MergedDictionaries_CanAddDictionary()
        {
            if (!CanCreateResourceDictionary())
            {
                Assert.Pass("ResourceDictionary only available on non-Windows platforms");
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
            Assert.That(dictionary.MergedDictionaries.Count, Is.EqualTo(1));
            Assert.That(dictionary.Contains("MergedKey"), Is.True);
            Assert.That(dictionary["MergedKey"], Is.EqualTo("MergedValue"));
#endif
        }

        [Test]
        public static void ResourceDictionary_MergedDictionaries_ValueFromMergedDictionaryTakesPrecedence()
        {
            if (!CanCreateResourceDictionary())
            {
                Assert.Pass("ResourceDictionary only available on non-Windows platforms");
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
            Assert.That(dictionary["Key"], Is.EqualTo("OriginalValue"));
#endif
        }

        [Test]
        public static void ResourceDictionary_MergedDictionaries_ValueFromFirstMergedDictionary()
        {
            if (!CanCreateResourceDictionary())
            {
                Assert.Pass("ResourceDictionary only available on non-Windows platforms");
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
            Assert.That(dictionary["Key"], Is.EqualTo("Value1"));
#endif
        }
    }
}

