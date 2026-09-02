using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Web;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();

string apiKeyGemini = "AQ.Ab8RN6Ja3fbKJfT-svJrZANykqsdUmdjbGPFEMR7lFNHCQFUKw";
string correoServidor = "gorrafiestera@gmail.com";
string claveAplicacionServidor = "nwyt aonm xykt ldwy";

var correosAreas = new Dictionary<string, string>
{
    { "Soporte Técnico", "mikelluno68@gmail.com" },
    { "Ventas", "emanuel20254057@gmail.com" },
    { "Recursos Humanos", "pepeey0983@gmail.com" },
    { "Facturación y Pagos", "pepeey0983@gmail.com" }
};

string cssEstilos = @"
<link rel='manifest' href='/manifest.json'>
<meta name='theme-color' content='#0f172a'>
<link href='https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;600;700&display=swap' rel='stylesheet'>
<style>
    * { box-sizing: border-box; font-family: 'Plus Jakarta Sans', sans-serif; transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1); }
    
    body { 
        background: linear-gradient(135deg, #09111e 0%, #0f172a 50%, #0369a1 100%);
        color: #ffffff; 
        display: flex; 
        flex-direction: column;
        justify-content: center; 
        align-items: center; 
        min-height: 100vh; 
        margin: 0; 
        padding: 80px 20px 60px 20px; 
        position: relative;
        overflow-x: hidden;
    }

    /* Barra Superior para el Perfil de Usuario */
    .top-user-bar {
        position: absolute;
        top: 25px;
        left: 30px;
        display: flex;
        align-items: center;
        gap: 12px;
        background: rgba(15, 23, 42, 0.75);
        backdrop-filter: blur(12px);
        -webkit-backdrop-filter: blur(12px);
        padding: 8px 18px 8px 10px;
        border-radius: 50px;
        border: 1px solid rgba(255, 255, 255, 0.15);
        box-shadow: 0 8px 20px rgba(0, 0, 0, 0.3);
        z-index: 20;
    }

    .top-user-avatar {
        width: 42px;
        height: 42px;
        border-radius: 50%;
        background-color: #64748b;
        object-fit: cover;
        border: 2px solid #ffffff;
    }

    .top-user-info {
        display: flex;
        flex-direction: column;
        justify-content: center;
    }

    .top-user-name {
        color: #ffffff;
        font-weight: 700;
        font-size: 14px;
        line-height: 1.2;
    }

    .top-user-email {
        color: #ffffff;
        font-size: 11px;
        opacity: 0.9;
        line-height: 1.2;
    }

    /* Firma en la esquina inferior derecha */
    .footer-credits {
        position: fixed;
        bottom: 20px;
        right: 30px;
        color: #ffffff;
        font-size: 13px;
        font-weight: 600;
        letter-spacing: 0.5px;
        background: rgba(15, 23, 42, 0.6);
        backdrop-filter: blur(8px);
        padding: 6px 14px;
        border-radius: 20px;
        border: 1px solid rgba(255, 255, 255, 0.15);
        z-index: 20;
    }

    /* Luces azules en los fondos laterales */
    .bg-decor-left {
        position: fixed;
        width: 450px;
        height: 450px;
        background: radial-gradient(circle, rgba(56, 189, 248, 0.25) 0%, rgba(0, 0, 0, 0) 70%);
        top: 10%;
        left: -100px;
        border-radius: 50%;
        filter: blur(60px);
        pointer-events: none;
    }

    .bg-decor-right {
        position: fixed;
        width: 450px;
        height: 450px;
        background: radial-gradient(circle, rgba(2, 132, 199, 0.25) 0%, rgba(0, 0, 0, 0) 70%);
        bottom: 5%;
        right: -100px;
        border-radius: 50%;
        filter: blur(60px);
        pointer-events: none;
    }

    /* Layout principal de 3 columnas */
    .main-wrapper {
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 30px;
        width: 100%;
        max-width: 1200px;
        z-index: 10;
    }

    .side-card {
        background: rgba(15, 23, 42, 0.65);
        backdrop-filter: blur(16px);
        -webkit-backdrop-filter: blur(16px);
        border: 1px solid rgba(255, 255, 255, 0.12);
        border-radius: 20px;
        padding: 25px;
        flex: 1;
        max-width: 280px;
        box-shadow: 0 15px 35px rgba(0, 0, 0, 0.4);
    }

    .side-card h3 {
        color: #ffffff;
        font-size: 16px;
        margin-top: 0;
        margin-bottom: 15px;
        display: flex;
        align-items: center;
        gap: 8px;
    }

    .side-card p {
        color: #ffffff;
        font-size: 13px;
        line-height: 1.5;
        text-align: left;
        margin-bottom: 12px;
    }

    .side-card ul {
        margin: 0;
        padding-left: 18px;
        color: #ffffff;
        font-size: 13px;
    }

    .side-card li { margin-bottom: 8px; }

    .container { 
        background: rgba(15, 23, 42, 0.85); 
        backdrop-filter: blur(20px);
        -webkit-backdrop-filter: blur(20px);
        padding: 40px; 
        border-radius: 24px; 
        width: 100%; 
        max-width: 460px; 
        box-shadow: 0 25px 50px rgba(0,0,0,0.6), inset 0 1px 0 rgba(255,255,255,0.15); 
        border: 1px solid rgba(255, 255, 255, 0.15); 
    }

    h2 { 
        text-align: center; 
        color: #ffffff;
        margin-top: 0; 
        font-size: 28px; 
        font-weight: 700;
        letter-spacing: -0.5px;
    }

    p { color: #ffffff; font-size: 14px; text-align: center; margin-bottom: 25px; }

    label { 
        font-size: 12px; 
        color: #ffffff; 
        display: block; 
        margin-top: 18px; 
        font-weight: 700; 
        text-transform: uppercase; 
        letter-spacing: 1px; 
    }

    input, select, textarea { 
        width: 100%; 
        padding: 14px 16px; 
        margin-top: 8px; 
        background: rgba(2, 6, 23, 0.7); 
        border: 1px solid rgba(255, 255, 255, 0.2); 
        border-radius: 12px; 
        color: #ffffff; 
        font-size: 15px; 
        outline: none; 
    }

    input::placeholder, textarea::placeholder {
        color: #94a3b8;
    }

    input:focus, select:focus, textarea:focus { 
        border-color: #38bdf8; 
        box-shadow: 0 0 18px rgba(56, 189, 248, 0.4); 
        background: rgba(2, 6, 23, 0.9);
        transform: translateY(-1px);
    }

    .tel-group { display: flex; gap: 10px; }
    .tel-group input[list] { width: 42%; }
    .tel-group input[type='tel'] { width: 58%; }

    button { 
        width: 100%; 
        padding: 16px; 
        margin-top: 25px; 
        background: linear-gradient(135deg, #0284c7 0%, #2563eb 100%); 
        color: #ffffff; 
        border: none; 
        border-radius: 12px; 
        font-weight: 700; 
        font-size: 16px; 
        cursor: pointer; 
        display: flex; 
        justify-content: center; 
        align-items: center; 
        gap: 10px; 
        box-shadow: 0 4px 20px rgba(2, 132, 199, 0.5);
    }

    button:hover { 
        transform: translateY(-2px); 
        box-shadow: 0 8px 25px rgba(37, 99, 235, 0.6); 
        filter: brightness(1.1);
    }

    button:active { transform: translateY(0); }

    .spinner { 
        display: none; 
        border: 3px solid rgba(255,255,255,0.3); 
        border-radius: 50%; 
        border-top: 3px solid #fff; 
        width: 20px; 
        height: 20px; 
        animation: spin 0.8s linear infinite; 
    }

    @keyframes spin { 0% { transform: rotate(0deg); } 100% { transform: rotate(360deg); } }
    #campoTelefono { display: none; }

    @media (max-width: 900px) {
        .side-card { display: none; }
        .top-user-bar { top: 15px; left: 15px; }
        .footer-credits { bottom: 10px; right: 15px; font-size: 11px; }
    }
</style>
<div class='bg-decor-left'></div>
<div class='bg-decor-right'></div>
<div class='footer-credits'>Aplicación echa por A.G</div>
<script>
    if ('serviceWorker' in navigator) {
        navigator.serviceWorker.register('/sw.js');
    }

    function cambiarCanal() {
        var canal = document.getElementById('canalSelect').value;
        var campoTel = document.getElementById('campoTelefono');
        var inputCod = document.getElementById('codigoPais');
        var inputTel = document.getElementById('numTelefono');
        
        if (canal === 'telefono') {
            campoTel.style.display = 'block';
            inputCod.required = true;
            inputTel.required = true;
        } else {
            campoTel.style.display = 'none';
            inputCod.required = false;
            inputTel.required = false;
        }
    }

    function mostrarCarga() {
        document.getElementById('spn').style.display = 'inline-block';
        document.getElementById('btnTexto').innerText = 'Procesando...';
    }
</script>";

app.MapGet("/", () => Results.Content($@"
<!DOCTYPE html>
<html lang='es'>
<head><meta charset='UTF-8'><meta name='viewport' content='width=device-width, initial-scale=1.0'><title>Iniciar Sesión</title>{cssEstilos}</head>
<body>
    <div class='main-wrapper'>
        <div class='side-card'>
            <h3>🌐 Portal Integrado</h3>
            <p>Plataforma para el envío rápido de tickets e interacciones mediante Inteligencia Artificial.</p>
        </div>

        <div class='container'>
            <h2>Bienvenido 👋</h2>
            <p>Inicia sesión para redactar tu mensaje</p>
            <form action='/menu' method='POST'>
                <label>Nombre de Usuario</label>
                <input type='text' name='usuario' placeholder='Ej. Juan Pérez' required>
                <label>Correo Electrónico</label>
                <input type='email' name='correo' placeholder='tu_correo@gmail.com' required>
                <button type='submit'>Ingresar al Sistema</button>
            </form>
        </div>

        <div class='side-card'>
            <h3>⚡ Beneficios</h3>
            <ul>
                <li>Clasificación con IA</li>
                <li>Redirección a WhatsApp</li>
                <li>Soporte técnico directo</li>
            </ul>
        </div>
    </div>
</body>
</html>", "text/html"));

app.MapPost("/menu", async (HttpContext context) =>
{
    var form = await context.Request.ReadFormAsync();
    string usuario = form["usuario"]!;
    string correo = form["correo"]!;
    return RenderizarMenu(usuario, correo);
});

app.MapPost("/enviar", async (HttpContext context) =>
{
    var form = await context.Request.ReadFormAsync();
    string usuario = form["usuario"]!;
    string correoUsuario = form["correoUsuario"]!;
    string canal = form["canal"]!;
    string areaSeleccionada = form["areaSeleccionada"]!;
    string asunto = form["asunto"]!;
    string cuerpo = form["cuerpo"]!;
    string codigoPais = form["codigoPais"]!;
    string numeroTelefono = form["numeroTelefono"]!;

    string areaFinal = areaSeleccionada;
    if (areaSeleccionada == "IA")
    {
        areaFinal = await ClasificarConGemini(asunto, cuerpo, apiKeyGemini);
    }

    if (canal == "telefono")
    {
        string codLimpio = new string(codigoPais.Where(char.IsDigit).ToArray());
        string numLimpio = new string(numeroTelefono.Where(char.IsDigit).ToArray());
        string telefonoCompleto = codLimpio + numLimpio;

        string mensajeTexto = $"*Solicitud de:* {usuario}\n*Área:* {areaFinal}\n*Asunto:* {asunto}\n------------------\n{cuerpo}";
        string urlWhatsapp = $"https://api.whatsapp.com/send?phone={telefonoCompleto}&text={HttpUtility.UrlEncode(mensajeTexto)}";

        return Results.Content($@"
        <!DOCTYPE html>
        <html lang='es'>
        <head>
            <meta charset='UTF-8'><meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>Redirigiendo...</title>
            {cssEstilos}
            <script>
                window.location.href = '{urlWhatsapp}';
            </script>
        </head>
        <body>
            <div class='top-user-bar'>
                <img src='/favicon.png' class='top-user-avatar' alt='Perfil'>
                <div class='top-user-info'>
                    <span class='top-user-name'>{usuario}</span>
                    <span class='top-user-email'>{correoUsuario}</span>
                </div>
            </div>

            <div class='container' style='text-align: center;'>
                <h2 style='color:#38bdf8;'>Enviando a WhatsApp... 🚀</h2>
                <p>Abriendo el chat con el número <b>+{telefonoCompleto}</b></p>
                
                <form action='/menu' method='POST' style='margin-top:20px;'>
                    <input type='hidden' name='usuario' value='{usuario}'>
                    <input type='hidden' name='correo' value='{correoUsuario}'>
                    <button type='submit' style='background:rgba(30, 41, 59, 0.8);'>← Volver al Menú</button>
                </form>
            </div>
        </body>
        </html>", "text/html");
    }

    if (correosAreas.TryGetValue(areaFinal, out string? correoDestino))
    {
        EnviarCorreoSMTP(correoServidor, claveAplicacionServidor, correoUsuario, correoDestino, asunto, cuerpo, areaFinal);

        return Results.Content($@"
        <!DOCTYPE html>
        <html lang='es'>
        <head><meta charset='UTF-8'><meta name='viewport' content='width=device-width, initial-scale=1.0'>{cssEstilos}</head>
        <body>
            <div class='top-user-bar'>
                <img src='/favicon.png' class='top-user-avatar' alt='Perfil'>
                <div class='top-user-info'>
                    <span class='top-user-name'>{usuario}</span>
                    <span class='top-user-email'>{correoUsuario}</span>
                </div>
            </div>

            <div class='container' style='text-align: center;'>
                <h2 style='color:#38bdf8;'>¡Correo Enviado! 🚀</h2>
                <p>Dirigido a: <b>{areaFinal}</b> ({correoDestino})</p>
                <form action='/menu' method='POST'>
                    <input type='hidden' name='usuario' value='{usuario}'>
                    <input type='hidden' name='correo' value='{correoUsuario}'>
                    <button type='submit'>← Volver al Menú</button>
                </form>
            </div>
        </body>
        </html>", "text/html");
    }

    return Results.Content("<h1 style='color:red;'>Error al procesar el envío.</h1>", "text/html");
});

app.Run();

IResult RenderizarMenu(string usuario, string correo)
{
    return Results.Content($@"
    <!DOCTYPE html>
    <html lang='es'>
    <head><meta charset='UTF-8'><meta name='viewport' content='width=device-width, initial-scale=1.0'><title>Menú de Envíos</title>{cssEstilos}</head>
    <body>
        <div class='top-user-bar'>
            <img src='/favicon.png' class='top-user-avatar' alt='Perfil'>
            <div class='top-user-info'>
                <span class='top-user-name'>{usuario}</span>
                <span class='top-user-email'>{correo}</span>
            </div>
        </div>

        <div class='main-wrapper'>
            <div class='side-card'>
                <h3>📌 Consejos</h3>
                <p>Usa la <b>Clasificación Automática por IA</b> para que el sistema analice tu mensaje y lo mande al área adecuada.</p>
            </div>

            <div class='container'>
                <h2>Hola, {usuario} ✨</h2>
                <p>Elige la vía y completa los datos.</p>
                
                <form action='/enviar' method='POST' onsubmit='mostrarCarga()'>
                    <input type='hidden' name='usuario' value='{usuario}'>
                    <input type='hidden' name='correoUsuario' value='{correo}'>

                    <label>Vía de Envío (Canal)</label>
                    <select name='canal' id='canalSelect' onchange='cambiarCanal()'>
                        <option value='correo'>📧 Correo Electrónico</option>
                        <option value='telefono'>💬 WhatsApp</option>
                    </select>

                    <div id='campoTelefono'>
                        <label>Número Destino</label>
                        <div class='tel-group'>
                            <input type='text' id='codigoPais' name='codigoPais' list='listaPaises' placeholder='🔎 País o Cód.' autocomplete='off'>
                            <input type='tel' id='numTelefono' name='numeroTelefono' placeholder='Número local (ej. 55554444)'>
                        </div>
                        <datalist id='listaPaises'>
                            <option value='+502'>Guatemala (+502)</option>
                            <option value='+52'>México (+52)</option>
                            <option value='+1'>Estados Unidos / Canadá (+1)</option>
                            <option value='+34'>España (+34)</option>
                            <option value='+57'>Colombia (+57)</option>
                            <option value='+54'>Argentina (+54)</option>
                            <option value='+56'>Chile (+56)</option>
                            <option value='+51'>Perú (+51)</option>
                            <option value='+503'>El Salvador (+503)</option>
                            <option value='+504'>Honduras (+504)</option>
                            <option value='+505'>Nicaragua (+505)</option>
                            <option value='+506'>Costa Rica (+506)</option>
                            <option value='+507'>Panamá (+507)</option>
                            <option value='+58'>Venezuela (+58)</option>
                            <option value='+593'>Ecuador (+593)</option>
                            <option value='+591'>Bolivia (+591)</option>
                            <option value='+595'>Paraguay (+595)</option>
                            <option value='+598'>Uruguay (+598)</option>
                            <option value='+509'>Haití (+509)</option>
                            <option value='+1-809'>República Dominicana (+1-809)</option>
                        </datalist>
                    </div>

                    <label>Área Destino</label>
                    <select name='areaSeleccionada'>
                        <option value='IA'>🤖 Clasificación Automática por IA</option>
                        <option value='Soporte Técnico'>🛠️ Soporte Técnico</option>
                        <option value='Ventas'>💼 Ventas</option>
                        <option value='Recursos Humanos'>👥 Recursos Humanos</option>
                        <option value='Facturación y Pagos'>💳 Facturación y Pagos</option>
                    </select>

                    <label>Asunto</label>
                    <input type='text' name='asunto' placeholder='Escribe el asunto aquí' required>

                    <label>Mensaje</label>
                    <textarea name='cuerpo' rows='4' placeholder='Escribe los detalles...' required></textarea>

                    <button type='submit'>
                        <span id='spn' class='spinner'></span>
                        <span id='btnTexto'>Procesar Solicitud</span>
                    </button>
                </form>
                
                <a href='/' style='display:block; text-align:center; margin-top:20px; color:#ffffff; font-size:13px; text-decoration:none;'>Cerrar Sesión</a>
            </div>

            <div class='side-card'>
                <h3>📞 Canales Activos</h3>
                <p>Recuerda seleccionar WhatsApp si deseas iniciar la conversación de forma inmediata desde tu teléfono.</p>
            </div>
        </div>
    </body>
    </html>", "text/html");
}

static async Task<string> ClasificarConGemini(string asunto, string cuerpo, string apiKey)
{
    using var client = new HttpClient();
    string endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-3.6-flash:generateContent?key={apiKey}";
    string prompt = $"Clasifica este correo ÚNICAMENTE en una de estas 4 opciones exactas:\n- Soporte Técnico\n- Ventas\n- Recursos Humanos\n- Facturación y Pagos\n\nAsunto: {asunto}\nCuerpo: {cuerpo}";
    var payload = new { contents = new[] { new { parts = new[] { new { text = prompt } } } } };

    try
    {
        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(endpoint, content);
        if (!response.IsSuccessStatusCode) return "Soporte Técnico";

        string responseJson = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(responseJson);
        string texto = doc.RootElement.GetProperty("candidates")[0].GetProperty("content")[0].GetProperty("parts")[0].GetProperty("text").GetString() ?? "";

        if (texto.Contains("Venta", StringComparison.OrdinalIgnoreCase)) return "Ventas";
        if (texto.Contains("Recursos", StringComparison.OrdinalIgnoreCase) || texto.Contains("Humano", StringComparison.OrdinalIgnoreCase)) return "Recursos Humanos";
        if (texto.Contains("Factur", StringComparison.OrdinalIgnoreCase) || texto.Contains("Pago", StringComparison.OrdinalIgnoreCase)) return "Facturación y Pagos";

        return "Soporte Técnico";
    }
    catch { return "Soporte Técnico"; }
}

static void EnviarCorreoSMTP(string servidorEmail, string claveServidor, string remitenteUsuario, string destinatario, string asunto, string cuerpo, string area)
{
    using var smtpClient = new SmtpClient("smtp.gmail.com")
    {
        Port = 587,
        Credentials = new NetworkCredential(servidorEmail, claveServidor),
        EnableSsl = true,
        DeliveryMethod = SmtpDeliveryMethod.Network,
        UseDefaultCredentials = false
    };

    var mailMessage = new MailMessage
    {
        From = new MailAddress(servidorEmail, "Portal Interactivo"),
        Subject = $"[{area}] {asunto}",
        Body = $"Mensaje enviado desde el Portal Web\n\nUsuario: {remitenteUsuario}\nÁrea: {area}\n----------------------------\n\n{cuerpo}",
        IsBodyHtml = false,
        Priority = MailPriority.Normal
    };

    mailMessage.To.Add(destinatario);
    mailMessage.Headers.Add("X-Mailer", "CSharp-Web-Router");
    smtpClient.Send(mailMessage);
}