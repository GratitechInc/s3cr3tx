using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using s3cr3tx.Controllers;
using System.Collections.Specialized;
using System.Reflection.Metadata;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;

namespace s3cr3tx.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValuesController : ControllerBase
    {

        /// <summary>
        /// Gets the Encrypted or Decrypted Text.
        /// </summary>
        /// <remarks>
        /// curl -X "GET" https://localhost:7192/Values --header "accept: text/plain" --header "Email: sales@gratitech.com" --header "APIToken: wrxyw7HDm3vCv+KAugoKwrjLhsOHw6Nlw5AmecO94oCedlnCocOELFnCrGtUIcK/fsOQy4bDuuKAnVUETxLigKFTw5nLhsKrUFItMxfDgMOmFcONwrc8a8O4X8OFw5Ruwqo94oCZ" --header "AuthCode: w4Rrxb7DnRPCqDUAw6ljK8O9w4Zw4oCcNMOawqnDlcKzJR8zbsOlw7zDlFFKP1DCrRImdFVz4oCUw6nDggRSw45/KcKjw7dgYRd8GcK0xb7DlHrigLrCvMOvN1ZdSzo=" --header "EorD: E" --header "Input: Secr3tUs3rnameOrS3cr3tP@Ssw0rd" > output.text
        /// </remarks>
        /// <returns>The Encrypted or Decrypted Text.</returns>
        [HttpGet(Name = "GetS3cr3tx")]
        [Produces("text/plain")]
        public string Get([FromHeader(Name = "Email")] string Email, [FromHeader(Name = "APIToken")] string APIKey, [FromHeader(Name = "AuthCode")] string Acode, [FromHeader(Name = "EorD")] string EorD, [FromHeader(Name = "Input")] string Input)
        {
            try
            {

                //foreach (System.Collections.Generic.KeyValuePair<string,Microsoft.Extensions.Primitives.StringValues> header in Request.Headers)
                //{
                string strEmail = @"";
                if (Request.Headers.TryGetValue(@"Email", out Microsoft.Extensions.Primitives.StringValues svEmail))
                {
                    strEmail = svEmail[0];
                }
                string strAuth = @"";
                if (Request.Headers.TryGetValue(@"AuthCode", out Microsoft.Extensions.Primitives.StringValues svAuth))
                {
                    strAuth = svAuth[0];
                }
                string strToken = @"";
                if (Request.Headers.TryGetValue(@"APIToken", out Microsoft.Extensions.Primitives.StringValues svToken))
                {
                    strToken = svToken[0];
                }
                if (strAuth != strToken)
                {
                    
                    string strInput = @"";
                    if (Request.Headers.TryGetValue(@"Input", out Microsoft.Extensions.Primitives.StringValues svInput))
                    {
                        strInput = svInput[0];
                    }
                    string strEoD = @"";
                    if (Request.Headers.TryGetValue(@"EorD", out Microsoft.Extensions.Primitives.StringValues svEorD))
                    {
                        strEoD = svEorD[0];
                    }
                    bool blnEnc = false;
                    if (strEoD.ToString().ToUpper().StartsWith(@"E"))
                    {
                        blnEnc = true;
                    }
                    string strD = @"";
                    if (Request.Headers.TryGetValue(@"Def", out Microsoft.Extensions.Primitives.StringValues svD))
                    {
                        strD = svD[0];
                    }
                    bool blnD = false;
                    if (strD.ToString().ToUpper().StartsWith(@"T"))
                    {
                        blnD = true;
                    }


                    string strResult = @"";
                    strResult = GetResult(strAuth, strToken, strEmail, strInput, blnEnc, blnD);
                    LogIt(blnEnc.ToString(), strEmail);
                    return strResult;
                }
                else
                {
                    NewK newK = new NewK();
                    newK.email = strEmail;
                    newK.pd = strAuth;
                    newK.pd2 = strToken;
                    string strNK = System.Text.Json.JsonSerializer.Serialize<NewK>(newK);
                    string strResult = Post(newK);
                    LogIt(@"NewKeyGen", strEmail);
                    return strResult;
                }
            }
            catch (Exception ex)
            {
                string strResult = @"";
                string strSource = @"s3cr3tx.api.ValuesController.Get";
                LogIt(ex.GetBaseException().ToString(), strSource);
                return strResult;
            }
        }
        //public String Get([FromHeader(Name = "Email")] string Email, [FromHeader(Name = "EorD")] string EorD,[FromHeader(Name = "Input")] string Input)
        //{
        //    try
        //    {

        //        //foreach (System.Collections.Generic.KeyValuePair<string,Microsoft.Extensions.Primitives.StringValues> header in Request.Headers)
        //        //{
        //        string strEmail = @"";
        //        if (Request.Headers.TryGetValue(@"Email", out Microsoft.Extensions.Primitives.StringValues svEmail))
        //        {
        //            strEmail = svEmail[0];
        //        }
                

        //            string strInput = @"";
        //            if (Request.Headers.TryGetValue(@"Input", out Microsoft.Extensions.Primitives.StringValues svInput))
        //            {
        //                strInput = svInput[0];
        //            }
        //            string strEoD = @"";
        //            if (Request.Headers.TryGetValue(@"EorD", out Microsoft.Extensions.Primitives.StringValues svEorD))
        //            {
        //                strEoD = svEorD[0];
        //            }
        //            bool blnEnc = false;
        //            if (strEoD.ToString().ToUpper().StartsWith(@"E"))
        //            {
        //                blnEnc = true;
        //            }
                    


        //            string strResult = @"";
        //            strResult = GetResult(strEmail, strInput);
        //            LogIt(strResult, @"Result-from ValuesController Get From: " + strEmail + @" using input: " + strInput);
        //            return strResult;
        //        }
        //        else
        //        {
        //            NewK newK = new NewK();
        //            newK.email = strEmail;
        //            newK.pd = strAuth;
        //            newK.pd2 = strToken;
        //            string strNK = System.Text.Json.JsonSerializer.Serialize<NewK>(newK);
        //            string strResult = Post(newK);
        //            return strResult;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string strResult = @"";
        //        string strSource = @"s3cr3tx.api.ValuesController.Get";
        //        LogIt(ex.GetBaseException().ToString(), strSource);
        //        return strResult;
        //    }
        //}

        /// <summary>
        /// Creates a new s3cr3tx API account.</summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Values
        ///     {        
        ///       "email": "sales@gratitech.com",
        ///       "pd": "abc123",
        ///       "pd2": "abc123"        
        ///     }
        /// </remarks>
        /// <returns>The new s3cr3tx API Key and Auth Code for the submitted email address</returns>
        /// <response code="200">Returns the newly created API Key and AuthCode</response>
        /// <response code="400">If the item is null</response>
        // POST: /Values
        [HttpPost(Name = "PostS3cr3tx")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Produces("text/plain")]
        public string Post([FromBody] NewK newAcct)//string jsonString)
        {

            try
            {
                NewK nk = newAcct;//System.Text.Json.JsonSerializer.Deserialize<NewK>(newAcct);
                string strResult = @"";
                string strUserEmail = nk.email;
                //string strUserEmail = data["name"];
                //string strPw = data["pwd"];
                string strPw = nk.pd;
                string strPw2 = nk.pd2;//"28A232D6-4E35-4DDF-88B0-5AC64678FD2E";
                                       //string strPw2 = data["pwd2"];
                string strMpM = @"";
                if (strPw == strPw2)
                {
                    strMpM = strPw;
                }
                //                You will use ajax to send the following formData

                // let formData: FormData;
                //                formData = new FormData();
                //                formData.append('imageFile', imageFile);
                //                formData.append('name', name);
                //                Then you will receive it in your controller like this:

                //public string Post(IFormCollection data, IFormFile imageFile)
                //Then you will access the data as you do normally:

                //            var name = data["name"];
                //strUserEmail = Guid.NewGuid().ToString();
                if ((strUserEmail != @"") && IsValidEmail(strUserEmail) && (strMpM != @""))
                {
                    //do stuff
                    //Authorize user
                    int intAuthResult = Bundle.Authorize(strUserEmail);
                    //create keys and tokens and API-Key and Authorization Header
                    string strGUID1 = Guid.NewGuid().ToString();
                    string strGUID2 = Guid.NewGuid().ToString();
                    string strKey1Pri = @"";
                    string strKey2Pri = @"";
                    string strKey1Pub = @"";
                    string strKey2Pub = @"";
                    string strAuthorizationCode = @"";
                    string strAPIKey = @"";
                    using (RSACryptoServiceProvider rSA = new RSACryptoServiceProvider(4096))
                    {
                        strKey1Pub = System.Convert.ToBase64String(Encoding.Convert(Encoding.GetEncoding(0), Encoding.UTF8, Encoding.GetEncoding(0).GetBytes(rSA.ToXmlString(false))));
                        strKey1Pri = System.Convert.ToBase64String(Encoding.Convert(Encoding.GetEncoding(0), Encoding.UTF8, Encoding.GetEncoding(0).GetBytes(rSA.ToXmlString(true))));
                    }
                    //using (RSACryptoServiceProvider rSA1 = new RSACryptoServiceProvider(4096))
                    //{
                    //    strKey2Pub = System.Convert.ToBase64String(Encoding.Convert(Encoding.GetEncoding(0), Encoding.UTF8, Encoding.GetEncoding(0).GetBytes(rSA1.ToXmlString(false))));
                    //    strKey2Pri = System.Convert.ToBase64String(Encoding.Convert(Encoding.GetEncoding(0), Encoding.UTF8, Encoding.GetEncoding(0).GetBytes(rSA1.ToXmlString(true))));
                    //}
                    string strMpMhash = @"";
                    using (SHA512 sHA512 = SHA512.Create())
                    {
                        byte[] bytHashUserGuid1Guid2mPm = sHA512.ComputeHash(Encoding.Convert(Encoding.GetEncoding(0), Encoding.UTF8, Encoding.GetEncoding(0).GetBytes(strGUID1 + strGUID2 + strUserEmail + strMpM)));
                        byte[] bytHashUsernameGUID2 = sHA512.ComputeHash(Encoding.Convert(Encoding.GetEncoding(0), Encoding.UTF8, Encoding.GetEncoding(0).GetBytes(strGUID2 + strUserEmail)));
                        byte[] bytHashGUID1 = sHA512.ComputeHash(Encoding.Convert(Encoding.GetEncoding(0), Encoding.UTF8, Encoding.GetEncoding(0).GetBytes(strGUID1)));
                        strAuthorizationCode = System.Convert.ToBase64String(Encoding.Convert(Encoding.GetEncoding(0), Encoding.UTF8, bytHashUsernameGUID2));
                        strAPIKey = System.Convert.ToBase64String(Encoding.Convert(Encoding.GetEncoding(0), Encoding.UTF8, bytHashGUID1));
                        strMpMhash = System.Convert.ToBase64String(Encoding.Convert(Encoding.GetEncoding(0), Encoding.UTF8, bytHashUserGuid1Guid2mPm));
                    }
                    Bundle bundleCurrent = new Bundle(strUserEmail, strAPIKey, strAuthorizationCode, strGUID1, strGUID2, strKey1Pub, strKey1Pri, strMpMhash);

                    int didItWork = Bundle.storeBundle(bundleCurrent);
                    if (didItWork.Equals(1))
                    {
                        LogIt(@"KeyGenSuccess", strUserEmail);
                        return @"Your API Key is: " + bundleCurrent.APIKey + @"  Your Auth Code is: " + bundleCurrent.Authorization + @" Please close your browser window and clear your internet cache and history after safely storing your API Key and Auth Code.  Contact Sales@Gratitech.com for support.";
                    }
                    else
                    {
                        LogIt(@"KeyGenFailure", strUserEmail);
                        return @"Something went wrong";
                    }
                }
                return @"Something went wrong";
            }
            catch (Exception ex)
            {
                string strResult = @"";
                string strSource = @"s3cr3tx.api.ValuesController.Get";
                LogIt(ex.GetBaseException().ToString(), strSource);
                return strResult;
            }
}
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                var con = new SqlConnection(@"Data Source=.;Initial Catalog=s3cr3tx;Integrated Security=SSPI");
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = @"dbo.insertLog";
                SqlParameter p1 = new SqlParameter(@"Source", @"s3cr3tx_RegexMatchTimeoutValidEmail");
                SqlParameter p2 = new SqlParameter(@"logMessage", e.GetBaseException().ToString());
                command.Parameters.Add(p1);
                command.Parameters.Add(p2);
                con.Open();
                command.Connection = con;
                int intResult = command.ExecuteNonQuery();
                return false;
            }
            catch (ArgumentException e)
            {
                SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=s3cr3tx;Integrated Security=SSPI");
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = @"dbo.insertLog";
                SqlParameter p1 = new SqlParameter(@"Source", @"s3cr3tx_ArgExValidEmail");
                SqlParameter p2 = new SqlParameter(@"logMessage", e.GetBaseException().ToString());
                command.Parameters.Add(p1);
                command.Parameters.Add(p2);
                con.Open();
                command.Connection = con;
                int intResult = command.ExecuteNonQuery();
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException e)
            {
                SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=s3cr3tx;Integrated Security=SSPI");
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = @"dbo.insertLog";
                SqlParameter p1 = new SqlParameter(@"Source", @"s3cr3tx_RegexMatchTimeoutValidEmail");
                SqlParameter p2 = new SqlParameter(@"logMessage", e.GetBaseException().ToString());
                command.Parameters.Add(p1);
                command.Parameters.Add(p2);
                con.Open();
                command.Connection = con;
                int intResult = command.ExecuteNonQuery();
                return false;
            }
        }
        private static string GetResult(string strAuth, string strToken, string strEmail, string strInput, bool blnEoD, bool blnDefault)
        {
            try
            {
                string strResult = @"";
                string strConnection = @"Data Source=.;Integrated Security=SSPI;Initial Catalog=s3cr3tx";
                SqlConnection sql = new SqlConnection(strConnection);
                SqlCommand command = new SqlCommand();
                command.CommandText = @"dbo.EorD";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter p1 = new SqlParameter(@"Auth", strAuth.Trim());
                SqlParameter p2 = new SqlParameter(@"APIToken", strToken.Trim());
                SqlParameter p3 = new SqlParameter(@"email", strEmail.Trim().ToLower());
                SqlParameter p4 = new SqlParameter(@"input", strInput);
                SqlParameter p5 = new SqlParameter(@"EorD", blnEoD);
                SqlParameter p6 = new SqlParameter(@"blnDefault", blnDefault);
                SqlParameter p7 = new SqlParameter(@"strOutput", System.Data.SqlDbType.NVarChar, -1);
                p7.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(p1);
                command.Parameters.Add(p2);
                command.Parameters.Add(p3);
                command.Parameters.Add(p4);
                command.Parameters.Add(p5);
                command.Parameters.Add(p6);
                command.Parameters.Add(p7);
                using (sql)
                {
                    sql.Open();
                    command.Connection = sql;
                    int result = command.ExecuteNonQuery();
                }
                strResult = (string)p7.Value;
                if (strResult == @"+True")
                {
                    string strRslt = @"";
                    strResult = uspEncDec.EncDec(strEmail, strInput, blnEoD, blnDefault, out strRslt);

                }
                else
                {
                    strResult = @"";
                };
                return strResult;
            }
            catch (Exception ex)
            {
                string strResult = @"";
                string strSource = @"s3cr3tx.api.ValuesController.GetResult";
                LogIt(ex.GetBaseException().ToString(), strSource);
                return strResult;
            }
        }
        private static string GetResultE(string strEmail, string input)
        {
            try
            {
                string strResult = @"";
                strResult = uspEncDec.Enc(strEmail, input);


                return strResult;
            }
            catch (Exception ex)
            {
                string strResult = @"";
                string strSource = @"s3cr3tx.api.ValuesController.GetResultE";
                LogIt(ex.GetBaseException().ToString(), strSource);
                return strResult;
            }
        }
        private static string GetResultD(string strEmail, string input)
        {
            try
            {
                string strResult = @"";
                    strResult = uspEncDec.Dec(strEmail, input);

                
                return strResult;
            }
            catch (Exception ex)
            {
                string strResult = @"";
                string strSource = @"s3cr3tx.api.ValuesController.GetResultD";
                LogIt(ex.GetBaseException().ToString(), strSource);
                return strResult;
            }
        }
        //// GET api/<ValuesController>/5
        //[HttpGet("{id}")]
        //public string Get(string id)
        //{
        //    try
        //    {
        //        string strResult;
        //        string strInput = @"";
        //        if (Request.Headers.TryGetValue(@"Input", out Microsoft.Extensions.Primitives.StringValues svInput))
        //        {
        //            strInput = svInput[0];
        //        }
        //        string strEoD = @"";
        //        if (Request.Headers.TryGetValue(@"EorD", out Microsoft.Extensions.Primitives.StringValues svEorD))
        //        {
        //            strEoD = svEorD[0];
        //        }
        //        if (strEoD.ToUpper().StartsWith(@"E"))
        //        {
        //            strResult = EncResultByID(id, strInput);
        //        }
        //        else
        //        {
        //            strResult = DecResultByID(id, strInput);
        //        }

        //        return strResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        string strResult = @"";
        //        string strSource = @"s3cr3tx.api.ValuesController.Get.id";
        //        LogIt(ex.GetBaseException().ToString(), strSource);
        //        return strResult;
        //    }
        //}

        //// POST api/<ValuesController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<ValuesController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<ValuesController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        public static void LogIt(string strMessage = @"", string strSource = @"")
        { //@"s3cr3tx.api.ValuesController"
            string strConnection = @"Data Source=.;Integrated Security=SSPI;Initial Catalog=s3cr3tx";
            SqlConnection sql = new SqlConnection(strConnection);
            SqlCommand command = new SqlCommand();
            command.CommandText = @"dbo.insertLog";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter p3 = new SqlParameter(@"Source", strSource);
            SqlParameter p4 = new SqlParameter(@"logMessage", strMessage);
            command.Parameters.Add(p3);
            command.Parameters.Add(p4);
            using (sql)
            {
                sql.Open();
                command.Connection = sql;
                int result = command.ExecuteNonQuery();
            }
        }
    }
    public class Bundle
    {
        public Bundle() { }
        public string Email { get; set; }
        public string APIKey { get; set; }
        public string Authorization { get; set; }
        public string Guid1 { get; set; }
        public string Guid2 { get; set; }

        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public string mPmHash { get; set; }

        public Bundle(string strEmail, string strAPIKey, string strAuthorizationCode, string strGuid1, string strGuid2, string strKeyPub, string strKeyPri, string strMpMhash)
        {
            Email = strEmail;
            APIKey = strAPIKey;
            Authorization = strAuthorizationCode;
            Guid1 = strGuid1;
            Guid2 = strGuid2;
            PrivateKey = strKeyPri;
            PublicKey = strKeyPub;
            mPmHash = strMpMhash;
        }
        public static int storeBundle(Bundle bundleCurrent)
        {
            try
            {
                bool result = false;
                //SQLConnection, Command, ExecuteNonQuery
                SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=s3cr3tx;Integrated Security=SSPI");
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = @"dbo.InsertM";
                SqlParameter p1 = new SqlParameter(@"Guid1", bundleCurrent.Guid1);
                SqlParameter p2 = new SqlParameter(@"Guid2", bundleCurrent.Guid2);
                SqlParameter p3 = new SqlParameter(@"Email", bundleCurrent.Email);
                SqlParameter p4 = new SqlParameter(@"APIKey", bundleCurrent.APIKey);
                SqlParameter p5 = new SqlParameter(@"Authorization", bundleCurrent.Authorization);
                SqlParameter p6 = new SqlParameter(@"PrivateKey", bundleCurrent.PrivateKey);
                SqlParameter p7 = new SqlParameter(@"PublicKey", bundleCurrent.PublicKey);
                SqlParameter p8 = new SqlParameter(@"PmHash", bundleCurrent.PublicKey);
                command.Parameters.Add(p1);
                command.Parameters.Add(p2);
                command.Parameters.Add(p3);
                command.Parameters.Add(p4);
                command.Parameters.Add(p5);
                command.Parameters.Add(p6);
                command.Parameters.Add(p7);
                command.Parameters.Add(p8);
                con.Open();
                command.Connection = con;
                int intResult = command.ExecuteNonQuery();
                if (intResult.Equals(1))
                {
                    result = true;
                }

                return intResult;
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.GetBaseException().ToString());

                SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=s3cr3tx;Integrated Security=SSPI");
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = @"dbo.insertLog";
                SqlParameter p1 = new SqlParameter(@"Source", @"s3cr3tx_StoreBundle");
                SqlParameter p2 = new SqlParameter(@"logMessage", ex.GetBaseException().ToString());
                command.Parameters.Add(p1);
                command.Parameters.Add(p2);
                con.Open();
                command.Connection = con;
                int intResult = command.ExecuteNonQuery();
                return -7;

            }
        }
        public static int Authorize(string Email)
        {
            try
            {
                if (IsValidEmail(Email))
                {
                    string strGuid = Guid.NewGuid().ToString();
                    string strCreated = @"ADMIN";
                    bool IsValid = true;
                    SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=s3cr3tx;Integrated Security=SSPI");
                    SqlCommand command = new SqlCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = @"dbo.InsertAuth";
                    SqlParameter p1 = new SqlParameter(@"IsValid", IsValid);
                    SqlParameter p2 = new SqlParameter(@"CreatedBY", strCreated);
                    SqlParameter p3 = new SqlParameter(@"Email", Email);
                    SqlParameter p4 = new SqlParameter(@"AuthCode", strGuid);
                    command.Parameters.Add(p1);
                    command.Parameters.Add(p2);
                    command.Parameters.Add(p3);
                    command.Parameters.Add(p4);
                    con.Open();
                    command.Connection = con;
                    int intResult = command.ExecuteNonQuery();
                    return intResult;
                }
                else
                {

                    return -5;
                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.GetBaseException().ToString());
                SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=s3cr3tx;Integrated Security=SSPI");
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = @"dbo.insertLog";
                SqlParameter p1 = new SqlParameter(@"Source", @"CreateKeysPKs3cr3tx-Bundle_Authorize");
                SqlParameter p2 = new SqlParameter(@"logMessage", ex.GetBaseException().ToString());
                command.Parameters.Add(p1);
                command.Parameters.Add(p2);
                con.Open();
                command.Connection = con;
                int intResult = command.ExecuteNonQuery();

                return -7;
            }
        }
        public static int Authorize2(string Email)
        {
            try
            {
                if (true)//IsValidEmail(Email))
                {
                    string strGuid = Guid.NewGuid().ToString();
                    string strCreated = @"ADMIN";
                    bool IsValid = true;
                    SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=s3cr3tx;Integrated Security=SSPI");
                    SqlCommand command = new SqlCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = @"dbo.InsertAuth";
                    SqlParameter p1 = new SqlParameter(@"IsValid", IsValid);
                    SqlParameter p2 = new SqlParameter(@"CreatedBY", strCreated);
                    SqlParameter p3 = new SqlParameter(@"Email", Email);
                    SqlParameter p4 = new SqlParameter(@"AuthCode", strGuid);
                    command.Parameters.Add(p1);
                    command.Parameters.Add(p2);
                    command.Parameters.Add(p3);
                    command.Parameters.Add(p4);
                    con.Open();
                    command.Connection = con;
                    int intResult = command.ExecuteNonQuery();
                    return intResult;
                }
                else
                {

                    return -5;
                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.GetBaseException().ToString());
                SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=s3cr3tx;Integrated Security=SSPI");
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = @"dbo.insertLog";
                SqlParameter p1 = new SqlParameter(@"Source", @"CreateKeysPKs3cr3tx-Bundle_Authorize");
                SqlParameter p2 = new SqlParameter(@"logMessage", ex.GetBaseException().ToString());
                command.Parameters.Add(p1);
                command.Parameters.Add(p2);
                con.Open();
                command.Connection = con;
                int intResult = command.ExecuteNonQuery();

                return -7;
            }
        }
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=s3cr3tx;Integrated Security=SSPI");
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = @"dbo.insertLog";
                SqlParameter p1 = new SqlParameter(@"Source", @"s3cr3tx_Console_IsValidEmailRegexMatchTimeout");
                SqlParameter p2 = new SqlParameter(@"logMessage", e.GetBaseException().ToString());
                command.Parameters.Add(p1);
                command.Parameters.Add(p2);
                con.Open();
                command.Connection = con;
                int intResult = command.ExecuteNonQuery();
                return false;
            }
            catch (ArgumentException e)
            {
                SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=s3cr3tx;Integrated Security=SSPI");
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = @"dbo.InsertM";
                SqlParameter p1 = new SqlParameter(@"Source", @"s3cr3tx_Console_IsValidEmailArgumentException");
                SqlParameter p2 = new SqlParameter(@"logMessage", e.GetBaseException().ToString());
                command.Parameters.Add(p1);
                command.Parameters.Add(p2);
                con.Open();
                command.Connection = con;
                int intResult = command.ExecuteNonQuery();
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException e)
            {
                SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=s3cr3tx;Integrated Security=SSPI");
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = @"dbo.insertLog";
                SqlParameter p1 = new SqlParameter(@"Source", @"s3cr3tx_Console_IsValidEmail-RegexMatchTimeout");
                SqlParameter p2 = new SqlParameter(@"logMessage", e.GetBaseException().ToString());
                command.Parameters.Add(p1);
                command.Parameters.Add(p2);
                con.Open();
                command.Connection = con;
                int intResult = command.ExecuteNonQuery();
                return false;
            }
        }
        public static Bundle GetBundle(string Email, string APIKey, string AuthCode)
        {
            try
            {
                Bundle bundleCurrent = new Bundle();
                if (IsValidEmail(Email))
                {
                    SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=s3cr3tx;Integrated Security=SSPI");
                    SqlCommand command = new SqlCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = @"dbo.USP_GetBundle_M";
                    SqlParameter p1 = new SqlParameter(@"mAuthorization", AuthCode);
                    SqlParameter p2 = new SqlParameter(@"mAPIKey", APIKey);
                    SqlParameter p3 = new SqlParameter(@"mEmail", Email);
                    command.Parameters.Add(p1);
                    command.Parameters.Add(p2);
                    command.Parameters.Add(p3);
                    con.Open();
                    command.Connection = con;
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(ds);
                    //Bundle bundleCurrent = new Bundle();
                    bundleCurrent.Guid1 = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                    bundleCurrent.Guid2 = ds.Tables[0].Rows[0].ItemArray[2].ToString();
                    bundleCurrent.Email = ds.Tables[0].Rows[0].ItemArray[3].ToString();
                    bundleCurrent.APIKey = ds.Tables[0].Rows[0].ItemArray[4].ToString();
                    bundleCurrent.Authorization = ds.Tables[0].Rows[0].ItemArray[5].ToString();
                    bundleCurrent.PrivateKey = ds.Tables[0].Rows[0].ItemArray[6].ToString();
                    bundleCurrent.PublicKey = ds.Tables[0].Rows[0].ItemArray[7].ToString();

                }
                return bundleCurrent;
            }
            catch (Exception ex)
            {
                Bundle bunEmpty = new Bundle();
                //Console.WriteLine(@"Error: " + ex.GetBaseException().ToString());
                SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=s3cr3tx;Integrated Security=SSPI");
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = @"dbo.insertLog";
                SqlParameter p1 = new SqlParameter(@"Source", @"s3cr3txConsole_GetBundle");
                SqlParameter p2 = new SqlParameter(@"logMessage", ex.GetBaseException().ToString());
                command.Parameters.Add(p1);
                command.Parameters.Add(p2);
                con.Open();
                command.Connection = con;
                int intResult = command.ExecuteNonQuery();
                return bunEmpty;
            }
        }
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        public static Bundle GetBundle(string Email)
        {
            try
            {
                Bundle bundleCurrent = new Bundle();
                if (IsValidEmail(Email))
                {
                    SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=s3cr3tx;Integrated Security=SSPI");
                    SqlCommand command = new SqlCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = @"dbo.USP_GetBundle_M_Host";
                    string currentHost = System.Environment.MachineName;
                    string currentIp = @"";
                    if (currentHost.Equals(@"Ps-MacBook-Air.local"))
                    {
                        currentIp = @"127.0.0.1";
                    }
                    else
                    {
                        currentIp = GetLocalIPAddress();
                    }
                    SqlParameter p2 = new SqlParameter(@"mHost", currentHost + @"@" + currentIp + @".org");
                    SqlParameter p3 = new SqlParameter(@"mEmail", Email);
                    command.Parameters.Add(p2);
                    command.Parameters.Add(p3);
                    con.Open();
                    command.Connection = con;
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(ds);
                    //Bundle bundleCurrent = new Bundle();
                    bundleCurrent.Guid1 = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                    bundleCurrent.Guid2 = ds.Tables[0].Rows[0].ItemArray[2].ToString();
                    bundleCurrent.Email = ds.Tables[0].Rows[0].ItemArray[3].ToString();
                    bundleCurrent.APIKey = ds.Tables[0].Rows[0].ItemArray[4].ToString();
                    bundleCurrent.Authorization = ds.Tables[0].Rows[0].ItemArray[5].ToString();
                    bundleCurrent.PrivateKey = ds.Tables[0].Rows[0].ItemArray[6].ToString();
                    bundleCurrent.PublicKey = ds.Tables[0].Rows[0].ItemArray[7].ToString();

                }
                return bundleCurrent;
            }
            catch (Exception ex)
            {
                Bundle bunEmpty = new Bundle();
                //Console.WriteLine(@"Error: " + ex.GetBaseException().ToString());
                SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=s3cr3tx;Integrated Security=SSPI");
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = @"dbo.insertLog";
                SqlParameter p1 = new SqlParameter(@"Source", @"s3cr3txConsole_GetBundle");
                SqlParameter p2 = new SqlParameter(@"logMessage", ex.GetBaseException().ToString());
                command.Parameters.Add(p1);
                command.Parameters.Add(p2);
                con.Open();
                command.Connection = con;
                int intResult = command.ExecuteNonQuery();
                return bunEmpty;
            }
        }
    }
    public class uspEncDec
    {
        //[Microsoft.SqlServer.Server.SqlProcedure]
        public static string EncDec(string email, string input, bool EorD, bool blnDefault, out string strText) //, out string text)
        {
            try
            {
                string result = "";
                if (blnDefault)
                {
                    ebundle eb = ebundle.GetEbundle(@"support@s3cr3tx.com");
                    if (eb.kyp is not null && !eb.kyp.Equals(@""))
                    {
                        if (EorD)
                        {
                            using (RSACryptoServiceProvider rSA1 = new RSACryptoServiceProvider(4096))
                            {
                                rSA1.FromXmlString(eb.kyp);
                                byte[] bytInput = System.Text.Encoding.Convert(System.Text.Encoding.GetEncoding(0), System.Text.Encoding.UTF8, System.Text.Encoding.GetEncoding(0).GetBytes(input));
                                byte[] pubEnc = rSA1.Encrypt(bytInput, true);
                                string strB64 = System.Convert.ToBase64String(pubEnc);
                                result = strB64;
                            }
                        }
                        else
                        {
                            string strB64Pri = eb.ky;
                            string strPriXML = System.Text.Encoding.GetEncoding(0).GetString(System.Text.Encoding.Convert(System.Text.Encoding.UTF8, System.Text.Encoding.GetEncoding(0), System.Convert.FromBase64String(strB64Pri)));
                            RSACryptoServiceProvider rSA2 = new RSACryptoServiceProvider(4096);
                            rSA2.FromXmlString(strPriXML);
                            string stringDec = System.Text.Encoding.GetEncoding(0).GetString(System.Text.Encoding.Convert(System.Text.Encoding.UTF8, System.Text.Encoding.GetEncoding(0), (rSA2.Decrypt(System.Convert.FromBase64String(input), true))));
                            result = stringDec;
                        }
                    }
                }
                else if (EorD)
                {
                    ebundle eb = ebundle.GetEbundle(email);
                    if (eb.kyp is not null && !eb.kyp.Equals(@""))
                    {
                        using (RSACryptoServiceProvider rSA1 = new RSACryptoServiceProvider(4096))
                        {
                            rSA1.FromXmlString(eb.kyp);
                            byte[] bytInput = System.Text.Encoding.Convert(System.Text.Encoding.GetEncoding(0), System.Text.Encoding.UTF8, System.Text.Encoding.GetEncoding(0).GetBytes(input));
                            byte[] pubEnc = rSA1.Encrypt(bytInput, true);
                            string strB64 = System.Convert.ToBase64String(pubEnc);
                            result = strB64;
                        }
                    }
                }
                else
                {
                    ebundle eb = ebundle.GetEbundle(email);
                    if (eb.kyp is not null && !eb.kyp.Equals(@""))
                    {
                        string strB64Pri = eb.ky;
                        //string strPriXML = System.Text.Encoding.GetEncoding(0).GetString(System.Text.Encoding.Convert(System.Text.Encoding.UTF8, System.Text.Encoding.GetEncoding(0), System.Convert.FromBase64String(strB64Pri)));
                        RSACryptoServiceProvider rSA2 = new RSACryptoServiceProvider(4096);
                        rSA2.FromXmlString(strB64Pri);
                        string stringDec = System.Text.Encoding.GetEncoding(0).GetString(System.Text.Encoding.Convert(System.Text.Encoding.UTF8, System.Text.Encoding.GetEncoding(0), (rSA2.Decrypt(System.Convert.FromBase64String(input), true))));
                        result = stringDec;
                    }
                }
                strText = result;
                return result;
                //text = result;
            }
            catch (Exception ex)
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=s3cr3tx;Integrated Security=SSPI"))
                {

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandText = @"dbo.insertLog";
                        command.CommandType = CommandType.StoredProcedure;
                        SqlParameter p1 = new SqlParameter(@"source", @"EncDec_Sproc");
                        SqlParameter p2 = new SqlParameter(@"logMessage", ex.GetBaseException().ToString());
                        con.Open();
                        command.Connection = con;
                        command.Parameters.Add(p1);
                        command.Parameters.Add(p2);
                        int i = command.ExecuteNonQuery();
                        strText = @"";
                        return strText;
                    }
                }
            }
        }
        public static string Enc(string email, string input) //, out string text)
        {
            try
            {
                string result = @"";
                ebundle eb = ebundle.GetEbundle(email);
                if (eb.kyp is not null && !eb.kyp.Equals(@""))
                {
                    using (RSACryptoServiceProvider rSA1 = new RSACryptoServiceProvider(4096))
                    {
                        rSA1.FromXmlString(eb.kyp);
                        byte[] bytInput = System.Text.Encoding.Convert(System.Text.Encoding.GetEncoding(0), System.Text.Encoding.UTF8, System.Text.Encoding.GetEncoding(0).GetBytes(input));
                        byte[] pubEnc = rSA1.Encrypt(bytInput, true);
                        string strB64 = System.Convert.ToBase64String(pubEnc);
                        result = strB64;
                    }
                }
                return result;
               
            }
            catch (Exception ex)
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=s3cr3tx;Integrated Security=SSPI"))
                {

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandText = @"dbo.insertLog";
                        command.CommandType = CommandType.StoredProcedure;
                        SqlParameter p1 = new SqlParameter(@"source", @"Enc_Sproc");
                        SqlParameter p2 = new SqlParameter(@"logMessage", ex.GetBaseException().ToString());
                        con.Open();
                        command.Connection = con;
                        command.Parameters.Add(p1);
                        command.Parameters.Add(p2);
                        int i = command.ExecuteNonQuery();
                        string strText = @"";
                        return strText;
                    }
                }
            }
        }
        public static string Dec(string email, string input) //, out string text)
        {
            try
            {
                string result = @"";
                ebundle eb = ebundle.GetEbundle(email);
                if (eb.ky is not null && !eb.ky.Equals(@""))
                {
                    string strB64Pri = eb.ky;
                    //string strPriXML = System.Text.Encoding.GetEncoding(0).GetString(System.Text.Encoding.Convert(System.Text.Encoding.UTF8, System.Text.Encoding.GetEncoding(0), System.Convert.FromBase64String(strB64Pri)));
                    RSACryptoServiceProvider rSA2 = new RSACryptoServiceProvider(4096);
                    rSA2.FromXmlString(strB64Pri);
                    string stringDec = System.Text.Encoding.GetEncoding(0).GetString(System.Text.Encoding.Convert(System.Text.Encoding.UTF8, System.Text.Encoding.GetEncoding(0), (rSA2.Decrypt(System.Convert.FromBase64String(input), true))));
                    result = stringDec;
                }

                return result;
                //text = result;
            }
            catch (Exception ex)
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=s3cr3tx;Integrated Security=SSPI"))
                {

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.CommandText = @"dbo.insertLog";
                        command.CommandType = CommandType.StoredProcedure;
                        SqlParameter p1 = new SqlParameter(@"source", @"Dec_Sproc");
                        SqlParameter p2 = new SqlParameter(@"logMessage", ex.GetBaseException().ToString());
                        con.Open();
                        command.Connection = con;
                        command.Parameters.Add(p1);
                        command.Parameters.Add(p2);
                        int i = command.ExecuteNonQuery();
                        string strText = @"";
                        return strText;
                    }
                }
            }
        }
    }
    
    public class NewK
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string pd { get; set; }
        [Required]
        public string pd2 { get; set; }
    }
    class ebundle
    {
        public string strG1;
        public string strG2;
        public string strapikey;
        public string strauth;
        public string ky;
        public string kyp;
        public ebundle() { }

        public ebundle(string G1, string G2, string apikey, string auth, string strKy, string strKyp)
        {
            strG1 = G1;
            strG2 = G2;
            strapikey = apikey;
            strauth = auth;
            ky = strKy;
            kyp = strKyp;
        }

        public static ebundle GetEbundle(string email)
        {
            using (SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=s3cr3tx;Integrated Security=SSPI"))
            {
                //string strConnection = "Server=localhost;Database=S;User Id=sa;Password=Sunsh1n3-20p;";
                //SqlConnection con = new SqlConnection(strConnection);
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = @"dbo.getBundle";
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter p1 = new SqlParameter(@"email", email);
                    con.Open();
                    command.Connection = con;
                    command.Parameters.Add(p1);
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(ds);
                    ebundle eb = new ebundle();
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            eb.strapikey = ds.Tables[0].Rows[0].ItemArray[4].ToString();
                            eb.strauth = ds.Tables[0].Rows[0].ItemArray[5].ToString();
                            eb.ky = Encoding.GetEncoding(0).GetString(Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(0), Convert.FromBase64String(ds.Tables[0].Rows[0].ItemArray[6].ToString().Trim())));
                            eb.kyp = Encoding.GetEncoding(0).GetString(Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(0), Convert.FromBase64String(ds.Tables[0].Rows[0].ItemArray[7].ToString().Trim())));
                            eb.strG2 = ds.Tables[0].Rows[0].ItemArray[2].ToString();

                        }
                    }
                    return eb;
                }
            }
        }
        //public class CustomHeaderSwaggerAttribute : IOperationFilter
        //{

        //    public void Apply(OpenApiOperation operation, OperationFilterContext context)
        //    {
        //        if (operation.Parameters == null)
        //            operation.Parameters = new List<OpenApiParameter>();

        //        operation.Parameters.Add(new OpenApiParameter
        //        {
        //            Name = "Email",
        //            In = ParameterLocation.Header,
        //            Required = true,
        //            Schema = new OpenApiSchema
        //            {
        //                Type = "string"
        //            }
        //        });
        //        operation.Parameters.Add(new OpenApiParameter
        //        {
        //            Name = "APIToken",
        //            In = ParameterLocation.Header,
        //            Required = true,
        //            Schema = new OpenApiSchema
        //            {
        //                Type = "string"
        //            }
        //        });
        //        operation.Parameters.Add(new OpenApiParameter
        //        {
        //            Name = "AuthCode",
        //            In = ParameterLocation.Header,
        //            Required = true,
        //            Schema = new OpenApiSchema
        //            {
        //                Type = "string"
        //            }
        //        });
        //        operation.Parameters.Add(new OpenApiParameter
        //        {
        //            Name = "EorD",
        //            In = ParameterLocation.Header,
        //            Required = true,
        //            Schema = new OpenApiSchema
        //            {
        //                Type = "string"
        //            }
        //        });
        //        operation.Parameters.Add(new OpenApiParameter
        //        {
        //            Name = "Input",
        //            In = ParameterLocation.Header,
        //            Required = true,
        //            Schema = new OpenApiSchema
        //            {
        //                Type = "string"
        //            }
        //        });
        //    }

        //}
    }
}
