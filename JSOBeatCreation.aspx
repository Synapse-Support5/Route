<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JSOBeatCreation.aspx.cs" Inherits="Route.JSOBeatCreation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>JSO Beat Creation</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />

    <style>
        .toast-custom {
            position: fixed;
            top: 10px;
            right: 20px;
            width: 300px;
            background-color: #fff;
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            padding: 15px;
        }

        .toast-success {
            border-left: 5px solid #28a745; /* Light green */
        }

        .toast-danger {
            border-left: 5px solid #dc3545; /* Red */
        }

        .form-control {
            text-align: center;
        }

        @media (max-width: 768px) {
            .col-12 {
                text-align: center;
            }

            #btnOpenModal {
                width: 100%;
            }

            .grid-wrapper {
                overflow-x: auto;
                -webkit-overflow-scrolling: touch;
            }

                .grid-wrapper table {
                    width: 100%;
                    display: block;
                }

                    .grid-wrapper table thead,
                    .grid-wrapper table tbody {
                        display: table;
                        width: 100%;
                    }
        }

        .navbar-white .navbar-toggler {
            border-color: rgba(0, 0, 0, 0.1);
        }

        .navbar-white .navbar-toggler-icon {
            background-image: url("data:image/svg+xml;charset=utf8,%3Csvg viewBox='0 0 30 30' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath stroke='rgba(0, 0, 0, 0.5)' stroke-width='2' stroke-linecap='round' stroke-miterlimit='10' d='M4 7h22M4 15h22M4 23h22'/%3E%3C/svg%3E");
        }

        .grid-wrapper {
            max-height: 300px;
            max-width: 100%;
            overflow: auto;
        }

        /* Spinner styles */
        .spinner {
            width: 30px;
            height: 30px;
            border: 4px solid #ddd;
            border-top: 4px solid #3498db;
            border-radius: 50%;
            animation: spin 1s linear infinite;
            margin: auto;
            margin-left: 0px;
            margin-top: 5px;
            display: none; /* Hidden by default */
        }

        /* Keyframes for spinner animation */
        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }

        /* Style for the file upload container */
        .file-upload-container {
            position: relative;
        }

        /* Style for the file input button */
        .file-upload-input {
            padding-right: 50px; /* Space for the image */
        }

        /* Style for the link that contains the image */
        .file-upload-link {
            position: absolute;
            top: 50%;
            right: 0px; /* Adjust position of the icon */
            transform: translateY(-50%);
            background-color: #e6e6e6; /* Same as the button color */
            border-radius: 3px;
            padding: 5px;
            display: inline-block;
        }

        /* Style for the Excel image */
        .file-upload-icon {
            width: 25px; /* Adjust the size of the image */
            height: 25px;
            background-color: transparent; /* Keep the background transparent */
        }

        /* Container for the progress bar */
        .progress-bar-container {
            width: 100%; /* Makes the container take up the full width */
            /*max-width: 600px;*/ /* Optional: sets a maximum width for larger screens */
            background-color: #e0e0e0;
            border-radius: 0px;
            overflow: hidden;
            position: relative;
            margin-top: -20px; /* Adjusts spacing from the <hr /> */
            margin-bottom: 20px; /* Optional: adds some space after the progress bar */
        }

        /* The animated progress bar */
        .progress-bar {
            height: 5px;
            background: linear-gradient(to right, #4caf50, #81c784, #4caf50); /* Green gradient */
            width: 0%; /* Starts from 0% width */
            animation: progress-animation 2s infinite; /* Animation for progress effect */
        }

        /* Keyframes for progress animation */
        @keyframes progress-animation {
            0% {
                width: 0%;
            }

            50% {
                width: 50%;
            }

            100% {
                width: 100%;
            }
        }

        /* Adjustments for small screens */
        @media (max-width: 768px) {
            .progress-bar-container {
                width: 100%; /* Full width on smaller screens */
                max-width: 100%; /* Ensures the progress bar can stretch to the screen size */
            }
        }

        /* Navbar */
        .navbar-white .navbar-toggler {
            border-color: rgba(0, 0, 0, 0.1);
        }

        .navbar-white .navbar-toggler-icon {
            background-image: url("data:image/svg+xml;charset=utf8,%3Csvg viewBox='0 0 30 30' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath stroke='rgba(0, 0, 0, 0.5)' stroke-width='2' stroke-linecap='round' stroke-miterlimit='10' d='M4 7h22M4 15h22M4 23h22'/%3E%3C/svg%3E");
        }

        /* Sidebar */
        .sidebar {
            position: fixed;
            top: 50px;
            left: -250px;
            width: 250px;
            height: calc(100vh - 50px);
            /*background-color: rgba(34, 34, 34, 0.95);*/
            color: white;
            overflow-y: auto;
            transition: left 0.3s ease-in-out;
            z-index: 1100;
            padding: 15px;
        }

            .sidebar.open {
                left: 0;
            }

            .sidebar ul {
                padding: 0;
                list-style: none;
            }

            .sidebar .nav-item {
                padding: 8px 0;
            }

            .sidebar .nav-link {
                color: white;
                text-decoration: none;
                display: block;
                padding: 10px;
                transition: background 0.3s;
            }

                .sidebar .nav-link:hover {
                    background: rgba(255, 255, 255, 0.2);
                }

        /* Overlay */
        .overlay {
            display: none;
            position: fixed;
            top: 50px;
            left: 0;
            width: 100%;
            height: calc(100vh - 50px);
            background: rgba(0, 0, 0, 0.5);
            z-index: 1050;
        }

        /* Content */
        .content {
            transition: opacity 0.3s ease-in-out;
        }

        .sidebar.open ~ .content {
            opacity: 0.7;
        }
    </style>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>
    <script>
        function showToast(message, styleClass) {
            var toast = $('<div class="toast-custom ' + styleClass + '">' + message + '</div>').appendTo('#toastContainer');

            // Show the toast
            toast.fadeIn();

            // Move existing toasts down
            $('.toast-custom').not(toast).each(function () {
                $(this).animate({ top: "+=" + (toast.outerHeight() + 10) }, 'fast');
            });

            // Hide the toast after 3 seconds
            setTimeout(function () {
                toast.fadeOut(function () {
                    // Remove the toast from DOM after fadeOut
                    $(this).remove();

                    // Move remaining toasts up
                    $('.toast-custom').each(function (index) {
                        $(this).animate({ top: "-=" + (toast.outerHeight() + 10) }, 'fast');
                    });
                });
            }, 3000);
        }

        function showNoteAlert() {
            $('#noteAlert').show();
        }

        function hideNoteAlert() {
            $('#noteAlert').hide();
        }
    </script>

    <script>
        function toggleSidebar() {
            $(".sidebar").toggleClass("open");
            $(".overlay").toggle();
        }

        $(document).ready(function () {
            $(".overlay").click(function () {
                $(".sidebar").removeClass("open");
                $(this).hide();
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="overlay"></div>

        <!-- Navbar -->
        <nav class="navbar navbar-white bg-white">
            <button class="navbar-toggler" type="button" onclick="toggleSidebar()">
                <span class="navbar-toggler-icon"></span>
            </button>
            <%--<span class="brand">SYNAPSE</span>--%>
            <a class="navbar-brand" runat="server" href="~/Home">SYNAPSE</a>
        </nav>
        <hr />

        <%--progress bar--%>
        <div id="loadingOverlay" style="display: none; z-index: 9999;">
            <div class="progress-bar-container">
                <div class="progress-bar"></div>
            </div>
        </div>

        <div class="container body-content">

            <aside class="sidebar">
                <ul class="nav flex-column">
                    <li class="nav-item">
                        <a class="nav-link" runat="server" href="~/Home" onclick="showLoading()">Home</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" runat="server" href="~/Create" onclick="showLoading()">Create</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" runat="server" href="~/Map" onclick="showLoading()">Map</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" runat="server" href="~/Transfer" onclick="showLoading()">Transfer</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" runat="server" href="~/NewGeo" onclick="showLoading()">New Geo</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" runat="server" href="~/BeatReailgnment" onclick="showLoading()">Beat Reailgnment</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" runat="server" href="~/SOBeatCreation" onclick="showLoading()">SO Beat Creation</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" runat="server" href="~/JSOBeatCreation" onclick="showLoading()">JSO Beat Creation</a>
                    </li>
                </ul>
            </aside>

            <!-- Main Content -->
            <main class="content">
                <div class="headtag">
                    <asp:Label ID="lblUserName" runat="server" Style="color: black; float: right; margin-top: 0px; margin-bottom: -20px; margin-right: 20px"></asp:Label>
                </div>
                <table style="width: 100%; font-family: Calibri; font-size: small">
                    <tr>
                        <td style="text-align: right">
                            <asp:Label ID="lbl_msg" Text="" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Large" BackColor="LightPink"></asp:Label>
                        </td>
                    </tr>
                </table>
                <h2 style="text-align: center; margin-top: 20px;">JSO Beat Creation</h2>
                <br />

                <div class="container">
                    <div class="row justify-content-center">
                        <div id="btnDivSplit" class="col-12 col-md-3 mb-2 mb-md-0" runat="server">
                            <div class="file-upload-container position-relative">
                                <asp:FileUpload ID="FileUpload_Id" runat="server" CssClass="form-control file-upload-input" accept=".xls, .xlsx, .xlsb" />
                                <a href="Excel/Sample.xlsx" download="Sample" class="file-upload-link" title="Download Sample Excel Template">
                                    <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/3/34/Microsoft_Office_Excel_%282019%E2%80%93present%29.svg/512px-Microsoft_Office_Excel_%282019%E2%80%93present%29.svg.png?20190925171014" alt="Excel Logo" class="file-upload-icon" /></a>
                            </div>
                        </div>
                        <div id="btnDivSplit2" class="col-12 col-md-3 mb-2 mb-md-0" runat="server">
                            <asp:Button ID="SubmitBtn" runat="server" Text="Submit" CssClass="btn btn-info form-control" />
                        </div>
                        <%--<div class="col-12 col-md-3 mb-2 mb-md-0">
                        <div id="spinner" class="spinner"></div>
                    </div>--%>
                    </div>
                </div>


            </main>



            <%--<asp:Button ID="btnContinue" runat="server" Text="Continue" OnClick="btnContinue_Click" />--%>


            <%-- Notification Label --%>
            <div id="toastContainer" aria-live="polite" aria-atomic="true" style="position: relative; min-height: 200px;"></div>
            <asp:HiddenField ID="hdnBusinessType" runat="server" />
            <asp:HiddenField ID="hdnRole" runat="server" />
        </div>
    </form>


    <%--scri[pt for progressbar--%>
    <script>
        function showLoading() {
            document.getElementById('loadingOverlay').style.display = 'block';
        }

        window.onload = function () {
            document.getElementById('loadingOverlay').style.display = 'none';
        };
    </script>

</body>
</html>
