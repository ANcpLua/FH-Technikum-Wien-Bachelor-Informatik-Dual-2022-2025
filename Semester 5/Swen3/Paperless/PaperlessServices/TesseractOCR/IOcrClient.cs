namespace PaperlessServices.TesseractOCR;

public interface IOcrClient
{
    string OcrPdf(Stream pdfStream);
}
