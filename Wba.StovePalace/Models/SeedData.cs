using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Wba.StovePalace.Data;
using System;
using System.Linq;

namespace Wba.StovePalace.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new StoveContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<StoveContext>>()))
            {
                if (!context.Fuel.Any() && !context.Brand.Any() && !context.Stove.Any())
                {
                    Fuel hout = new Fuel {FuelName = "Hout"};
                    Fuel kolen = new Fuel {FuelName = "Kolen"};
                    Fuel pellets = new Fuel { FuelName = "Pellets" };
                    Fuel lpg = new Fuel { FuelName = "LPG" };
                    Fuel butaan = new Fuel { FuelName = "Butaan" };
                    Fuel propaan = new Fuel { FuelName = "Propaan" };
                    Fuel elektriciteit = new Fuel { FuelName = "Elektriciteit" };
                    context.Fuel.AddRange(
                        hout, kolen, pellets, lpg, butaan, propaan,elektriciteit
                    );
                    context.SaveChanges();

                    Brand saey = new Brand {BrandName = "Saey"};
                    Brand jotul = new Brand {BrandName = "Jotul"};
                    Brand moderna = new Brand { BrandName = "Moderna" };
                    Brand attech = new Brand { BrandName = "Attech" };
                    Brand barbas = new Brand { BrandName = "Barbas" };
                    Brand contura = new Brand { BrandName = "Contura" };
                    Brand dalzotto = new Brand { BrandName = "Dal Zotto" };
                    Brand invicta = new Brand { BrandName = "Invicta" };
                    context.Brand.AddRange(
                        saey, jotul, moderna, attech, barbas, contura, dalzotto, invicta
                    );
                    context.SaveChanges();

                    context.Stove.AddRange(
                        new Stove { Brand = moderna, Fuel = hout, ModelName = "Modena", Performance = 19000, SalesPrice = 2590m, ImagePath = "dovre01.jpg" },
                        new Stove { Brand = moderna, Fuel = pellets, ModelName="Bella", Performance=14000, SalesPrice=1599m, ImagePath = "dovre02.jpg" },
                        new Stove { Brand = saey, Fuel = hout, ModelName="Gustav", Performance=9000, SalesPrice=2200m, ImagePath = "saey01.jpg" },
                        new Stove { Brand = saey, Fuel = hout, ModelName="Scope", Performance=9000, SalesPrice=2199m, ImagePath = "saey02.jpg" },
                        new Stove { Brand = saey, Fuel = kolen, ModelName="Duo", Performance=7000, SalesPrice=1999m, ImagePath = "saey03.jpg" },
                        new Stove { Brand = saey, Fuel = pellets, ModelName = "Qubic 9", Performance = 9600, SalesPrice = 1599m, ImagePath = "saey04.jpg" },
                        new Stove { Brand = saey, Fuel = hout, ModelName = "Buster 1", Performance = 1200, SalesPrice = 1799m, ImagePath = "saey05.jpg" },
                        new Stove { Brand = saey, Fuel = hout, ModelName = "Buster 2", Performance = 1500, SalesPrice = 2199m, ImagePath = "saey06.png" },
                        new Stove { Brand = jotul, Fuel=hout, ModelName="F167", Performance=10500, SalesPrice=3075m, ImagePath = "jotul02.jpg" },
                        new Stove { Brand = jotul, Fuel=hout, ModelName="F400", Performance=15000, SalesPrice=3500m, ImagePath = "yotul01.jpg" }
                        );
                    context.SaveChanges();
                }

            }
        }

    }
}
