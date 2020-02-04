using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using DotNetNuke;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search;

namespace GIBS.Modules.FlexMLS_Favorites.Components
{
    public class FlexMLS_FavoritesController :  IPortable
    {

        #region public method


        public List<FlexMLS_FavoritesInfo> FlexMLS_Favorites_Get_List(int moduleId, int userId, string favoriteType)
        {
            return CBO.FillCollection<FlexMLS_FavoritesInfo>(DataProvider.Instance().FlexMLS_Favorites_Get_List(moduleId, userId, favoriteType));
        }

        public void FlexMLS_Favorites_Delete(int itemId)
        {
            DataProvider.Instance().FlexMLS_Favorites_Delete(itemId);
        }

        public void FlexMLS_Favorites_Delete_By_MlsNumber(int moduleId, int userId, string mlsNumber)
        {
            DataProvider.Instance().FlexMLS_Favorites_Delete_By_MlsNumber( moduleId, userId, mlsNumber);
        }
        /// <summary>
        /// Gets all the FlexMLS_FavoritesInfo objects for items matching the this moduleId
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public List<FlexMLS_FavoritesInfo> GetFlexMLS_Favoritess(int moduleId)
        {
            return CBO.FillCollection<FlexMLS_FavoritesInfo>(DataProvider.Instance().GetFlexMLS_Favoritess(moduleId));
        }

        /// <summary>
        /// Get an info object from the database
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public FlexMLS_FavoritesInfo GetFlexMLS_Favorites(int moduleId, int itemId)
        {
          //  return (FlexMLS_FavoritesInfo)CBO.FillObject(DataProvider.Instance().GetFlexMLS_Favorites(moduleId, itemId), typeof(FlexMLS_FavoritesInfo));
            return CBO.FillObject<FlexMLS_FavoritesInfo>(DataProvider.Instance().GetFlexMLS_Favorites(moduleId, itemId));
        }


        /// <summary>
        /// Adds a new FlexMLS_FavoritesInfo object into the database
        /// </summary>
        /// <param name="info"></param>
        public void AddFlexMLS_Favorites(FlexMLS_FavoritesInfo info)
        {
            //check we have some content to store
            if (info.Favorite != string.Empty)
            {
                DataProvider.Instance().AddFlexMLS_Favorites(info.ModuleId, info.Favorite, info.CreatedByUser);
            }
        }

        /// <summary>
        /// update a info object already stored in the database
        /// </summary>
        /// <param name="info"></param>
        public void FlexMLS_Favorites_Update_EmailSearch(FlexMLS_FavoritesInfo info)
        {
            //check we have some content to update
            if (info.ItemId.ToString() != string.Empty)
            {
                DataProvider.Instance().FlexMLS_Favorites_Update_EmailSearch(info.ItemId, info.EmailSearch);
            }
        }


        /// <summary>
        /// Delete a given item from the database
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="itemId"></param>



        #endregion

        //#region ISearchable Members

        ///// <summary>
        ///// Implements the search interface required to allow DNN to index/search the content of your
        ///// module
        ///// </summary>
        ///// <param name="modInfo"></param>
        ///// <returns></returns>
        //public DotNetNuke.Services.Search.SearchItemInfoCollection GetSearchItems(ModuleInfo modInfo)
        //{
        //    SearchItemInfoCollection searchItems = new SearchItemInfoCollection();

        //    List<FlexMLS_FavoritesInfo> infos = GetFlexMLS_Favoritess(modInfo.ModuleID);

        //    foreach (FlexMLS_FavoritesInfo info in infos)
        //    {
        //        SearchItemInfo searchInfo = new SearchItemInfo(modInfo.ModuleTitle, info.Favorite, info.CreatedByUser, info.CreatedDate,
        //                                            modInfo.ModuleID, info.ItemId.ToString(), info.Favorite, "Item=" + info.ItemId.ToString());
        //        searchItems.Add(searchInfo);
        //    }

        //    return searchItems;
        //}

        //#endregion

        #region IPortable Members

        /// <summary>
        /// Exports a module to xml
        /// </summary>
        /// <param name="ModuleID"></param>
        /// <returns></returns>
        public string ExportModule(int moduleID)
        {
            StringBuilder sb = new StringBuilder();

            List<FlexMLS_FavoritesInfo> infos = GetFlexMLS_Favoritess(moduleID);

            if (infos.Count > 0)
            {
                sb.Append("<FlexMLS_Favoritess>");
                foreach (FlexMLS_FavoritesInfo info in infos)
                {
                    sb.Append("<FlexMLS_Favorites>");
                    sb.Append("<content>");
                    sb.Append(XmlUtils.XMLEncode(info.Favorite));
                    sb.Append("</content>");
                    sb.Append("</FlexMLS_Favorites>");
                }
                sb.Append("</FlexMLS_Favoritess>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// imports a module from an xml file
        /// </summary>
        /// <param name="ModuleID"></param>
        /// <param name="Content"></param>
        /// <param name="Version"></param>
        /// <param name="UserID"></param>
        public void ImportModule(int ModuleID, string Content, string Version, int UserID)
        {
            XmlNode infos = DotNetNuke.Common.Globals.GetContent(Content, "FlexMLS_Favoritess");

            foreach (XmlNode info in infos.SelectNodes("FlexMLS_Favorites"))
            {
                FlexMLS_FavoritesInfo FlexMLS_FavoritesInfo = new FlexMLS_FavoritesInfo();
                FlexMLS_FavoritesInfo.ModuleId = ModuleID;
                FlexMLS_FavoritesInfo.Favorite = info.SelectSingleNode("Favorite").InnerText;
                FlexMLS_FavoritesInfo.CreatedByUser = UserID;

                AddFlexMLS_Favorites(FlexMLS_FavoritesInfo);
            }
        }

        #endregion
    }
}
