<Query Kind="Program">
  <NuGetReference>IdentityModel</NuGetReference>
  <NuGetReference>RestSharp</NuGetReference>
  <Namespace>IdentityModel.Client</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>RestSharp</Namespace>
  <Namespace>System.Dynamic</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	// Request token from Auth0
	var authClient = new RestClient("https://whoiskevinrich.auth0.com/oauth/");
	var authRequest = new RestRequest("token", Method.POST);
	authRequest.AddHeader("content-type", "application/json");
	//authRequest.AddParameter("application/json", "{\"client_id\":\"Bnx5PL1B8M1RdsJT6W8c6IgWp1FRKdKe\",\"client_secret\":\"6oaPYtvS7W9vLpzIlqeQ-WcjLiyqeiLx3MrO0kDmcHw8ZGVW8-k-szCjlE2IiJ_Y\",\"audience\":\"https://sample.auth0.webapi/\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
	var authRequestParameter = new AuthRequestParameter
	{
		ClientId = "Bnx5PL1B8M1RdsJT6W8c6IgWp1FRKdKe",
		ClientSecret = "6oaPYtvS7W9vLpzIlqeQ-WcjLiyqeiLx3MrO0kDmcHw8ZGVW8-k-szCjlE2IiJ_Y",
		Audience = "https://sample.auth0.webapi/",
		GrantType = "client_credentials",
	};
	authRequest.AddParameter("application/json", JsonConvert.SerializeObject(authRequestParameter), ParameterType.RequestBody);
	var authOperation = authClient.Execute(authRequest);
	
	// Deserialize results from JSON
	var token = JsonConvert.DeserializeObject<Token>(authOperation.Content);
	token.Dump("Credentials Provided by Auth0");

	// Use JWT to request from our API by adding the Authorization Header
	var client = new RestClient("http://localhost:5001/api/");
	var request = new RestRequest("claims/PolicyRestricted", Method.GET);
	request.AddHeader("Authorization", $"{token.TokenType} {token.TokenValue}");
	var operation = client.Execute(request);

	// Print result
	if(operation.StatusDescription == "OK") 
		JsonConvert.DeserializeObject(operation.Content).DumpJson("API Result");
	else if(operation?.ErrorMessage.StartsWith("Unable to connect") ?? false)
		"Turn on the project, silly".Dump();
	else 
		operation.StatusCode.Dump("API Result");
}

public class Token
{
	[JsonProperty("token_type")]
	public string TokenType { get; set; }
	
	[JsonProperty("access_token")]
	public string TokenValue { get; set; }
}

public class AuthRequestParameter
{
	[JsonProperty("client_id")]
	public string ClientId { get; set; }

	[JsonProperty("client_secret")]
	public string ClientSecret { get; set; }

	[JsonProperty("audience")]
	public string Audience { get; set; }

	[JsonProperty("grant_type")]
	public string GrantType { get; set; }
}