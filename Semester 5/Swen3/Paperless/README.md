

<div align="center">
  
# Document Management System SWKOM 2024/2025

## Team Members: Alexander Nachtmann - Jasmin Mondre - Stephanie Rauscher

</div>

## Development Resources

### Documentation
- [MinIO .NET SDK API Reference](https://min.io/docs/minio/linux/developers/dotnet/API.html)
- [MinIO .NET Getting Started](https://min.io/docs/minio/linux/developers/dotnet/minio-dotnet.html)
- [Elasticsearch .NET Client Guide](https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/getting-started-net.html#_searching_documents)

### OCR Integration
- [Tesseract Examples](https://github.com/charlesw/tesseract/blob/master/docs/examples.md)
- [Tesseract Error Handling](https://github.com/charlesw/tesseract/wiki/Errors)
- [Ghostscript Downloads](https://ghostscript.com/releases/gsdnld.html)
  - Currently using: Ghostscript 10.04.0 for Windows (64 bit) AGPL Release

## Service URLs
- RabbitMQ Management: http://localhost:15672/
- Elasticsearch Management: http://localhost:5601/app/management/data/index_management/indices
- MinIO Management: http://localhost:9090/browser
- Document Upload Interface: http://localhost/upload.html
  
## Project Overview
A comprehensive document management system featuring OCR processing and full-text search capabilities.

![Paperless System Overview](https://github.com/user-attachments/assets/eb992045-a6d3-48c5-bc83-3a91269bc544)

### User Interface

#### 
- **Dashboard:** 
  
  ![Dashboard](https://github.com/user-attachments/assets/a57f75b3-60f4-4c7f-a654-2970814cc7c1)

- **Document Table:**
  
  ![Table View](https://github.com/user-attachments/assets/7a4edd6d-9172-41ab-8550-2d57a328efdf)

- **Upload:**
  
  ![Upload Interface](https://github.com/user-attachments/assets/af121a85-bbb6-4b44-8da1-d89ce0276474)

#### Services
- **Elasticsearch:** Full-text search and document indexing
  
  ![Elasticsearch](https://github.com/user-attachments/assets/93eb4f27-c6a7-4c01-a96a-d2265853fac1)

- **MinIO:** Object storage
  
  ![MinIO](https://github.com/user-attachments/assets/81efa875-354c-4f8b-babe-72eaa6d76f42)

- **RabbitMQ:** Message queue

  ![RabbitMQ](https://github.com/user-attachments/assets/656363fe-53c1-4c15-a7d2-065830a628a0)



## Getting Started

### Database Setup
```bash
dotnet restore
cd PaperlessREST
dotnet ef database update
```
