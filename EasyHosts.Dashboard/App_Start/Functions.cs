using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace EasyHosts.Dashboard.Models
{
    public class Functions
    {
        public static string HashText(string text, string nameHash)
        {
            HashAlgorithm algoritmo = HashAlgorithm.Create(nameHash);
            if (algoritmo == null)
            {
                throw new ArgumentException("Nome de hash incorreto", "nomeHash");
            }
            byte[] hash = algoritmo.ComputeHash(Encoding.UTF8.GetBytes(text));
            return Convert.ToBase64String(hash);
        }

        public static string SendEmail(string emailRecipient, string subject, string bodymessage)
        {
            try
            {
                //Cria o endereço de email do remetente
                MailAddress inemail = new MailAddress("Easy Hosts <hostseasy@gmail.com>");
                //Cria o endereço de email do destinatário -->
                MailAddress foremail = new MailAddress(emailRecipient);
                MailMessage message = new MailMessage(inemail, foremail);
                message.IsBodyHtml = true;
                //Assunto do email
                message.Subject = subject;
                //Conteúdo do email
                message.Body = bodymessage;
                //Prioridade E-mail
                message.Priority = MailPriority.Normal;
                //Cria o objeto que envia o e-mail
                SmtpClient client = new SmtpClient();
                //Envia o email
                client.Send(message);
                return "success|E-mail enviado com sucesso";
            }
            catch { return "error|Erro ao enviar e-mail"; }
        }

        public static string Encode(string text)
        {
            byte[] stringBase64 = new byte[text.Length];
            stringBase64 = Encoding.UTF8.GetBytes(text);
            string encode = Convert.ToBase64String(stringBase64);
            return encode;
        }

        public static string Decode(string text)
        {
            var encode = new UTF8Encoding();
            var utf8Decode = encode.GetDecoder();
            byte[] stringValue = Convert.FromBase64String(text);
            int cont = utf8Decode.GetCharCount(stringValue, 0,
            stringValue.Length);
            char[] decodeChar = new char[cont];
            utf8Decode.GetChars(stringValue, 0, stringValue.Length, decodeChar, 0);
            string result = new String(decodeChar);
            return result;
        }

        public static string GerarGraficoPizza(string titulo, string dados)
        {
            string graf = @"<script type='text/javascript'  src='https://www.gstatic.com/charts/loader.js'></script>
                            <script type='text/javascript'>
                                google.charts.load('current', {packages:['corechart']});
                                google.charts.setOnLoadCallback(drawChart);
                                function drawChart() {
                                    var data = google.visualization.arrayToDataTable([['', '']," + dados + @"]);
                                    var options = {
                                        title: '" + titulo + @"',
                                        is3D: false, 
                                    };
                                    var chart = new google.visualization.PieChart(document.getElementById('piechart_" + titulo.Replace(" ", "") + @"'));
                                    chart.draw(data, options);
                                }
                                </script>
                                <div id='piechart_" + titulo.Replace(" ", "") + @"' style='height: 350px'></div>";
            return graf;
        }

        public static string GerarGraficoBarraColuna(string titulo, string subtitulo, string dados, bool barra)
        {
            string tipo = "";
            if (barra)
                tipo = "bars: 'horizontal'";
            string graf = @"<script type='text/javascript' src='https://www.gstatic.com/charts/loader.js'></script>
                            <script type='text/javascript'>
                                google.charts.load('current', {'packages':['bar']});
                                google.charts.setOnLoadCallback(drawChart);
                                function drawChart() {
                                    var data = google.visualization.arrayToDataTable([" + dados + @" ]);
                                    var options = {
                                        chart: {
                                            title: '" + titulo + @"',
                                            subtitle: '" + subtitulo + @"', }, " + tipo + @"
                                    };
                                    var chart = new google.charts.Bar(document.getElementById('barchart_" + titulo.Replace(" ", "") + @"'));
                                    chart.draw(data, google.charts.Bar.convertOptions(options));
                                }
                            </script>
                            <div id='barchart_" + titulo.Replace(" ", "") + @"' style='min-height: 350px;'></div>";
            return graf;
        }

        public static DataTable GenerateDataTableBedrooms(List<Bedroom> bedrooms)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Nome do Quarto", typeof(string));
            dataTable.Columns.Add("Valor", typeof(string));
            dataTable.Columns.Add("Tipo de Quarto", typeof(string));
            dataTable.Columns.Add("Descrição", typeof(string));

            foreach (var item in bedrooms)
                dataTable.Rows.Add(item.NameBedroom, item.Value, item.TypeBedroom.NameTypeBedroom, item.Description);
            return dataTable;
        }

        public static DataTable GenerateDataTableEvents(List<Event> events)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Nome do Evento", typeof(string));
            dataTable.Columns.Add("Organizador", typeof(string));
            dataTable.Columns.Add("Data", typeof(string));
            dataTable.Columns.Add("Localização", typeof(string));
            dataTable.Columns.Add("Descrição", typeof(string));
            dataTable.Columns.Add("Atrações", typeof(string));

            foreach (var item in events)
                dataTable.Rows.Add(item.NameEvent, item.Organizer, item.DateEvent, item.EventsPlace, item.DescriptionEvent, item.Attractions);
            return dataTable;
        }

        public static DataTable GenerateDataTableProducts(List<Product> products)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Nome do Produto", typeof(string));
            dataTable.Columns.Add("Preço", typeof(string));
            dataTable.Columns.Add("Quantidade", typeof(string));

            foreach (var item in products)
                dataTable.Rows.Add(item.Name, item.Value, item.QuantityProduct);
            return dataTable;
        }

        public static DataTable GenerateDataTableTypeBedrooms(List<TypeBedroom> typebedrooms)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Quantidade de Pessoas", typeof(string));
            dataTable.Columns.Add("Quantidade de Camas", typeof(string));
            dataTable.Columns.Add("Utilitários", typeof(string));

            foreach (var item in typebedrooms)
                dataTable.Rows.Add(item.AmountOfPeople, item.AmountOfBed, item.ApartmentAmenities);
            return dataTable;
        }

        public static DataTable GenerateDataTablePerfils(List<Perfil> perfils)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Descrição", typeof(string));

            foreach (var item in perfils)
                dataTable.Rows.Add(item.Description);
            return dataTable;
        }

        public static DataTable GenerateDataTableUsers(List<User> users)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Nome", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));
            dataTable.Columns.Add("CPF", typeof(string));

            foreach (var item in users)
                dataTable.Rows.Add(item.Name, item.Email, item.Cpf);
            return dataTable;
        }

        public static DataTable GenerateDataTableBookings(List<Booking> booking)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Número", typeof(string));
            dataTable.Columns.Add("Usuário", typeof(string));
            dataTable.Columns.Add("Data de Checkin", typeof(string));
            dataTable.Columns.Add("Data de Checkout", typeof(string));
            dataTable.Columns.Add("Quarto", typeof(string));
            dataTable.Columns.Add("Valor", typeof(string));

            foreach (var item in booking)
                dataTable.Rows.Add(item.CodeBooking, item.User.Name, item.DateCheckin, item.DateCheckout, item.BedroomId, item.ValueBooking);
            return dataTable;
        }
    }
}