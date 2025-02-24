class Program
{
    static void Main()
    {
        // Generar ciudadanos ficticios
        HashSet<string> ciudadanos = new HashSet<string>();
        for (int i = 1; i <= 500; i++)
        {
            ciudadanos.Add($"Ciudadano {i}");
        }

        // Generar vacunados con Pfizer y AstraZeneca
        HashSet<string> vacunadosPfizer = new HashSet<string>(ciudadanos.OrderBy(x => Guid.NewGuid()).Take(75));
        HashSet<string> vacunadosAstraZeneca = new HashSet<string>(ciudadanos.Except(vacunadosPfizer).OrderBy(x => Guid.NewGuid()).Take(75));
        
        // Generar ciudadans vacunados con ambas dosis
        HashSet<string> vacunadosAmbasDosis = new HashSet<string>(vacunadosPfizer.Intersect(vacunadosAstraZeneca));
        
        // Generar no vacunados
        HashSet<string> noVacunados = new HashSet<string>(ciudadanos.Except(vacunadosPfizer).Except(vacunadosAstraZeneca));
        
        // Generar reportes
        GenerarReporte(noVacunados, vacunadosAmbasDosis, vacunadosPfizer.Except(vacunadosAmbasDosis), vacunadosAstraZeneca.Except(vacunadosAmbasDosis));
        
        Console.WriteLine("Reporte generado: reporte_vacunacion.pdf");
    }

    static void GenerarReporte(HashSet<string> noVacunados, HashSet<string> vacunadosAmbasDosis, HashSet<string> soloPfizer, HashSet<string> soloAstraZeneca)
    {
        Document doc = new Document();
        PdfWriter.GetInstance(doc, new FileStream("reporte_vacunacion.pdf", FileMode.Create));
        doc.Open();

        doc.Add(new Paragraph("Reporte de Vacunación COVID"));
        doc.Add(new Paragraph("\nListado de ciudadanos que no se han vacunado:"));
        AgregarListaAlDocumento(doc, noVacunados);

        doc.Add(new Paragraph("\nListado de ciudadanos que han recibido las dos vacunas:"));
        AgregarListaAlDocumento(doc, vacunadosAmbasDosis);

        doc.Add(new Paragraph("\nListado de ciudadanos que solamente han recibido la vacuna de Pfizer:"));
        AgregarListaAlDocumento(doc, soloPfizer);

        doc.Add(new Paragraph("\nListado de ciudadanos que solamente han recibido la vacuna de AstraZeneca:"));
        AgregarListaAlDocumento(doc, soloAstraZeneca);

        doc.Close();
    }

    static void AgregarListaAlDocumento(Document doc, HashSet<string> lista)
    {
        foreach (var ciudadano in lista)
        {
            doc.Add(new Paragraph(ciudadano));
        }
    }
}
