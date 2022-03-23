using System;

using GHIElectronics.TinyCLR.Data.Json;

using Bytewizer.TinyCLR.Drivers.Blues.Notecard;

namespace Bytewizer.TinyCLR.Boards.Huxley
{
    public static class ControllerExtensions
    {
        public static object Request(this NotecardController source, JsonRequest noteRequest, Type type)
        {
            var results = source.Request(noteRequest);
            if (results.IsSuccess)
            {
                try
                {
                    return JsonConverter.DeserializeObject(results.Response, type);
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        public static JsonResults Request(this NotecardController source, JsonRequest noteRequest)
        {
            return new JsonResults(source.Transaction(noteRequest));
        }
    }
}