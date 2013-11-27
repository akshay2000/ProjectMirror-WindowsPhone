using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using Microsoft.WindowsAzure.MobileServices;
using System.Data.Linq;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace ProjectMirror
{
    public class MirrorSyncService
    {
        //DataContext object to manage data
        private dynamic MainDB { get; set; }

        //MobileServiceClient
        private MobileServiceClient MobileServiceConnection { get; set; }

        //MobileService User
        private MobileServiceUser mUser;

        private readonly DateTime beginingOfTheTime = new DateTime(1992, 12, 31, 11, 40, 06);
        private readonly DateTime riseOfHumanity = new DateTime(1993, 1, 1);
        private readonly DateTime fallOfHumanity = new DateTime(3993, 12, 30);

        //Constructor
        public MirrorSyncService()
        { }

        public async void ConfigureSQLCE<T>(T mainDataContext) where T : DataContextBase
        {           
            MainDB = mainDataContext;
        }

        public async void ConfigureMobileService(string endpoint, string accesskey)
        {
            MobileServiceConnection = new MobileServiceClient(endpoint, accesskey);
        }

        #region Authentication--------------------------------------------------------------------------------------
        public async Task<MobileServiceUser> AuthenticateMobileServiceAsync(MobileServiceAuthenticationProvider provider)
        {
            var user = await MobileServiceConnection.LoginAsync(provider);
            return user;
        }

        public async Task<MobileServiceUser> AuthenticateMobileServiceAsync(MobileServiceAuthenticationProvider provider, JObject authToken)
        {
            var user = await MobileServiceConnection.LoginAsync(provider, authToken);
            return user;
        }

        public async Task<MobileServiceUser> AuthenticateWithMicrosoftAsync(string authToken)
        {
            var user = await MobileServiceConnection.LoginWithMicrosoftAccountAsync(authToken);
            return user;
        }

        #endregion--------------------------------------------------------------------------------------------------

        private async Task<DateTime?> GetLastSyncedTime<TEntity>() where TEntity : IMirrorSyncable
        {
            var connection = await GetConnection();
            string entityName = typeof(TEntity).Name;
            var table = connection.GetTable<SyncTimeStamp>();
            var tableRow = (from item in table
                                  where item.EntityName == entityName
                                  select item).FirstOrDefault();
            if (tableRow != null)
                return tableRow.LastSynced;
            else
                return null;
        }

        private async void SetLastSyncedTime<TEntity>(DateTime recentUpdated) where TEntity : IMirrorSyncable
        {
            var connection = await GetConnection();
            
            var table = connection.GetTable<SyncTimeStamp>();
            var entityName = typeof(TEntity).Name;
            var tableRow = (from item in table
                            where item.EntityName == entityName
                            select item).FirstOrDefault();

            if (tableRow != null)
                tableRow.LastSynced = recentUpdated;
            else
                table.InsertOnSubmit(new SyncTimeStamp { EntityName = entityName, LastSynced = recentUpdated });

            connection.SubmitChanges();
        }

        public async Task SynchronizeAsync<TEntity>() where TEntity : class, IMirrorSyncable
        {
            var connection = await GetConnection();
            var localTable = connection.GetTable(typeof(TEntity));

            var remoteTable = MobileServiceConnection.GetTable<TEntity>();

            var lastsynced = await GetLastSyncedTime<TEntity>();
            var currentTimeStamp = lastsynced ?? beginingOfTheTime;
            var newTimeStamp = currentTimeStamp;

            //Get updated items from local to put them in cloud
            var newLocalItems = (from TEntity p in localTable
                                 where p.LastModified > currentTimeStamp
                                 select p).ToList();

            var newRemoteItems = await (from p in remoteTable
                                        where p.LastSynchronized > currentTimeStamp
                                        select p).ToListAsync();

            //Create two arrays with Ids and take intersection.
            //Create third collection with the conflicted Ids.
            var localIds = (from item in newLocalItems
                           select item.RemoteId).ToList();
            var remoteIds = (from item in newRemoteItems
                            select item.RemoteId).ToList();

            var conflictedIds = remoteIds.Intersect<int>(localIds).ToList();

            foreach(int t in conflictedIds)
            {
                var localItem = newLocalItems.FirstOrDefault(item => item.RemoteId == t);
                var remoteItem = newRemoteItems.FirstOrDefault(item => item.RemoteId == t);
                var remove = localItem.LastModified > remoteItem.LastModified ? newRemoteItems.Remove(remoteItem) : newLocalItems.Remove(localItem);
            }
          
            //Put cloud changes in local table
            foreach (var remoteItem in newRemoteItems)
            {
                var localItem = (from TEntity p in localTable
                                 where p.RemoteId == remoteItem.RemoteId
                                 select p).FirstOrDefault();

                //SQLCE doesn't save DateTime.MinValue;
                remoteItem.LastModified = remoteItem.LastModified < beginingOfTheTime ? riseOfHumanity : remoteItem.LastModified;
                remoteItem.LastSynchronized = remoteItem.LastSynchronized < beginingOfTheTime ? riseOfHumanity : remoteItem.LastSynchronized;

                if (localItem == null)
                {
                    localTable.InsertOnSubmit(remoteItem);
                    connection.SubmitChanges();
                }
                else// if(remoteItem.LastModified>localItem.LastModified)
                {
                    UpdateLocalItem<TEntity>(localItem, remoteItem);//TODO: Pay closer attention to the update!
                }

                newTimeStamp = newTimeStamp > remoteItem.LastSynchronized ? newTimeStamp : remoteItem.LastSynchronized;
            }

            //Put local changes in cloud table
            foreach (var localItem in newLocalItems)
            {
                if (localItem.RemoteId == 0)
                {
                    //Update LastSynchronized as well as RemoteId
                    await remoteTable.InsertAsync(localItem);                   
                    connection.SubmitChanges();
                }
                else
                {
                    //Update LastSynchronized
                    await remoteTable.UpdateAsync(localItem);                   
                    connection.SubmitChanges();
                }
                newTimeStamp = newTimeStamp > localItem.LastSynchronized ? newTimeStamp : localItem.LastSynchronized;
            }

            SetLastSyncedTime<TEntity>(newTimeStamp);
        }

        private async Task<DataContextBase> GetConnection()
        {
            if (!MainDB.DatabaseExists())
            {
                MainDB.CreateDatabase();
            }
            
            return MainDB;
        }

        #region CRUD operations ---------------------------------------------------------------------------------------------------

        public async Task<List<TEntity>> LoadItemsAsync<TEntity>() where TEntity : IMirrorSyncable
        {
            var listToReturn = new List<TEntity>();

            ////Remote list
            //listToReturn = await MobileServiceConnection.GetTable<TEntity>().ToListAsync();

            //Local list
            var connection = await GetConnection();
            var table = connection.GetTable(typeof(TEntity));
            var itemsInDB = from TEntity item in table
                            where item.IsDeleted == false
                            select item;
            listToReturn = itemsInDB.ToList<TEntity>();

            return listToReturn;
        }

        public async void AddItemAsync<TEntity>(TEntity itemToAdd) where TEntity : IMirrorSyncable
        {
            itemToAdd.LastSynchronized = riseOfHumanity;
            itemToAdd.LastModified = DateTime.Now;

            //Local storage
            var connection = await GetConnection();
            var table = connection.GetTable(typeof(TEntity));
            table.InsertOnSubmit(itemToAdd);
            connection.SubmitChanges();
        }

        public async void DeleteItemAsync<TEntity>(TEntity itemToDelete) where TEntity : NotifyBase, IMirrorSyncable
        {
            itemToDelete.IsDeleted = true;

            //Local delete
            var connection = await GetConnection();
            connection.Log = new DebugTextWriter();
            var table = connection.GetTable(typeof(TEntity));
            var query = from TEntity item in table
                        where item.LocalId == itemToDelete.LocalId
                        select item;
            query.First().IsDeleted = true;
            query.First().LastModified = DateTime.Now;
            connection.SubmitChanges();
        }

        public async void UpdateItemAsync<TEntity>(TEntity itemToUpdate) where TEntity : NotifyBase, IMirrorSyncable
        {
            itemToUpdate.LastModified = DateTime.Now;

            //Local update
            var connection = await GetConnection();
            connection.SubmitChanges();
            var table = connection.GetTable<TEntity>();
            var originalItem = (from TEntity item in table
                                where item.LocalId == itemToUpdate.LocalId
                                select item).FirstOrDefault();
            UpdateLocalItem<TEntity>(originalItem, itemToUpdate);
        }

        //Stupid LINQ doesn't provide a update method to update record.
        //So here's an utter dumb implementation
        private async void UpdateLocalItem<TEntity>(TEntity originalItem, TEntity newItem) where TEntity : class, IMirrorSyncable
        {
            var connection = await GetConnection();
            connection.Log = new DebugTextWriter();
            var table = connection.GetTable(typeof(TEntity));
            table.DeleteOnSubmit(originalItem);
            table.InsertOnSubmit(newItem);
            connection.SubmitChanges();
        }

        #endregion ----------------------------------------------------------------------------------------------------------------
    }
}
