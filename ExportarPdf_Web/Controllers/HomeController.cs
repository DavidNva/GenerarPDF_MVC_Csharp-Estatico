using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
//using QuestPDF.Previewer;

namespace ExportarPdf_Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult DescargarPdf()
        {
            
            var data = Document.Create(document =>
            {
                document.Page(page =>
                {
                    // page content
                    page.Margin(30);
                    // page.Header().Height(100).Background(Colors.Blue.Medium);
                    page.Header().ShowOnce().Row(row =>
                    {//el ShowOnce sirve para que el header solo aparezca en la primera hoja
                        //D:\ConsolePdf\ExportarPdf_Web\Content\images\cuborubikcode.png
                        var rutaImagen = Path.Combine("D:\\ConsolePdf\\ExportarPdf_Web\\Content\\images\\cuborubikcode.png");

                        byte[] imageData = System.IO.File.ReadAllBytes(rutaImagen);

                        row.ConstantItem(150).Image(imageData);


                        //row.ConstantItem(140).Height(60).Placeholder();//Elegimos el ancho del item

                        row.RelativeItem().Column(col =>//El ancho se coloca relativamente automatica
                        {
                            col.Item().AlignCenter().Text("Biblioteca: Luis Cabrera Lobato").Bold().FontSize(14);
                            col.Item().AlignCenter().Text("Zacatlán, Puebla").Bold().FontSize(9);
                            col.Item().AlignCenter().Text("764 129 1840").Bold().FontSize(9);
                            col.Item().AlignCenter().Text("cuborubikcode@gmail.com").Bold().FontSize(9);
                            //col.Item().Background(Colors.Orange.Medium).Height(10);
                            //col.Item().Background(Colors.Green.Medium).Height(10);
                        });
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Border(1).BorderColor("#257272").
                            AlignCenter().Text("RUC 1234567890");

                            col.Item().Background("#257272").Border(1)
                            .BorderColor("#257272").AlignCenter()
                            .Text("Boleto de Venta").FontColor("#fff");

                            col.Item().Border(1).BorderColor("#257272").
                            AlignCenter().Text("B0001 - 234");

                        });

                    });

                    // page.Content().Background(Colors.Yellow.Medium);
                    page.Content().PaddingVertical(10).Column(col1 =>
                    {
                        col1.Item().Column(col2 =>//Columna de datos de usuario
                        {
                            col2.Item().Text("Datos del Usuario").Underline().Bold();

                            col2.Item().Text(txt =>
                            {
                                txt.Span("Nombre: ").SemiBold().FontSize(10);
                                txt.Span("David Nava Garcia").FontSize(10);
                            });

                            col2.Item().Text(txt =>
                            {
                                txt.Span("DNI: ").SemiBold().FontSize(10);
                                txt.Span("0877625727").FontSize(10);
                            });

                            col2.Item().Text(txt =>
                            {
                                txt.Span("Dirección: ").SemiBold().FontSize(10);
                                txt.Span("Calle Luis Cabrera S/N").FontSize(10);
                            });
                        });

                        col1.Item().LineHorizontal(0.5f);
                        col1.Item().Table(tabla =>
                        {//Seccion de la tabla
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#257272")
                                .Padding(2).Text("Libro").FontColor("#fff");

                                header.Cell().Background("#257272")
                                 .Padding(2).Text("Precio Unitario").FontColor("#fff");

                                header.Cell().Background("#257272")
                                .Padding(2).Text("Cantidad").FontColor("#fff");

                                header.Cell().Background("#257272")
                                .Padding(2).Text("Total").FontColor("#fff");
                            });

                            foreach (var item in Enumerable.Range(1, 45))
                            {
                                var cantidad = Placeholders.Random.Next(1, 10);
                                var precio = Placeholders.Random.Next(5, 15);
                                var total = cantidad * precio;

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(Placeholders.Label()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cantidad.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text($"S/.{precio}").FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).AlignRight().Text($"S/.{total}").FontSize(10);
                            }
                        });

                        col1.Item().AlignRight().Text("Total: 1500").FontSize(12);


                        col1.Item().Background(Colors.Grey.Lighten3).Padding(10)//Seccion de comentarios
                        .Column(column =>
                        {
                            column.Item().Text("Comentarios").FontSize(14);
                            column.Item().Text(Placeholders.LoremIpsum());
                            column.Spacing(5);
                        });

                        col1.Spacing(10);
                    });

                    page.Footer()
                    .AlignRight()
                    .Text(txt =>
                    {
                        txt.Span("Pagina ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);

                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                    //page.Footer().Height(50).Background(Colors.Red.Medium);
                });
            }).GeneratePdf();

            Stream stream = new MemoryStream(data);
            return File(stream, "applicacion/pdf", "detallePrestamo.pdf");
            //return View();
        }
    }
}