using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using WorkItemFields;

string jsonContent = "";
ILogger log = null;

// Read the Json request file generated from webhooks, sample file stored at below path.
using (StreamReader reader = new StreamReader(@"C:\Umakant\Azure\AZ-204\PracticeProjects\PracticeSQLApp\WorkItemFields\webhookJsonRequest.json"))
{
    jsonContent = reader.ReadToEnd();
}

JObject originalWI = JObject.Parse(jsonContent);

string WIT_TYPE = Uri.UnescapeDataString((string)originalWI["resource"]["fields"]["System.WorkItemType"]);
string title = Uri.UnescapeDataString((string)originalWI["resource"]["fields"]["System.Title"]);
string description = Uri.UnescapeDataString((string)originalWI["resource"]["fields"]["System.Description"]);
string areaPath = Uri.UnescapeDataString((string)originalWI["resource"]["fields"]["System.AreaPath"]);
string iterationPath = Uri.UnescapeDataString((string)originalWI["resource"]["fields"]["System.IterationPath"]);
string workItemID = (string)originalWI["resource"]["id"];
string workItemType = "$" + WIT_TYPE;


// Create and initialize HttpClient instance.
HttpClient client = new HttpClient();

// Set Media Type of Response.
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

// Generate base64 encoded authorization header.
client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", Azure.PAT))));

// Build the URI for creating Work Item.
string uri = String.Join("?", String.Join("/", Azure.BASE, Azure.ORG, Azure.PROJECT, "_apis/wit/workitems", workItemType), Azure.API);

var jsonPatch = new JsonPatch();
foreach (JProperty property in ((JObject)originalWI["resource"]["fields"]).Properties())
{
    //Console.WriteLine(property.Name + " - " + Utility.JSONEncode(property.Value.ToString()));
    jsonPatch.ParseAndAddProperty(property);
}


//string json = "[{ 'op': 'add', 'path': '/fields/System.Title', 'value':'" + title +
//                 "'},{ 'op': 'add', 'path': '/fields/System.Description', 'value': '" + description + "'}]";

Console.WriteLine(jsonPatch.ToString());
HttpContent content = new StringContent(jsonPatch.ToString(), Encoding.UTF8, "application/json-patch+json");

// Call CreateWIT method.
string result = Utility.CreateWIT(client, uri, content, log).Result;


Console.ReadLine();


