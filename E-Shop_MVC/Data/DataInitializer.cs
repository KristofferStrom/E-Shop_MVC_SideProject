using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Shop_MVC.Models.Data
{
    public class DataInitializer
    {
        internal static void SeedData(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            dbContext.Database.Migrate();

            SeedRoles(dbContext);
            SeedUsers(userManager);

            SeedColors(dbContext);
            SeedCompanies(dbContext);
            SeedCategories(dbContext);
            SeedSubCategories(dbContext);
            SeedProducts(dbContext);
            SeedReviews(dbContext);
        }

        private static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            AddUserIfNotExists(userManager, "kristoffer.strom@hotmail.se", "Hejsan123#", new string[] { "ProductManager", "Admin" });
        }


        private static void SeedRoles(ApplicationDbContext dbContext)
        {
            AddRoleIfNotExists(dbContext, "Admin");
            AddRoleIfNotExists(dbContext, "ProductManager");
        }



        private static void SeedReviews(ApplicationDbContext dbContext)
        {
            AddReviewIfNotExists(dbContext, "Hans", 4, "Bra trådlösa hörlurar, men lite klen bas.\r\n\r\nPlus+ väldigt lätta och bekväma\r\nMinus- går inte att lyssna samtidigt som du laddar dem.\r\nOch alldeles för dyra.", "Bose QuietComfort");
            AddReviewIfNotExists(dbContext, "Henrik", 5, "Köpte den till sonen som är 15. Passar perfekt. Ta bort brus runt om. Mycket bra ljud och hög komfort att ha på sig", "Bose QuietComfort");
            AddReviewIfNotExists(dbContext, "Jennifer", 5, "Denna är fantiskt MacBook Air! klass rakt igenom !", "MacBook Air 13.3");
            AddReviewIfNotExists(dbContext, "Julia", 4, "Strålande, extremt snabb, rekommendera verkligen denna.", "MacBook Pro 13");
            AddReviewIfNotExists(dbContext, "Krille", 5, "Hur snabb som helst...Rosetta programmet funkar klockrent för alla program som inte uppdaterats till M1 processorn ännu.", "MacBook Pro 13");
            AddReviewIfNotExists(dbContext, "Tomas", 1, "Efter två år beslutade jag att avfrosta (normal användning, och avfrostning enligt instruktionerna).\r\nNu knäpper den lite, och det var det :(", "Matsui kyl/frys");
            AddReviewIfNotExists(dbContext, "Astrid", 5, "En liten frys som är lättplacerad och tystgående. Har inte haft den så länge men är väldigt nöjd hittills.", "Matsui kyl/frys");
            AddReviewIfNotExists(dbContext, "Gustav", 4, "Perfekt med dubbla simkort, slipper två telefoner.", "iPhone XR 64 GB");
            AddReviewIfNotExists(dbContext, "Nisse", 5, "Jag uppgradera ifrån iPhone 6s och ångrar inte någonting. Detta är med ljusår den bästa iPhone som jag haft, fantastiskt batteri, snabb faceID, bra kamera och trevlig LCD-skärm. Var först skeptiskt till storleken och tyngden, men detta är inget problem, går bra att ha i jeansen.", "iPhone XR 64 GB");
            AddReviewIfNotExists(dbContext, "Mia", 5, "Mycket nöjd med min iPhone. Bytte från Huawei till iPhone för cirka en månad sedan och är mer än nöjd med mitt byte. Snabb telefon och väldigt lätthanterlig.", "iPhone XR 64 GB");
            AddReviewIfNotExists(dbContext, "Micke", 5, "Fantastiskt ljud och otrolig brusreducering. Nya fiffiga funktioner!", "Around-ear");
            AddReviewIfNotExists(dbContext, "Christoffer", 5, "Jag köpte den här telefonen idag och den är jättebra, jättesnabb och har väldigt bra batteritid. Jag rekommenderar denna telefon väldigt mycket till alla ni som funderar på att köpa den", "iPhone XR 64 GB");
            AddReviewIfNotExists(dbContext, "Peter", 3, "", "Bosch kylskåp/frys");
            AddReviewIfNotExists(dbContext, "Kennet", 2, "", "Harper3-sits soffa");
        }



        private static void SeedProducts(ApplicationDbContext dbContext)
        {
            AddProductIfNotExists(dbContext, "MacBook Air 13.3",
                "Otroligt tunna och lätta Apple MacBook Air (2020) är nu kraftfullare än någonsin",
                "Otroligt tunna och lätta Apple MacBook Air (2020) är nu kraftfullare än någonsin. Den har en kristallklar Retina-skärm, nya Magic Keyboard, Touch ID, processorer med upp till dubbel prestanda, snabbare grafik och dubbelt så stor lagringskapacitet.",
                "Apple",
                "Datorer & surfplattor",
                "macbook.png",
                "Silver",
                12690m,
                8,
                2);
            AddProductIfNotExists(dbContext, "Matsui kyl/frys",
                "En energisnål kyl/frys",
                "Matsui kyl/frys är en fristående och energisnål kyl/frys med stor grönsakslåda och en 50 cm bred fryslåda.",
                "Matsui",
                "Kyl & frys",
                "matsuiFrys.png",
                "Orange",
                2790m,
                10,
                3);
            AddProductIfNotExists(dbContext, "Harper3-sits soffa",
                "En soffa",
                "Tyg Jump beige med svarta metallben. 3-sits soffa. Plymåer med bollfiber/skuren polyeter som ger en ytmjuk känsla. Höga ben som underlättar vid städning",
                "Mio",
                "Soffor",
                "harper.png",
                "Grå",
                13495m,
                5,
                1);
            AddProductIfNotExists(dbContext, "Dell XPS 17-9700 17",
                "En bärbar dator",
                "Dell XPS 17-9700 17 bärbar dator har en 17 tum stor 4K UHD HDR InfinityEdge-skärm i ett chassi jämförbart med 15,6-tums enheter. Spela din favoritmusik med Maxx Audio PRO-högtalarna och ta med datorn överallt tack vare den lätta vikten på 2,5kg.",
                "Dell",
                "Datorer & surfplattor",
                "dell.png",
                "Svart",
                24996m,
                3,
                2);
            AddProductIfNotExists(dbContext, "MacBook Pro 13",
                "MacBook Pro 13 M1 2020 levererar överladdad prestanda med hjälp av ett Apple M1 chip",
                "MacBook Pro 13 M1 2020 levererar överladdad prestanda med hjälp av ett Apple M1 chip som är designat för proffsen. Den kommer med en Retina-skärm med True Tone balans, P3 färgskala med gamut-stöd och superhög ljusstyrka.",
                "Apple",
                "Datorer & surfplattor",
                "macbookPro.png",
                "Silver",
                15990m,
                15,
                2);
            AddProductIfNotExists(dbContext, "Bose QuietComfort",
                "Bose QuietComfort trådlösa hörlurar II erbjuder en förstklassig akustisk upplevelse",
                "Bose QuietComfort trådlösa hörlurar II erbjuder en förstklassig akustisk upplevelse tack vare aktiv brusreduceringsteknik som dämpar yttre brus. Bluetooth teknik med NFC parning gör att du kan du använda hörlurarna utan trassliga kablar.",
                "Bose",
                "Ljud & bild",
                "bose.png",
                "Svart",
                2489m,
                30,
                3);
            AddProductIfNotExists(dbContext, "A20e smartphone",
                "Samsung Galaxy A20e smartphone erbjuder en 13+5 Mpx dubbel bakre kamera",
                "Samsung Galaxy A20e smartphone erbjuder en 13+5 Mpx dubbel bakre kamera och låter dig spara bilder och videoklipp i minnet på 32 GB (utbyggbart med SD-minneskort). Tack vare dess 8-kärniga processor och 3 GB RAM har du tillräckligt med kraft för alla vardagliga uppgifter.",
                "Samsung",
                "Telefoner",
                "samsung.png",
                "Svart",
                1790m,
                20,
                3);
            AddProductIfNotExists(dbContext, "iPhone XR 64 GB",
                "Upplev nya möjligheter med revolutionerande iPhone XR.",
                "Upplev nya möjligheter med revolutionerande iPhone XR. Telefonen är utrustad med en kant-till-kant 6,1” Liquid Retina LCD-skärm med stort färgomfång, och en exceptionell bakre kamera på 12 MP som fångar det rätta ögonblicket.",
                "Apple",
                "Telefoner",
                "iphone.png",
                "Svart",
                6090m,
                20,
                3);
            AddProductIfNotExists(dbContext, "Around-ear",
                "Samsung trådlösa around-ear hörlurar WH-1000XM4 är ett smart, intuitivt val som ger dig högupplöst ljud",
                "Samsung trådlösa around-ear hörlurar WH-1000XM4 är ett smart, intuitivt val som ger dig högupplöst ljud, branschledande brusreducering, röstassistent-kompabilitet, kraftfullt batteri med snabbladdning och inbyggd mikrofon för samtal.",
                "Samsung",
                "Ljud & bild",
                "headphones.png",
                "Svart",
                3990m,
                20,
                3);
            AddProductIfNotExists(dbContext, "Headphones",
                "Bose Noise Canceling Headphones 700 är trådlösa around ear-hörlurar med en suverän kvalitet.",
                "Bose Noise Canceling Headphones 700 är trådlösa around ear-hörlurar med en suverän kvalitet. Ett fantastiskt ljud, revolutionerande Bose AR och 11 (!) olika inställningsnivåer för brusreducering gör detta till trådlösa hörlurar på en helt egen nivå",
                "Bose",
                "Ljud & bild",
                "bose2.png",
                "Silver",
                2990m,
                30,
                2);
            AddProductIfNotExists(dbContext, "Bosch kylskåp/frys",
                "Välj din favoritfärg och gör ditt kök personligt med Bosch VarioStyle kombiskåp",
                "Välj din favoritfärg och gör ditt kök personligt med Bosch VarioStyle kombiskåp och denna färgfront. Denna front passar 186 cm Bosch VarioStyle kylskåp/frys kombiskåp KGN36IJ3A.",
                "Bosch",
                "Kyl & frys",
                "bosch2.png",
                "Lila",
                2295m,
                5,
                5);
            //AddProductIfNotExists(dbContext, "",
            //    "",
            //    "",
            //    "",
            //    "",
            //    "",
            //    "",
            //    3990m,
            //    20,
            //    3);
            //AddProductIfNotExists(dbContext, "",
            //    "",
            //    "",
            //    "",
            //    "",
            //    "",
            //    "",
            //    3990m,
            //    20,
            //    3);
            //AddProductIfNotExists(dbContext, "",
            //    "",
            //    "",
            //    "",
            //    "",
            //    "",
            //    "",
            //    3990m,
            //    20,
            //    3);
            //AddProductIfNotExists(dbContext, "",
            //    "",
            //    "",
            //    "",
            //    "",
            //    "",
            //    "",
            //    3990m,
            //    20,
            //    3);
            //AddProductIfNotExists(dbContext, "",
            //    "",
            //    "",
            //    "",
            //    "",
            //    "",
            //    "",
            //    3990m,
            //    20,
            //    3);
            //AddProductIfNotExists(dbContext, "",
            //    "",
            //    "",
            //    "",
            //    "",
            //    "",
            //    "",
            //    3990m,
            //    20,
            //    3);


        }



        private static void SeedSubCategories(ApplicationDbContext dbContext)
        {
            AddSubCategoryIfNotExists(dbContext, "Datorer & surfplattor", "Elektronik");
            AddSubCategoryIfNotExists(dbContext, "Telefoner", "Elektronik");
            AddSubCategoryIfNotExists(dbContext, "Ljud & bild", "Elektronik");
            AddSubCategoryIfNotExists(dbContext, "Kyl & frys", "Vitvaror");
            AddSubCategoryIfNotExists(dbContext, "Diskmaskin", "Vitvaror");
            AddSubCategoryIfNotExists(dbContext, "Spis", "Vitvaror");
            AddSubCategoryIfNotExists(dbContext, "Soffor", "Inredning");
            AddSubCategoryIfNotExists(dbContext, "Bord", "Inredning");
            AddSubCategoryIfNotExists(dbContext, "Tavlor", "Inredning");

        }



        private static void SeedCategories(ApplicationDbContext dbContext)
        {
            AddCategoryIfNotExists(dbContext, "Elektronik");
            AddCategoryIfNotExists(dbContext, "Vitvaror");
            AddCategoryIfNotExists(dbContext, "Inredning");
        }



        private static void SeedCompanies(ApplicationDbContext dbContext)
        {
            AddCompanyIfNotExists(dbContext, "Apple");
            AddCompanyIfNotExists(dbContext, "Matsui");
            AddCompanyIfNotExists(dbContext, "Dell");
            AddCompanyIfNotExists(dbContext, "Samsung");
            AddCompanyIfNotExists(dbContext, "Philips");
            AddCompanyIfNotExists(dbContext, "Bosch");
            AddCompanyIfNotExists(dbContext, "Mio");
            AddCompanyIfNotExists(dbContext, "Ellos Home");
            AddCompanyIfNotExists(dbContext, "Bose");
        }



        private static void SeedColors(ApplicationDbContext dbContext)
        {
            AddColorIfNotExists(dbContext, "Grön");
            AddColorIfNotExists(dbContext, "Svart");
            AddColorIfNotExists(dbContext, "Silver");
            AddColorIfNotExists(dbContext, "Grå");
            AddColorIfNotExists(dbContext, "Vit");
            AddColorIfNotExists(dbContext, "Brun");
            AddColorIfNotExists(dbContext, "Guld");
            AddColorIfNotExists(dbContext, "Orange");
            AddColorIfNotExists(dbContext, "Lila");
        }

        private static void AddUserIfNotExists(UserManager<IdentityUser> userManager, string userName, string password, string[] roles)
        {
            if (userManager.FindByEmailAsync(userName).Result != null) return;

            var user = new IdentityUser
            {
                UserName = userName,
                Email = userName,
                EmailConfirmed = true
            };

            var result = userManager.CreateAsync(user, password).Result;
            var r = userManager.AddToRolesAsync(user, roles).Result;
        }
        private static void AddRoleIfNotExists(ApplicationDbContext dbContext, string roleName)
        {
            var role = dbContext.Roles.FirstOrDefault(dbRole => dbRole.Name == roleName);
            if (role == null)
            {
                dbContext.Roles.Add(new IdentityRole
                {
                    Name = roleName,
                    NormalizedName = roleName
                });

                dbContext.SaveChanges();
            }
        }

        private static void AddColorIfNotExists(ApplicationDbContext dbContext, string colorTitle)
        {
            var color = dbContext.Colors.FirstOrDefault(dbCol => dbCol.Title == colorTitle);
            if (color == null)
            {
                dbContext.Colors.Add(new ProductColor
                {
                    Title = colorTitle
                });

                dbContext.SaveChanges();
            }
        }
        private static void AddCompanyIfNotExists(ApplicationDbContext dbContext, string companyTitle)
        {
            var company = dbContext.Companies.FirstOrDefault(dbCom => dbCom.Title == companyTitle);
            if (company == null)
            {
                dbContext.Companies.Add(new Company
                {
                    Title = companyTitle
                });

                dbContext.SaveChanges();
            }
        }
        private static void AddCategoryIfNotExists(ApplicationDbContext dbContext, string categoryTitle)
        {
            var category = dbContext.Categories.FirstOrDefault(dbCat => dbCat.Title == categoryTitle);
            if (category == null)
            {
                dbContext.Categories.Add(new ProductCategory
                {
                    Title = categoryTitle
                });

                dbContext.SaveChanges();
            }
        }
        private static void AddSubCategoryIfNotExists(ApplicationDbContext dbContext, string subCategoryTitle, string categoryTitle)
        {
            var subCategory = dbContext.SubCategories.FirstOrDefault(dbSubCat => dbSubCat.Title == subCategoryTitle);
            if (subCategory == null)
            {
                dbContext.SubCategories.Add(new SubProductCategory
                {
                    Title = subCategoryTitle,
                    Category = dbContext.Categories.FirstOrDefault(dbCat => dbCat.Title == categoryTitle)
                });

                dbContext.SaveChanges();
            }
        }
        private static void AddProductIfNotExists(ApplicationDbContext dbContext, string productTitle, string shortDescription, string longDescription, string companyTitle, string subCategoryTitle, string imgTitle, string colorTitle, decimal price, int inStock, int warranty)
        {
            var product = dbContext.Products.FirstOrDefault(dbProd => dbProd.Title == productTitle);
            if (product == null)
            {
                dbContext.Products.Add(new Product
                {
                    Title = productTitle,
                    ShortDescription = shortDescription,
                    LongDescription = longDescription,
                    Price = price,
                    InStock = inStock,
                    Warranty = warranty,
                    Company = dbContext.Companies.FirstOrDefault(dbCom => dbCom.Title == companyTitle),
                    SubCategory = dbContext.SubCategories.FirstOrDefault(dbSubCat => dbSubCat.Title == subCategoryTitle),
                    imgTitle = imgTitle,
                    Color = dbContext.Colors.FirstOrDefault(dbCol => dbCol.Title == colorTitle)
                });
                dbContext.SaveChanges();
            }
        }
        private static void AddReviewIfNotExists(ApplicationDbContext dbContext, string name, int rate, string reviewText, string productTitle)
        {
            var review = dbContext.Reviews.FirstOrDefault(dbRev => dbRev.CustomerName == name);
            if (review == null)
            {
                dbContext.Reviews.Add(new ProductReview
                {
                    CustomerName = name,
                    Rate = rate,
                    Date = DateTime.Now,
                    ReviewText = reviewText,
                    Product = dbContext.Products.FirstOrDefault(dbProd => dbProd.Title == productTitle)
                });

                dbContext.SaveChanges();
            }

        }
    }
}
