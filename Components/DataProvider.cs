using System;
using System.Data;
using DotNetNuke;
using DotNetNuke.Framework;

namespace GIBS.Modules.FlexMLS_Favorites.Components
{
    public abstract class DataProvider
    {

        #region common methods

        /// <summary>
        /// var that is returned in the this singleton
        /// pattern
        /// </summary>
        private static DataProvider instance = null;

        /// <summary>
        /// private static cstor that is used to init an
        /// instance of this class as a singleton
        /// </summary>
        static DataProvider()
        {
            instance = (DataProvider)Reflection.CreateObject("data", "GIBS.Modules.FlexMLS_Favorites.Components", "");
        }

        /// <summary>
        /// Exposes the singleton object used to access the database with
        /// the conrete dataprovider
        /// </summary>
        /// <returns></returns>
        public static DataProvider Instance()
        {
            return instance;
        }

        #endregion


        #region Abstract methods

        /* implement the methods that the dataprovider should */

        public abstract IDataReader FlexMLS_Favorites_Get_List(int moduleId, int userId, string favoriteType);
        public abstract void FlexMLS_Favorites_Delete(int itemId);
        public abstract void FlexMLS_Favorites_Delete_By_MlsNumber(int moduleId, int userId, string mlsNumber);

        public abstract IDataReader GetFlexMLS_Favoritess(int moduleId);
        public abstract IDataReader GetFlexMLS_Favorites(int moduleId, int itemId);
        public abstract void AddFlexMLS_Favorites(int moduleId, string content, int userId);

        public abstract void FlexMLS_Favorites_Update_EmailSearch(int itemId, bool emailSearch);
        

        #endregion

    }



}
