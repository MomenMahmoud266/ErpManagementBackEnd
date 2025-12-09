namespace ErpManagement.API.Utilities;

public static class HttpRequestData
{
    public static RequestLangEnum GetCurrentRequestLanguage(this IHeaderDictionary keyValuePairs)
    {
        string? lang = keyValuePairs.ToString();

        if (lang!.StartsWith(RequestLang.Ar))
            return RequestLangEnum.Ar;

        else if (lang.StartsWith(RequestLang.En))
            return RequestLangEnum.En;

        else
            return RequestLangEnum.Tr;
    }
}
