﻿using System;
using NLog;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terminal_Firefox.Utils;

namespace Terminal_Firefox.classes {
    class Command {

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();


        public static string Prepare(CommandTypes act, object obj) {
            try {
            Message msg = new Message {
                                          login = TerminalSettings.Instance.TerminalNumber,
                                          passw = TerminalSettings.Instance.TerminalPassword,
                                          act = ((int) act).ToString(),
                                          body = JsonConvert.SerializeObject(obj)
                                      };
            return JsonConvert.SerializeObject(msg);

            } catch (Exception exception) {
                Log.Error(exception);
                return "";
            }
        }

        internal static void HandleAnswer(string p) {
            try {
            // act = 0 (Payment) {"status": "0", "hesh_id": "732a4dbf-1289-4259-b163-35e6b74248a3", "act": "0" }
            JObject jObj = JObject.Parse(p);
            byte status;
            byte.TryParse(jObj.Property("status").Value.ToString(), out status);
            if (status == 0) {
                Payment.FinishPayment(jObj.SelectToken("hesh_id").ToString());
            }

            } catch (Exception exception) {
                Log.Error(exception);
            }
        }
    }
}
