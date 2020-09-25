using System;
using System.Net.Http;
using Newtonsoft.Json;
using Plugin.Media;
using Xamarin.Forms;
using Plugin.Media.Abstractions;
using Plugin.FilePicker;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;
using System.Collections.Generic;

namespace MyPart2
{
    public partial class ScanDocPage : ContentPage
    {
        public ScanDocPage()
        {
            InitializeComponent();
            BindingContext = new MyPart2ViewModel();
            DocumentTypes();
            //StackLayout stackLayout2 = new StackLayout();
            //Picker picker1 = new Picker();
            //picker1.Title = "Select Document";
            //picker1.ItemsSource = DocTypePage;

        }
        ObservableCollection<MediaFile> files = new ObservableCollection<MediaFile>();
        private async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            ImageSource result;
            var action = await DisplayActionSheet("Please Select an Option", "Cancel", null, "Scan Document", "Upload Document from Gallery", "Upload Document from files");
            switch (action)
            {
                case "Scan Document":
                    try
                    {
                        result = await TakePhoto();
                        //if (result != null)
                        //    //var viewPhotoImage.Source = result;
                        //    viewPhotoImage = result;
                    }
                    catch (Exception)
                    {
                        await DisplayAlert("Permission Denied", "Give camera permission to the device.\nError: ", "OK");
                        // handle your exception
                        //await DisplayAlert("Permisson Denied", "Give camera permission to the device.\nError: " + ex.Message, "Ok");
                    }

                    break;
                case "Upload Document from Gallery":
                    try
                    {
                        result = await SelectPhoto();
                        //    if (result != null)
                        //        //viewPhotoImage.Source = result;
                        //       viewPhotoImage = result;
                    }
                    catch (Exception)
                    {
                        await DisplayAlert("Permission Denied", "Give permission to access gallery on the device.\nError: ", "OK");
                        // handle your exception
                    }
                    break;
                case "Upload Document from files":
                    try
                    {
                        var file = await CrossFilePicker.Current.PickFile();
                        if (file != null)
                        {
                            await DisplayAlert("File obtained", "", "OK");
                        }
                    }
                    catch (Exception)
                    {
                        await DisplayAlert("Permission Denied", "Give permission to access files on the device.\nError: ", "OK");
                    }
                    break;
                default:
                    break;
            };
            //await PopupNavigation.Instance.PushAsync(new Rg.Plugins.Popup.Pages.ScanPopUpPage());
            //await Navigation.PushAsync(new ScanPopUpPage());
        }

        public async Task<ImageSource> TakePhoto()
        {
            await CrossMedia.Current.Initialize();
            files.Clear();
            if (!CrossMedia.Current.IsCameraAvailable ||
                    !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera","Sorry! No camera available.","OK");
                //await DisplayAlert("No Camera", "Sorry! No camera available.", "OK");
                return null;
            }

            //var isPermissionGranted = await RequestPermissions(new List<Permission>() { Permission.Camera, Permission.Storage });
            //if (!isPermissionGranted)
            //    return null;

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "TestPhotoFolder",
                SaveToAlbum = true,
                CompressionQuality = 75,
                CustomPhotoSize = 50,
                PhotoSize = PhotoSize.Medium,
                MaxWidthHeight = 1000,
                Name = "Id.jpg"
                //DefaultCamera = CameraDevice.Front
            });

            if (file == null)
                return null;

            var imageSource = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });

            return imageSource;
        }



        public async Task<ImageSource> SelectPhoto()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Photos Not Supported", "Sorry! Permission not granted to photos", "OK");
                //await DisplayAlert("Photos Not Supported", "Sorry! Permission not granted to photos.", "OK");
                return null;
            }

            

            var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
            });

            if (file == null)
                return null;

            var imageSource = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });

            return imageSource;
        }
        public async void DocumentTypes()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync("https://testappbackend.fastpacetransfer.com/backend/api/v0/document/types");
            var docType = JsonConvert.DeserializeObject<DocumentTypes>(response);
            ((MyPart2ViewModel)BindingContext).DocumentTypes = docType.data;
            DocumentTypeEntry.ItemsSource = (System.Collections.IList)((MyPart2ViewModel)BindingContext).DocumentTypes;
        }

    }
    

}
