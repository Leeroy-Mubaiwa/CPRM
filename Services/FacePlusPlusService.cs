using Microsoft.Extensions.Configuration;

public class FacePlusPlusService
{
    private readonly string _apiKey;
    private readonly string _apiSecret;
    private readonly HttpClient _client;

    public FacePlusPlusService(IConfiguration configuration)
    {
        _apiKey = configuration["FaceAPI:Key"];
        _apiSecret = configuration["FaceAPI:Scret"];
        _client = new HttpClient();
    }

    private async Task<string> PostAsync(string url, Dictionary<string, string> parameters)
    {
        var content = new FormUrlEncodedContent(parameters);
        var response = await _client.PostAsync(url, content);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<bool> CompareFacesAsync(string image1Base64, string image2Base64)
    {
        try
        {
            // First, detect faces in both images
            var detectUrl = "https://api-us.faceplusplus.com/facepp/v3/detect";
            var detectData1 = new Dictionary<string, string>
            {
                { "api_key", _apiKey },
                { "api_secret", _apiSecret },
                { "image_base64", image1Base64 },
                { "return_face_token", "1" }
            };

            var detectData2 = new Dictionary<string, string>
            {
                { "api_key", _apiKey },
                { "api_secret", _apiSecret },
                { "image_base64", image2Base64 },
                { "return_face_token", "1" }
            };

            var response1 = await PostAsync(detectUrl, detectData1);
            var response2 = await PostAsync(detectUrl, detectData2);

            // Parse responses to get face tokens
            var faceToken1 = GetFaceToken(response1);
            var faceToken2 = GetFaceToken(response2);

            if (string.IsNullOrEmpty(faceToken1) || string.IsNullOrEmpty(faceToken2))
            {
                return false;
            }

            // Compare the faces
            var compareUrl = "https://api-us.faceplusplus.com/facepp/v3/compare";
            var compareData = new Dictionary<string, string>
            {
                { "api_key", _apiKey },
                { "api_secret", _apiSecret },
                { "face_token1", faceToken1 },
                { "face_token2", faceToken2 }
            };

            var compareResponse = await PostAsync(compareUrl, compareData);
            var confidence = GetConfidence(compareResponse);

            // Consider faces matching if confidence is above 80%
            return confidence >= 80;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private string GetFaceToken(string response)
    {
        try
        {
            var json = System.Text.Json.JsonDocument.Parse(response);
            var faces = json.RootElement.GetProperty("faces");
            if (faces.GetArrayLength() > 0)
            {
                return faces[0].GetProperty("face_token").GetString();
            }
        }
        catch
        {
            // Ignore parsing errors
        }
        return null;
    }

    private double GetConfidence(string response)
    {
        try
        {
            var json = System.Text.Json.JsonDocument.Parse(response);
            return json.RootElement.GetProperty("confidence").GetDouble();
        }
        catch
        {
            return 0;
        }
    }

    public async Task<string> DetectFaceAsync(string imageUrl)
    {
        var url = "https://api-us.faceplusplus.com/facepp/v3/detect";
        var data = new Dictionary<string, string>
        {
            { "api_key", _apiKey },
            { "api_secret", _apiSecret },
            { "image_url", imageUrl },
            { "return_attributes", "gender,age,smiling,emotion" }
        };
        return await PostAsync(url, data);
    }

    public async Task<string> CreateFaceSetAsync(string outerId)
    {
        var url = "https://api-us.faceplusplus.com/facepp/v3/faceset/create";
        var data = new Dictionary<string, string>
        {
            { "api_key", _apiKey },
            { "api_secret", _apiSecret },
            { "outer_id", outerId }
        };
        return await PostAsync(url, data);
    }

    public async Task<string> AddFaceToSetAsync(string faceToken, string outerId)
    {
        var url = "https://api-us.faceplusplus.com/facepp/v3/faceset/addface";
        var data = new Dictionary<string, string>
        {
            { "api_key", _apiKey },
            { "api_secret", _apiSecret },
            { "face_tokens", faceToken },
            { "outer_id", outerId }
        };
        return await PostAsync(url, data);
    }

    public async Task<string> SearchFaceAsync(string faceToken, string outerId)
    {
        var url = "https://api-us.faceplusplus.com/facepp/v3/search";
        var data = new Dictionary<string, string>
        {
            { "api_key", _apiKey },
            { "api_secret", _apiSecret },
            { "face_token", faceToken },
            { "outer_id", outerId }
        };
        return await PostAsync(url, data);
    }
}
