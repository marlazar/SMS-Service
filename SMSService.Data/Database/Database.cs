using ServiceStack.OrmLite;
using SMSService.Core.Models;
using System.Data;

namespace SMSService.Data.Database
{
    public class Database
    {
        public static void Create(IDbConnection db)
        {
            if (!db.TableExists<SMS>())
                db.CreateTable<SMS>();

            if (!db.TableExists<Country>())
            {
                db.CreateTable<Country>();
                AddDefaultCountries(db);
            }
        }

        private static void AddDefaultCountries(IDbConnection db)
        {
            db.Insert(new Country { Name = "Germany", CountryCode = "49", MobileCountryCode = "262", PricePerSMS = 0.055m });
            db.Insert(new Country { Name = "Austria", CountryCode = "43", MobileCountryCode = "232", PricePerSMS = 0.053m });
            db.Insert(new Country { Name = "Poland", CountryCode = "48", MobileCountryCode = "260", PricePerSMS = 0.032m });
        }
    }
}
