using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Masterclass.Revit.Utilities
{
    public static class SchemaUtils
    {
        public static Guid SchemaId = new Guid("ab1757a0-4892-46cf-83f3-7e3ce93d17d1");
        public static Schema Schema = Schema.Lookup(SchemaId);
        public static string FieldName = "Settings";

        public static StoredSettings LoadSchema(Document doc)
        {
            try
            {
                if (Schema == null)
                    Schema = CreateSchema();

                if (Schema != null)
                {
                    var storage = GetDataStorage(doc);
                    var entity = storage?.GetEntity(Schema);
                    if (entity?.Schema == null)
                        return null;

                    var array = entity.Get<IList<byte>>(Schema.GetField(FieldName)).ToArray();
                    var storedSettings = AppSettings.Instance.StoredSettings.FromByteArray(array);

                    return storedSettings;
                }
            }
            catch (Exception e)
            {
                // ignored
            }

            return null;
        }

        public static bool SaveSchema(Document doc)
        {
            var updated = false;
            using (var trans = new Transaction(doc, "Store Settings"))
            {
                trans.Start();
                try
                {
                    Schema = Schema ?? CreateSchema();
                    if (Schema != null)
                    {
                        var savedData = GetDataStorage(doc) ?? DataStorage.Create(doc);
                        var entity = savedData.GetEntity(Schema);
                        if (entity.Schema == null)
                            entity = new Entity(Schema);

                        var field = Schema.GetField(FieldName);
                        var storedSettings = AppSettings.Instance.StoredSettings.ToByteArray();
                        entity.Set<IList<byte>>(field, storedSettings);
                        savedData.SetEntity(entity);

                        updated = true;
                    }

                    trans.Commit();
                }
                catch (Exception e)
                {
                    trans.RollBack();
                }
            }

            return updated;
        }

        public static Schema CreateSchema()
        {
            var builder = new SchemaBuilder(SchemaId);
            builder.SetReadAccessLevel(AccessLevel.Public);
            builder.SetWriteAccessLevel(AccessLevel.Application);
            // (Konrad) Must match *.addin value set for ApplicationGUID.
            builder.SetApplicationGUID(new Guid("CFB146A6-76B8-4F1D-8C0E-28B177DCB935"));

            // (Konrad) No spaces allowed in vendor id.
            // Must match *.addin value set for VendorId.
            builder.SetVendorId("Archi-lab,LLC");
            builder.SetDocumentation("Masterclass Revit plugin.");

            // (Konrad) No periods allowed in schema name.
            builder.SetSchemaName("Masterclass_v1_0");
            builder.AddArrayField(FieldName, typeof(byte));
            var schema = builder.Finish();

            return schema;
        }

        #region Utilities

        private static DataStorage GetDataStorage(Document doc)
        {
            var storage = new FilteredElementCollector(doc)
                .OfClass(typeof(DataStorage))
                .Cast<DataStorage>()
                .FirstOrDefault(x =>
                {
                    var entity = x.GetEntity(Schema);
                    return entity != null && entity.IsValid();
                });

            return storage;
        }

        #endregion
    }
}
