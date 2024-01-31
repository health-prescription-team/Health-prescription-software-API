
using Health_prescription_software_API.Hubs;
var builder = WebApplication.CreateBuilder(args);

ServiceRegistration.RegisterServices(builder.Services);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("CORSPolicy");

app.UseAuthentication();   
app.UseAuthorization();    

app.MapControllers();
app.MapHub<ChatHub>("/chatHub");

app.Run();
