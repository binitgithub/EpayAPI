using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_Pay_Web_API.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
//using E_Pay_Web_API.MiddleWare.COB

namespace E_Pay_Web_API.MiddleWare
{
    public static class COB
    {
        public static async Task<COBAccount[]> GetMemberAccountList(string memberId, string pin)
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://epay.cobcreditunion.com:443/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.GetAsync("api/Accounts/Accounts/?memnum="+memberId+"&pin="+pin);
            if (response.IsSuccessStatusCode)
            {
                COBAccount[] accounts = await response.Content.ReadAsAsync<COBAccount[]>();
                return accounts.OrderByDescending(p=>p.AcctDesc).ToArray();
            }
            return null;
        }
        public static async Task<string[]> GetAccountBalance(string accountNum, string pin)
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://epay.cobcreditunion.com:443/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.GetAsync("api/Accounts/Balance/?acctnum=" + accountNum + "&pin=" + pin);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    string responseString = await response.Content.ReadAsAsync<string>();
                    string[] balanceSegments = responseString.Split(new string[] { "ITC" }, StringSplitOptions.RemoveEmptyEntries)[0].Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] balance = new string[] { balanceSegments[3], balanceSegments[4] };
                    return balance;
                }
                catch(Exception ex)
                {
                    return new string[] { ex.Message };
                }
            }
            return null;
        }
        public static async Task<string[]> GetAccountHistory(string accountNum, string memId, string pin, string fromDate)
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://epay.cobcreditunion.com:443/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.GetAsync("api/Accounts/TransactionHistory?acctnum="+accountNum+"&memid="+memId+"&pin="+pin+"&fromDate="+fromDate);
            if (response.IsSuccessStatusCode)
            {
                string[] responseString = await response.Content.ReadAsAsync<string[]>();
                return responseString;
            }
            return null;
        }

        public static async Task<string> TransferFunds(E_Pay_Web_API.Helpers.Transfer_Light transfer)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://epay.cobcreditunion.com:443/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(transfer), Encoding.UTF8, "application/json");
            
            HttpResponseMessage response = await client.PostAsync(
                "api/Transfers?"+
                "memberId="+transfer.memid+
                "&pin="+transfer.pin+
                "&fromAcct="+transfer.fromAcct+
                "&toAcct="+transfer.toAccountNumber+
                "&amount="+transfer.amount.ToString(),
                content
                );
            return await response.Content.ReadAsAsync<string>();
        }
    }
    public class COBAccount
    {
        public string AcctNbr;
        public string ProductCategoryCode;
        public string ProductCategoryDesc;
        public string MajorTypeCode;
        public string MajorTypeDesc;
        public string MinorTypeCode;
        public string MinorTypeDesc;
        public string MinorCustDesc;
        public string AcctStatusCode;
        public string AcctStatusDesc;
        public string DateOpened;
        public string AcctDesc;
        public string AcctAnalysisYN;
    }
}