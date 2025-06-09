using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;



namespace Node.Net
{
    public static class StringExtension
    {
        public static bool IsFileDialogFilter(this string value)
        {
            return value.Contains('|');
        }

        public static bool IsValidFileName(this string value)
        {
            if (value.Contains('\\'))
            {
                if (value.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                {
                    return false;
                }

                string[]? parts = value.Split('\\');
                if (parts[parts.Length - 1].IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                {
                    return false;
                }
            }
            else
            {
                if (value.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                {
                    return false;
                }
            }
            return true;
        }

        public static double GetMeters(this string value) => Length.GetMeters(value);

        public static double GetRawValue(this string value)
        {
            try
            {
                string[]? words = value.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (words.Length > 0)
                {
                    string? word = words[0].Replace("'", "");
                    return Convert.ToDouble(word);
                }
                return 0.0;
            }
            catch
            {
                return 0.0;
            }
        }

        public static Stream GetStream(this string value)
        {
            if (File.Exists(value))
            {
                return new FileStream(value, FileMode.Open, FileAccess.Read);
            }
            StackTrace? stackTrace = new StackTrace();
            foreach (System.Reflection.Assembly? assembly in stackTrace.GetAssemblies())
            {
                Stream? stream = assembly.GetStream(value);
                if (stream != null) { return stream; }
            }

            return new MemoryStream(Encoding.UTF8.GetBytes(value));
        }

#if IS_FRAMEWORK
#else
		/// <summary>
		/// Formats a JSON string with proper indentation for better readability.
		/// </summary>
		/// <param name="json">The JSON string to format.</param>
		/// <returns>A formatted JSON string with proper indentation.</returns>
		/// <exception cref="ArgumentNullException">Thrown when json is null.</exception>
		/// <exception cref="ArgumentException">Thrown when json is not valid JSON.</exception>
		public static string ToPrettyJson(this string json)
		{
			if (json is null)
			{
				throw new ArgumentNullException(nameof(json));
			}

			try
			{
				// First parse the JSON to validate it
				using var document = System.Text.Json.JsonDocument.Parse(json);

				// Create options for pretty printing
				var options = new System.Text.Json.JsonSerializerOptions
				{
					WriteIndented = true
				};

				// Serialize back to string with indentation
				return System.Text.Json.JsonSerializer.Serialize(document, options);
			}
			catch (System.Text.Json.JsonException ex)
			{
				throw new ArgumentException("Invalid JSON string", nameof(json), ex);
			}
		}

		public static bool TryFindJsonValue(this string json,string key, out string value)
		{
			using System.Text.Json.JsonDocument doc = System.Text.Json.JsonDocument.Parse(json);
			return TryFindProperty(doc.RootElement, key, out value);
		}
		public static bool TryFindProperty(System.Text.Json.JsonElement element, string propertyName, out string value)
		{
			value = null;

			if (element.ValueKind == System.Text.Json.JsonValueKind.Object)
			{
				foreach (var prop in element.EnumerateObject())
				{
					if (prop.NameEquals(propertyName))
					{
						value = prop.Value.GetString();
						return true;
					}

					if (prop.Value.ValueKind == System.Text.Json.JsonValueKind.Object || prop.Value.ValueKind == System.Text.Json.JsonValueKind.Array)
					{
						if (TryFindProperty(prop.Value, propertyName, out value))
							return true;
					}
				}
			}
			else if (element.ValueKind == System.Text.Json.JsonValueKind.Array)
			{
				foreach (var item in element.EnumerateArray())
				{
					if (TryFindProperty(item, propertyName, out value))
						return true;
				}
			}

			return false;
		}
#endif

	}
}