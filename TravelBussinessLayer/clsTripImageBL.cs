using System.Collections.Generic;
using TravelDataAccess;

namespace TravelBussinessLayer
{
    public class clsTripImageBl
    {
        /// <summary>
        /// إضافة صورة جديدة لرحلة.
        /// تمرير اسم الملف فقط (fileName).
        /// </summary>
        public static bool AddNewTripImage(int tripID, string fileName)
        {
            return clsTripImageDA.AddNewTripImage(fileName, tripID);
        }

        /// <summary>
        /// جلب جميع صور الرحلة مع بناء رابط الصورة الكامل.
        /// </summary>
        public static List<TripImageDTO> GetAllTripImages(int tripId, string baseUrl)
        {
            var images = clsTripImageDA.GetAllTripImageByTripID(tripId);

            foreach (var img in images)
            {
                img.ImageUrl = $"{baseUrl}/api/TripImage/GetImage/{img.FileName}";
            }

            return images;
        }


        public static bool DeleteTripImageByImageID(int ImageID)
        {
            return clsTripImageDA.DeleteTripImageByImageID(ImageID);
        }
        public static bool DeleteTripImageByTripID(int TripID)
        {
            return clsTripImageDA.DeleteTripImageByTripID(TripID);
        }
    }
}