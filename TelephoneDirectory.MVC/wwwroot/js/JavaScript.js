   function confirmUserRoleDeletion(userId,userName, roleId, roleName) {
        Swal.fire({
            title: 'Emin misiniz?',
            text: `'${userName}' adlı kişinin '${roleName}' rolünü silmek üzeresiniz.Bu işlem geri alınamaz!`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Evet, sil!',
            cancelButtonText: 'Hayır, iptal et'
        }).then((result) => {
            if (result.isConfirmed) {
                window.location.href = '/Users/DeleteUserRole?userId=' + userId + '&roleId=' + roleId;
            }
        });
    }
function confirmDeletion(personId, personName) {
    Swal.fire({
        title: 'Emin misiniz?',
        text: `'${personId}' ID'li '${personName}' adlı kişi silinecek. Bu işlem geri alınamaz!`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Evet, sil!',
        cancelButtonText: 'Hayır, iptal et'
    }).then((result) => {
        if (result.isConfirmed) {
            // Silme işlemini gerçekleştirecek URL'ye yönlendirme
            window.location.href = '/Persons/Delete?id=' + personId;
        }
    });
}


function toggleRoles(element) {
    console.log("Tıklandı!"); // Kontrol için
    const rolesContainer = element.nextElementSibling;

    if (rolesContainer) {
        if (rolesContainer.style.display === "none" || rolesContainer.style.display === "") {
            rolesContainer.style.display = "block"; // Aç
        } else {
            rolesContainer.style.display = "none"; // Kapat
        }
    } else {
        console.error("Roller bölümü bulunamadı!");
    }
}
