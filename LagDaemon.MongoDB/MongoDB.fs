namespace LagDaemon.MongoDB


[<RequireQualifiedAccess>]
module MongoDB =
    open MongoDB.Driver
    //open MongoDB.Bson
    open MongoDB.FSharp
    open System
    open System.Linq
    open System.Linq.Expressions
    open MongoDB.FSharp
    open MongoDB.Bson
    open MongoDB.Driver
    open MongoDB.Driver.Linq
    open Microsoft.FSharp.Linq

    //MongoDB.FSharp.Serializers.Register()


    type Predicate<'T> = Expression<Func<'T,bool>>


    NamelessInteractive.FSharp.MongoDB.SerializationProviderModule.Register()
    NamelessInteractive.FSharp.MongoDB.Conventions.ConventionsModule.Register()


    let config = Configuration.config.Mongo

    type MongoConnection<'T> = {
        client: IMongoClient
        db: IMongoDatabase
        collection: IMongoCollection<'T>
    }


    let connect<'T> (collection) =
        let client = MongoClient(config.ConnectionString)
        let db = client.GetDatabase(config.DBName)
        {
            client = client
            db = db
            collection = db.GetCollection<'T>(collection)
        }

    let create<'T> connection collection (document: 'T) =
        let col = connection.db.GetCollection(collection)
        col.InsertOne document

    let createMany<'T> connection collection (documentList: 'T list) =
        let col = connection.db.GetCollection(collection)
        col.InsertMany documentList

    let readAll<'T> connection collection  : 'T list =
        let col = connection.db.GetCollection(collection)
        col.Find<'T>(Builders.Filter.Empty).ToEnumerable() |> Seq.toList

    let find<'T> connection collection (predicate:Predicate<'T>)  : 'T list =
        let col = connection.db.GetCollection(collection)
        col.Find<'T>(predicate).ToEnumerable() |> Seq.toList 

    let findOne<'T> connection collection predicate : 'T =
        find<'T> connection collection predicate |> List.head

    let replace<'T> connection collection (document:'T) =
        let col = connection.db.GetCollection(collection)
        col.ReplaceOne(Builders.Filter.Empty, document)
       
    let replaceMany<'T> connection collection (documentList: 'T list) =
        let rec loop list results = 
            match list with
            | [] -> results
            | head::tail -> loop tail (replace<'T> connection collection head :: results)
        loop documentList []

    let delete<'T> connection collection (document: 'T) =
        let col = connection.db.GetCollection(collection)
        let filter = Builders<'T>.Filter.Where(fun x -> x.ToBsonDocument().GetElement("Id") = document.ToBsonDocument().GetElement("Id"))
        col.DeleteOne(filter)

    let deleteMany<'T> connection collection (documentList: 'T list) =
        let rec loop list results =
            match list with
            | [] -> results
            | head::tail -> loop tail (delete<'T> connection collection head :: results)
        loop documentList []

    let deleteAll connection collection =
        let col = connection.db.GetCollection(collection)
        col.DeleteMany(Builders.Filter.Empty)


    type MongoCollection<'T> (name:string) =
        let connection = connect(name)
        let collection = name

        member x.Create(document: 'T) =
            create<'T> connection collection document 
        member x.CreateMany(documentList: 'T list) =
            createMany<'T> connection collection documentList
        member x.ReadAll() =
            readAll<'T> connection collection
        member x.Find(predicate) =
            find<'T> connection collection predicate
        member x.FindOne(predicate) =
            findOne<'T> connection collection predicate
        member x.Replace(document) =
            replace<'T> connection collection document
        member x.ReplaceMany(documentList) =
            replaceMany<'T> connection collection documentList
        member x.Delete(document) =
            delete<'T> connection collection document
        member x.DeleteMany(documentList) =
            deleteMany<'T> connection collection documentList
        member x.DeleteAll() =
            deleteAll connection collection






        
             
        