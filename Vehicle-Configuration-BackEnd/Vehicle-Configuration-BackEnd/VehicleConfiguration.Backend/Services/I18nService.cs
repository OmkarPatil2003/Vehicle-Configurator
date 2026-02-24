namespace VehicleConfiguration.Backend.Services;

public class I18nService
{
    private readonly Dictionary<string, Dictionary<string, string>> _locales = new();
    private readonly string _defaultLocale = "en";
    public I18nService()
    {
        try 
        {
            var basePath = Path.Combine(AppContext.BaseDirectory, "Resources");
            LoadMessages("en", Path.Combine(basePath, "messages.properties"));
            LoadMessages("fr", Path.Combine(basePath, "messages_fr.properties"));
            LoadMessages("mr", Path.Combine(basePath, "messages_mr.properties"));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"CRITICAL ERROR in I18nService: {ex.Message} \nStack: {ex.StackTrace}");
            // Initialize empty to prevent crash
            _locales["en"] = new Dictionary<string, string>();
        }
    }

    private void LoadMessages(string locale, string filePath)
    {
        var messages = new Dictionary<string, string>();
        if (File.Exists(filePath))
        {
            try
            {
                foreach (var line in File.ReadLines(filePath))
                {
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;
                    var parts = line.Split('=', 2);
                    if (parts.Length == 2)
                    {
                        string key = parts[0].Trim();
                        string value = parts[1].Trim().Replace("\\n", "\n");
                        messages[key] = value;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading i18n file {filePath}: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine($"Warning: i18n file not found: {filePath}");
        }
        _locales[locale] = messages;
    }

    public string GetMessage(string key, string? locale = "en")
    {
        if (string.IsNullOrEmpty(locale)) locale = _defaultLocale;
        
        // Fallback or exact match
        if (!_locales.TryGetValue(locale, out var messages))
        {
            // Try to default to english if locale not found
            if (!_locales.TryGetValue(_defaultLocale, out messages))
            {
                return key;
            }
        }

        return messages.TryGetValue(key, out var value) ? value : key;
    }
}
