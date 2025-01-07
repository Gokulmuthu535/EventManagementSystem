// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
<script>
        // Auto-hide the success message after 3 seconds
    setTimeout(function () {
            var successMessage = document.getElementById('successMessage');
    if (successMessage) {
        successMessage.style.display = 'none';
            }
        }, 3000); // 3000 milliseconds = 3 seconds
</script>