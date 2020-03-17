using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Common.Utilities
{
    public class GeneralService
    {
        public static void PushNotification(NotificationRequest req)
        {
            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            try
            {

                var applicationID = "AAAAvGg0-wQ:APA91bGzC_SCTCqoNHakDVyKtnakLznCwOK9NfITrRHlStK1Dppl_mz52TRCsBmN1-7c-SiWSeRHivwSZB0BPGXH18fMACQ_7Rv7Cl5S6HQGK6GMW45pyFfYr2g4A_OHJKDxnx3JOk7m";
                var senderId = "809202154244";
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";


                var data = GetData(req);
                var json = JsonConvert.SerializeObject(data);
                var byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add($"Authorization: key={applicationID}");
                tRequest.Headers.Add($"Sender: id={senderId}");
                tRequest.ContentLength = byteArray.Length;
                using (var dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (var tResponse = tRequest.GetResponse())
                    {
                        using (var dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (new StreamReader(dataStreamResponse ?? throw new InvalidOperationException()))
                            {
                                // var sResponseFromServer = tReader.ReadToEnd();
                                // var str = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

        private static object GetData(NotificationRequest req)
        {
            var min = 1000;
            var max = 1000000000;
            var rdm = new Random();

            if (req.ids.Count == 1)
            {
                var data = new
                {
                    to = string.Join(",", req.ids),
                    priority = "high",
                    data = new
                    {
                        req.body,
                        //ActivityName = req.noteType == NoteType.OpenActivity ? req.activityName : null,//"com.cafe_book.cafebook.ui.activities.AdActivity"
                        //URLToOpen = req.noteType == NoteType.OpenURL ? req.url : null,
                        ImageURL = !string.IsNullOrWhiteSpace(req.imageUrl) ? req.imageUrl : "",
                        req.title,
                        CatId = req.catId,
                        TRId = req.trId,
                        //NoteType = req.noteType,
                        sound = "Enabled",
                        NotificationId = rdm.Next(min, max),
                        FragmentName = req.fragmentName,
                        Tag = req.tag,
                        req.sType
                    }
                };
                return data;
            }
            else
            {
                var data = new
                {
                    registration_ids = req.ids,
                    priority = "high",
                    data = new
                    {
                        req.body,
                        //ActivityName = req.activityName,
                        //URLToOpen = req.noteType == NoteType.OpenURL ? req.url : null,
                        ImageURL = !string.IsNullOrWhiteSpace(req.imageUrl) ? req.imageUrl : "",
                        req.title,
                        CatId = req.catId,
                        TRId = req.trId,
                        //NoteType = req.noteType,
                        sound = "Enabled",
                        NotificationId = rdm.Next(min, max),
                        FragmentName = req.fragmentName,
                        Tag = req.tag,
                        req.sType
                    }

                };
                return data;
            }
        }
    }
}
