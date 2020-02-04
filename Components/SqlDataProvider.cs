using System;
using System.Data;
using DotNetNuke.Framework.Providers;
using Microsoft.ApplicationBlocks.Data;

namespace GIBS.Modules.FlexMLS_Favorites.Components
{
    public class SqlDataProvider : DataProvider
    {


        #region vars

        private const string providerType = "data";
        private const string moduleQualifier = "GIBS_";

        private ProviderConfiguration providerConfiguration = ProviderConfiguration.GetProviderConfiguration(providerType);
        private string connectionString;
        private string providerPath;
        private string objectQualifier;
        private string databaseOwner;

        #endregion

        #region cstor

        /// <summary>
        /// cstor used to create the sqlProvider with required parameters from the configuration
        /// section of web.config file
        /// </summary>
        public SqlDataProvider()
        {
            Provider provider = (Provider)providerConfiguration.Providers[providerConfiguration.DefaultProvider];
            connectionString = DotNetNuke.Common.Utilities.Config.GetConnectionString();

            if (connectionString == string.Empty)
                connectionString = provider.Attributes["connectionString"];

            providerPath = provider.Attributes["providerPath"];

            objectQualifier = provider.Attributes["objectQualifier"];
            if (objectQualifier != string.Empty && !objectQualifier.EndsWith("_"))
                objectQualifier += "_";

            databaseOwner = provider.Attributes["databaseOwner"];
            if (databaseOwner != string.Empty && !databaseOwner.EndsWith("."))
                databaseOwner += ".";
        }

        #endregion

        #region properties

        public string ConnectionString
        {
            get { return connectionString; }
        }


        public string ProviderPath
        {
            get { return providerPath; }
        }

        public string ObjectQualifier
        {
            get { return objectQualifier; }
        }


        public string DatabaseOwner
        {
            get { return databaseOwner; }
        }

        #endregion

        #region private methods

        private string GetFullyQualifiedName(string name)
        {
            return DatabaseOwner + ObjectQualifier + moduleQualifier + name;
        }

        private object GetNull(object field)
        {
            return DotNetNuke.Common.Utilities.Null.GetNull(field, DBNull.Value);
        }

        #endregion

        #region override methods

        public override IDataReader FlexMLS_Favorites_Get_List(int moduleId, int userId, string favoriteType)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("FlexMLS_Favorites_Get_List"), moduleId, userId, favoriteType);
        }

        public override void FlexMLS_Favorites_Delete(int itemId)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("FlexMLS_Favorites_Delete"), itemId);
        }

        public override void FlexMLS_Favorites_Delete_By_MlsNumber(int moduleId, int userId, string mlsNumber)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("FlexMLS_Favorites_Delete_By_MlsNumber"), moduleId, userId, mlsNumber);
        }

        public override IDataReader GetFlexMLS_Favoritess(int moduleId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("GetFlexMLS_Favoritess"), moduleId);
        }

        public override IDataReader GetFlexMLS_Favorites(int moduleId, int itemId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("GetFlexMLS_Favorites"), moduleId, itemId);
        }

        public override void AddFlexMLS_Favorites(int moduleId, string content, int userId)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("AddFlexMLS_Favorites"), moduleId, content, userId);
        }

        public override void FlexMLS_Favorites_Update_EmailSearch(int itemId, bool emailSearch)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("FlexMLS_Favorites_Update_EmailSearch"), itemId, emailSearch);
        }	



        #endregion
    }
}
