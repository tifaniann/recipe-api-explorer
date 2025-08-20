using TheMealDBApp.Controllers;

var builder = WebApplication.CreateBuilder(args);

// 1. Tambahkan service sebelum builder.Build()
builder.Services.AddHttpClient<HomeController>();
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // contoh: session 30 menit
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// 2. Configure pipeline setelah Build()
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();   // supaya wwwroot bisa diakses

app.UseRouting();

app.UseSession();       // session harus sebelum Authorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); //menentukan halaman pertama mana yang akan diakses (indexnya)

app.Run();
