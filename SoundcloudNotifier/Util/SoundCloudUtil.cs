using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using SoundcloudNotifier.Model;
using SoundcloudNotifier.SQL.Models;

namespace SoundcloudNotifier.Util; 

public static class SoundCloudUtil {
  private const string UserIdRegex = @"soundcloud:\/\/users:(\d+)";
  private const string JavascriptMatchRegex = @"script crossorigin src=""(.+)""";
  private const string ClientIdMatchRegex = @"exports={\""api-v2\"".*client_id:\""(\w*)\""";
  //private const string ClientIdMatchRegex = @"client_id:""(.+)""|client_id=(.+)""";
  private const string SoundCloudDomain = @"https://soundcloud.com";
  private const string SoundCloudApiDomain = @"https://api-v2.soundcloud.com";
  private const string ClientIdCacheFile = "soundcloudkey.txt";

  private static string? _clientId;
  
  public static void LoadClientIdFromCache() {
    if (File.Exists(ClientIdCacheFile)) {
      _clientId = File.ReadAllText(ClientIdCacheFile);
    }
  }

  public static void SaveClientIdToCache() {
    File.WriteAllText(ClientIdCacheFile,_clientId);
  }

  private static async Task<string?> SendRequestAsync(HttpClient client,string url) {
    return await RecursiveRetry(client, url, 0);
  }

  private static async Task<string?> RecursiveRetry(HttpClient client, string url, int attempts) {
    if (attempts >= 5) return null;
    try {
      return await client.GetStringAsync(url);
    }catch (HttpRequestException e) {
      if (e.StatusCode == HttpStatusCode.Unauthorized) {
        await RefreshClientIdAsync();
        return await RecursiveRetry(client, url, ++attempts);
      }
    }

    return null;
  }

  private static long? GetUserId(string html) {
    var regex = Regex.Match(html,UserIdRegex);
    if (regex.Success) {
      if (long.TryParse(regex.Groups[1].Value, out var val)) {
        return val;
      }
    }
    return null;
  }

  private static string? GetArtistName(string html,string username) {
    var usernameRegex = $@"href=""\/(?i){username}"">(.+)<\/a>";
    var regex = Regex.Match(html,usernameRegex);
    if (regex.Success) {
      return regex.Groups[1].Value;
    }
    return null;
  }

  public static async Task<Artist?> GetArtistAsync(string soundCloudUsername) {
    using var client = GetHttpClient();
    var result = await client.GetStringAsync($"{SoundCloudDomain}/{soundCloudUsername}");
    if (result is null) return null;
    var userId = GetUserId(result);
    var artistName = GetArtistName(result,soundCloudUsername);
    if (userId != null && artistName != null) {
      return new Artist() {
        Id = userId.Value,
        Name = artistName,
        Username = soundCloudUsername
      };
    }
    return null;
  }
  
  public static async Task RefreshClientIdAsync() {
    using var client = GetHttpClient();
    client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("UTF8");
    var result = await client.GetStringAsync(SoundCloudDomain);
    if (result is null) return;
    var jsMacthes = Regex.Match(result, JavascriptMatchRegex);
    while (jsMacthes.Success) {
      var jsUrl = jsMacthes.Groups[1].Value;
      var script = await client.GetStringAsync(jsUrl);
      if (script != null) {
        var clientIdMatches = Regex.Match(script, ClientIdMatchRegex);
        if (clientIdMatches.Success) {
            _clientId = clientIdMatches.Groups[1].Value;
            SaveClientIdToCache();
            return;
        }
      }

      jsMacthes = jsMacthes.NextMatch();
    }
    return;
  }

  public static async Task<Tracks?> GetUserTracksAsync(long userId, int limit = 20) {
    var url = $"{SoundCloudApiDomain}/users/{userId}/tracks?representation=&client_id={_clientId}&limit={limit.ToString()}&offset=0&linked_partitioning=1&app_version=1665133451&app_locale=en";
    using var client = GetHttpClient();
    
    client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
    var result = await SendRequestAsync(client,url);
    if (result is null) return null;
    return JsonConvert.DeserializeObject<Tracks>(result);
  }
  
  static HttpClient GetHttpClient() {
    var httpClient = new HttpClient();
    httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36");
    return httpClient;
  }
}