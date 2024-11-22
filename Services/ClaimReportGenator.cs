using System.Collections.Generic;
using System.IO;
using CMCS.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;

public class ClaimReportGenerator
{
    private readonly List<Claim> _claims;

    public ClaimReportGenerator(List<Claim> claims)
    {
        _claims = claims;
    }

    public byte[] GeneratePdf()
    {
        using (var memoryStream = new MemoryStream())
        {
            Document document = new Document();
            PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            document.Add(new Paragraph("Approved Claims Report"));
            document.Add(new Paragraph(" "));
            
            PdfPTable table = new PdfPTable(5);
            table.AddCell("Claim ID");
            table.AddCell("Lecturer Name");
            table.AddCell("Hours Worked");
            table.AddCell("Hourly Rate");
            table.AddCell("Total Payment");

            foreach (var claim in _claims)
            {
                table.AddCell(claim.Id.ToString());
                table.AddCell(claim.Name);
                table.AddCell(claim.HoursWorked.ToString());
                table.AddCell(claim.HourlyRate.ToString("C"));
                table.AddCell((claim.HoursWorked * claim.HourlyRate).ToString("C"));
            }

            document.Add(table);
            document.Close();

            return memoryStream.ToArray();
        }
    }
}
