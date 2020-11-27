using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Pay_Web_API.MiddleWare.Digicel
{
    public static class DigicelTopUp
    {
        public static DigicelTopUpResponse TopUp(string territory,string mobileNumber, decimal amount)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                (se, cert, chain, sslerror) =>
                {
                    return true;
                };

            int denominationTypeId = 1;
            if (!territory.Equals("bb"))
            {
                denominationTypeId = 2;
            }
            DigicelTopUpResponse topUpResponse = new DigicelTopUpResponse();
            //mobileNumber = "00" + mobileNumber;
            EPINServices svc = new EPINServices();
            DTOTopUpConfigRequest configRequest = new DTOTopUpConfigRequest();
            //configRequest.userId = "esolutions";
            //configRequest.userPassword = "Test2016!";
            configRequest.userId = "COBEPAYWEB";
            configRequest.userPassword = "DigiBBD2017!";
            configRequest.language = "EN";
            configRequest.utfi = "SU1L34C902";
            //Original Value - Replaced for testing
            //configRequest.interfaceId = 2;
            configRequest.interfaceId = 2;
            //CrossBorder TopUps
            //configRequest.denominationTypeId = 2;
            //Local TopUps
            //configRequest.denominationTypeId = 1;
            configRequest.denominationTypeId = denominationTypeId;
            configRequest.location = "13.67.217.101";
            configRequest.interfaceIdSpecified = true;
            configRequest.denominationTypeIdSpecified = true;
            svc.Credentials = new NetworkCredential("COBEPAYWEB", "DigiBBD2017!");
            DTOTopUpConfigResponse configResponse = svc.configTopUpService(configRequest);
            //return configResponse;
            if (configResponse.code == "0")
            {
                DTOFlexibleDenomination flexibleDenomination = configResponse.flexibleDenomination[0];
                DTOTopUpPreRequest preRequest = new DTOTopUpPreRequest();
                preRequest.userId = "COBEPAYWEB";
                preRequest.userPassword = "DigiBBD2017!";
                preRequest.language = "EN";
                preRequest.utfi = "SU1L34C902";
                preRequest.interfaceId = 2;
                preRequest.location = "13.67.217.101";
                preRequest.amount = amount;
                preRequest.amountSpecified = true;
                preRequest.denominationId = flexibleDenomination.id;
                preRequest.denominationServer = flexibleDenomination.denominationServer;
                preRequest.targetMSISDN = mobileNumber;
                preRequest.interfaceIdSpecified = true;
                DTOTopUpPreResponse preResponse = svc.preTopUpService(preRequest);
                if (preResponse.code == "0")
                {
                    DTOTopUpRequest request = new DTOTopUpRequest();
                    request.userId = "COBEPAYWEB";
                    request.userPassword = "DigiBBD2017!";
                    request.language = "EN";
                    request.utfi = "SU1L34C902";
                    request.interfaceId = 2;
                    request.location = "13.67.217.101";
                    request.targetMSISDN = mobileNumber;
                    request.denominationId = flexibleDenomination.id;
                    request.denominationServer = flexibleDenomination.denominationServer;
                    request.amount = amount;
                    request.amountSpecified = true;
                    request.externalTransactionId = DateTime.Now.Ticks.ToString();
                    request.interfaceIdSpecified = true;
                    DTOTopUpResponse response = svc.topUpService(request);
                    topUpResponse.Code = response.code;
                    topUpResponse.Message = response.message;
                    topUpResponse.TransactionID = response.transactionId;
                }
                else
                {
                    topUpResponse.Code = preResponse.code;
                    topUpResponse.Message = preResponse.message;
                }
            }
            else
            {
                topUpResponse.Code = configResponse.code;
                topUpResponse.Message = configResponse.message;
            }
            return topUpResponse;
        }
    }

    public class DigicelTopUpResponse
    {
        public string Code;
        public string Message;
        public string TransactionID;
    }
}