#ProjectMirror

A library to mirror structured data on multiple devices

##What is it?

Ever felt that itch to create a killer ToDo app which would make ToDos available on every device user had and let them work anywhere ranging from their office to [Attu Island](http://en.wikipedia.org/wiki/Attu_Island)? Or a game which would keep the scores updated no matter phone or tablet? Well, this project is specifically meant for you!  
It is a library to synchronize data between Windows Phone's local database and Windows Azure. You can synchronize data between multiple Windows Phone devices and provide offline access to all that data. You also have a copy of user's data in Windows Azure. It supports the features provided by Windows Azure Mobile Services (such as authentication) and you have full control over the data.

##How do I use it?

I'm glad you ask. You have to follow a few simple steps and you'll have your data synchronized in no time!
If you prefer to learn from code, take a look at `ColorsWP8` project. It is a demo app.

First of all, you need to have a project targetted to Windows Phone 8 in your solution. Then download this code and add `ProjectMirror` to your solution. Make sure you are using latest versions of assemblies referred by your projects. If not, update them using NuGet.
**Prepare your solution**  

**Prepare the models**  
Now, I'm going to assume that your app uses MVVM pattern. (If it doesn't, [it should](http://stackoverflow.com/questions/1644453/why-mvvm-and-what-are-its-core-benefits).) Inherit your model classes from `NotifyBase` and implement `ISyncable` interface on them. (Both are available in namespace `ProjectMirror`.) Each model which wants to be synced must:
- Implement `ISyncable` interface.
- Inherit from NotifyBase (or implement `INotifyPropertyChanged` and `INotifyPropertyChanging` somehow).
- Raise `NotifyPropertyChanged` event on each property (including those from `ISyncable`) when it is, well, changed.  

We are also going to modify the `DataContext` a bit. All you have to do is to inherit your `ToDoDataContext` from `ProjectMirror.DataContextBase` instead of `System.Data.Linq.DataContext`. Rest of the procedure remains the same.

**Create MirrorSyncService**  
`MirrorSyncService` does all the storage and synchronization magic. We need it throughout lifecycle of the app. So, we are going to create a public property in App class to hold our instance. Add following code to your App class.

```
private static MirrorSyncService mirrorService;
public static MirrorSyncService MirrorService
{
    get
    {
	    return mirrorService;
    }
}
```

Then create a new instance of `MirrorSyncService` in constructor.

```
//Constructor
Public App()
{
	//Other code
	mirrorService = new MirrorSyncService();
}
```
We are going to refer to this instance anywhere from the app as `App.MirrorService`.

**Configure MirrorSyncService**  
Now that we have created an instance, we need to configure it. How else is it going to know where our data is!  
Configuration is to be done as soon as the instance is created. We are going to do the configuration in our `MainViewModel`. Add this code in the constructor of `MainViewModel` class:

```
Public MainViewModel()
{
	App.MirrorService.ConfigureSQLCE<ToDoDataContext>(new ToDoDataContext("Data Source=isostore:/MainDB.sdf"));
	App.MirrorService.ConfigureMobileService("WAMS ENDPOINT", "WAMS ACCESSKEY");
}
```

The `ToDoDataContext` passed to the first method tells the service which tables have to be created and where it should put the database file on local storage.  
The second method takes information about your WAMS.

**Authentication**  
Make sure that you have set table access permissions to 'Only Authenticated Users'. We don't want random Drep to access data, do we?  
Authentication methods provided by `MirrorSyncService` are exactly like the methods provided by WAMS SDK. In addition to that, the `AuthenticationManager` class provides an example of how other SDKs can be used to provide seamless authentication experience.

**Configure WAMS**  
We need to do some modifications in scripts before data synchronization can work. Modify the `Read` script so that it retuns data belonging to our user only. This is also explained in detail in Windows Azure [documentation](http://www.windowsazure.com/en-us/develop/mobile/tutorials/get-started-with-users-wp8/).

```
function read(query, user, request) {

    query.where({userId:user.userId});
    request.execute();

}
```

Also, modify `Insert` and `Update` queries so that they look something like this:

```
function insert(item, user, request) {

    item.LastSynchronized = new Date();
    item.MSuserId = user.userId; //Not needed in update script
    request.execute();

}
```
Setting `item.LastSynchronized` is important here.

**All done!**  
You're almost ready to go. For CRUD operations, you can call the respective method in `MirrorSyncService` with the item like so:

```
App.MirrorService.AddItemAsync<ToDoCategory>(categoryToAdd);

App.MirrorService.DeleteItemAsync<ToDoCategory>(categoryToDelete);

App.MirrorService.UpdateItemAsync<ToDoCategory>(categoryToUpdate);

var categories = await App.MirrorService.LoadItemsAsync<ToDoCategory>();
```

Then finally, when you're done manupulating data, call the `SynchronizeAsync` method to synchronize your data. This method synchronizes only the table you specify.

```
await App.MirrorService.SynchronizeAsync<ToDoCategory>();
```

Bingo! All your data is magically available on Azure. You can go and check it on the portal.