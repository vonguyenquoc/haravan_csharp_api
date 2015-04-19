## Installation

For now, the easiest and only, download the source code and add the project to your solution.

## Haravan API Authorization

In order to understand how haravan authorizes your code to make API calls for a certain haravan customer, I recommend reading this document: [Haravan API Authentication](https://docs.haravan.com/api)

### HaravanAPIAuthorizer

This is the class in this library that will enable your code to quickly authorize your app.

```csharp

    /// <summary>
    /// this class is used to obtain the authorization
    /// from the haravan customer to make api calls on their behalf
    /// </summary>
    public class HaravanAPIAuthorizer
    {
        /// <summary>
        /// Creates an instance of this class in order to obtain the authorization
        /// from the haravan customer to make api calls on their behalf
        /// </summary>
        /// <param name="shopName">name of the shop to make the calls for.</param>
        /// <param name="apiKey">the unique api key of your app (obtained from the partner area when you create an app).</param>
        /// <param name="secret">the secret associated with your api key.</param>
        /// <remarks>make sure that the shop name parameter is the only the subdomain part of the myharavan.com url.</remarks>
        public HaravanAPIAuthorizer(string shopName, string apiKey, string secret, string redirectUrl)

        /// <summary>
        /// Get the URL required by you to redirect the User to in which they will be 
        /// presented with the ability to grant access to your app with the specified scope
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public string GetAuthorizationURL(string[] scope)

        /// <summary>
        /// After the shop owner has authorized your app, Haravan will give you a code.
        /// Use this code to get your authorization state that you will use to make API calls
        /// </summary>
        /// <param name="code">a code given to you by haravan</param>
        /// <returns>Authorization state needed by the API client to make API calls</returns>
        public HaravanAuthorizationState AuthorizeClient(string code)
    }

```

### Using HaravanAPIAuthorizer

This is a quick litte example to show you how you would use the HaravanAPIAuthorizer class

```csharp

	string shopName = "";// get the shop name from the user (i.e. a web form)
	// you will need to pass a URL that will handle the response from Haravan when it passes you the code parameter
	Uri returnURL = new Uri("http://yourappdomain.com/HandleAuthorization");
	var authorizer = new HaravanAPIAuthorizer(shopName, 
		ConfigurationManager.AppSettings["Haravan.ConsumerKey"], // In this case I keep my key and secret in my config file
		ConfigurationManager.AppSettings["Haravan.ConsumerSecret"]);
	
	// get the Authorization URL and redirect the user
	var authUrl = authorizer.GetAuthorizationURL(new string[] { ConfigurationManager.AppSettings["Haravan.Scope"] }, returnURL.ToString());
	Redirect(authUrl);

	// Meanwhile the User is click "yes" to authorize your app for the specified scope.  
	// Once this click, yes or no, they are redirected back to the return URL

	// Handle the haravan response at the Return URL:

	// get the following variables from the Query String of the request
	string code = "";
	string shop = ""; 
	string error = ""; 

	// check for an error first
	if (!String.IsNullOrEmpty(error))
    {
        this.TempData["Error"] = error;
        return RedirectToAction("Login");
    }

	// make sure we have the code
    if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(shop))
        return RedirectToAction("Index", "Home");

    var shopName = shop.Replace(".myharavan.com", String.Empty);
	var authorizer = new HaravanAPIAuthorizer(shopName, 
		ConfigurationManager.AppSettings["Haravan.ConsumerKey"], // In this case I keep my key and secret in my config file
		ConfigurationManager.AppSettings["Haravan.ConsumerSecret"]);

	// get the authorization state
    HaravanAuthorizationState authState = authorizer.AuthorizeClient(code);

    if (authState != null && authState.AccessToken != null)
    {
        // store the auth state in the session or DB to be used for all API calls for the specified shop
    }

```

## Haravan API Usage

In order to use the Haravan API you will have to become intimate knowledge-wise with this documentation: [API Docs](http://api.haravan.com/). It is for that reason that I have purposly designed this class.  You will not be hidden from the URLs of the API or the ways in which the API will require the data to be passed.

Once you have used the HaravanAPIAuthorizer class to get the authorization state you can make API calls.

### Using HaravanAPIClient

Get all Products from the API

```csharp

	HaravanAPIClient api = new HaravanAPIClient(authState);

	// by default JSON string is returned
	object data = api.Get("/admin/products.json");

	// use your favorite JSON library to decode the string into a C# object

```
