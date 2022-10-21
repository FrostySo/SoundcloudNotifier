using Newtonsoft.Json;

namespace SoundcloudNotifier.Model; 

public class Badges
    {
        [JsonProperty("pro")]
        public bool Pro { get; set; }

        [JsonProperty("pro_unlimited")]
        public bool ProUnlimited { get; set; }

        [JsonProperty("verified")]
        public bool Verified { get; set; }
    }

    public class Collection
    {
        [JsonProperty("artwork_url")]
        public string? ArtworkUrl { get; set; }

        [JsonProperty("caption")]
        public object? Caption { get; set; }

        [JsonProperty("commentable")]
        public bool Commentable { get; set; }

        [JsonProperty("comment_count")]
        public int? CommentCount { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("downloadable")]
        public bool Downloadable { get; set; }

        [JsonProperty("download_count")]
        public int? DownloadCount { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("full_duration")]
        public int FullDuration { get; set; }

        [JsonProperty("embeddable_by")]
        public string? EmbeddableBy { get; set; }

        [JsonProperty("genre")]
        public string? Genre { get; set; }

        [JsonProperty("has_downloads_left")]
        public bool HasDownloadsLeft { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("kind")]
        public string? Kind { get; set; }

        [JsonProperty("label_name")]
        public string? LabelName { get; set; }

        [JsonProperty("last_modified")]
        public DateTime LastModified { get; set; }

        [JsonProperty("license")]
        public string? License { get; set; }

        [JsonProperty("likes_count")]
        public int? LikesCount { get; set; }

        [JsonProperty("permalink")]
        public string? Permalink { get; set; }

        [JsonProperty("permalink_url")]
        public string? PermalinkUrl { get; set; }

        [JsonProperty("playback_count")]
        public int? PlaybackCount { get; set; }

        [JsonProperty("public")]
        public bool Public { get; set; }

        [JsonProperty("publisher_metadata")]
        public PublisherMetadata? PublisherMetadata { get; set; }

        [JsonProperty("purchase_title")]
        public string? PurchaseTitle { get; set; }

        [JsonProperty("purchase_url")]
        public string? PurchaseUrl { get; set; }

        [JsonProperty("release_date")]
        public DateTime? ReleaseDate { get; set; }

        [JsonProperty("reposts_count")]
        public int RepostsCount { get; set; }

        [JsonProperty("secret_token")]
        public object? SecretToken { get; set; }

        [JsonProperty("sharing")]
        public string? Sharing { get; set; }

        [JsonProperty("state")]
        public string? State { get; set; }

        [JsonProperty("streamable")]
        public bool Streamable { get; set; }

        [JsonProperty("tag_list")]
        public string? TagList { get; set; }

        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("track_format")]
        public string? TrackFormat { get; set; }

        [JsonProperty("uri")]
        public string? Uri { get; set; }

        [JsonProperty("urn")]
        public string? Urn { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("visuals")]
        public object? Visuals { get; set; }

        [JsonProperty("waveform_url")]
        public string? WaveformUrl { get; set; }

        [JsonProperty("display_date")]
        public DateTime DisplayDate { get; set; }

        [JsonProperty("media")]
        public Media? Media { get; set; }

        [JsonProperty("station_urn")]
        public string? StationUrn { get; set; }

        [JsonProperty("station_permalink")]
        public string? StationPermalink { get; set; }

        [JsonProperty("track_authorization")]
        public string? TrackAuthorization { get; set; }

        [JsonProperty("monetization_model")]
        public string? MonetizationModel { get; set; }

        [JsonProperty("policy")]
        public string? Policy { get; set; }

        [JsonProperty("user")]
        public User? User { get; set; }
    }

    public class Format
    {
        [JsonProperty("protocol")]
        public string? Protocol { get; set; }

        [JsonProperty("mime_type")]
        public string? MimeType { get; set; }
    }

    public class Media {
        [JsonProperty("transcodings")] 
        public List<Transcoding> Transcodings { get; set; } = new List<Transcoding>();
    }

    public class PublisherMetadata
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("urn")]
        public string? Urn { get; set; }

        [JsonProperty("contains_music")]
        public bool ContainsMusic { get; set; }

        [JsonProperty("artist")]
        public string? Artist { get; set; }

        [JsonProperty("album_title")]
        public string? AlbumTitle { get; set; }

        [JsonProperty("upc_or_ean")]
        public string? UpcOrEan { get; set; }

        [JsonProperty("isrc")]
        public string? Isrc { get; set; }

        [JsonProperty("explicit")]
        public bool? Explicit { get; set; }

        [JsonProperty("p_line")]
        public string? PLine { get; set; }

        [JsonProperty("p_line_for_display")]
        public string? PLineForDisplay { get; set; }

        [JsonProperty("c_line")]
        public string? CLine { get; set; }

        [JsonProperty("c_line_for_display")]
        public string? CLineForDisplay { get; set; }

        [JsonProperty("release_title")]
        public string? ReleaseTitle { get; set; }

        [JsonProperty("writer_composer")]
        public string? WriterComposer { get; set; }
    }

    public class Tracks {
        [JsonProperty("collection")] 
        public List<Collection> Collection { get; set; } = new List<Collection>();

        [JsonProperty("next_href")]
        public string? NextHref { get; set; }

        [JsonProperty("query_urn")]
        public object? QueryUrn { get; set; }
    }

    public class Transcoding
    {
        [JsonProperty("url")]
        public string? Url { get; set; }

        [JsonProperty("preset")]
        public string? Preset { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("snipped")]
        public bool Snipped { get; set; }

        [JsonProperty("format")]
        public Format? Format { get; set; }

        [JsonProperty("quality")]
        public string? Quality { get; set; }
    }

    public class User
    {
        [JsonProperty("avatar_url")]
        public string? AvatarUrl { get; set; }

        [JsonProperty("first_name")]
        public string? FirstName { get; set; }

        [JsonProperty("followers_count")]
        public int FollowersCount { get; set; }

        [JsonProperty("full_name")]
        public string? FullName { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("kind")]
        public string? Kind { get; set; }

        [JsonProperty("last_modified")]
        public DateTime LastModified { get; set; }

        [JsonProperty("last_name")]
        public string? LastName { get; set; }

        [JsonProperty("permalink")]
        public string? Permalink { get; set; }

        [JsonProperty("permalink_url")]
        public string? PermalinkUrl { get; set; }

        [JsonProperty("uri")]
        public string? Uri { get; set; }

        [JsonProperty("urn")]
        public string? Urn { get; set; }

        [JsonProperty("username")]
        public string? Username { get; set; }

        [JsonProperty("verified")]
        public bool Verified { get; set; }

        [JsonProperty("city")]
        public string? City { get; set; }

        [JsonProperty("country_code")]
        public object? CountryCode { get; set; }

        [JsonProperty("badges")]
        public Badges? Badges { get; set; }

        [JsonProperty("station_urn")]
        public string? StationUrn { get; set; }

        [JsonProperty("station_permalink")]
        public string? StationPermalink { get; set; }
    }