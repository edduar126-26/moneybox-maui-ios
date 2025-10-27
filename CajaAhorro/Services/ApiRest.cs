using System;
using System.Collections.Generic;
using System.Text;

namespace Money_Box.Services
{
    public static class ApiRest
    {
       
        private static string sEnvironment = "PROD";
        private static string sBaseUrlProd = "https://casutcecytebcs.tasisaas.com/API/";
        private static string sBaseUrlTest = "http://cajaalupecoqa.tasisaas.com/API/";
        private static string sBaseUrlDev = "http://10.0.2.2:6463/API/";
        private static string sBaseUrlLocal = "https://casutcecytebcs.tasisoft.com/API/";
        private static string sBaseUrlCliente = "https://moneybox.tasisaas.com/API/";

        //configuracion para WebView
        private static string Environment = "Soft";
        private static string sUrlBaseSoft = "https://casutcecytebcs.tasisaas.com";
        private static string sUrlBadeSaas = "https://casutcecytebcs.tasisoft.com";
        private static string sUrlBaseLocal = "http://10.0.2.2:6463";
        public static string sBaseUrlWebApi =>
            sEnvironment == "PROD" ? sBaseUrlProd :
            sEnvironment == "QA" ? sBaseUrlTest :
            sEnvironment == "DEV" ? sBaseUrlDev:
            sEnvironment == "Client"? sBaseUrlCliente:
            sBaseUrlLocal;

        //url de WebView
        public static string sUrlBase =>
            Environment == "Saas" ? sUrlBadeSaas :
            Environment == "Soft" ? sUrlBaseSoft :
            sUrlBaseLocal;
    }

}
