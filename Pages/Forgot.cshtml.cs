using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using s3cr3tx.Models;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web;
using System.Net;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Net.Mail;
//using Microsoft.Data.SqlClient;


namespace s3cr3tx.Pages
{
    //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ForgotModel : PageModel
    {
        
        public string _message = @"";
        //private readonly ILogger<ForgotModel> _logger;
        public s3cr3tx.Models.Forgot forgotCurrent;
        public s3cr3tx.Models.ForgotDbContext ForgotContext;
        public string _output;

        public ForgotModel(s3cr3tx.Models.ForgotDbContext forgotDbContext)
        {
            ForgotContext = forgotDbContext;
            forgotCurrent = new Forgot();
        }

        public void OnPostView()
        {
            try
            {
                HttpRequest Request = HttpContext.Request;
                if (Request.Form.TryGetValue("forgotCurrent.member_email", out Microsoft.Extensions.Primitives.StringValues Email))
                {

                    forgotCurrent.member_email = Email[0].ToLower();
                    string strConnection = @"Data Source=.;Integrated Security=SSPI;Initial Catalog=s3cr3tx";

                    SqlConnection sql3 = new SqlConnection(strConnection);
                    SqlCommand command3 = new SqlCommand();
                    command3.CommandText = @"dbo.usp_tbl_member_sel_email";
                    command3.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter p5 = new SqlParameter(@"email", forgotCurrent.member_email);
                    //SqlParameter p4 = new SqlParameter(@"member_code", strResult);
                    command3.Parameters.Add(p5);
                    //command2.Parameters.Add(p4);
                    DataSet dataSet = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter();
                    using (sql3)
                    {
                        sql3.Open();
                        command3.Connection = sql3;
                        da.SelectCommand = command3;
                        da.Fill(dataSet);
                    }
                    if (dataSet.Tables.Count >0)
                    { 
                    //create member object
                    Member member = new Member()
                    {
                        id = (long)dataSet.Tables[0].Rows[0].ItemArray[0],
                        email = dataSet.Tables[0].Rows[0].ItemArray[1].ToString(),
                        Code = dataSet.Tables[0].Rows[0].ItemArray[2].ToString(),
                        ConfirmCode = @"",
                        FirstName = dataSet.Tables[0].Rows[0].ItemArray[3].ToString(),
                        LastName = dataSet.Tables[0].Rows[0].ItemArray[4].ToString(),
                        regcode = dataSet.Tables[0].Rows[0].ItemArray[5].ToString(),
                        created = (DateTime)dataSet.Tables[0].Rows[0].ItemArray[6],
                        updated = (DateTime)dataSet.Tables[0].Rows[0].ItemArray[7],
                        enabled = (bool)dataSet.Tables[0].Rows[0].ItemArray[8],
                        confirmed = (bool)dataSet.Tables[0].Rows[0].ItemArray[9],
                        country = dataSet.Tables[0].Rows[0].ItemArray[10].ToString(),
                        state = dataSet.Tables[0].Rows[0].ItemArray[11].ToString(),
                        gender = dataSet.Tables[0].Rows[0].ItemArray[12].ToString(),
                        mobile = dataSet.Tables[0].Rows[0].ItemArray[13].ToString(),
                        MobileCarrier = dataSet.Tables[0].Rows[0].ItemArray[14].ToString(),
                        city = dataSet.Tables[0].Rows[0].ItemArray[15].ToString(),
                        zipcode = dataSet.Tables[0].Rows[0].ItemArray[16].ToString(),
                        address = dataSet.Tables[0].Rows[0].ItemArray[17].ToString(),
                        address2 = dataSet.Tables[0].Rows[0].ItemArray[18].ToString()
                    };
                    string strIP = @"";
                    if (HttpContext.Connection.RemoteIpAddress is not null)
                    {
                        IPAddress iP = HttpContext.Connection.RemoteIpAddress;
                        strIP = iP.ToString();
                    }
                    if (member.confirmed.Equals(true) && (member.enabled.Equals(true)))
                    {
                        //store the forgot data
                        SqlConnection sql4 = new SqlConnection(strConnection);
                        SqlCommand command4 = new SqlCommand();
                        command4.CommandText = @"dbo.usp_tbl_forgot_ins";
                        command4.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter p73 = new SqlParameter(@"member_id", member.id);
                        SqlParameter p71 = new SqlParameter(@"member_email", member.email);
                        SqlParameter p72 = new SqlParameter(@"member_token", forgotCurrent.member_code);
                        SqlParameter p74 = new SqlParameter(@"session_start", forgotCurrent.session_start);
                        SqlParameter p77 = new SqlParameter(@"session_end", forgotCurrent.code_expires);
                        SqlParameter p78 = new SqlParameter(@"member_ip", strIP);
                        command4.Parameters.Add(p71);
                        command4.Parameters.Add(p73);
                        command4.Parameters.Add(p72);
                        command4.Parameters.Add(p74);
                        command4.Parameters.Add(p77);
                        command4.Parameters.Add(p78);
                        int lngResult4 ;
                        using (sql4)
                        {
                            sql4.Open();
                            command4.Connection = sql4;
                            lngResult4 = (int)command4.ExecuteNonQuery();
                        }
                        if (lngResult4.Equals(0))
                        {
                            forgotCurrent.Output = @"Something went wrong.";
                            return;
                        }
                        else
                        {


                            Member _member = new Member();
                            MailMessage mail = new MailMessage();
                            mail.From = new MailAddress(@"support@s3cr3tx.com");
                            mail.Sender = new MailAddress(Environment.GetEnvironmentVariable(@"s3cr3tx_mail_usr"));
                            mail.Subject = @"s3cr3tx account verification";
                            mail.To.Add(new MailAddress(forgotCurrent.member_email));
                            mail.IsBodyHtml = true;
                            mail.Body = @"Dear " + member.FirstName + @",<br/><br/>Please click the following link to reset your password: <a href='https://s3cr3tx.com/Freset?=" + System.Web.HttpUtility.UrlEncode(forgotCurrent.member_email) + @"_" + forgotCurrent.member_code + "'>https://s3cr3tx.com/Freset?=" + System.Web.HttpUtility.UrlEncode(forgotCurrent.member_email) + @"_" + forgotCurrent.member_code + @"</a><br/><br/>For your account protection, the link will only be active for 20 minutes. <br/><br/>Thank you!<br/><br/>s3cr3tx support@s3cr3tx.com";
                            SmtpClient smtp = new SmtpClient();
                            smtp.EnableSsl = true;
                            smtp.Host = GetS3cr3txD(Environment.GetEnvironmentVariable(@"s3r3tx_mail_host"));
                            smtp.Credentials = new NetworkCredential(Environment.GetEnvironmentVariable(@"s3cr3tx_mail_usr"),Environment.GetEnvironmentVariable(@"s3cr3tx_mail_wrd"));
                            smtp.Port = 587;
                            smtp.Send(mail);
                            //_member.message = strResult;
                            _output = @"Please check your email for password reset instructions.";
                            //Response.Redirect(@"https://s3cr3tx.com/Login");
                        }
                    }
                    else
                    {
                        _output = @"Please check for an email from us to confirm your account or contact us at support@s3cr3tx.com";
                    }
                }
                else
                {
                    _output = @"Something went wrong please contact us at support@s3cr3tx.com";
                }
            }
                else
            {
                _output = @"Something went wrong please contact us at support@s3cr3tx.com";
            }
        }
            catch (Exception ex)
            {
                //string strResult = @"";
                string strSource = @"s3cr3tx.api.ConfirmPage.OnGet";
                s3cr3tx.Controllers.ValuesController.LogIt(ex.GetBaseException().ToString(), strSource);
                Redirect(@"https://s3cr3tx.com/Login");
            }
        }

        private string GetS3cr3txD(string strS3cr3tx)
        {
            try { 
            string strEmail = Environment.GetEnvironmentVariable(@"s3cr3tx_mail_usr");
            string strAuth = Environment.GetEnvironmentVariable(@"s3cr3tx_a");
            string strCode = Environment.GetEnvironmentVariable(@"s3cr3tx_c");
            WebClient wc = new WebClient();
            wc.BaseAddress = Environment.GetEnvironmentVariable(@"s3cr3tx_api");
            WebHeaderCollection webHeader = new WebHeaderCollection();
            webHeader.Add(@"Email:" + strEmail);
            webHeader.Add(@"AuthCode:" + strCode);
            webHeader.Add(@"APIToken:" + strAuth);
            webHeader.Add(@"Input:" + strS3cr3tx);
            webHeader.Add(@"EorD:" + @"d");
            webHeader.Add(@"Def:" + @"z");
            wc.Headers = webHeader;
            string result = @"";
            result = wc.DownloadString(Environment.GetEnvironmentVariable(@"s3cr3tx_api"));
            return result;
            }
            catch (Exception ex)
            {
                //string strResult = @"";
                string strSource = @"s3cr3tx.api.ConfirmPage.OnGet";
                s3cr3tx.Controllers.ValuesController.LogIt(ex.GetBaseException().ToString(), strSource);
                Redirect(Environment.GetEnvironmentVariable(@"s3cr3tx_plogin"));
                return @"";
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
        private string CreateBundle(string strEmail)
        {
            try { 
            NewK newK = new NewK();
            newK.pd = @"1";
            newK.pd2 = @"1";
            newK.email = strEmail;
            WebClient wc = new WebClient();
            //wc.Credentials.GetCredential();
            wc.BaseAddress = @"https://s3cr3tx.com/Values";
            WebHeaderCollection webHeader = new WebHeaderCollection();
            wc.Headers = webHeader;
            string result = @"";
            webHeader.Add(@"content-type:application/json");
            NewK nk = new NewK();
            nk.email = strEmail;
            nk.pd = @"1";
            nk.pd2 = @"1";
            string strNk = JsonSerializer.Serialize<NewK>(nk);
            result = wc.UploadString(@"https://s3cr3tx.com/Values", strNk);
            return result;
            }
            catch (Exception ex)
            {
                //string strResult = @"";
                string strSource = @"s3cr3tx.api.ConfirmPage.OnGet";
                s3cr3tx.Controllers.ValuesController.LogIt(ex.GetBaseException().ToString(), strSource);
                Redirect(@"https://s3cr3tx.com/Login");
                return @"";
            }
        }


    }
    }
