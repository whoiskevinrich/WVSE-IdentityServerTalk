<Query Kind="Program">
  <NuGetReference>IdentityModel</NuGetReference>
  <NuGetReference>RestSharp</NuGetReference>
  <Namespace>IdentityModel.Client</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>RestSharp</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

// Install-Package IdentityModel
// Install-Package RestSharp

void Main()
{
	// IdentityServer Allows for auto-discovery of endpoints from metadata
	var disco = DiscoveryClient.GetAsync("http://localhost:5001").Result;
	
	// request token
	var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
	var tokenResponse = tokenClient.RequestClientCredentialsAsync("claims.read").Result;
	if (tokenResponse.IsError) tokenResponse.Error.Dump("Error");
	var token = new { Type = tokenResponse.TokenType, Token = tokenResponse.AccessToken };
	token.Dump("Credentials Provided by IdentityServer");

	// Use JWT to request from our API by adding the Authorization Header
	var client = new RestClient("http://localhost:6001/api/");
	var request = new RestRequest("claims/PolicyRestricted", Method.GET);
	request.AddHeader("Authorization", $"{token.Type} {token.Token}");
	var operation = client.Execute(request);

	// Print result
	if (operation.StatusDescription == "OK")
		JsonConvert.DeserializeObject(operation.Content).DumpJson("API Result");
	else
		operation.StatusCode.Dump("API Result");
}

// Define other methods and classes here
public class Token
{
	[JsonProperty("token_type")]
	public string TokenType { get; set; }

	[JsonProperty("access_token")]
	public string TokenValue { get; set; }
}

public class Claim
{
	[JsonProperty("type")]
	public string Type { get; set; }
	
	[JsonProperty("value")]
	public string Value { get; set; }
}