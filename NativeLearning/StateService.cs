using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace NativeLearning
{
    public class StateService
    {

        public async Task deleteStore(string fileName)
        {

            try
            {

                var localFolder = ApplicationData.Current.LocalFolder;
                var file = await localFolder.GetFileAsync(fileName);

                await file.DeleteAsync();
            }
            catch (Exception ex)
            {

            }

        }

        public async Task SaveStateAsync<T>(string fileName, T objectToSave)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            //var jsonString = JsonSerializer.Serialize(objectToSave, options);

            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(objectToSave);

            var localFolder = ApplicationData.Current.LocalFolder;
            var file = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);






            await FileIO.WriteTextAsync(file, jsonString);


        }

        public async Task<T> LoadStateAsync<T>(string fileName)
        {
            try
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                var file = await localFolder.GetFileAsync(fileName);
                var jsonString = await FileIO.ReadTextAsync(file);
                //return JsonSerializer.Deserialize<T>(jsonString);


                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (FileNotFoundException)
            {
                return default;
            }
        }
    }
}
