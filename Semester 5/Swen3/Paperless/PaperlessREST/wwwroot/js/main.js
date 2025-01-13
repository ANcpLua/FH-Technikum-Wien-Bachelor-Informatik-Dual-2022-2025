function showAlert(message, type) {
    const alertContainer = document.getElementById('alert-container');
    if (!alertContainer) return;

    // Clear existing alerts to prevent stacking
    const existingAlerts = alertContainer.querySelectorAll('.alert');
    existingAlerts.forEach(alert => alert.remove());

    // Creation and injection
    const alert = document.createElement('div');
    alert.className = `alert alert-${type} alert-dismissible fade show`;
    alert.role = 'alert';
    alert.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    `;
    alertContainer.appendChild(alert);

    // Automatic cleanup with animation
    setTimeout(() => {
        alert.classList.add('fade-out');
        setTimeout(() => alert.remove(), 300);
    }, 4700);
}
async function loadDocuments() {
    try {
        const response = await fetch('/documents');
        if (!response.ok) {
            throw new Error(`An error has occurred: ${response.status}`);
        }
        const documents = await response.json();
        const tbody = document.getElementById('documentsTableBody');
        if (!tbody) return;

        tbody.innerHTML = '';
        documents.forEach(doc => {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${doc.id}</td>
                <td>${doc.name}</td>
                <td>${new Date(doc.dateUploaded).toLocaleString()}</td>
                <td class="document-actions">
                    <a href="/documents/${doc.id}/download" class="btn btn-sm btn-primary" download="${doc.name}">Download</a>
                    <button onclick="deleteDocument(${doc.id})" class="btn btn-sm btn-danger">Delete</button>
                </td>
            `;
            tbody.appendChild(row);
        });
    } catch (error) {
        console.error("Error loading documents:", error);
        showAlert("Failed to load documents", "danger");
    }
}
async function deleteDocument(id) {
    if (!confirm("Are you sure you want to delete this document?")) return;

    try {
        const response = await fetch(`/documents/${id}`, { method: 'DELETE' });
        if (!response.ok) {
            const error = await response.json();
            throw new Error(error.message || "Failed to delete document");
        }
        showAlert("Document deleted successfully", "success");
        loadDocuments();
    } catch (error) {
        console.error("Error deleting document:", error);
        showAlert(error.message, "danger");
    }
}
async function performSearch() {
    const searchInput = document.getElementById('searchInput');
    const resultsList = document.getElementById('searchResults');
    if (!searchInput || !resultsList) return;

    const query = searchInput.value.trim();
    if (!query) {
        showAlert("Please enter a search term", "warning");
        return;
    }

    try {
        const response = await fetch(`/documents/search?query=${encodeURIComponent(query)}`);
        if (!response.ok) throw new Error(`An error has occurred: ${response.status}`);

        const result = await response.json();
        resultsList.innerHTML = '';

        if (!result.documents?.length) {
            resultsList.innerHTML = '<li class="list-group-item">No documents found.</li>';
            return;
        }

        result.documents.forEach(doc => {
            resultsList.innerHTML += `
                <li class="list-group-item">
                    <strong>${doc.name}</strong><br>
                    Uploaded at: ${new Date(doc.dateUploaded).toLocaleString()}<br>
                    <a href="/documents/${doc.id}/download" class="btn btn-sm btn-primary mt-2" download="${doc.name}">Download</a>
                </li>
            `;
        });
    } catch (error) {
        console.error("Error searching documents:", error);
        showAlert("Failed to perform search", "danger");
    }
}
function handleUpload(e) {
    e.preventDefault();
    const formData = new FormData();
    const file = document.getElementById('file')?.files[0];

    if (!file) {
        showAlert("Please select a file", "warning");
        return;
    }

    formData.append('file', file);
    formData.append('name', file.name);

    fetch('/documents/upload', {
        method: 'POST',
        body: formData
    })
        .then(async response => {
            if (!response.ok) {
                const result = await response.json();
                throw new Error(result.message || result.errors?.join(', ') || 'Upload failed');
            }
            return response.json();
        })
        .then(() => {
            showAlert("Document uploaded successfully", "success");
            setTimeout(() => window.location.href = '/documents.html', 1500);
        })
        .catch(error => {
            console.error("Error uploading document:", error);
            showAlert(error.message, "danger");
        });
}
document.addEventListener('DOMContentLoaded', () => {
    if (document.getElementById('documentsTableBody')) {
        loadDocuments();
    }

    const searchButton = document.getElementById('searchButton');
    const searchInput = document.getElementById('searchInput');
    if (searchButton) {
        searchButton.addEventListener('click', performSearch);
    }
    if (searchInput) {
        searchInput.addEventListener('keypress', e => {
            if (e.key === 'Enter') {
                e.preventDefault();
                performSearch();
            }
        });
    }

    const uploadForm = document.getElementById('uploadForm');
    if (uploadForm) {
        uploadForm.addEventListener('submit', handleUpload);
    }
});