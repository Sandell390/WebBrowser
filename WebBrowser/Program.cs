using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace WebBrowser
{
    internal class Program
    {
        // List for HTML tags to search for
        static List<string> HTMLTags = new List<string>() {"h1","span","p","a","button","h2"};

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, Enter an URL (With https prefix)");
            Uri res;

            // User input an URL and it is verifited 
            while (!Uri.TryCreate(Console.ReadLine(), UriKind.Absolute, out res))
            {
                Console.WriteLine("That is not a valid URL");
                Console.WriteLine();
            }

            HttpClient client = new HttpClient();
            try
            {
                Console.Clear();
                Console.WriteLine("Website: " + res.Host);
                Console.WriteLine();
                // Sends a request and gets the response message
                HttpResponseMessage response = await client.GetAsync(res);
                response.EnsureSuccessStatusCode();
                // Reads the response body
                string responseBody = await response.Content.ReadAsStringAsync();

                List<string> HtmlText = new List<string>();

                foreach (string tag in HTMLTags)
                {
                    // Makes a regex filter that searches HTML Tags and puts the innerHTML in groups
                    Regex regex = new Regex($"<{tag}" + "[^\\>]{1,}>([^\\<]{1,})");

                    foreach (Match match in regex.Matches(responseBody))
                    {
                        for (int x = 1; x < match.Groups.Count; x++)
                        {
                            string text = match.Groups[x].Value.Replace("\n", "");
                            //print the names wherever there is a succesful match
                            if (!string.IsNullOrEmpty(text))
                                HtmlText.Add(text);
                        }
                    }
                }

                foreach (string text in HtmlText) { Console.WriteLine(WebUtility.HtmlDecode(text)); }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Could not find the entered website");
            }

        }
    }
}