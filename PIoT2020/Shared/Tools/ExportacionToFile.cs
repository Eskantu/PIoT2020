using iTextSharp.text;
using iTextSharp.text.pdf;
using PIoT2020.Shared.Modelos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using PIoT2020.COMMON.Entidades;
using System.IO;
using CsvHelper;
using System.Globalization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using PIoT2020.COMMON.Modelos;

namespace PIoT2020.Shared.Tools
{
    public static class ExportacionToFile
    {
        public static async Task<bool> CreateFile(this IJSRuntime js, List<Exportacion> datos, string proyecto, Sensor sensor, string tipo)
        {

            try
            {
                switch (tipo)
                {
                    case "PDF":
                        await PDF(js, datos, proyecto, sensor);
                        break;
                    case "CSV":
                        await CSV(js, datos, proyecto, sensor);
                        break;
                    case "Excel":
                        Excel(js, datos, proyecto, sensor);
                        break;
                    default:
                        break;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async static void Excel(IJSRuntime js, List<Exportacion> datos, string proyecto, Sensor sensor)
        {
            using (var package=new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(sensor.Name);
                var tableBody = worksheet.Cells["A1:A1"].LoadFromCollection(datos,true);
                await js.SaveAs($"PIoT2020-{DateTime.Now.ToString()}.xlsx",package.GetAsByteArray());
            }
        }

        private async static Task CSV(IJSRuntime js, List<Exportacion> datos, string proyecto, Sensor sensor)
        {
            using (var memory = new MemoryStream())
            {
                using (var writer = new StreamWriter(memory))
                {
                    writer.WriteLine($"#,Fecha hora creación,{sensor.UnidadDeMedida}");
                    foreach (Exportacion exportacion in datos)
                    {
                        writer.WriteLine($"{exportacion.NumeroRegistro},{exportacion.FechaHoraCreacion},{exportacion.Valor}");
                    }
                    writer.Close();
                    var arr = memory.ToArray();
                    await js.SaveAs($"PIoT2020-{DateTime.Now.ToString()}.csv", arr);
                }
            }
        }

        private static async Task PDF(IJSRuntime js, List<Exportacion> datos, string proyecto, Sensor sensor)
        {
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                Document document = new Document(PageSize.A4, 10, 10, 10, 10);
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

                document.Open();
                iTextSharp.text.Font tittle = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 16f, BaseColor.Black);
                Paragraph paragraph = new Paragraph($"{proyecto}->{sensor.Name}", tittle);
                paragraph.SpacingBefore = 10;
                paragraph.SpacingAfter = 10;
                paragraph.Alignment = Element.ALIGN_CENTER;
                document.Add(paragraph);
                PdfPTable table = new PdfPTable(3);
                table.AddCell("#");
                table.AddCell("Fecha hora creación");
                table.AddCell(sensor.UnidadDeMedida);
                foreach (Exportacion exportacion in datos)
                {
                    table.AddCell(exportacion.NumeroRegistro.ToString());
                    table.AddCell(exportacion.FechaHoraCreacion.ToString());
                    table.AddCell(exportacion.Valor.ToString());

                }
                document.Add(table);

                document.Close();
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                await js.SaveAs($"PIoT2020{DateTime.Now.ToString()}.pdf", bytes);
            }
        }

       
    }
}
