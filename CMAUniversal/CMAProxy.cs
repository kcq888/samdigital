using LuisParser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Calls;
using Windows.Data.Json;
using Windows.Foundation.Metadata;
using Windows.Media.SpeechSynthesis;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Geolocation;

namespace CMAUniversal
{
    public class CMAProxy
    {
        private static CMAProxy _instance = null;

        public static CMAProxy Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CMAProxy();
                }

                return _instance;
            }
        }

        public Frame CurrentFrame { get; set; }

        public void AnalyzeTextCommand(string text)
        {
            SymptomPhrase = text;
        }

        public async Task<bool> RetrieveUserInfo()
        {
            var uri = new Uri("http://cmsuserappwebservice.azurewebsites.net/api/User/Account/f7213315-4761-4d08-a870-101f7857e34e");
            var httpClient = new HttpClient();

            // Always catch network exceptions for async methods
            try
            {
                var result = await httpClient.GetStringAsync(uri);
                CurrentUser = JsonConvert.DeserializeObject<CmaBusiness.User>(result);
                CurrentUser.ImageUrl = "https://s-media-cache-ak0.pinimg.com/236x/33/c9/6a/33c96a6971b18c297c5435984d61f7de.jpg";
            }
            catch (Exception e)
            {
                Debug.Write(e.StackTrace);
            }

            return true;
        }

        public async void RetrieveIntentEntities()
        {
            var uri = new Uri("https://api.projectoxford.ai/luis/v1/application?id=24212ec1-a34b-4af1-9e3c-0e98b3cbf851&subscription-key=4e8b31c0835c4fd89a60d7f6d89f650d&q=" + SymptomPhrase);
            var httpClient = new HttpClient();

            // Always catch network exceptions for async methods
            try
            {
                var result = await httpClient.GetStringAsync(uri);
                Response = JsonConvert.DeserializeObject<LuisResponse>(result);
                ResultsList = new ObservableCollection<ResultModel>()
                {
                    new ResultModel
                    {
                        ImgUrl = "http://h2oh.us/wp-content/uploads/2015/05/teardropLogo00ffc84c79876dd3a5948a162667550d-thumb_large.jpg",
                        Name = "CVS Pharmacy",
                        Description = "Medications / Pharmacy",
                        Position = new { Latitude = "37.7896457", Longitude = "-122.4009613" }
                    },
                    new ResultModel
                    {
                        ImgUrl = "http://2.bp.blogspot.com/-YGHsWEcsThE/TcgZ-PhKEeI/AAAAAAAAAB0/-5fyuufaDBY/s1600/Walgreens-logo.jpg",
                        Name = "Walgreens",
                        Description = "Pharmacy / Personal Items / Medications",
                        Position = new { Latitude = "37.7852774", Longitude = "-122.4064785" }
                    }
                };


                Intent potentialIntent = Response.intents.First();
                //Entity potentialEntity = Response.entities.First();

                switch (potentialIntent.intent)
                {
                    case "HeadacheCheck":
                        CustomMessage = $"{GetCustomRandomExpression()}. These are the list of pharmacies where you can get medications for it.";
                        CustomBanner = "Assets/tylenol_banner.png";
                        SpeechSynthesizer s = new SpeechSynthesizer();
                        s.Voice = SpeechSynthesizer.AllVoices.First(voice => voice.Gender == VoiceGender.Female);
                        SpeechSynthesisStream stream = await s.SynthesizeTextToStreamAsync(CustomMessage);
                        MediaElement media = new MediaElement();
                        media.SetSource(stream, stream.ContentType);
                        media.Play();
                        break;
                    case "StomachacheCheck":
                        CustomMessage = $"{GetCustomRandomExpression()}. These are the list of pharmacies where you can get medications for it.";
                        CustomBanner = "Assets/stomachache_banner.png";
                        break;
                    case "SprainedAnkleCheck":
                        CustomMessage = $"{GetCustomRandomExpression()}. These are the list of places where you can get ankle braces for it.";
                        CustomBanner = "Assets/terapeak_banner.png";
                        break;
                    case "EmergencyCheck":
                        CustomMessage = $"Contacting the following emergency contacts";

                        if (ApiInformation.IsTypePresent("Windows.ApplicationModel.Calls.PhoneCallManager"))
                        {
                            PhoneCallManager.ShowPhoneCallUI(CurrentUser.EmergencyContacts.First().PhoneNumber, CurrentUser.EmergencyContacts.First().FirstName);
                        }
                        else
                        {
                            await Launcher.LaunchUriAsync(new Uri("tel://" + CurrentUser.EmergencyContacts.First().PhoneNumber));
                        }

                        break;
                }

                var dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    CurrentFrame.Navigate(typeof(AssistancePage));
                });
            }
            catch (Exception e)
            {
                // Details in ex.Message and ex.HResult.       
            }
        }

        public string GetCustomRandomExpression()
        {
            List<string> expressions = new List<string>
            {
                "Oh, no!", "Bummer!", "Sorry to hear that", "Too bad", "I can help you with that"
            };

            string randomExpression = expressions[new Random((int)DateTime.UtcNow.Ticks).Next(0, expressions.Count - 1)];
            return randomExpression;
        }

        public string SymptomPhrase { get; set; }

        public string CustomMessage { get; set; }

        public string CustomBanner { get; set; }

        public LuisResponse Response { get; set; }

        public CmaBusiness.User CurrentUser { get; set;  }

        public ObservableCollection<ResultModel> ResultsList { get; set; }

    }

    public class ResultModel
    {
        public string ImgUrl { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public dynamic Position { get; set; }
    }

}
