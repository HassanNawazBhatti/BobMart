using BobMart.Models;
using Microsoft.AspNetCore.Identity;

namespace BobMart.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Admin", "Customer" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                    await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            var adminEmail = "admin@bobmart.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "System",
                    LastName = "Administrator",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newAdmin, "Admin@123");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
            }
        }

        public static async Task SeedProductsAsync(ApplicationDbContext context)
        {
            if (context.Categories.Any() || context.Products.Any())
                return;

            var categories = new List<Category>
            {
                new Category { Name = "Gothic Clothing", Description = "Premium dark fashion clothing including coats, denim, and layered apparel." },
                new Category { Name = "Dark Romance Books", Description = "Classic and modern gothic romance literature collection." },
                new Category { Name = "Gothic Accessories", Description = "Elegant dark-themed accessories including jewelry and ornaments." },
                new Category { Name = "Alternative Fashion", Description = "Contemporary alternative streetwear and expressive fashion pieces." },
                new Category { Name = "Dark Gaming", Description = "Gaming peripherals, collectibles, and aesthetic gear for gamers." },
                new Category { Name = "Gothic Home", Description = "Interior decor items with dark, Victorian-inspired aesthetics." },
                new Category { Name = "Tech Accessories", Description = "High-performance tech gadgets with minimalist dark styling." },
                new Category { Name = "Mobile Accessories", Description = "Protective and aesthetic mobile accessories with modern design." },
                new Category { Name = "Fitness Gear", Description = "Workout equipment and apparel with a sleek dark aesthetic." }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            var products = new List<Product>();

            Product Create(int categoryId, string name, string desc, decimal price, string brand, string imageUrl, bool featured = false)
            {
                return new Product
                {
                    CategoryId = categoryId,
                    Name = name,
                    Description = desc,
                    Price = price,
                    Stock = Random.Shared.Next(15, 90),
                    Brand = brand,
                    Rating = Math.Round(Random.Shared.NextDouble() * 1.2 + 3.7, 1),
                    ReviewCount = Random.Shared.Next(30, 800),
                    IsFeatured = featured,
                    DateAdded = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 120)),
                    ImageUrl = imageUrl
                };
            }

            // ==================== GOTHIC CLOTHING ====================
            var gothic = categories.First(c => c.Name == "Gothic Clothing");
            products.AddRange(new[]
            {
                Create(gothic.Id, "Tailored Black Trench Coat",
                    "A premium long trench coat crafted with structured tailoring and a modern gothic silhouette. Made from durable poly-blend fabric with satin lining, this coat features sharp lapels, a double-breasted front, and a belted waist for an imposing yet elegant look.",
                    8999, "Nocturne Atelier",
                    "https://images.unsplash.com/photo-1544022613-e87ca75a784a?w=600&h=600&fit=crop", true),

                Create(gothic.Id, "Slim Fit Dark Denim Jeans",
                    "Refined slim-fit jeans designed with a deep indigo-black wash for everyday wear. Reinforced stitching and stretch-blend denim provide comfort and durability. Perfect for layering with any dark aesthetic outfit.",
                    3999, "NightThread",
                    "https://images.unsplash.com/photo-1542272604-787c3835535d?w=600&h=600&fit=crop"),

                Create(gothic.Id, "Velvet Hooded Cape",
                    "An elegant velvet cape inspired by Victorian-era design language. Features a dramatic hood, front clasp closure, and flowing silhouette that drapes beautifully. Ideal for events or gothic-themed occasions.",
                    7499, "RavenStyle",
                    "https://images.unsplash.com/photo-1578587018452-892bacefd3f2?w=600&h=600&fit=crop"),

                Create(gothic.Id, "Structured Leather Pants",
                    "High-quality faux leather pants with durable stitching and a contemporary slim fit. Soft interior lining ensures comfort while the sleek exterior delivers a powerful aesthetic statement.",
                    6499, "DarkWear",
                    "https://images.unsplash.com/photo-1594938298603-c8148c4dae35?w=600&h=600&fit=crop"),

                Create(gothic.Id, "Victorian Corset Blouse",
                    "A refined corset-style blouse combining elegance with alternative fashion aesthetics. Features lace-up front detailing, puffed sleeves, and a structured bodice in breathable cotton blend.",
                    5199, "Nocturne",
                    "https://images.unsplash.com/photo-1596755094514-f87e34085b2c?w=600&h=600&fit=crop"),

                Create(gothic.Id, "Longline Wool Coat",
                    "Warm wool-blend coat designed with minimal gothic detailing for winter styling. Features a notch lapel, concealed button closure, and mid-calf length for maximum warmth and dramatic effect.",
                    8299, "BlackVeil",
                    "https://images.unsplash.com/photo-1539533113208-f6df8cc8b543?w=600&h=600&fit=crop"),

                Create(gothic.Id, "Monochrome Layered Shirt",
                    "Layered shirt design with contrasting black tones for a structured look. The asymmetric hem and tonal stitching create visual depth while maintaining a clean, modern silhouette.",
                    2899, "UrbanShade",
                    "https://images.unsplash.com/photo-1602810318383-e386cc2a3ccf?w=600&h=600&fit=crop"),

                Create(gothic.Id, "Dark Oversized Hoodie",
                    "Premium heavyweight hoodie with minimalist dark aesthetic design. Constructed from 400gsm cotton fleece with a relaxed drop-shoulder fit, kangaroo pocket, and adjustable drawstring hood.",
                    3499, "ShadowWear",
                    "https://images.unsplash.com/photo-1556821840-3a63f95609a7?w=600&h=600&fit=crop")
            });

            // ==================== DARK ROMANCE BOOKS ====================
            var books = categories.First(c => c.Name == "Dark Romance Books");
            products.AddRange(new[]
            {
                Create(books.Id, "Wuthering Heights — Emily Brontë",
                    "A timeless gothic romance exploring passion, revenge, and emotional intensity on the Yorkshire moors. This hardcover edition includes scholarly annotations, a biographical introduction, and period illustrations.",
                    1899, "Penguin Classics",
                    "https://images.unsplash.com/photo-1544947950-fa07a98d237f?w=600&h=600&fit=crop", true),

                Create(books.Id, "Jane Eyre — Charlotte Brontë",
                    "A deeply emotional narrative of resilience and gothic romance. Follow Jane's journey from an oppressed orphan to an independent woman confronting dark secrets at Thornfield Hall.",
                    1999, "Penguin Classics",
                    "https://images.unsplash.com/photo-1512820790803-83ca734da794?w=600&h=600&fit=crop"),

                Create(books.Id, "Rebecca — Daphne du Maurier",
                    "A suspenseful gothic romance filled with mystery and psychological depth. The unnamed narrator must unravel the haunting legacy of the beautiful and enigmatic Rebecca at the grand Manderley estate.",
                    2199, "Virago Modern Classics",
                    "https://images.unsplash.com/photo-1476275466078-4007374efbbe?w=600&h=600&fit=crop"),

                Create(books.Id, "Dracula — Bram Stoker",
                    "The legendary gothic horror novel that defined vampire mythology. Follow Jonathan Harker's terrifying encounter with Count Dracula and the desperate fight to stop his spread of darkness across England.",
                    2499, "Classic Press",
                    "https://images.unsplash.com/photo-1535666669445-e8ace5681338?w=600&h=600&fit=crop"),

                Create(books.Id, "The Phantom of the Opera",
                    "A tragic gothic romance set in the atmospheric Paris Opera House. Gaston Leroux's masterpiece tells the story of a mysterious masked figure and his obsessive love for a young soprano.",
                    1799, "Vintage Books",
                    "https://images.unsplash.com/photo-1543002588-bfa74002ed7e?w=600&h=600&fit=crop"),

                Create(books.Id, "The Picture of Dorian Gray",
                    "Oscar Wilde's philosophical gothic novel exploring vanity, moral decay, and the corrupting influence of beauty. A young man's portrait ages while he remains eternally youthful — at a terrible cost.",
                    2099, "Oxford Press",
                    "https://images.unsplash.com/photo-1544716278-ca5e3f4abd8c?w=600&h=600&fit=crop"),

                Create(books.Id, "Frankenstein — Mary Shelley",
                    "A pioneering gothic science fiction tale of ambition and consequence. Victor Frankenstein's creation forces readers to confront questions about humanity, responsibility, and the limits of science.",
                    2299, "Penguin Classics",
                    "https://images.unsplash.com/photo-1519682337058-a94d519337bc?w=600&h=600&fit=crop"),

                Create(books.Id, "Northanger Abbey — Jane Austen",
                    "A witty gothic parody blending romance and satire. Young Catherine Morland's overactive imagination, fueled by gothic novels, leads to humorous misadventures at the mysterious Northanger Abbey.",
                    1599, "Vintage Classics",
                    "https://images.unsplash.com/photo-1524578271613-d550eacf6090?w=600&h=600&fit=crop")
            });

            // ==================== GOTHIC ACCESSORIES ====================
            var accessories = categories.First(c => c.Name == "Gothic Accessories");
            products.AddRange(new[]
            {
                Create(accessories.Id, "Onyx Signet Ring",
                    "A polished black onyx ring designed with minimalist elegance. The heavy sterling silver band features a flat-cut onyx stone with subtle engraving around the bezel. Perfect for daily wear.",
                    2999, "NightCraft",
                    "https://images.unsplash.com/photo-1605100804763-247f67b3557e?w=600&h=600&fit=crop", true),

                Create(accessories.Id, "Raven Pendant Necklace",
                    "A finely crafted pendant inspired by symbolic gothic motifs. Features a detailed raven design in oxidized silver on a 22-inch adjustable chain with lobster clasp.",
                    2499, "DarkWear",
                    "https://images.unsplash.com/photo-1599643478518-a784e5dc4c8f?w=600&h=600&fit=crop"),

                Create(accessories.Id, "Minimal Black Choker",
                    "A sleek and elegant choker suitable for modern alternative styling. Made from soft velvet ribbon with an adjustable clasp and a small silver pendant accent at the center.",
                    1499, "Nocturne",
                    "https://images.unsplash.com/photo-1611652022419-a9419f74343d?w=600&h=600&fit=crop"),

                Create(accessories.Id, "Sterling Silver Bracelet",
                    "A refined cuff bracelet with dark metallic finishing. Hand-polished sterling silver with an oxidized patina gives it a vintage gothic character that pairs well with any outfit.",
                    2199, "BlackForge",
                    "https://images.unsplash.com/photo-1573408301185-9146fe634ad0?w=600&h=600&fit=crop"),

                Create(accessories.Id, "Victorian Drop Earrings",
                    "Elegant earrings inspired by classical Victorian design. Feature cascading black crystal drops suspended from ornate silver filigree posts with secure butterfly backs.",
                    1799, "RavenStyle",
                    "https://images.unsplash.com/photo-1535632066927-ab7c9ab60908?w=600&h=600&fit=crop"),

                Create(accessories.Id, "Leather Utility Belt",
                    "Functional genuine leather belt with dark aesthetic detailing. Features a heavy antique-brass buckle, riveted grommets, and a subtle embossed texture along the strap.",
                    2599, "UrbanShade",
                    "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=600&h=600&fit=crop"),

                Create(accessories.Id, "Crystal Brooch Pin",
                    "Decorative brooch with dark crystal accent design. A cluster of black and smoky grey crystals set in a vintage silver frame. Perfect for pinning on lapels, scarves, or bags.",
                    1999, "Nocturne",
                    "https://images.unsplash.com/photo-1611591437281-460bfbe1220a?w=600&h=600&fit=crop"),

                Create(accessories.Id, "Metal Chain Wallet",
                    "Durable bi-fold wallet with integrated chain detailing. Made from genuine leather with a detachable 18-inch gunmetal chain, multiple card slots, and a zippered coin pocket.",
                    2399, "DarkCraft",
                    "https://images.unsplash.com/photo-1627123424574-724758594e93?w=600&h=600&fit=crop")
            });

            // ==================== ALTERNATIVE FASHION ====================
            var altFashion = categories.First(c => c.Name == "Alternative Fashion");
            products.AddRange(new[]
            {
                Create(altFashion.Id, "Distressed Graphic Tee",
                    "A premium cotton t-shirt featuring a vintage-washed distressed graphic print. Relaxed unisex fit with reinforced collar and soft-hand screenprint that won't crack or peel.",
                    2499, "UrbanShade",
                    "https://images.unsplash.com/photo-1576566588028-4147f3842f27?w=600&h=600&fit=crop", true),

                Create(altFashion.Id, "Cargo Jogger Pants",
                    "Relaxed-fit cargo joggers with multiple utility pockets and elastic ankle cuffs. Made from durable ripstop cotton in a washed black finish for an effortlessly cool streetwear look.",
                    3799, "ShadowWear",
                    "https://images.unsplash.com/photo-1624378439575-d8705ad7ae80?w=600&h=600&fit=crop"),

                Create(altFashion.Id, "Platform Combat Boots",
                    "Chunky platform combat boots with a 2-inch sole, side zip closure, and lace-up front. Constructed from durable synthetic leather with a padded collar for comfort during all-day wear.",
                    6999, "DarkWear",
                    "https://images.unsplash.com/photo-1605812860427-4024433a70fd?w=600&h=600&fit=crop"),

                Create(altFashion.Id, "Fishnet Long Sleeve Top",
                    "A versatile fishnet layering piece in classic black. Stretchy mesh construction with a crew neckline and thumbhole cuffs. Designed to be worn over or under other pieces for textured styling.",
                    1999, "Nocturne",
                    "https://images.unsplash.com/photo-1529139574466-a303027c1d8b?w=600&h=600&fit=crop"),

                Create(altFashion.Id, "Plaid Mini Skirt",
                    "A dark tartan plaid skirt with pleated construction and an adjustable waistband. The bold pattern adds visual interest while the structured drape keeps it looking sharp and polished.",
                    2899, "RavenStyle",
                    "https://images.unsplash.com/photo-1583496661160-fb5886a0aeae?w=600&h=600&fit=crop"),

                Create(altFashion.Id, "Oversized Denim Jacket",
                    "A washed-black oversized denim jacket with raw-edge detailing and antique brass hardware. Features chest flap pockets, side hand pockets, and an adjustable button-tab waist.",
                    5499, "NightThread",
                    "https://images.unsplash.com/photo-1551537482-f2075a1d41f2?w=600&h=600&fit=crop"),

                Create(altFashion.Id, "Chain Detail Belt",
                    "A faux leather belt with decorative chain draping and a large ring buckle. An instant outfit transformer that adds an edgy, punk-inspired touch to jeans, skirts, or trousers.",
                    1599, "BlackForge",
                    "https://images.unsplash.com/photo-1590874103328-eac38a683ce7?w=600&h=600&fit=crop"),

                Create(altFashion.Id, "Beanie with Patches",
                    "A ribbed knit beanie in charcoal black adorned with embroidered and woven patches. The slouchy fit keeps you warm while the patches let you express your personal style.",
                    1299, "UrbanShade",
                    "https://images.unsplash.com/photo-1576871337632-b9aef4c17ab9?w=600&h=600&fit=crop")
            });

            // ==================== DARK GAMING ====================
            var gaming = categories.First(c => c.Name == "Dark Gaming");
            products.AddRange(new[]
            {
                Create(gaming.Id, "Wireless Gaming Controller",
                    "An ergonomic wireless controller with textured grips, programmable buttons, and low-latency Bluetooth 5.0. Compatible with PC, console, and mobile. Includes a matte black charging dock.",
                    7999, "Razer",
                    "https://images.unsplash.com/photo-1592840496694-26d035b52b48?w=600&h=600&fit=crop", true),

                Create(gaming.Id, "RGB Gaming Mouse Pad XL",
                    "An extended desk pad with customizable RGB edge lighting and a micro-textured surface for precise mouse tracking. Non-slip rubber base keeps it locked in place during intense sessions.",
                    3499, "SteelSeries",
                    "https://images.unsplash.com/photo-1616588589676-62b3d4ff6e04?w=600&h=600&fit=crop"),

                Create(gaming.Id, "Gaming Headset 7.1 Surround",
                    "Over-ear gaming headset with virtual 7.1 surround sound, a detachable boom mic, and memory foam ear cushions. USB-C and 3.5mm connectivity for multi-platform use.",
                    9499, "HyperX",
                    "https://images.unsplash.com/photo-1599669454699-248893623440?w=600&h=600&fit=crop"),

                Create(gaming.Id, "Collectible Dragon Figurine",
                    "A hand-painted 8-inch dragon figurine crafted from high-quality resin. Exquisite detailing on scales, wings, and base makes it a stunning display piece for any gaming setup or shelf.",
                    4999, "DarkCraft",
                    "https://images.unsplash.com/photo-1566577134770-3d85bb3a9cc4?w=600&h=600&fit=crop"),

                Create(gaming.Id, "LED Strip Light Kit (2m)",
                    "An RGB LED strip kit with a remote control and 16 million color options. Self-adhesive backing allows easy installation behind monitors, desks, or shelves for immersive ambient lighting.",
                    1999, "Govee",
                    "https://images.unsplash.com/photo-1550745165-9bc0b252726f?w=600&h=600&fit=crop"),

                Create(gaming.Id, "Ergonomic Gaming Chair",
                    "A high-back gaming chair with lumbar support, adjustable armrests, and a 155-degree recline. Covered in breathable PU leather with contrast red stitching for a bold look.",
                    24999, "SecretLab",
                    "https://images.unsplash.com/photo-1598550476439-6847785fcea6?w=600&h=600&fit=crop"),

                Create(gaming.Id, "Desktop Monitor Riser",
                    "A sleek wooden monitor riser with built-in cable management and hidden storage compartments. Elevates your screen to an ergonomic height while keeping your desk clean and organized.",
                    3999, "UrbanShade",
                    "https://images.unsplash.com/photo-1593062096033-9a26b09da705?w=600&h=600&fit=crop"),

                Create(gaming.Id, "Mechanical Keyswitch Tester",
                    "A 12-switch sample board featuring popular mechanical switches from Cherry MX, Gateron, and Kailh. An essential tool for finding your perfect keyswitch feel before committing to a keyboard.",
                    1499, "WASD",
                    "https://images.unsplash.com/photo-1595225476474-87563907a212?w=600&h=600&fit=crop")
            });

            // ==================== GOTHIC HOME ====================
            var home = categories.First(c => c.Name == "Gothic Home");
            products.AddRange(new[]
            {
                Create(home.Id, "Black Pillar Candle Set",
                    "A set of three unscented black pillar candles in graduated heights. Made from premium paraffin wax with a cotton wick for a clean, even burn. Creates a dramatic ambient atmosphere.",
                    1499, "DarkCraft",
                    "https://images.unsplash.com/photo-1602607687939-8e4e0a75a72c?w=600&h=600&fit=crop", true),

                Create(home.Id, "Ornate Wall Mirror",
                    "A stunning Victorian-inspired wall mirror with an intricately carved black and gold baroque frame. The beveled glass edge adds depth and elegance. Measures 24x36 inches.",
                    8999, "Nocturne",
                    "https://images.unsplash.com/photo-1618220179428-22790b461013?w=600&h=600&fit=crop"),

                Create(home.Id, "Velvet Throw Pillow (Set of 2)",
                    "Luxurious crushed velvet throw pillows in deep black with invisible zipper closure. Filled with hypoallergenic polyester fiberfill. Each pillow measures 18x18 inches.",
                    2499, "RavenStyle",
                    "https://images.unsplash.com/photo-1584100936595-c0654b55a2e2?w=600&h=600&fit=crop"),

                Create(home.Id, "Skull Bookend Pair",
                    "A pair of heavyweight resin skull bookends with a matte black finish. Each skull features realistic anatomical detail and a felt-padded base to protect shelves. Holds books securely.",
                    3499, "DarkCraft",
                    "https://images.unsplash.com/photo-1588345921523-c2dcdb7f1dcd?w=600&h=600&fit=crop"),

                Create(home.Id, "Incense Burner — Dragon",
                    "A beautifully detailed dragon incense burner sculpted from cold-cast resin. Smoke flows through the dragon's mouth creating a mesmerizing waterfall effect. Includes a starter pack of cones.",
                    2999, "BlackForge",
                    "https://images.unsplash.com/photo-1603006905003-be475563bc59?w=600&h=600&fit=crop"),

                Create(home.Id, "Gothic Table Lamp",
                    "A Victorian-style table lamp with a stained glass shade in deep reds and blacks. The ornate metal base features scroll detailing. Uses a standard E26 bulb (not included).",
                    5499, "Nocturne",
                    "https://images.unsplash.com/photo-1507473885765-e6ed057ab6fe?w=600&h=600&fit=crop"),

                Create(home.Id, "Dark Floral Wall Art Print",
                    "A high-quality giclée print on archival cotton paper featuring a moody Dutch master-style floral arrangement. Ships unframed, ready for your choice of frame. 16x20 inches.",
                    1999, "UrbanShade",
                    "https://images.unsplash.com/photo-1579783902614-a3fb3927b6a5?w=600&h=600&fit=crop"),

                Create(home.Id, "Wrought Iron Candelabra",
                    "A five-arm wrought iron candelabra with a matte black powder-coated finish. Stands 18 inches tall and accommodates standard taper candles. A centrepiece that commands attention.",
                    3999, "BlackForge",
                    "https://images.unsplash.com/photo-1633609753556-4cdea988cfb3?w=600&h=600&fit=crop")
            });

            // ==================== TECH ACCESSORIES ====================
            var tech = categories.First(c => c.Name == "Tech Accessories");
            products.AddRange(new[]
            {
                Create(tech.Id, "Mechanical RGB Keyboard",
                    "High-performance mechanical keyboard with hot-swappable switches and per-key RGB lighting. Features a detachable USB-C cable, PBT keycaps, and a slim aluminium frame in matte black.",
                    12999, "HyperX",
                    "https://images.unsplash.com/photo-1587829741301-dc798b83add3?w=600&h=600&fit=crop", true),

                Create(tech.Id, "Precision Gaming Mouse",
                    "Ergonomic gaming mouse with a 25,600 DPI optical sensor, 6 programmable buttons, and onboard memory for custom profiles. Lightweight honeycomb design at just 69 grams.",
                    5999, "Razer",
                    "https://images.unsplash.com/photo-1527814050087-3793815479db?w=600&h=600&fit=crop"),

                Create(tech.Id, "Noise Cancelling Headphones",
                    "Premium over-ear headphones with active noise cancellation, 30-hour battery life, and multipoint Bluetooth 5.2 connectivity. Plush protein-leather ear pads and a foldable design for travel.",
                    14999, "Sony",
                    "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=600&h=600&fit=crop"),

                Create(tech.Id, "USB-C Multiport Hub",
                    "Compact 7-in-1 hub with 4K HDMI output, USB-C PD passthrough charging, SD/microSD slots, and two USB-A 3.0 ports. CNC aluminium shell dissipates heat effectively.",
                    3999, "Baseus",
                    "https://images.unsplash.com/photo-1625842268584-8f3296236761?w=600&h=600&fit=crop"),

                Create(tech.Id, "Minimal LED Desk Lamp",
                    "Modern LED desk lamp with five brightness levels, three colour temperatures, and a built-in wireless charging base. Touch-sensitive controls and a flexible gooseneck arm.",
                    2999, "Philips",
                    "https://images.unsplash.com/photo-1507473885765-e6ed057ab6fe?w=600&h=600&fit=crop"),

                Create(tech.Id, "Portable SSD 1TB",
                    "High-speed external NVMe SSD with read speeds up to 1,050 MB/s. Shock-resistant aluminium enclosure, USB-C interface, and AES 256-bit hardware encryption.",
                    7499, "Samsung",
                    "https://images.unsplash.com/photo-1597872200969-2b65d56bd16b?w=600&h=600&fit=crop"),

                Create(tech.Id, "Wireless Charging Dock",
                    "A 3-in-1 wireless charging station for phone, earbuds, and smartwatch simultaneously. Supports Qi2 fast charging up to 15W. Sleek matte black finish with LED indicator.",
                    3499, "Anker",
                    "https://images.unsplash.com/photo-1586953208448-b95a79798f07?w=600&h=600&fit=crop"),

                Create(tech.Id, "Portable Bluetooth Speaker",
                    "Compact waterproof speaker (IP67) with deep bass and 360-degree sound. 12-hour battery life and a built-in carabiner clip for on-the-go use. Available in stealth black.",
                    4999, "JBL",
                    "https://images.unsplash.com/photo-1608043152269-423dbba4e7e1?w=600&h=600&fit=crop")
            });

            // ==================== MOBILE ACCESSORIES ====================
            var mobile = categories.First(c => c.Name == "Mobile Accessories");
            products.AddRange(new[]
            {
                Create(mobile.Id, "Matte Protective Phone Case",
                    "Durable polycarbonate case with a soft-touch anti-scratch matte finish. Raised bezels protect the camera and screen from drops. Precise cutouts for all ports and buttons.",
                    1499, "Spigen",
                    "https://images.unsplash.com/photo-1601784551446-20c9e07cdbdb?w=600&h=600&fit=crop"),

                Create(mobile.Id, "Tempered Glass Screen Protector (2-Pack)",
                    "Ultra-clear 9H tempered glass protector with oleophobic coating to resist fingerprints. Easy bubble-free installation with the included alignment frame. Fits most modern smartphones.",
                    999, "Anker",
                    "https://images.unsplash.com/photo-1605236453806-6ff36851218e?w=600&h=600&fit=crop"),

                Create(mobile.Id, "Wireless Charging Pad",
                    "A slim fast-charging pad with minimalist design and anti-slip silicone surface. Supports Qi standard up to 15W with case-compatible charging through cases up to 8mm thick.",
                    2999, "Baseus",
                    "https://images.unsplash.com/photo-1586953208448-b95a79798f07?w=600&h=600&fit=crop"),

                Create(mobile.Id, "Foldable Phone Stand",
                    "Adjustable aluminium alloy stand for hands-free device usage. Multi-angle viewing, folds flat for portability, and compatible with phones and tablets up to 12.9 inches.",
                    899, "Ugreen",
                    "https://images.unsplash.com/photo-1586920740199-47ce4a4b2e70?w=600&h=600&fit=crop"),

                Create(mobile.Id, "Magnetic Car Mount",
                    "A powerful N52 neodymium magnetic mount with a ball-joint swivel for 360-degree rotation. Attaches to air vents securely and holds even heavy phones steady on bumpy roads.",
                    1299, "Xiaomi",
                    "https://images.unsplash.com/photo-1558618666-fcd25c85f82e?w=600&h=600&fit=crop"),

                Create(mobile.Id, "Leather Phone Wallet Case",
                    "All-in-one phone case with genuine leather exterior, 3 card slots, a cash pocket, and a magnetic flap closure. RFID-blocking lining protects your cards from skimming.",
                    1899, "DarkGear",
                    "https://images.unsplash.com/photo-1612902456551-404854679917?w=600&h=600&fit=crop"),

                Create(mobile.Id, "True Wireless Earbuds",
                    "Compact Bluetooth 5.3 earbuds with active noise cancellation, touch controls, and 8-hour playtime (32 hours with case). IPX5 water resistance for workouts and commutes.",
                    4999, "Soundpeats",
                    "https://images.unsplash.com/photo-1590658268037-6bf12f032f55?w=600&h=600&fit=crop"),

                Create(mobile.Id, "USB-C Fast Charging Cable (2m)",
                    "Durable braided nylon cable with USB-C to USB-C connectors supporting 100W PD fast charging and 480 Mbps data transfer. Reinforced strain-relief joints prevent fraying.",
                    799, "Baseus",
                    "https://images.unsplash.com/photo-1583394838336-acd977736f90?w=600&h=600&fit=crop")
            });

            // ==================== FITNESS GEAR ====================
            var fitness = categories.First(c => c.Name == "Fitness Gear");
            products.AddRange(new[]
            {
                Create(fitness.Id, "Compression Training Leggings",
                    "High-waist compression leggings with moisture-wicking fabric and a hidden pocket for your phone. Four-way stretch material provides full range of motion during any workout.",
                    3499, "ShadowFit",
                    "https://images.unsplash.com/photo-1506629082955-511b1aa562c8?w=600&h=600&fit=crop", true),

                Create(fitness.Id, "Resistance Band Set (5 Levels)",
                    "A set of five natural latex resistance bands in progressive resistance levels from 5 to 50 lbs. Includes a carry pouch, door anchor, and two cushioned handles.",
                    1999, "DarkFit",
                    "https://images.unsplash.com/photo-1598289431512-b97b0917affc?w=600&h=600&fit=crop"),

                Create(fitness.Id, "Non-Slip Yoga Mat (6mm)",
                    "A premium 6mm thick yoga mat with a dual-texture non-slip surface. Made from eco-friendly TPE material, it's lightweight, easy to clean, and comes with an adjustable carry strap.",
                    2499, "UrbanShade",
                    "https://images.unsplash.com/photo-1601925260368-ae2f83cf8b7f?w=600&h=600&fit=crop"),

                Create(fitness.Id, "Adjustable Dumbbell (20kg)",
                    "Space-saving adjustable dumbbell that replaces 10 individual dumbbells. Quick-change weight selector lets you switch from 2kg to 20kg in seconds. Durable steel construction.",
                    8999, "NordFit",
                    "https://images.unsplash.com/photo-1583454110551-21f2fa2afe61?w=600&h=600&fit=crop"),

                Create(fitness.Id, "Gym Duffel Bag",
                    "A spacious 40L duffel bag with a ventilated shoe compartment, wet/dry separation pocket, and padded shoulder strap. Water-resistant 600D polyester in matte black.",
                    3999, "ShadowFit",
                    "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=600&h=600&fit=crop"),

                Create(fitness.Id, "Foam Roller (45cm)",
                    "A high-density EVA foam roller with a textured surface for deep-tissue massage and myofascial release. Perfect for post-workout recovery and improving flexibility.",
                    1499, "DarkFit",
                    "https://images.unsplash.com/photo-1570691079236-4beb83f10ed4?w=600&h=600&fit=crop"),

                Create(fitness.Id, "Stainless Steel Water Bottle (750ml)",
                    "Double-wall vacuum insulated water bottle that keeps drinks cold for 24 hours or hot for 12. BPA-free, leak-proof lid with a finger loop. Available in matte black.",
                    1799, "UrbanShade",
                    "https://images.unsplash.com/photo-1602143407151-7111542de6e8?w=600&h=600&fit=crop"),

                Create(fitness.Id, "Workout Gloves with Wrist Support",
                    "Breathable mesh-back gloves with padded palms and integrated wrist wraps for extra support. Silicone grip prevents calluses during heavy lifting. Easy pull-off tabs included.",
                    1299, "NordFit",
                    "https://images.unsplash.com/photo-1583473848882-f9a5bc7fd2ee?w=600&h=600&fit=crop")
            });

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }
}