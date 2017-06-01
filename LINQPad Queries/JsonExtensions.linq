<Query Kind="Program">
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>System.Dynamic</Namespace>
</Query>

void Main()
{
	// Write code to test your extensions here. Press F5 to compile and run.
	var json = "{\"client_id\":\"Hcun61j6V16cfHCUktHzxI6qg1cliYv4\",\"client_secret\":\"68Yql8AUurpaysFxgWwY-zYvXQNRfILJ4ZEuNTCWE5wguNa2JVxvrM04xauikU3B\",\"audience\":\"https://apidemo.auth0.sample\",\"grant_type\":\"client_credentials\"}";
	json.DumpJson("Description");
}

public static class JsonExtensions
{
	public static object DumpJson(this object value, string description = null)
	{
		return GetJsonDumpTarget(value).Dump(description);
	}

	public static object DumpJson(this object value, string description, int depth)
	{
		return GetJsonDumpTarget(value).Dump(description, depth);
	}

	public static object DumpJson(this object value, string description, bool toDataGrid)
	{
		return GetJsonDumpTarget(value).Dump(description, toDataGrid);
	}

	private static object GetJsonDumpTarget(object value)
	{
		object dumpTarget = value;
		//if this is a string that contains a JSON object, do a round-trip serialization to format it:
		var stringValue = value as string;
		if (stringValue != null)
		{
			if (stringValue.Trim().StartsWith("{"))
			{
				var obj = JsonConvert.DeserializeObject(stringValue);
				dumpTarget = JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
			}
			else
			{
				dumpTarget = stringValue;
			}
		}
		else
		{
			dumpTarget = JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.Indented);
		}
		return dumpTarget;
	}
}

public static class ExtMethods
{
	public static JObject DumpPretty(this JObject jo)
	{
		var jsonString = JsonConvert.SerializeObject(jo);
		JsonConvert.DeserializeObject<ExpandoObject>(jsonString).Dump();

		return jo;  // return input in the spirit of LINQPad's Dump() method.
	}
}

// You can also define non-static classes, enums, etc.