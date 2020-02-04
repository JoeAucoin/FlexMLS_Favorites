using System;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;

namespace GIBS.Modules.FlexMLS_Favorites.Components
{
    public class FlexMLS_FavoritesInfo
    {
        //private vars exposed thro the
        //properties
        private int moduleId;
        private int itemId;
        private int userId;
        
        private int createdByUser;
        private DateTime createdDate;
        private string createdByUserName = null;

        private int createdByUserID;

        private string favorite;
        private string favoriteType;
        private bool emailSearch;

        private string mlsNumber;



        /// <summary>
        /// empty cstor
        /// </summary>
        public FlexMLS_FavoritesInfo()
        {
        }


        #region properties

        public string MlsNumber
        {
            get { return mlsNumber; }
            set { mlsNumber = value; }
        }

        public bool EmailSearch
        {
            get { return emailSearch; }
            set { emailSearch = value; }
        }

        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        public int ItemId
        {
            get { return itemId; }
            set { itemId = value; }
        }

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public string Favorite
        {
            get { return favorite; }
            set { favorite = value; }
        }

        public string FavoriteType
        {
            get { return favoriteType; }
            set { favoriteType = value; }
        }

        public int CreatedByUser
        {
            get { return createdByUser; }
            set { createdByUser = value; }
        }

        public DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        public int CreatedByUserID
        {
            get { return createdByUserID; }
            set { createdByUserID = value; }
        }

        public string CreatedByUserName
        {
            get
            {
                if (createdByUserName == null)
                {
                    if (createdByUserID > 0)
                    {
                        int portalId = PortalController.Instance.GetCurrentPortalSettings().PortalId;
                        //UserInfo user = UserController.GetUser(portalId, createdByUser, false);
                        UserInfo user = UserController.GetUserById(portalId, createdByUser);
                        createdByUserName = user.DisplayName;

                     //   UserInfo user = UserController.GetUser(portalId, createdByUserID, false);
                    //    createdByUserName = user.DisplayName;
                    }
                    else
                    {

                        createdByUserName = "Anonymous User";
                    }

                }

                return createdByUserName;
            }
        }
        #endregion
    }
}
