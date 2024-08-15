namespace LinkBoxUI.Migrations
{
    using DataCipher;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LinkBoxUI.Context.LinkboxDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(LinkBoxUI.Context.LinkboxDb context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            if (!context.Users.Any(u => u.UserName == "Admin1"))
            {
                string _usr = "admin", _pword = "1234";
                string passw = Cryption.Encrypt(_usr + _pword);
                context.Users.AddOrUpdate(
                    usr => usr.UserName,
                    new DomainLayer.User
                    {
                        UserName = _usr
                                        ,
                        Password = passw
                                        ,
                        CreateDate = DateTime.Now
                                        ,
                        LastName = "Admin"
                                        ,
                        MiddleName = "Top"
                                        ,
                        FirstName = "Role"
                                        ,
                        IsActive = true
                                        ,
                        CreateUserID = 1
                                        ,
                        AuthorizationID = 1
                    }
                    );
            }
            //context.APISetups.AddOrUpdate(
            //        t => t.APICode,
            //        new DomainLayer.APISetup
            //        {
            //            APICode = "EBlast",
            //            APIMethod = "POST"
            //                            ,
            //            APIURL = "http://localhost:40710/Linkbox/Post"
            //                            ,
            //            IsActive = false,
            //            CreateUserID = 1,
            //            CreateDate = DateTime.Now
            //        });
            //context.APISetups.AddOrUpdate(
            //    t => t.APICode,
            //    new DomainLayer.APISetup
            //    {
            //        APICode = "SAPDocPost",
            //        APIMethod = "POST"
            //                        ,
            //        APIURL = "http://localhost:40710/Linkbox/sap/post/documents"
            //                        //, APIURL = "http://localhost:PORTNUM/Linkbox/sap/post/documents"
            //                        ,
            //        IsActive = false,
            //        CreateUserID = 1,
            //        CreateDate = DateTime.Now
            //    });
            //context.APISetups.AddOrUpdate(
            //    t => t.APICode,
            //    new DomainLayer.APISetup
            //    {
            //        APICode = "SHOPIFY_API_POSTITEM"
            //        ,
            //        APIMethod = "POST"
            //        ,
            //        APIURL = "http://localhost:40710/Linkbox/shopify/post/items"
            //        ,
            //        IsActive = false
            //        ,
            //        CreateUserID = 1
            //        ,
            //        CreateDate = DateTime.Now
            //    });
            //context.APISetups.AddOrUpdate(
            //    t => t.APICode,
            //    new DomainLayer.APISetup
            //    {
            //        APICode = "SAP_POSTBPCATALOG"
            //        ,
            //        APIMethod = "POST"
            //        ,
            //        APIURL = "http://localhost:40710/Linkbox/sap/post/bpcatalog"
            //        ,
            //        IsActive = false
            //        ,
            //        CreateUserID = 1
            //        ,
            //        CreateDate = DateTime.Now
            //    });
            //context.APISetups.AddOrUpdate(
            //    t => t.APICode,
            //    new DomainLayer.APISetup
            //    {
            //        APICode = "SHOPIFY_INVTYLEVEL"
            //        ,
            //        APIMethod = "POST"
            //        ,
            //        APIURL = "http://localhost:40710/Linkbox/shopify/post/inventory"
            //        ,
            //        IsActive = false
            //        ,
            //        CreateUserID = 1
            //        ,
            //        CreateDate = DateTime.Now
            //    });

            //context.APISetups.AddOrUpdate(
            //    t => t.APICode,
            //    new DomainLayer.APISetup
            //    {
            //        APICode = "ZD_API_POST"
            //        ,
            //        APIMethod = "POST"
            //        ,
            //        APIURL = "http://localhost:40710/Linkbox/zendesk/post/tickets"
            //        ,
            //        IsActive = false
            //        ,
            //        CreateUserID = 1
            //        ,
            //        CreateDate = DateTime.Now
            //    });

            context.APISetups.AddOrUpdate(
                t => t.APICode,
                new DomainLayer.APISetup
                {
                    APICode = "UploadSAPToAddon"
                    ,
                    APIMethod = "POST"
                    ,
                    APIURL = "http://localhost:8970/Linkbox/sap/upload/documents"
                    ,
                    IsActive = true
                    ,
                    CreateUserID = 1
                    ,
                    CreateDate = DateTime.Now
                });

            #region MODULE_SETUP
            if (!context.ModuleSetup.Any(u => u.ModuleCode == "OITM"))
            {
                context.ModuleSetup.AddOrUpdate(
                t => t.ModuleCode,
                new DomainLayer.Models.ModuleSetup
                {
                    ModuleCode = "OITM"
                    ,
                    ModuleName = "Item Master Data"
                    ,
                    PrimaryKey = "ItemCode"
                    ,
                    IsActive = true
                    ,
                    CreateUserID = 1
                    ,
                    CreateDate = DateTime.Now
                    ,
                    EntityType = "Item"
                    ,
                    EntityName = "Items"
                });
            }

            if (!context.ModuleSetup.Any(u => u.ModuleCode == "OCRD"))
            {
                context.ModuleSetup.AddOrUpdate(
                t => t.ModuleCode,
                new DomainLayer.Models.ModuleSetup
                {
                    ModuleCode = "OCRD"
                    ,
                    ModuleName = "Business Partner Master Data"
                    ,
                    PrimaryKey = "CardCode"
                    ,
                    IsActive = true
                    ,
                    CreateUserID = 1
                    ,
                    CreateDate = DateTime.Now
                    ,
                    EntityType = "BusinessPartner"
                    ,
                    EntityName = "BusinessPartners"
                });
            }

            if (!context.ModuleSetup.Any(u => u.ModuleCode == "ORDR"))
            {
                context.ModuleSetup.AddOrUpdate(
                t => t.ModuleCode,
                new DomainLayer.Models.ModuleSetup
                {
                    ModuleCode = "ORDR"
                    ,
                    ModuleName = "Sales Order"
                    ,
                    PrimaryKey = "DocEntry"
                    ,
                    IsActive = true
                    ,
                    CreateUserID = 1
                    ,
                    CreateDate = DateTime.Now
                    ,
                    EntityType = "Document"
                    ,
                    EntityName = "Orders"
                });
            }

            if (!context.ModuleSetup.Any(u => u.ModuleCode == "OPRQ"))
            {
                context.ModuleSetup.AddOrUpdate(
                t => t.ModuleCode,
                new DomainLayer.Models.ModuleSetup
                {
                    ModuleCode = "OPRQ"
                    ,
                    ModuleName = "Purchase Request"
                    ,
                    PrimaryKey = "DocEntry"
                    ,
                    IsActive = true
                    ,
                    CreateUserID = 1
                    ,
                    CreateDate = DateTime.Now
                    ,
                    EntityType = "Document"
                    ,
                    EntityName = "PurchaseRequests"
                });
            }

            if (!context.ModuleSetup.Any(u => u.ModuleCode == "OPOR"))
            {
                context.ModuleSetup.AddOrUpdate(
                t => t.ModuleCode,
                new DomainLayer.Models.ModuleSetup
                {
                    ModuleCode = "OPOR"
                    ,
                    ModuleName = "Purchase Order"
                    ,
                    PrimaryKey = "DocEntry"
                    ,
                    IsActive = true
                    ,
                    CreateUserID = 1
                    ,
                    CreateDate = DateTime.Now
                    ,
                    EntityType = "Document"
                    ,
                    EntityName = "PurchaseOrders"
                });
            }

            if (!context.ModuleSetup.Any(u => u.ModuleCode == "OPCH"))
            {
                context.ModuleSetup.AddOrUpdate(
                t => t.ModuleCode,
                new DomainLayer.Models.ModuleSetup
                {
                    ModuleCode = "OPCH"
                    ,
                    ModuleName = "Purchase Invoices"
                    ,
                    PrimaryKey = "DocEntry"
                    ,
                    IsActive = true
                    ,
                    CreateUserID = 1
                    ,
                    CreateDate = DateTime.Now
                    ,
                    EntityType = "Document"
                    ,
                    EntityName = "PurchaseInvoices"
                });
            }

            if (!context.ModuleSetup.Any(u => u.ModuleCode == "OPDN"))
            {
                context.ModuleSetup.AddOrUpdate(
                t => t.ModuleCode,
                new DomainLayer.Models.ModuleSetup
                {
                    ModuleCode = "OPDN"
                    ,
                    ModuleName = "Purchase Delivery Notes"
                    ,
                    PrimaryKey = "DocEntry"
                    ,
                    IsActive = true
                    ,
                    CreateUserID = 1
                    ,
                    CreateDate = DateTime.Now
                    ,
                    EntityType = "Document"
                    ,
                    EntityName = "PurchaseDeliveryNotes"
                });
            }

            if (!context.ModuleSetup.Any(u => u.ModuleCode == "ORPC"))
            {
                context.ModuleSetup.AddOrUpdate(
                t => t.ModuleCode,
                new DomainLayer.Models.ModuleSetup
                {
                    ModuleCode = "ORPC"
                    ,
                    ModuleName = "Purchase Credit Notes"
                    ,
                    PrimaryKey = "DocEntry"
                    ,
                    IsActive = true
                    ,
                    CreateUserID = 1
                    ,
                    CreateDate = DateTime.Now
                    ,
                    EntityType = "Document"
                    ,
                    EntityName = "PurchaseCreditNotes"
                });
            }

            if (!context.ModuleSetup.Any(u => u.ModuleCode == "OSCN"))
            {
                context.ModuleSetup.AddOrUpdate(
                t => t.ModuleCode,
                new DomainLayer.Models.ModuleSetup
                {
                    ModuleCode = "OSCN"
                    ,
                    ModuleName = "BP Catalog Numbers"
                    ,
                    PrimaryKey = "ItemCode,CardCode,Substitute"
                    ,
                    IsActive = true
                    ,
                    CreateUserID = 1
                    ,
                    CreateDate = DateTime.Now
                    ,
                    EntityType = "AlternateCatNum"
                    ,
                    EntityName = "AlternateCatNum"
                });
            }

            if (!context.ModuleSetup.Any(u => u.ModuleCode == "OITT"))
            {
                context.ModuleSetup.AddOrUpdate(
                t => t.ModuleCode,
                new DomainLayer.Models.ModuleSetup
                {
                    ModuleCode = "OITT"
                    ,
                    ModuleName = "Bill of Materials"
                    ,
                    PrimaryKey = "Code"
                    ,
                    IsActive = true
                    ,
                    CreateUserID = 1
                    ,
                    CreateDate = DateTime.Now
                    ,
                    EntityType = "ProductTree"
                    ,
                    EntityName = "ProductTrees AlternateCatNum"
                });
            }


            //This is my Add
            if (!context.ModuleSetup.Any(u => u.ModuleCode == "OINV"))
            {
                context.ModuleSetup.AddOrUpdate(
                t => t.ModuleCode,
                new DomainLayer.Models.ModuleSetup
                {
                    ModuleCode = "OINV"
                    ,
                    ModuleName = "Debit Notes"
                    ,
                    PrimaryKey = "DocEntry"
                    ,
                    IsActive = true
                    ,
                    CreateUserID = 1
                    ,
                    CreateDate = DateTime.Now
                    ,
                    EntityType = "Document"
                    ,
                    EntityName = "DebitNotes"
                });
            }

            if (!context.ModuleSetup.Any(u => u.ModuleCode == "ORCT"))
            {
                context.ModuleSetup.AddOrUpdate(
                t => t.ModuleCode,
                new DomainLayer.Models.ModuleSetup
                {
                    ModuleCode = "ORCT"
                    ,
                    ModuleName = "Payments"
                    ,
                    PrimaryKey = "DocEntry"
                    ,
                    IsActive = true
                    ,
                    CreateUserID = 1
                    ,
                    CreateDate = DateTime.Now
                    ,
                    EntityType = "Payment"
                    ,
                    EntityName = "Payment"
                });
            }

            if (!context.ModuleSetup.Any(u => u.ModuleCode == "OJDT"))
            {
                context.ModuleSetup.AddOrUpdate(
                t => t.ModuleCode,
                new DomainLayer.Models.ModuleSetup
                {
                    ModuleCode = "OJDT"
                    ,
                    ModuleName = "Journal Entry"
                    ,
                    PrimaryKey = "JdtNum"
                    ,
                    IsActive = true
                    ,
                    CreateUserID = 1
                    ,
                    CreateDate = DateTime.Now
                    ,
                    EntityType = "JournalEntry"
                    ,
                    EntityName = "JournalEntry"
                });
            }
            //to here this is my add
            #endregion

            //context.QueryManager.AddOrUpdate(
            //    t => t.QueryCode, new DomainLayer.Models.QueryManager
            //    {
            //        QueryCode = "EBlast",
            //        QueryName = "Get all data from SAP"
            //                        ,
            //        CreateDate = DateTime.Now
            //                        ,
            //        IsActive = false
            //                        ,
            //        CreateUserId = 1
            //                        ,
            //        ConnectionString = "1"
            //                        ,
            //        ConnectionType = "SAP"
            //                        ,
            //        QueryString = $@"SELECT * FROM (
            //                                                        SELECT
            //                                                            T0.""U_AR_INVOICENO"" as ""Billing Statement No."",
            //                                                            TO_VARCHAR(CAST(T0.""DocDate"" as Date), 'MM-dd-yyyy') as ""Billing Date"",
            //                                                            T0.""U_PO_PONo"" as ""Customers PO number"",
            //                                                            TO_VARCHAR(CAST(T0.""DocDueDate"" as Date), 'MM-dd-yyyy') ""Due Date"",
            //                                                            T0.""Comments"" as ""Description"",
            //                                                            TO_VARCHAR(DAYS_BETWEEN(T0.""DocDueDate"", CURRENT_DATE)) ""Age"",
            //                                                            TO_VARCHAR(T0.""DocTotal"", '9,999.00') as ""Amount"",
            //                                                            T0.""U_Remarks"" as ""Remarks""
            //                                                        FROM

            //                                                            ODPI T0 INNER JOIN
            //                                                            OCPR T1 ON T0.""CardCode"" = T1.""CardCode""
            //                                                        WHERE
            //                                                            T0.""CANCELED"" = 'N' AND
            //                                                            T0.""DocStatus"" = 'O' AND
            //                                                            IFNULL(T1.""E_MailL"", '') <> '' AND
            //                                                            T1.""EmlGrpCode"" = 'eBlast' AND
            //                                                            T0.""CardCode"" = '#CARDCODE#'
            //                                                            ORDER BY T0.""CardName"", T0.""DocDate"" ) AS TB1
            //                                                union all
            //                                                SELECT
            //                                                    '' ""Document"",
            //                                                 '' ""Billing Date"",
            //                                                 '' ""Customers PO number"",
            //                                                 '' ""DocDueDate"",
            //                                                 'Total: ' ""Description"",
            //                                                 '' ""Age"",
            //                                                 TO_VARCHAR(SUM(T0.""DocTotal""), '9,999.00') as ""Amount"", 
            //                                                 '' as ""Remarks""
            //                                                FROM
            //                                                    ODPI T0 INNER JOIN
            //                                                    OCPR T1 ON T0.""CardCode"" = T1.""CardCode""
            //                                                WHERE
            //                                                    T0.""CANCELED"" = 'N' AND
            //                                                    T0.""DocStatus"" = 'O' AND
            //                                                    IFNULL(T1.""E_MailL"", '') <> '' AND
            //                                                    T1.""EmlGrpCode"" = 'eBlast' AND
            //                                                    T0.""CardCode"" = '#CARDCODE#' "
            //    });
            //context.QueryManager.AddOrUpdate(
            //    t => t.QueryCode, new DomainLayer.Models.QueryManager
            //    {
            //        QueryCode = "Email",
            //        QueryName = "List of Email",
            //        CreateDate = DateTime.Now,
            //        IsActive = false,
            //        CreateUserId = 1,
            //        ConnectionString = "1",
            //        ConnectionType = "SAP",
            //        QueryString = $@"SELECT T0.""CardCode"",
            //                                                T0.""CardName"",
            //                                                T1.""E_MailL"" ""Email""--'danilo.veroy@direcbsi.com'
            //                                            FROM ODPI T0 INNER JOIN OCPR T1 ON T0.""CardCode"" = T1.""CardCode""
            //                                            WHERE T0.""CANCELED"" = 'N' AND T0.""DocStatus"" = 'O' 
            //                                                AND IFNULL(T1.""E_MailL"", '') <> '' AND T1.""EmlGrpCode"" = 'eBlast'
            //                                            GROUP BY
            //                                                T0.""CardCode"",
            //                                                T0.""CardName"",
            //                                                T1.""E_MailL""
            //                                            ORDER BY
            //                                                T0.""CardName"" "
            //    });
            context.SaveChanges();
        }
    }
}
