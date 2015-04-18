using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HaravanAPIAdapterLibrary;
using System.Net;

namespace UnitTest
{
    [TestClass]
    public class ApiTest
    {
        [TestMethod]
        public void AuthorizerTest()
        {
            var redirect = "http://localhost:8000/finalize";

            var authorizer = new HaravanAPIAuthorizer("mrshop", "317d665f79f81602907ea633c125e6a9", "41469500e03a9ffd9d7b9df38a5710cd", redirect);

            var urlauthorize = authorizer.GetAuthorizationURL(new string[] { "read_products", "write_products" });

            var code = "7a42ebd12bf6498a915fbb368b6bbb0518ed730ea2184d4197b0cb1fa39e2554";

            var authorizeState = authorizer.AuthorizeClient(code);
        }


        [TestMethod]
        public void TestApi()
        {
            var authorizeState = new HaravanAuthorizationState() { ShopName = "mrshop", AccessToken = "LNniFLi1NXES-h9W2BSO0G_OS-WADl1eoKISLj4TehkeqYHRDU77rOAPVA4S90N_fT4qmJGjOT_8Kc4bF0xzr7EvIX-vpfJJFZv5ZXnsXfx2f9n3uTmA1yu4ck1yqy7c6AVLW9mp8OWNgL2q9uWHSe02XnPsDGfjNJDBM_bdbe6u2lTwwTZsIOR9uZxHpxBQli1JgzYz9HhTz6dxGPLpMRGBwAUVw2FbgVYMjRgkL3mtdk9RobtrHS1td1ibMeXtjR2UR3fT2JQyxTMSmCG8ynjVztGWn6kZJiYThZm2C6NYk1dXlK70pW4R1_esBSY6Y02DhuPUPMKc9Ss6fnfZ_AXhqfrPkDYVdR6MHfAfmSgCX9jQc3txaPEONY0uL1j55buStpGE_2VuxGYeikujwNHNaKxFt5bWBbTgNnMMKgB7HVmhUZXCPfWSkweJXCr5XsBGqaR2hYhdZ3dsZXNSdZgTBSHL1bxrSsILclutxxc9vADW" };
            var client = new HaravanAPIClient(authorizeState);
            var obj = client.Get("/admin/products.json");
        }
    }
}
